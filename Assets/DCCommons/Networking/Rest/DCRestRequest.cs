using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using DCCommons.Networking.Rest.Converter;
using UnityEngine;

namespace DCCommons.Networking.Rest {
	public class DCRestRequest<T> : DCRestBaseRequest {
		
		public delegate void RequestSuccessDelegate(T data);
		public delegate void RequestErrorDelegate(Exception e);
		public delegate void RequestCompleteDelegate(T data, Exception e);

		private readonly DCRestObjectConverter converter;
		private readonly string path;
		private readonly HTTPMethods method;

		private Dictionary<string, string> headers = new Dictionary<string, string>();
		private Dictionary<string, object> queryParams = new Dictionary<string, object>();
		private RequestSuccessDelegate onSuccess;
		private RequestErrorDelegate onFail;
		private RequestCompleteDelegate onComplete;
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

			if (globalHeaders != null) {
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

		public DCRestRequest<T> OnSuccess(RequestSuccessDelegate onSuccess) {
			this.onSuccess = onSuccess;
			return this;
		}
		
		public DCRestRequest<T> OnFail(RequestErrorDelegate onFail) {
			this.onFail = onFail;
			return this;
		}
		
		public DCRestRequest<T> OnComplete(RequestCompleteDelegate onComplete) {
			this.onComplete = onComplete;
			return this;
		}

		public void Send() {
			HTTPRequest request = new HTTPRequest(createUri(path, queryParams), method,
				delegate(HTTPRequest originalRequest, HTTPResponse response) {
					try {
						T data = processResponse(originalRequest, response);
						Debug.Log("<REST> RESPONSE Uri: " + originalRequest.Uri + " Response: " + response.DataAsText);
						if (onComplete != null) {
							onComplete(data, null);
						}
						if (onSuccess != null) {
							onSuccess(data);
						}
					}
					catch (Exception e) {
						Debug.Log("<REST> ERROR: " + e.Message);
						if (onComplete != null) {
							onComplete(default(T), e);
						}
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
		
		private T processResponse(HTTPRequest request, HTTPResponse response) {
			if (response == null) {
				throw request.Exception ?? new Exception("Unknown Exception");
			}

			DCRestResponse<T> result = converter.ToObject<DCRestResponse<T>>(response.DataAsText);

			if (!response.IsSuccess) {
				throw new Exception(result.Error);
			}

			return result.Data;
		}
	}
}