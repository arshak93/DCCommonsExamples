namespace DCCommons.Networking.WebSocket.Actionhero.Request {
	public class ActionheroRequest : WebSocketRequest {
		
		public string Action { get; private set; }

		protected ActionheroRequest(string action) {
			Action = action;
		}
	}
}