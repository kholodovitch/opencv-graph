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

		public IDictionary<string, IFilterProperty> Properties
		{
			get
			{
				lock (_properties)
					return _properties.AsReadOnly();
			}
		}

		public abstract void Process();

		#endregion

		protected void AddPin(IPin pin)
		{
			lock (_pins)
				_pins.Add(pin);
		}

		protected void RemovePin(IPin pin)
		{
			lock (_pins)
				_pins.Remove(pin);
		}

		protected void AddProperty(IFilterProperty property)
		{
			lock (_properties)
				_properties.Add(property.Name, property);
		}
	}
}