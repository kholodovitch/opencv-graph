using System;
using DataStructures;
using DataStructures.Enums;

namespace FilterImplementation.Base
{
	public abstract class Pin : IPin
	{
		private readonly PinMediaType _mediaType;
		private readonly string _name;
		private IPin _connectedTo;

		public Pin(string name, PinMediaType mediaType)
		{
			_name = name;
			_mediaType = mediaType;
		}

		#region Implementation of IPin

		public string Name
		{
			get { return _name; }
		}

		public IFilter Filter { get; set; }

		public PinMediaType MediaType
		{
			get { return _mediaType; }
		}

		public IPin ConnectedTo
		{
			get { return _connectedTo; }
			private set
			{
				_connectedTo = value;
				OnConnected(_connectedTo);
			}
		}

		public abstract bool IsOutput { get; }

		public bool IsConnected
		{
			get { return ConnectedTo != null; }
		}

		public void Connect(IPin receivePin)
		{
			ConnectedTo = receivePin;
			receivePin.ReceiveConnection(this);
		}

		public void ReceiveConnection(IPin receivePin)
		{
			ConnectedTo = receivePin;
		}

		public void Disconnect()
		{
			if (ConnectedTo == null)
				return;

			ConnectedTo.Disconnect();
			ConnectedTo = null;
		}

		#endregion

		public event Action<IPin> OnConnected = pin => { };
	}
}