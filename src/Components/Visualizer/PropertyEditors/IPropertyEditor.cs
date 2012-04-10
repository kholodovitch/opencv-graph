using System;

namespace Visualizer.PropertyEditors
{
	public interface IPropertyEditor
	{
		object Value { get; set; }

		event Action<object> OnValueChanged;
	}
}