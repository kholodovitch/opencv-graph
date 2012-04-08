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

		public GraphNode(IFilter filter)
		{
			InitializeComponent();

			_filter = filter;
			_filter.OnPinsChanged += OnPinsChanged;

			UpdateSize(_filter);
		}

		private void OnPinsChanged(IFilter sender, IPin args, PinChangedAction action)
		{
			UpdateSize(sender);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.Clear(ColorBackground);
			e.Graphics.FillRectangle(new SolidBrush(ColorHeaderBackground), 0, 0, Width, HeaderHeight);
			
			var borderPen = new Pen(BorderColor);
			e.Graphics.DrawRectangle(borderPen, new Rectangle(Point.Empty, Size.Subtract(Size, new Size(1, 1))));
			e.Graphics.DrawString(_filter.Name, HeaderFont, Brushes.Black, 4, 3);
			e.Graphics.DrawLine(borderPen, 0, HeaderHeight, Width, HeaderHeight);

			var array = _filter.Pins.Where(x => x.IsOutput).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				var pinPortX = Width - PinPortSize - 1;
				var fieldY = HeaderHeight + FieldHeight * i;

				e.Graphics.FillRectangle(new SolidBrush(BorderColor), new Rectangle(pinPortX, fieldY + (FieldHeight - PinPortSize) / 2, PinPortSize, PinPortSize));

				var y = e.Graphics.MeasureString(array[i].Name, FieldFont);
				e.Graphics.DrawString(array[i].Name, FieldFont, Brushes.Black, pinPortX - y.Width - 2, fieldY + 1);
			}
		}

		private void UpdateSize(IFilter sender)
		{
			var pins = sender.Pins.ToArray();
			var count = Math.Max(pins.Count(x => x.IsOutput), pins.Count(x => !x.IsOutput));
			var size = new Size(140, FieldHeight * count + HeaderHeight);

			Size = size;
		}
	}
}
