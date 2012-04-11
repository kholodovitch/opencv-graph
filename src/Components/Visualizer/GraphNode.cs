using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataStructures;
using DataStructures.Enums;
using Visualizer.PropertyEditors;

namespace Visualizer
{
	internal partial class GraphNode : UserControl
	{
		private const int HeaderHeight = 20;
		internal const int PinPortSize = 4;
		private const int FieldHeight = 16;
		private const int PropertyHeight = 18;

		private static readonly Color ColorBackground = Color.FromArgb(224, 224, 224);
		private static readonly Color ColorHeaderBackground = Color.FromArgb(208, 208, 208);
		private static readonly Color BorderColor = Color.FromArgb(192, 192, 192);
		private static readonly Font FieldFont = new Font(FontFamily.GenericSansSerif, 8);
		private static readonly Font HeaderFont = new Font(FontFamily.GenericSansSerif, 10);

		private readonly IFilter _filter;
		private double _progress;
		private ProcessingState _state = ProcessingState.NotStarted;

		public GraphNode(IFilter filter)
		{
			InitializeComponent();

			_filter = filter;
			_filter.OnPinsChanged += OnPinsChanged;
			_filter.OnProcessingProgressChanged += OnProcessingProgressChanged;
			_filter.OnProcessingStateChanged += OnProcessingStateChanged;

			UpdateSize(_filter);

			var table = new TableLayoutPanel
			            	{
			            		Size = new Size(Width - 2, _filter.Properties.Count*PropertyHeight),
			            		Location = new Point(1, HeaderHeight + 2)
			            	};
			table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			
			for (int i = 0; i < _filter.Properties.Count; i++)
			{
				IFilterProperty property = _filter.Properties.Skip(i).First().Value;
				BasePropertyEditor editor;
				switch (property.Type)
				{
					case FilterPropertyType.String:
						editor = new PathEditor {Value = property.Value};
						break;

					case FilterPropertyType.Float:
					case FilterPropertyType.Integer:
						editor = new NumericEditor((INumericProperty)property) { Value = Convert.ToDecimal(property.Value) };
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}

				editor.Dock = DockStyle.Fill;
				editor.Margin = new Padding(0);
				editor.OnValueChanged += newValue => { property.Value = newValue; };

				table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				table.SetRow(editor, i);
				table.SetColumn(editor, 1);

				var label = new Label
				            	{
				            		Text = property.Name, 
									Margin = new Padding(0),
									Dock = DockStyle.Right,
									AutoSize = true,
									Anchor = AnchorStyles.Right
				            	};
				table.SetRow(label, i);
				table.SetColumn(label, 0);
				table.Controls.Add(label);
				table.Controls.Add(editor);
			}
			Controls.Add(table);
		}

		public IFilter Filter
		{
			get { return _filter; }
		}

		public Point GetPinPort(int index, bool isOutput, PinPointOptions options = PinPointOptions.None)
		{
			int fieldY = HeaderHeight + _filter.Properties.Count*PropertyHeight + FieldHeight*index + 3;
			int pinPortX = isOutput ? Width - PinPortSize - 1 : 1;
			int pinPortY = fieldY + (FieldHeight - PinPortSize)/2;
			if ((options & PinPointOptions.ToCenter) == PinPointOptions.ToCenter)
			{
				pinPortX += PinPortSize/2;
				pinPortY += PinPortSize/2;
			}
			if ((options & PinPointOptions.Absolute) == PinPointOptions.Absolute)
			{
				pinPortX += Location.X;
				pinPortY += Location.Y;
			}

			return new Point(pinPortX, pinPortY);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.Clear(ColorBackground);
			SolidBrush headerBrush;

			switch (_state)
			{
				case ProcessingState.Started:
					headerBrush = new SolidBrush(Color.FromArgb(128, 128, 255));
					break;
				case ProcessingState.Finished:
					headerBrush = new SolidBrush(Color.FromArgb(128, 255, 128));
					break;
				case ProcessingState.NotStarted:
					headerBrush = new SolidBrush(ColorHeaderBackground);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (_state != ProcessingState.Started)
			{
				e.Graphics.FillRectangle(headerBrush, 0, 1, Width, HeaderHeight);
			}
			else
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 128, 255)), 0, 1, Width, HeaderHeight);
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 255, 128)), 0, 1, (int) (Width*_progress), HeaderHeight);
			}

			var borderPen = new Pen(BorderColor);
			e.Graphics.DrawRectangle(borderPen, new Rectangle(Point.Empty, Size.Subtract(Size, new Size(1, 1))));
			e.Graphics.DrawLine(borderPen, 0, HeaderHeight + 1, Width, HeaderHeight + 1);
			if (_filter.Properties.Count > 0)
			{
				int y1 = HeaderHeight + _filter.Properties.Count*PropertyHeight + 2;
				e.Graphics.DrawLine(borderPen, 0, y1, Width, y1);
			}

			string header = !string.IsNullOrEmpty(_filter.Name) ? _filter.Name : _filter.GetType().Name;
			e.Graphics.DrawString(header, HeaderFont, Brushes.Black, 4, 3);

			DrawPinPorts(e.Graphics, _filter.Pins.Where(x => x.IsOutput).ToArray());
			DrawPinPorts(e.Graphics, _filter.Pins.Where(x => !x.IsOutput).ToArray());
		}

		private void OnPinsChanged(IFilter sender, IPin args, PinChangedAction action)
		{
			UpdateSize(sender);
		}

		private void OnProcessingProgressChanged(IFilter sender, double progress)
		{
			_progress = progress;
			Invalidate();
		}

		private void OnProcessingStateChanged(IFilter sender, ProcessingState state)
		{
			_state = state;
			Invalidate();
		}

		private void DrawPinPorts(Graphics graphics, IPin[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				IPin pin = array[i];
				Point pinPortLocation = GetPinPort(i, pin.IsOutput);
				int fieldY = HeaderHeight + _filter.Properties.Count*PropertyHeight + FieldHeight*i + 3;

				SizeF nameSize = graphics.MeasureString(pin.Name, FieldFont);
				float fieldX = pin.IsOutput ? pinPortLocation.X - nameSize.Width - 2 : pinPortLocation.X + PinPortSize + 2;

				graphics.FillRectangle(new SolidBrush(BorderColor), new Rectangle(pinPortLocation.X, pinPortLocation.Y, PinPortSize, PinPortSize));
				graphics.DrawString(pin.Name, FieldFont, Brushes.Black, fieldX, fieldY + 1);
			}
		}

		private void UpdateSize(IFilter sender)
		{
			IPin[] pins = sender.Pins.ToArray();
			int count = Math.Max(pins.Count(x => x.IsOutput), pins.Count(x => !x.IsOutput));
			var size = new Size(140, FieldHeight*count + _filter.Properties.Count*PropertyHeight + HeaderHeight + 4);

			Size = size;
		}

		#region Nested type: PinPointOptions

		[Flags]
		internal enum PinPointOptions
		{
			None,
			ToCenter,
			Absolute
		}

		#endregion
	}
}