using System;

namespace Visualizer.PropertyEditors
{
	public interface IPropertyEditor
	{
		event Action<object> OnValueChanged;
	}
}