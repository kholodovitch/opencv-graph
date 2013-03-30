using System;
using System.ComponentModel;
using System.Drawing;
using DataStructures;
using DataStructures.Enums;

namespace Visualizer.PropertyEditors
{
	public partial class PointEditor : BasePropertyEditor
	{
		private readonly IPointProperty _property;

		public PointEditor(IPointProperty property)
		{
			InitializeComponent();

			_property = property;

			numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
			numericUpDown2.ValueChanged += numericUpDown1_ValueChanged;
		}

		public override object Value
		{
			get { return numericUpDown1.Value; }
			set
			{
				if (value is Size)
				{
					if (_property.Type != FilterPropertyType.Size)
						throw new InvalidCastException();

					numericUpDown1.Value = ((Size) value).Width;
					numericUpDown2.Value = ((Size) value).Height;
				}
				else if (value is Point)
				{
					if (_property.Type != FilterPropertyType.Point)
						throw new InvalidCastException();

					numericUpDown1.Value = ((Point) value).X;
					numericUpDown2.Value = ((Point) value).Y;
				}
				else
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			switch (_property.Type)
			{
				case FilterPropertyType.Point:
					FireValueChanged(new Point((int) numericUpDown1.Value, (int) numericUpDown2.Value));
					break;

				case FilterPropertyType.Size:
					FireValueChanged(new Size((int) numericUpDown1.Value, (int) numericUpDown2.Value));
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}