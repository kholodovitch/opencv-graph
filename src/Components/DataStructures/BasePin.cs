using System;

namespace DataStructures
{
	public abstract class BasePin<T> : IPin<T>
	{
		private T _data;

		#region IPin<T> Members

		public abstract string Name { get; }

		public abstract PinMediaType MediaType { get; }

		public bool IsConnected
		{
			get { throw new NotImplementedException(); }
		}

		public abstract bool IsOutput { get; }

		public T GetData()
		{
			return _data;
		}

		public void SetData(T value)
		{
			_data = value;
		}

		#endregion
	}
}