using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using DataStructures;

namespace FilterImplementation.Base
{
	public abstract class Filter : IFilter
	{
		private readonly List<IPin> _pins = new List<IPin>();

		private readonly Dictionary<string, IFilterProperty> _properties = new Dictionary<string, IFilterProperty>();

		protected Filter()
		{
			NodeGuid = Guid.NewGuid();
		}

		public IEnumerable<IPin> InputPins
		{
			get
			{
				lock (_pins)
					return _pins.Where(x => !x.IsOutput).ToArray();
			}
		}

		public IEnumerable<IPin> OutputPins
		{
			get
			{
				lock (_pins)
					return _pins.Where(x => x.IsOutput).ToArray();
			}
		}

		#region IFilter Members

		public Guid NodeGuid { get; set; }

		public abstract Guid TypeGuid { get; }

		public string Name { get; set; }

		public IEnumerable<IPin> Pins
		{
			get
			{
				lock (_pins)
					return _pins.AsReadOnly();
			}
		}

		public IDictionary<string, IFilterProperty> Properties
		{
			get
			{
				lock (_properties)
					return _properties.AsReadOnly();
			}
		}

		public abstract void Process();

		public event PinsChangedHandler OnPinsChanged = (sender, args, action) => { };
		public event ProcessingProgressChangedHandler OnProcessingProgressChanged = (sender, progress) => { }; 
		public event ProcessingStateChangedHandler OnProcessingStateChanged = (sender, state) => { };

		#endregion

		protected void AddPin(IPin pin)
		{
			lock (_pins)
			{
				_pins.Add(pin);
				pin.Filter = this;
				OnPinsChanged(this, pin, PinChangedAction.Added);
			}
		}

		protected void RemovePin(IPin pin)
		{
			lock (_pins)
			{
				_pins.Remove(pin);
				pin.Filter = null;
				OnPinsChanged(this, pin, PinChangedAction.Removed);
			}
		}

		protected void AddProperty(IFilterProperty property)
		{
			lock (_properties)
				_properties.Add(property.Name, property);
		}

		protected void FireProcessingProgressChanged(double progress)
		{
			OnProcessingProgressChanged(this, progress);
		}

		protected void FireProcessingStateChanged(ProcessingState state)
		{
			OnProcessingStateChanged(this, state);
		}
	}
}