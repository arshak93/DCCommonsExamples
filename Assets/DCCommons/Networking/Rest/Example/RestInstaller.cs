using DCCommons.Networking.Rest.Config;
using DCCommons.Networking.Rest.Converter;
using Zenject;

namespace DCCommons.Networking.Rest.Example {
	public class RestInstaller : MonoInstaller<RestInstaller> {
		public override void InstallBindings() {
			Container.Bind<DCRestConfig>().To<LocalDCRestConfig>().AsSingle();
			Container.Bind<DCRestObjectConverter>().To<NewtonsoftJsonConverter>().AsSingle();
			Container.Bind<DCRestClient>().AsSingle();
			Container.Bind<DCRestTest>().AsSingle().NonLazy();
		}
	}
}