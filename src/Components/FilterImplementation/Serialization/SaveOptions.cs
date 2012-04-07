using System;

namespace FilterImplementation.Serialization
{
	[Flags]
	public enum SaveOptions
	{
		Default = 0x0000,
		AddComments = 0x0001,
	}
}