using System;
using System.Collections.Generic;
using System.Text;
using DCCommons.Networking.WebSocket.Command;
using DCCommons.Networking.WebSocket.Config;
using DCCommons.Networking.WebSocket.Converter;
using DCCommons.Networking.WebSocket.Message;
using DCCommons.Networking.WebSocket.Promise;
using Newtonsoft.Json;
using UnityEngine;

namespace DCCommons.Networking.WebSocket {
	public abstract class WebSocketClient {

		public delegate void ServerPushDelegate(object basePush);
		
		private readonly Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();
		private readonly Dictionary<string, Type> serverPushTypes = new Dictionary<string, Type>();
		private readonly Dictionary<string, ServerPushDelegate> serverPushHandlers = new Dictionary<string, ServerPushDelegate>();
		private readonly ConnectionPromise promise = new ConnectionPromise();
		protected WebSocketConfig config;
		protected WebSocketObjectConverter converter;
		protected BestHTTP.WebSocket.WebSocket socket;
		
		public ConnectionStatus ConnectionStatus { get; private set; }
		
		public bool HasActiveConnection {
			get { return (socket != null); }
		}

		public WebSocketClient(WebSocketConfig config, WebSocketObjectConverter converter) {
			this.config = config;
			this.converter = converter;
			
		}

		public ConnectionPromise Connect() {
			if (!HasActiveConnection) {
				openSocket(config.Server);
			}

			return promise;
		}
		
		public ConnectionPromise Disconnect() {
			clearSocket();
			return promise;
		}
		
		public void RegisterServerPushHandler<T>(string pushCode, ServerPushDelegate handler) {
			serverPushTypes.Add(pushCode, typeof(T));
			serverPushHandlers.Add(pushCode, handler);
		}
		
		protected CommandPromise<T> sendRequest<T>(WebSocketRequest request) where T : WebSocketResponse {
			var command = new Command<T>(request);
			commands.Add(command.Id, command);
			send(command);
			return command.Promise;
		}
		
		private void openSocket(string server) {
			Debug.LogFormat("<WS> Connecting to server {0}", server);
			ConnectionStatus = ConnectionStatus.Connecting;
			socket = new BestHTTP.WebSocket.WebSocket(new Uri(server));

			socket.OnOpen += handleWebSocketOpen;
			socket.OnClosed += handleWebSocketClosed;
			socket.OnMessage += handleMessageReceived;
			socket.OnError += handleError;
			socket.OnErrorDesc += handleErrorDescription;
			socket.Open();
		}

		private void send<T>(Command<T> command) where T : WebSocketResponse {
			if (config.SendFormat == WebSocketSendDataFormat.String) {
				var msg = encodeToString(command);
				Debug.LogFormat("<WS> SND: {0}", msg);
				socket.Send(msg);
			}
			else if(config.SendFormat == WebSocketSendDataFormat.ByteArray) {
				var msg = encodeToByteArray(command);
				Debug.LogFormat("<WS> SND: {0}", msg);
				socket.Send(msg);
			}
		}
		
		private void decode(string message) {
			try {
				var info = decodeMessageInfo(message);
				switch (info.Type) {
					case WebSocketMessageType.Response:
						processResponse(info);
						Debug.LogFormat("<WS> RCV {0}", message);
						break;
					/*case SocketMessageType.Request:
			            processRequest(info, length, message);
			            break;*/
					case WebSocketMessageType.Message:
						Debug.LogFormat("<WS> PUSH {0}", message);
						processMessage(info);
						break;
					case WebSocketMessageType.Unsupported:
						Debug.Log("<WS> Unsupported context: " + message);
						break;
					default:
						Debug.LogErrorFormat("<WS> Unknown Message Type {0}", info.Type);
						break;
				}
			}
			catch (JsonSerializationException e) {
				Debug.LogWarning(e.Message);
				Debug.Log("<WS> Unsupported or System Message: " + message);
			}
		}

		protected virtual string encodeToString(ICommand command) {
			return converter.ToString(command.Request);
		}

		protected virtual byte[] encodeToByteArray(ICommand command) {
			var message = converter.ToString(command.Request);
			return Encoding.UTF8.GetBytes(message);
		}

		protected abstract WebSocketMessageInfo decodeMessageInfo(string message);
		protected abstract object decodeResponse(string data, Type type);
		protected abstract object decodeServerPush(string data, Type type);

		private void processResponse(WebSocketMessageInfo info) {
			var command = commands[info.Id];
			if (info.Exception == null) {
				command.SetResponse(decodeResponse(info.Data, command.ResponseType));
				command.Success();
			}
			else {
				command.Fail(info.Exception);
			}
		}
		
		private void processMessage(WebSocketMessageInfo info) {
			if (info.Exception == null) {
				if (!string.IsNullOrEmpty(info.ServerPushCode) && serverPushHandlers.ContainsKey(info.ServerPushCode)) {
					object push = decodeServerPush(info.Data, serverPushTypes[info.ServerPushCode]);
					
					var handler = serverPushHandlers[info.ServerPushCode];
					handler(push);
				}
			}
			else {
				Debug.Log("<WS> EXCEPTION " + info.Exception.Message);
			}
		}
		
		private void clearSocket() {
			if (socket != null && socket.IsOpen) {
				socket.Close();
			}
			socket = null;
			ConnectionStatus = ConnectionStatus.Disconnected;
		}
		
		protected virtual void handleWebSocketOpen(BestHTTP.WebSocket.WebSocket socket) {
			Debug.Log("<WS> Connection Established!");
			ConnectionStatus = ConnectionStatus.Connected;
			promise.ConnectComplete(null);
		}

		protected virtual void handleWebSocketClosed(BestHTTP.WebSocket.WebSocket webSocket, ushort code, string msg) {
			Debug.LogFormat("<WS> Connection Closed (code:{0}, message:{1})", code, msg);
			clearSocket();
			promise.DisconnectComplete(null);
		}

		protected virtual void handleMessageReceived(BestHTTP.WebSocket.WebSocket socket, string msg) {
			decode(msg);
		}
		
		protected virtual void handleError(BestHTTP.WebSocket.WebSocket socket, Exception error) {
			/*BaseError err = null;
			if (error != null) {
				Debug.LogErrorFormat("Connection Error {0}, Stack: {1}", error.Message, error.StackTrace);
				err = new BaseError("0", error.Message);
			}
			clearSocket();
			Promise.Disconnect(err);*/
		}
		
		protected virtual void handleErrorDescription(BestHTTP.WebSocket.WebSocket error, string description) {
			Debug.LogErrorFormat("Connection Error Description {0}", description);
		}
	}
}