using DCCommons.Networking.WebSocket.Config;
using DCCommons.Networking.WebSocket.Converter;
using Zenject;

namespace DCCommons.Networking.WebSocket.Actionhero.Example {
	public class ActionheroInstaller : MonoInstaller<ActionheroInstaller> {
		
		public override void InstallBindings() {
			Container.Bind<WebSocketConfig>().To<LocalActionheroConfig>().AsSingle();
			Container.Bind<WebSocketObjectConverter>().To<NewtonsoftJsonConverter>().AsSingle();
			Container.Bind<ActionheroClient>().AsSingle();
		}
	}
}