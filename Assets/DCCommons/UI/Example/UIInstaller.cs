using DCCommons.UI.Controller;
using DCCommons.UI.Utils;
using UnityEngine;
using Zenject;

namespace DCCommons.UI.Example {
	public class UIInstaller : MonoInstaller<UIInstaller> {

		[SerializeField] private Transform viewContainer;

		public override void InstallBindings() {
			Container.Bind<ControllerFactory>().AsSingle().NonLazy();
			Container.Bind<UIManager>().AsSingle().WithArguments(viewContainer).NonLazy();
			
			Container.BindViewController<TestView, TestController>("Views/TestView");
		}
	}
}