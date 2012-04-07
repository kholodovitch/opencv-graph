using System;
using System.Threading;
using DataStructures;

namespace FilterImplementation.Base
{
	public class InputPin : Pin, IInputPin
	{
		private readonly AutoResetEvent _dataReciveEvent = new AutoResetEvent(false);
		private object _data;

		public InputPin(string name, PinMediaType mediaType)
			: base(name, mediaType)
		{
		}

		#region Overrides of Pin

		public override bool IsOutput
		{
			get { return false; }
		}

		#endregion

		#region IInputPin Members

		public object GetData()
		{
			_dataReciveEvent.WaitOne();
			return _data;
		}

		#endregion

		public void SetData(object value)
		{
			_data = value;
			_dataReciveEvent.Set();
		}
	}
}