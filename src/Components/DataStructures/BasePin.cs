using System;

namespace DataStructures
{
	public abstract class BasePin : IPin
	{
		#region IPin Members

		public abstract PinMediaType MediaType { get; }

		public bool IsConnected
		{
			get { throw new NotImplementedException(); }
		}

		public abstract bool IsOutput { get; }

		#endregion
	}
}