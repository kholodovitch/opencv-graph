using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataStructures;

namespace Visualizer
{
	internal partial class GraphNode : UserControl
	{
		private const int HeaderHeight = 20;
		private const int PinPortSize = 4;
		private const int FieldHeight = 16;

		private static readonly Color ColorBackground = Color.FromArgb(224, 224, 224);
		private static readonly Color ColorHeaderBackground = Color.FromArgb(208, 208, 208);
		private static readonly Color BorderColor = Color.FromArgb(192, 192, 192);
		private static readonly Font FieldFont = new Font(FontFamily.GenericSansSerif, 8);
		private static readonly Font HeaderFont = new Font(FontFamily.GenericSansSerif, 10);

		private readonly IFilter _filter;
		private ProcessingState _state = ProcessingState.NotStarted;

		public GraphNode(IFilter filter)
		{
			InitializeComponent();

			_filter = filter;
			_filter.OnPinsChanged += OnPinsChanged;
			_filter.OnProcessingProgressChanged += OnProcessingProgressChanged;
			_filter.OnProcessingStateChanged += OnProcessingStateChanged;

			UpdateSize(_filter);
		}

		public IFilter Filter
		{
			get { return _filter; }
		}

		public Point GetPinPort(int index, bool isOutput)
		{
			int fieldY = HeaderHeight + FieldHeight*index;
			int pinPortX = isOutput ? Width - PinPortSize - 1 : 1;
			int pinPortY = fieldY + (FieldHeight - PinPortSize)/2;
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
			e.Graphics.FillRectangle(headerBrush, 0, 0, Width, HeaderHeight);

			var borderPen = new Pen(BorderColor);
			e.Graphics.DrawRectangle(borderPen, new Rectangle(Point.Empty, Size.Subtract(Size, new Size(1, 1))));
			e.Graphics.DrawLine(borderPen, 0, HeaderHeight, Width, HeaderHeight);

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
				int fieldY = HeaderHeight + FieldHeight*i;

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
			var size = new Size(140, FieldHeight*count + HeaderHeight);

			Size = size;
		}
	}
}