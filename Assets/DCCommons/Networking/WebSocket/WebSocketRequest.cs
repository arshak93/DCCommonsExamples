using System;
using DCCommons.Utils;

namespace DCCommons.Networking.WebSocket {
	public class WebSocketRequest {
		public string RequestId { get; protected set; }

		protected WebSocketRequest() {
			RequestId = Hashing.MD5(Guid.NewGuid().ToString());
		}
	}
}