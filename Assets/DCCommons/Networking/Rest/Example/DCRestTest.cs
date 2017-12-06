using System;
using UnityEngine;

namespace DCCommons.Networking.Rest.Example {
	public class DCRestTest {
		private DCRestClient restClient;

		public DCRestTest(DCRestClient restClient) {
			this.restClient = restClient;

			RunTest();
		}

		public void RunTest() {
			restClient.AddGlobalHearder("Content-Type", "application/json");
			
			/*restClient.Get<TestClass>("/test")
				.OnSuccess(delegate(TestClass data) { Debug.Log("Test name " + data.Name); })
				.OnFail(delegate(Exception error) { Debug.Log(error.Message); })
				.Send();*/
			
			restClient.Post<TestClass>("/test", new TestClass() {Name = "Vahagn", Age = 22})
				.AddQueryParam("testParam", "Gurgen")
				.OnSuccess(delegate(TestClass data) { Debug.Log("Test name " + data.Name); })
				.OnFail(delegate(Exception error) { Debug.Log(error.Message); })
				.Send();
		}
	}
}