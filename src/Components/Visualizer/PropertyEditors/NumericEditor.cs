using System;
using System.ComponentModel;
using DataStructures;

namespace Visualizer.PropertyEditors
{
	public partial class NumericEditor : BasePropertyEditor
	{
		public NumericEditor(INumericProperty property)
		{
			InitializeComponent();

			((ISupportInitialize)numericUpDown1).BeginInit();
			numericUpDown1.Maximum = property.Max;
			numericUpDown1.Minimum = property.Min;
			numericUpDown1.Value = property.Min;
			numericUpDown1.DecimalPlaces = property.DecimalPlaces;
			numericUpDown1.Increment = property.Step;
			((ISupportInitialize)numericUpDown1).EndInit();

			numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
		}

		public override object Value
		{
			get { return numericUpDown1.Value; }
			set { numericUpDown1.Value = (decimal) value; }
		}

		void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
		{
			FireValueChanged(Convert.ToInt32(numericUpDown1.Value));
		}
	}
}