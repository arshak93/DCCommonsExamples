using DCCommons.UI.View;
using UnityEngine;
using UnityEngine.UI;

namespace DCCommons.UI.Example {
	public class TestView : View<TestController> {

		[SerializeField] private InputField name;
		[SerializeField] private InputField age;
		[SerializeField] private Text message;
		[SerializeField] private Button sendButton;
		[SerializeField] private Button nextButton;

		public override void Init(TestController controller) {
			base.Init(controller);
			Debug.Log("View Init, Controller: " + controller);
		}

		void Start() {
			Debug.Log("View Start");
			sendButton.onClick.AddListener(delegate {
				controller.Send(name.text, int.Parse(age.text));
			});
			
			sendButton.onClick.AddListener(delegate {
				controller.Next();
			});
		}

		public void SetMessage(string message) {
			this.message.text = message;
		}
	}
}