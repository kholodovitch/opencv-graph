namespace DataStructures
{
	public interface IPin<T> : IPin
	{
		T GetData();

		void SetData(T value);
	}

	public interface IPin
	{
		PinMediaType MediaType { get; }

		bool IsConnected { get; }

		bool IsOutput { get; }
	}
}