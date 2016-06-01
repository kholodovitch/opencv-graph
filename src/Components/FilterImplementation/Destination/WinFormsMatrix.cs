using System;
using FilterImplementation.Base;

namespace FilterImplementation.Destination
{
	internal class WinFormsMatrix : Filter
	{
		public override Guid TypeGuid
		{
			get { return new Guid("ECF4A9F5-3C92-4A4D-8D1D-56DF184A5C8E"); }
		}

		public override void Process()
		{
			throw new NotImplementedException();
		}
	}
}
