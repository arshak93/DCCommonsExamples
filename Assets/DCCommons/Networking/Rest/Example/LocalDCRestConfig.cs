using System.Collections.Generic;
using DCCommons.Networking.Rest.Config;

namespace DCCommons.Networking.Rest.Example {
	public class LocalDCRestConfig : DCRestConfig {
		private const string BASE_URL = "http://127.0.0.1:8888/api";
		private const string API_VERSION = "1";
		private const string API_VERSION_KEY = "apiVersion";
		
		public string BaseUrl { get { return BASE_URL; } }

		public Dictionary<string, object> GlobalQueryParams {
			get {
				return new Dictionary<string, object> {
					{ API_VERSION_KEY, API_VERSION }
				};
			}
		}
	}
}