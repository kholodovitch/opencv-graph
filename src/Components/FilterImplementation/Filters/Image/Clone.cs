using System;
using System.Collections.Generic;
using DataStructures;
using Emgu.CV;
using FilterImplementation.Base;

namespace FilterImplementation.Filters.Image
{
	internal class Clone : Filter
	{
		private readonly InputPin _input;
		private readonly OutputPin _originalOutput;
		private readonly List<OutputPin> _outputs = new List<OutputPin>();
		private readonly Property _countProperty;

		public Clone()
		{
			_countProperty = new Property("Count of copies", FilterPropertyType.Integer);
			_countProperty.OnValueChanged += OnChangedCountOfCopies;
			AddProperty(_countProperty);

			_input = new InputPin("Input", PinMediaType.Image);
			AddPin(_input);
			_originalOutput = new OutputPin("Original", PinMediaType.Image);
			AddPin(_originalOutput);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("27A97FFE-065B-4684-9035-80793463EB7B"); }
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);
			var frame = (IImage)_input.GetData();
			lock (_outputs)
			{
				for (int index = 0; index < _outputs.Count; index++)
				{
					OutputPin outputPin = _outputs[index];
					object clone = frame.Clone();
					outputPin.SetData(clone);
					FireProcessingProgressChanged((index + 1)/(double) _outputs.Count);
				}
			}
			_originalOutput.SetData(frame);
			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion

		private void OnChangedCountOfCopies(object obj)
		{
			if (!(obj is int)) 
				return;

			int copies = (int)obj;
			if (_outputs.Count == copies)
				return;

			lock (_outputs)
			{
				foreach (IPin outputPin in _outputs)
				{
					outputPin.Disconnect();
					RemovePin(outputPin);
				}
				_outputs.Clear();

				for (uint i = 0; i < copies; i++)
				{
					var outputPin = new OutputPin("Copy № " + (i + 1), PinMediaType.Image);
					_outputs.Add(outputPin);
					AddPin(outputPin);
				}
			}
		}
	}
}