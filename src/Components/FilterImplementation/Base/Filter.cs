using System;
using System.Collections.Generic;
using System.Linq;
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

		protected IEnumerable<IPin> InputPins
		{
			get
			{
				lock (_pins) 
					return _pins.Where(x => !x.IsOutput).ToArray();
			}
		}

		protected IEnumerable<IPin> OutputPins
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

		public IEnumerable<IFilterProperty> Properties
		{
			get
			{
				lock (_properties)
					return _properties.Values;
			}
			set
			{
				lock (_properties)
				{
					_properties.Clear();
					foreach (var filterProperty in value)
						_properties[filterProperty.Name] = filterProperty;
				}
			}
		}

		public abstract void Process();

		#endregion

		protected void AddPin(IPin pin)
		{
			lock (_pins)
				_pins.Add(pin);
		}

		protected void AddProperty(IFilterProperty property)
		{
			lock (_properties)
				_properties.Add(property.Name, property);
		}
	}
}