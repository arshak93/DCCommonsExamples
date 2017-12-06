using DCCommons.UI.Controller;
using UnityEngine;

namespace DCCommons.UI.Example {
	public class TestController : Controller<TestView> {

		private UIManager uiManager;

		public TestController(TestView view, UIManager uiManager) : base(view) {
			this.uiManager = uiManager;
			
			Debug.Log("Test Controller, View: " + view.name);
		}

		public void Send(string name, int age) {
			View.SetMessage("My name is " + name + ", I am " + age);
		}

		public void Next() {
			View.Close();
		}
	}
}