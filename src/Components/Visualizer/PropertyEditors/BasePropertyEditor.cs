using System;
using System.Windows.Forms;

namespace Visualizer.PropertyEditors
{
	public class BasePropertyEditor : UserControl, IPropertyEditor
	{
		#region Implementation of IPropertyEditor

		public virtual object Value { get; set; }

		public event Action<object> OnValueChanged;

		#endregion

		protected void FireValueChanged(object value)
		{
			if (OnValueChanged != null)
				OnValueChanged(value);
		}
	}
}