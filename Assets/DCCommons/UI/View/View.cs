using UnityEngine;

namespace DCCommons.UI.View {

	public abstract class View : MonoBehaviour {
		
	}
	
	public abstract class View<TController> : View {

		protected TController controller;

		public virtual void Init(TController controller) {
			this.controller = controller;
		}

		public void Close() {
			Destroy(gameObject);
		}
	}
}