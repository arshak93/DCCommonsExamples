using System;

namespace DCCommons.UI.Controller {

	public abstract class Controller {
		
	} 
	
	public abstract class Controller<TView> : Controller 
		where TView : View.View
	{
		protected static int globalCount = 0;
		public int LocalCount = 0;
		
		private TView _view;
		public TView View {
			get {
				if (_view == null) {
					throw new Exception("View (" + typeof(TView) + ") not created");
				}
				return _view;
			}
		}

		public Controller(TView view) {
			LocalCount = ++globalCount;
			_view = view;
		}

		~Controller() {
			--globalCount;
		}
	}
}