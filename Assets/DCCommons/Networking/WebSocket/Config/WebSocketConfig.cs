namespace DCCommons.Networking.WebSocket.Config {
	public interface WebSocketConfig {
		string Server { get; }
		WebSocketSendDataFormat SendFormat { get; }
	}
}