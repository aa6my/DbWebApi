﻿// Copyright (c) 2015 Abel Cheng <abelcys@gmail.com> and other contributors.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DbParallel.DataAccess;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DataBooster.DbWebApi.Csv;

namespace DataBooster.DbWebApi
{
	public static class DbWebApiOptions
	{
		public static class QueryStringContract
		{
			const string _DefaultMediaTypeParameterName = "format";
			const string _DefaultNamingCaseParameterName = "NamingCase";
			const string _DefaultResultSetParameterName = "ResultSet";
			const string _DefaultFileNameParameterName = "FileName";
			const string _DefaultJsonInputParameterName = "JsonInput";
			const string _DefaultJsonpCallbackParameterName = "callback";
			const string _DefaultJsonpStateParameterName = "jsonpState";
			const string _DefaultRazorEncodingParameterName = "RazorEncoding";
			const string _DefaultRazorLanguageParameterName = "RazorLanguage";
			const string _DefaultRazorTemplateParameterName = "RazorTemplate";

			private static string _MediaTypeParameterName = _DefaultMediaTypeParameterName;
			public static string MediaTypeParameterName
			{
				get { return _MediaTypeParameterName; }
				set { _MediaTypeParameterName = string.IsNullOrEmpty(value) ? _DefaultMediaTypeParameterName : value; }
			}

			private static string _NamingCaseParameterName = _DefaultNamingCaseParameterName;
			public static string NamingCaseParameterName
			{
				get { return _NamingCaseParameterName; }
				set { _NamingCaseParameterName = string.IsNullOrEmpty(value) ? _DefaultNamingCaseParameterName : value; }
			}

			private static string _ResultSetParameterName = _DefaultResultSetParameterName;
			public static string ResultSetParameterName
			{
				get { return _ResultSetParameterName; }
				set { _ResultSetParameterName = string.IsNullOrEmpty(value) ? _DefaultResultSetParameterName : value; }
			}

			private static string _FileNameParameterName = _DefaultFileNameParameterName;
			public static string FileNameParameterName
			{
				get { return _FileNameParameterName; }
				set { _FileNameParameterName = string.IsNullOrEmpty(value) ? _DefaultFileNameParameterName : value; }
			}

			private static string _JsonInputParameterName = _DefaultJsonInputParameterName;
			public static string JsonInputParameterName
			{
				get { return _JsonInputParameterName; }
				set { _JsonInputParameterName = string.IsNullOrEmpty(value) ? _DefaultJsonInputParameterName : value; }
			}

			private static string _JsonpCallbackParameterName = _DefaultJsonpCallbackParameterName;
			public static string JsonpCallbackParameterName
			{
				get { return _JsonpCallbackParameterName; }
				set { _JsonpCallbackParameterName = string.IsNullOrEmpty(value) ? _DefaultJsonpCallbackParameterName : value; }
			}

			private static string _JsonpStateParameterName = _DefaultJsonpStateParameterName;
			public static string JsonpStateParameterName
			{
				get { return _JsonpStateParameterName; }
				set { _JsonpStateParameterName = string.IsNullOrEmpty(value) ? _DefaultJsonpStateParameterName : value; }
			}

			private static string _RazorEncodingParameterName = _DefaultRazorEncodingParameterName;
			public static string RazorEncodingParameterName
			{
				get { return _RazorEncodingParameterName; }
				set { _RazorEncodingParameterName = string.IsNullOrEmpty(value) ? _DefaultRazorEncodingParameterName : value; }
			}

			private static string _RazorLanguageParameterName = _DefaultRazorLanguageParameterName;
			public static string RazorLanguageParameterName
			{
				get { return _RazorLanguageParameterName; }
				set { _RazorLanguageParameterName = string.IsNullOrEmpty(value) ? _DefaultRazorLanguageParameterName : value; }
			}

			private static string _RazorTemplateParameterName = _DefaultRazorTemplateParameterName;
			public static string RazorTemplateParameterName
			{
				get { return _RazorTemplateParameterName; }
				set { _RazorTemplateParameterName = string.IsNullOrEmpty(value) ? _DefaultRazorTemplateParameterName : value; }
			}
		}

		public static TimeSpan DerivedParametersCacheExpireInterval
		{
			get { return DerivedParametersCache.ExpireInterval; }
			set { DerivedParametersCache.ExpireInterval = value; }
		}

		private static PropertyNamingConvention _DefaultPropertyNamingConvention = PropertyNamingConvention.None;
		public static PropertyNamingConvention DefaultPropertyNamingConvention
		{
			get { return _DefaultPropertyNamingConvention; }
			set { _DefaultPropertyNamingConvention = value; }
		}

		private static readonly CsvConfiguration _CsvConfiguration;
		public static CsvConfiguration CsvConfiguration
		{
			get { return _CsvConfiguration; }
		}

		static DbWebApiOptions()
		{
			DerivedParametersCacheExpireInterval = new TimeSpan(0, 10, 0);

			_CsvConfiguration = new CsvConfiguration();

			SetCsvDateTimeConverter();
		}

		private static void SetCsvDateTimeConverter()
		{
			Type dt = typeof(DateTime);
			Type cvt = TypeConverterFactory.GetConverter(dt).GetType();

			if (cvt == typeof(DateTimeConverter) || cvt == typeof(DefaultTypeConverter))
				TypeConverterFactory.AddConverter(dt, new CsvDateTimeConverter());
		}

		private static RazorEngine.Encoding _DefaultRazorEncoding = RazorEngine.Encoding.Raw;
		public static RazorEngine.Encoding DefaultRazorEncoding
		{
			get { return DbWebApiOptions._DefaultRazorEncoding; }
			set { DbWebApiOptions._DefaultRazorEncoding = value; }
		}

		private static RazorEngine.Language _DefaultRazorLanguage = RazorEngine.Language.CSharp;
		public static RazorEngine.Language DefaultRazorLanguage
		{
			get { return DbWebApiOptions._DefaultRazorLanguage; }
			set { DbWebApiOptions._DefaultRazorLanguage = value; }
		}
	}
}
