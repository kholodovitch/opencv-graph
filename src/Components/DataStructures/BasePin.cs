using System;
using System.Threading;

namespace DataStructures
{
	public abstract class BasePin<T> : IPin<T>
	{
		private readonly AutoResetEvent _dataReciveEvent = new AutoResetEvent(false);
		private T _data;
		private IPin _connectedTo;

		#region IPin<T> Members

		public abstract string Name { get; }

		public abstract PinMediaType MediaType { get; }

		public bool IsConnected
		{
			get { return _connectedTo != null; }
		}

		public IPin ConnectedTo
		{
			get { return _connectedTo; }
			private set { _connectedTo = value; }
		}

		public abstract bool IsOutput { get; }

		public void Connect(IPin receivePin)
		{
			_connectedTo = receivePin;
			receivePin.ReceiveConnection(this);
		}

		public void ReceiveConnection(IPin receivePin)
		{
			_connectedTo = receivePin;
		}

		public void Disconnect()
		{
			if (_connectedTo == null) 
				return;

			_connectedTo.Disconnect();
			_connectedTo = null;
		}

		public T GetData()
		{
			if (IsOutput)
				throw new Exception();

			_dataReciveEvent.WaitOne();
			return _data;
		}

		public void SetData(T value)
		{
			if (!IsOutput)
				throw new Exception();

			((BasePin<T>)_connectedTo)._data = value;
			((BasePin<T>)_connectedTo)._dataReciveEvent.Set();
		}

		#endregion
	}
}