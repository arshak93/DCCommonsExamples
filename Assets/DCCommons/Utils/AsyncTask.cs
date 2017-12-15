using System;

namespace DCCommons.Utils {
	
	public class AsyncTask {

		public delegate void TaskSuccessDelegate();
		public delegate void TaskFailDelegate(Exception error);
		public delegate void TaskCompleteDelegate(Exception error);

		private TaskSuccessDelegate onSuccess;
		private TaskFailDelegate onFail;
		private TaskCompleteDelegate onComplete;

		public AsyncTask OnSuccess(TaskSuccessDelegate onSuccess) {
			this.onSuccess = onSuccess;
			return this;
		}

		public AsyncTask OnFail(TaskFailDelegate onFail) {
			this.onFail = onFail;
			return this;
		}
		
		public AsyncTask OnComplete(TaskCompleteDelegate onComplete) {
			this.onComplete = onComplete;
			return this;
		}

		public void Success() {
			if (onComplete != null) {
				onComplete(null);
			}
			if (onSuccess != null) {
				onSuccess();
			}
		}

		public void Fail(Exception error) {
			if (onComplete != null) {
				onComplete(error);
			}
			if (onFail != null) {
				onFail(error);
			}
		}
	}
	
	public class AsyncTask<T> {

		public delegate void TaskSuccessDelegate(T result);
		public delegate void TaskFailDelegate(Exception error);
		public delegate void TaskCompleteDelegate(T result, Exception error);

		private TaskSuccessDelegate onSuccess;
		private TaskFailDelegate onFail;
		private TaskCompleteDelegate onComplete;

		public AsyncTask<T> OnSuccess(TaskSuccessDelegate onSuccess) {
			this.onSuccess = onSuccess;
			return this;
		}

		public AsyncTask<T> OnFail(TaskFailDelegate onFail) {
			this.onFail = onFail;
			return this;
		}
		
		public AsyncTask<T> OnComplete(TaskCompleteDelegate onComplete) {
			this.onComplete = onComplete;
			return this;
		}

		public void Success(T result) {
			if (onComplete != null) {
				onComplete(result, null);
			}
			if (onSuccess != null) {
				onSuccess(result);
			}
		}

		public void Fail(Exception error) {
			if (onComplete != null) {
				onComplete(default(T), error);
			}
			if (onFail != null) {
				onFail(error);
			}
		}
	}
}