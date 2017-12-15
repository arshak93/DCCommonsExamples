using System;

namespace DCCommons.UI.Controller {

	public abstract class Controller {
		
	} 
	
	public abstract class Controller<TView> : Controller 
		where TView : View.View
	{
		private TView _view;
		public TView View {
			get {
				if (_view == null) {
					throw new Exception("View (" + typeof(TView) + ") not created");
				}
				return _view;
			}
		}

		protected Controller(TView view) {
			_view = view;
		}

		public virtual void Init() { }
	}
}