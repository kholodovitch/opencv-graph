namespace DataStructures
{
	public interface IPin
	{
		string Name { get; }

		PinMediaType MediaType { get; }

		bool IsConnected { get; }

		IPin ConnectedTo { get; }

		bool IsOutput { get; }

		void Connect(IPin receivePin);

		void ReceiveConnection(IPin receivePin);

		void Disconnect();
	}

	public interface IInputPin : IPin
	{
		object GetData();
	}

	public interface IOutputPin : IPin
	{
		void SetData(object value);
	}
}