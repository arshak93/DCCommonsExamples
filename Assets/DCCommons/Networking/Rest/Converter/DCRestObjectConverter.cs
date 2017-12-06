namespace DCCommons.Networking.Rest.Converter {
	public interface DCRestObjectConverter {
		T ToObject<T>(string data);
		string ToString(object obj);
		byte[] ToRawData(object obj);
	}
}