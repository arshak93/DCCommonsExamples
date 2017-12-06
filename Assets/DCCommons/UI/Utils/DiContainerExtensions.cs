using Zenject;

namespace DCCommons.UI.Utils {
	public static class DiContainerExtensions {

		public static void BindViewController<TView, TController>(this DiContainer container, string viewPath) {
			container.Bind<TView>().FromComponentInNewPrefabResource(viewPath).AsTransient();
			container.Bind<TController>().AsTransient();
		}
	}
}