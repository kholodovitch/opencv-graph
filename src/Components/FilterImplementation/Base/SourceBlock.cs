using System;
using DataStructures;

namespace FilterImplementation.Base
{
	[Serializable]
	public abstract class SourceBlock : IFilter
	{
		public abstract void Start();
	}
}
