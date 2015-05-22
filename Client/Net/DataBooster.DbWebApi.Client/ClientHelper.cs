﻿// Copyright (c) 2015 Abel Cheng <abelcys@gmail.com> and other contributors.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Repository:	https://github.com/DataBooster/DbWebApi

using System;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataBooster.DbWebApi.Client
{
	public static class ClientHelper
	{
		public static IEnumerable<DbWebApiResponse> BulkReadDbJson(this HttpResponseMessage httpResponse)
		{
			var content = httpResponse.Content;
			var contentType = content.GetContentType();

			if (contentType != null && contentType.ToLower().EndsWith("/json") == false)
				throw new HttpRequestException("Response Content-Type is not JSON");

			if (httpResponse.IsSuccessStatusCode)
			{
				Task<IEnumerable<DbWebApiResponse>> readTask = content.ReadAsAsync<IEnumerable<DbWebApiResponse>>();
				IEnumerable<DbWebApiResponse> dbWebApiResponse = readTask.Result;

				if (readTask.IsFaulted)
					throw readTask.Exception;

				return dbWebApiResponse;
			}
			else
				throw httpResponse.CreateUnsuccessException();
		}

		public static DbWebApiResponse ReadDbJson(this HttpResponseMessage httpResponse)
		{
			var content = httpResponse.Content;
			var contentType = content.GetContentType();

			if (contentType != null && contentType.ToLower().EndsWith("/json") == false)
				throw new HttpRequestException("Response Content-Type is not JSON");

			if (httpResponse.IsSuccessStatusCode)
			{
				Task<DbWebApiResponse> readTask = content.ReadAsAsync<DbWebApiResponse>();
				DbWebApiResponse dbWebApiResponse = readTask.Result;

				if (readTask.IsFaulted)
					throw readTask.Exception;

				return dbWebApiResponse;
			}
			else
				throw httpResponse.CreateUnsuccessException();
		}

		private static HttpRequestException CreateUnsuccessException(this HttpResponseMessage httpResponse)
		{
			var errorDictionary = httpResponse.Content.ReadAsAsync<HttpErrorClient>().Result;

			if (errorDictionary.Count == 0)
				return new HttpRequestException(string.Format("{0} ({1})", (int)httpResponse.StatusCode, httpResponse.ReasonPhrase));
			else
				return new HttpResponseClientException(errorDictionary);
		}

		private static string GetContentType(this HttpContent content)
		{
			if (content == null)
				return null;
			if (content.Headers == null)
				return null;
			if (content.Headers.ContentType == null)
				return null;
			return content.Headers.ContentType.MediaType;
		}

		public static IDictionary<string, Array> SeparateArrayByProperties<T>(this ICollection<T> sourceRows) where T : IDictionary<string, object>
		{
			if (sourceRows == null)
				throw new ArgumentNullException("sourceRows");

			int i = 0, size = sourceRows.Count;
			Dictionary<string, Array> propArrayDict = null;
			Array separateArray;

			foreach (var row in sourceRows)
			{
				if (propArrayDict == null)
				{
					propArrayDict = new Dictionary<string, Array>();

					foreach (var prop in row)
						propArrayDict.Add(prop.Key, new object[size]);
				}

				foreach (var prop in row)
					if (propArrayDict.TryGetValue(prop.Key, out separateArray))
						separateArray.SetValue(prop.Value, i);

				i++;
			}

			return propArrayDict;
		}

		public static IDictionary<string, Array> SeparateArrayByProperties(this ICollection anonymousTypeSourceRows)
		{
			if (anonymousTypeSourceRows == null)
				throw new ArgumentNullException("sourceRows");

			int i = 0, size = anonymousTypeSourceRows.Count;
			Dictionary<string, Array> propArrayDict = null;
			PropertyDescriptorCollection properties = null;

			foreach (var rowObj in anonymousTypeSourceRows)
			{
				if (propArrayDict == null)
				{
					propArrayDict = new Dictionary<string, Array>();
					properties = TypeDescriptor.GetProperties(rowObj);

					foreach (PropertyDescriptor prop in properties)
						propArrayDict.Add(prop.Name, Array.CreateInstance(prop.PropertyType, size));
				}

				foreach (PropertyDescriptor prop in properties)
					propArrayDict[prop.Name].SetValue(prop.GetValue(rowObj), i);

				i++;
			}

			return propArrayDict;
		}
	}
}
