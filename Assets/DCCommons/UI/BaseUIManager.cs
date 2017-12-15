using DCCommons.UI.Controller;
using DCCommons.UI.Example;
using DCCommons.UI.View;
using UnityEngine;

namespace DCCommons.UI {
    public class BaseUIManager {
        
        private ControllerFactory controllerFactory;

        protected Transform viewContainer;

        protected BaseUIManager(Transform viewContainer, ControllerFactory controllerFactory) {
            this.controllerFactory = controllerFactory;
            this.viewContainer = viewContainer;
            Debug.Log("BaseUIManager ctor. viewContainer = " + viewContainer.name);
        }

        protected void createView<TView, TController>() 
            where TView : View<TController>
            where TController : Controller<TView>
        {
            var controller = controllerFactory.Create<TController>();
            controller.View.Init(controller);
            controller.View.transform.SetParent(viewContainer, false);
            controller.Init();
        }
    }
}