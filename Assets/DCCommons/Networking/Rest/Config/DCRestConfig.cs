using System.Collections.Generic;

namespace DCCommons.Networking.Rest.Config {
	public interface DCRestConfig {
		string BaseUrl { get; }
		Dictionary<string, object> GlobalQueryParams { get; }
	}
}