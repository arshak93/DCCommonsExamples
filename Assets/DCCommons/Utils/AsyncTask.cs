using System;

namespace DCCommons.Utils {
	public class AsyncTask<T> {

		public delegate void TaskSuccessDelegate(T result);
		public delegate void TaskFailDelegate(Exception error);

		private TaskSuccessDelegate onSuccess;
		private TaskFailDelegate onFail;

		public AsyncTask<T> OnSuccess(TaskSuccessDelegate onSuccess) {
			this.onSuccess = onSuccess;
			return this;
		}

		public AsyncTask<T> OnFail(TaskFailDelegate onFail) {
			this.onFail = onFail;
			return this;
		}

		public void Success(T result) {
			if (onSuccess != null) {
				onSuccess(result);
			}
		}

		public void Fail(Exception error) {
			if (onFail != null) {
				onFail(error);
			}
		}
	}
}