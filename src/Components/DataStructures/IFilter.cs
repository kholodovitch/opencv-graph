using System;
using System.Collections.Generic;

namespace DataStructures
{
	public interface IFilter
	{
		Guid NodeGuid { get; set; }

		Guid TypeGuid { get; }

		string Name { get; set; }

		IEnumerable<IPin> Pins { get; }

		IDictionary<string, IFilterProperty> Properties { get; }

		void Process();

		event PinsChangedHandler OnPinsChanged;
	}

	public delegate void PinsChangedHandler(IFilter sender, IPin args, PinChangedAction action);

	public enum PinChangedAction
	{
		Added,
		Removed,
	}
}