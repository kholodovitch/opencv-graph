namespace DataStructures
{
	public interface IPin
	{
		PinMediaType MediaType { get; }

		bool IsConnected { get; }

		bool IsOutput { get; }
	}
}