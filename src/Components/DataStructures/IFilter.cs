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

		void Reset();

		event PinsChangedHandler OnPinsChanged;

		event ProcessingProgressChangedHandler OnProcessingProgressChanged;

		event ProcessingStateChangedHandler OnProcessingStateChanged;
	}

	public delegate void ProcessingStateChangedHandler(IFilter sender, ProcessingState state);

	public enum ProcessingState
	{
		Started,
		Finished,
		NotStarted
	}

	public delegate void ProcessingProgressChangedHandler(IFilter sender, double progress);

	public delegate void PinsChangedHandler(IFilter sender, IPin args, PinChangedAction action);

	public enum PinChangedAction
	{
		Added,
		Removed,
	}
}