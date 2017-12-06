using System;
using DCCommons.Networking.WebSocket.Actionhero.Message;
using DCCommons.Networking.WebSocket.Actionhero.Request;
using DCCommons.Networking.WebSocket.Actionhero.Response;
using DCCommons.Networking.WebSocket.Command;
using DCCommons.Networking.WebSocket.Config;
using DCCommons.Networking.WebSocket.Converter;
using DCCommons.Networking.WebSocket.Message;
using DCCommons.Networking.WebSocket.Promise;
using UnityEngine;

namespace DCCommons.Networking.WebSocket.Actionhero {
	public class ActionheroClient : WebSocketClient {
		
		private class ActionSchema {
			public string Event { get; private set; }
			public ActionheroRequest Params { get; set; }

			public ActionSchema(ActionheroRequest request) {
				Event = "action";
				Params = request;
			}
		}
		
		public ActionheroClient(WebSocketConfig config, WebSocketObjectConverter converter) : base(config, converter) { }

		public CommandPromise<T> SendRequest<T>(ActionheroRequest request) where T : ActionheroResponse {
			return sendRequest<T>(request);
		}

		protected override string encodeToString(ICommand command) {
			return converter.ToString(new ActionSchema((ActionheroRequest) command.Request));
		}

		protected override WebSocketMessageInfo decodeMessageInfo(string message) {
			return converter.ToObject<ActionheroMessage>(message).GetWebSocketMessageInfo();
		}

		protected override object decodeResponse(string data, Type type) {
			return converter.ToObject(data, type);
		}

		protected override object decodeServerPush(string data, Type type) {
			return converter.ToObject(data, type);
		}

		protected override void handleMessageReceived(BestHTTP.WebSocket.WebSocket socket, string msg) {
			msg = msg.Trim('"');
			if (msg.StartsWith("primus")) {
				handlePrimusMessage(msg);
				return;
			}
			base.handleMessageReceived(socket, msg);
		}
		
		private void handlePrimusMessage(string msg) {
			string[] tokens = msg.Split(new string[] {"::"}, StringSplitOptions.None);
			string systemEvent = tokens[1];
			string param = tokens[2];

			switch (systemEvent) {
				case "ping":
					Debug.Log("<WS> Sending " + "primus::pong::" + param);
					socket.Send("\"primus::pong::" + param + "\"");
					break;
				default:
					Debug.Log("<WS> System event: " + systemEvent);
					break;
			}
		}
	}
}