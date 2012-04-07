/*
using DataStructures;

namespace FilterImplementation.Base
{
	public class Pin : IPin
	{
		private readonly bool _isOutput;
		private readonly PinMediaType _mediaType;
		private readonly string _name;
		private bool _isConnected;

		public Pin(string name, PinMediaType mediaType, bool isOutput)
		{
			_name = name;
			_mediaType = mediaType;
			_isOutput = isOutput;
		}

		#region Implementation of IPin

		public string Name
		{
			get { return _name; }
		}

		public PinMediaType MediaType
		{
			get { return _mediaType; }
		}

		public bool IsConnected
		{
			get { return _isConnected; }
		}

		public bool IsOutput
		{
			get { return _isOutput; }
		}

		#endregion
	}
}
*/