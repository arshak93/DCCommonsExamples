using Zenject;

namespace DCCommons.UI.Controller {
	public class ControllerFactory {

		private DiContainer diContainer;

		public ControllerFactory(DiContainer diContainer) {
			this.diContainer = diContainer;
		}

		public T Create<T>() where T : Controller {
			return diContainer.Instantiate<T>();
		}
	}
}