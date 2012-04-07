using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;

namespace FilterImplementation.Base
{
	[Serializable]
	public abstract class Filter : IFilter
	{
		private readonly List<IPin> _pins = new List<IPin>();

		private readonly List<IFilterProperty> _properties = new List<IFilterProperty>();

		protected Filter()
		{
			NodeGuid = Guid.NewGuid();
		}

		protected IEnumerable<IPin> InputPins
		{
			get { return _pins.Where(x => !x.IsOutput).ToArray(); }
		}

		protected IEnumerable<IPin> OutputPins
		{
			get { return _pins.Where(x => x.IsOutput).ToArray(); }
		}

		#region IFilter Members

		public Guid NodeGuid { get; set; }

		public abstract Guid TypeGuid { get; }

		public string Name { get; set; }

		public abstract void Process();

		

		#endregion

		protected void AddPin(IPin pin)
		{
			_pins.Add(pin);
		}
	}
}