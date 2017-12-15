using DCCommons.UI.Controller;
using DCCommons.UI.View;
using UnityEngine;

namespace DCCommons.UI.Example {
	public class UIManager : BaseUIManager{

		protected UIManager(Transform viewContainer, ControllerFactory controllerFactory) : base(viewContainer, controllerFactory) {
		}
		
		public void ShowTestView() {
			createView<TestView, TestController>();
		}
	}
}