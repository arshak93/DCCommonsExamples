using DCCommons.Networking.WebSocket.Actionhero.Request;

namespace DCCommons.Networking.WebSocket.Actionhero.Example {
	public class TestRequest : ActionheroRequest {

		public string TestParam;
		public string Name;
		public int Age;
		
		public TestRequest(string testParam, string name, int age) : base("test") {
			TestParam = testParam;
			Name = name;
			Age = age;
		}
	}
}