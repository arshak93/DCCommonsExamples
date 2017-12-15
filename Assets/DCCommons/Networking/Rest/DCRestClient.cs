using System.Collections.Generic;
using BestHTTP;
using DCCommons.Networking.Rest.Config;
using DCCommons.Networking.Rest.Converter;

namespace DCCommons.Networking.Rest {
	public class DCRestClient {
		
		private DCRestConfig config;
		private DCRestObjectConverter converter;

		private readonly Dictionary<string, string> globalHeaders = new Dictionary<string, string>();

		public DCRestClient(DCRestConfig config, DCRestObjectConverter converter) {
			this.config = config;
			this.converter = converter;
		}

		public void AddGlobalHearder(string key, string value) {
			globalHeaders[key] = value;
		}

		public DCRestRequest<T> Get<T>(string path) {
			return new DCRestRequest<T>(converter, config.BaseUrl + path, HTTPMethods.Get, config.GlobalQueryParams, globalHeaders);
		}
		
		public DCRestRequest<T> Post<T>(string path, object body) {
			return new DCRestRequest<T>(converter, config.BaseUrl + path, HTTPMethods.Post, config.GlobalQueryParams, globalHeaders).AddBody(body);
		}
		
		public DCRestRequest<T> Put<T>(string path, object body) {
			return new DCRestRequest<T>(converter, config.BaseUrl + path, HTTPMethods.Put, config.GlobalQueryParams, globalHeaders).AddBody(body);
		}
		
		public DCRestRequest<T> Delete<T>(string path) {
			return new DCRestRequest<T>(converter, config.BaseUrl + path, HTTPMethods.Delete, config.GlobalQueryParams, globalHeaders);
		}
		
		public static DcRestBaseSimpleRequest Get(string url) {
			return new DcRestBaseSimpleRequest(url, HTTPMethods.Get);
		}
		
		public static DcRestBaseSimpleRequest Post(string url, byte[] body) {
			return new DcRestBaseSimpleRequest(url, HTTPMethods.Post).AddBody(body);
		}
		
		public static DcRestBaseSimpleRequest Put(string url, byte[] body) {
			return new DcRestBaseSimpleRequest(url, HTTPMethods.Put).AddBody(body);
		}
		
		public static DcRestBaseSimpleRequest Delete(string url) {
			return new DcRestBaseSimpleRequest(url, HTTPMethods.Delete);
		}
	}
}