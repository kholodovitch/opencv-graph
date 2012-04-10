using System;

namespace Visualizer.PropertyEditors
{
	public partial class NumericEditor : BasePropertyEditor
	{
		public NumericEditor()
		{
			InitializeComponent();
		}

		public override object Value
		{
			get { return base.Value; }
			set { base.Value = value; }
		}
	}
}