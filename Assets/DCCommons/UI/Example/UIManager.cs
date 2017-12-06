using DCCommons.UI.Controller;
using DCCommons.UI.View;
using UnityEngine;

namespace DCCommons.UI.Example {
	public class UIManager {

		private ControllerFactory controllerFactory;
		
		public Transform ViewContainer { get; private set; }

		public UIManager(Transform viewContainer, ControllerFactory controllerFactory) {
			this.controllerFactory = controllerFactory;
			ViewContainer = viewContainer;
			Debug.Log("UIManager ctor. viewContainer = " + viewContainer.name);
		}

		public void ShowTestView() {
			createView<TestView, TestController>();
		}

		private void createView<TView, TController>() 
			where TView : View<TController>
			where TController : Controller<TView>
		{
			var controller = controllerFactory.Create<TController>();
			controller.View.Init(controller);
			controller.View.transform.SetParent(ViewContainer, false);
		}
	}
}