using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using DCCommons.Networking.Rest.Converter;
using UnityEngine;

namespace DCCommons.Networking.Rest {
	public class DCRestRequest<T> {
		
		public delegate void RequestSuccessDelegate<T>(T data);
		public delegate void RequestErrorDelegate(Exception e);

		private readonly DCRestObjectConverter converter;
		private readonly string path;
		private readonly HTTPMethods method;

		private Dictionary<string, string> headers = new Dictionary<string, string>();
		private Dictionary<string, object> queryParams = new Dictionary<string, object>();
		private RequestSuccessDelegate<T> onSuccess;
		private RequestErrorDelegate onFail;
		private object body;

		public DCRestRequest(DCRestObjectConverter converter, string path, HTTPMethods method, 
			Dictionary<string, object> globalQueryParams, Dictionary<string, string> globalHeaders) {
			this.converter = converter;
			this.path = path;
			this.method = method;

			if (globalQueryParams != null) {
				foreach (var globalQueryParam in globalQueryParams) {
					queryParams.Add(globalQueryParam.Key, globalQueryParam.Value);
				}
			}

			if (headers != null) {
				foreach (var header in globalHeaders) {
					headers.Add(header.Key, header.Value);
				}
			}
		}

		public DCRestRequest<T> AddQueryParam(string key, object value) {
			queryParams[key] = value;
			return this;
		}

		public DCRestRequest<T> AddHeader(string key, string value) {
			headers[key] = value;
			return this;
		}

		public DCRestRequest<T> AddBody(object body) {
			this.body = body;
			return this;
		}

		public DCRestRequest<T> OnSuccess(RequestSuccessDelegate<T> onSuccess) {
			this.onSuccess = onSuccess;
			return this;
		}
		
		public DCRestRequest<T> OnFail(RequestErrorDelegate onFail) {
			this.onFail = onFail;
			return this;
		}

		public void Send() {
			HTTPRequest request = new HTTPRequest(createUri(path, queryParams), method,
				delegate(HTTPRequest originalRequest, HTTPResponse response) {
					try {
						T data = processResponse<T>(originalRequest, response);
						Debug.Log("<REST> RESPONSE Uri: " + originalRequest.Uri + " Response: " + response.DataAsText);
						if (onSuccess != null) {
							onSuccess(data);
						}
					}
					catch (Exception e) {
						Debug.Log("<REST> ERROR: " + e.Message);
						if (onFail != null) {
							onFail(e);
						}
					}
				});

			foreach (var header in headers) {
				request.AddHeader(header.Key, header.Value);
			}

			if (body != null) {
				request.RawData = converter.ToRawData(body);
				Debug.Log("<REST> CALLING SERVER Uri: " + request.Uri + "\nBody: " + converter.ToString(body));
			}
			else {
				Debug.Log("<REST> CALLING SERVER Uri: " + request.Uri);				
			}
			
			request.Send();
		}
		
		private T processResponse<T>(HTTPRequest request, HTTPResponse response) {
			if (response == null) {
				throw request.Exception ?? new Exception("Unknown Exception");
			}

			DCRestResponse<T> result = converter.ToObject<DCRestResponse<T>>(response.DataAsText);

			if (!response.IsSuccess) {
				throw new Exception(result.Error);
			}

			return result.Data;
		}
		
		private Uri createUri(string path, Dictionary<string, object> queryParams) {
			StringBuilder query = new StringBuilder();
			foreach (var key in queryParams.Keys) {
				query.Append(string.Format("{0}={1}&", key, queryParams[key]));
			}

			// Remove last &
			if (query.Length > 0) {
				query.Remove(query.Length - 1, 1);
			}
			return new Uri(path + (query.Length > 0 ? "?" + query : ""));
		}
	}
}