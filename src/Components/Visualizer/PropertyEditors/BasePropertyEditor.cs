using System;
using System.Windows.Forms;

namespace Visualizer.PropertyEditors
{
	public class BasePropertyEditor : UserControl, IPropertyEditor
	{
		#region Implementation of IPropertyEditor

		public event Action<object> OnValueChanged;

		#endregion

		protected void FireValueChanged(object value)
		{
			if (OnValueChanged != null)
				OnValueChanged(value);
		}
	}
}