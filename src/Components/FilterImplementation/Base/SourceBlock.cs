﻿using System;

namespace FilterImplementation.Base
{
	[Serializable]
	public abstract class SourceBlock : Filter
	{
		public abstract void Start();
	}
}
