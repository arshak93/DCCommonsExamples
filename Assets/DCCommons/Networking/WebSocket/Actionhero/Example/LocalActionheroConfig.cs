using DCCommons.Networking.WebSocket.Config;

namespace DCCommons.Networking.WebSocket.Actionhero.Example {
	public class LocalActionheroConfig : WebSocketConfig {

		public string Server {
			//get { return "ws://192.168.0.101:8888/primus"; }
			get { return "ws://127.0.0.1:8888/primus"; }
		}

		public WebSocketSendDataFormat SendFormat {
			get { return WebSocketSendDataFormat.String; }
		}
	}
}