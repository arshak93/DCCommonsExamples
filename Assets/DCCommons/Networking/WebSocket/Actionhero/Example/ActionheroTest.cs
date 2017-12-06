using UnityEngine;

namespace DCCommons.Networking.WebSocket.Actionhero.Example {
	public class ActionheroTest {

		private ActionheroClient client;

		public ActionheroTest(ActionheroClient client) {
			this.client = client;

			RunTest();
		}

		public void RunTest() {
			client.Connect()
				.OnConnectSuccess(delegate {
					client.SendRequest<TestResponse>(new TestRequest("pram", "Arturik", 17))
						.OnSuccess(delegate(TestResponse response) {
							Debug.Log(response.Message);
						});
				});
			
			client.RegisterServerPushHandler<TestServerPushMessage>("gift", delegate(object push) {
				var gift = (TestServerPushMessage) push;
				Debug.Log(gift.ToString());
			});
		}
	}
}