using System;
using DCCommons.Networking.WebSocket.Message;
using Newtonsoft.Json.Linq;

namespace DCCommons.Networking.WebSocket.Actionhero.Message {
	public class ActionheroMessage {
		
		public string Id { get; set; }
		public string Context { get; set; }
		public string Error { get; set; }
		public JObject Data { get; set; }
		public JObject Message { get; set; }
		
		public WebSocketMessageInfo GetWebSocketMessageInfo() {
			WebSocketMessageType type;
			Exception exception = null;
			string data = null;
			string serverPushCode = null;
			
			switch (Context) {
				case "response":
					type = WebSocketMessageType.Response;
					break;
				case "user":
					type = WebSocketMessageType.Message;
					break;
				default:
					type = WebSocketMessageType.Unsupported;
					break;
			}

			if (!string.IsNullOrEmpty(Error)) {
				exception = new Exception(Error);
			}
			else {
				switch (type) {
					case WebSocketMessageType.Response:
						data = Data.ToString();
						break;
					case WebSocketMessageType.Message:
						data = Message.ToString();
						serverPushCode = Message["action"].Value<string>();
						break;
				}
			}
			
			return new WebSocketMessageInfo(type, Id, exception, data, serverPushCode);
		}
	}
}