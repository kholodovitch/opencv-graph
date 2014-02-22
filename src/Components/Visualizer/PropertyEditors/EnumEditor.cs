using System;
using System.Drawing;
using DataStructures;

namespace Visualizer.PropertyEditors
{
	public partial class EnumEditor : BasePropertyEditor
	{
		private readonly IEnumProperty _property;

		public EnumEditor(IEnumProperty property)
		{
			InitializeComponent();

			_property = property;
			comboBox1.Items.AddRange(_property.Values);
			comboBox1.SelectedIndex = 0;
			comboBox1.SelectedIndexChanged += ComboBox1OnSelectedIndexChanged;
		}

		public override object Value
		{
			get { return comboBox1.SelectedItem; }
			set { comboBox1.SelectedItem = value; }
		}

		private void ComboBox1OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			FireValueChanged(comboBox1.Items[comboBox1.SelectedIndex]);
		}
	}
}
