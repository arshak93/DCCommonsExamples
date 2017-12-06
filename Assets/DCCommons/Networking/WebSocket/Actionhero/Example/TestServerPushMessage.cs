namespace DCCommons.Networking.WebSocket.Actionhero.Example {
	public class TestServerPushMessage {
		public int SenderId;
		public GiftType Type;
		public int ReceiverId;

		public override string ToString() {
			return string.Format("{0} from {1} to {2}", Type, SenderId, ReceiverId);
		}
	}
}