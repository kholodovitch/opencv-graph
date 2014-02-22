using System;
using System.Drawing;
using System.Windows.Forms;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.UI;
using FilterImplementation.Base;
using FilterImplementation.FilterProperties;

namespace FilterImplementation.Destination
{
	internal class WinFormsRenderer : Filter
	{
		private readonly Property _nameProperty;
		private readonly Property _sizeProperty;
		private readonly Property _locationProperty;
		private readonly EnumProperty _borderStyleProperty;
		private readonly InputPin _inputPin;
		private readonly IntegerProperty _zoomProperty;
		private ImageViewer _viewer;
		private object _ = new object();

		public WinFormsRenderer()
		{
			_nameProperty = new Property("Name", FilterPropertyType.String) { Value = "Image" };
			AddProperty(_nameProperty);

			_sizeProperty = new PointProperty("Size", FilterPropertyType.Size) { Value = new Size(400, 300) };
			AddProperty(_sizeProperty);

			_locationProperty = new PointProperty("Point", FilterPropertyType.Point) { Value = Point.Empty };
			AddProperty(_locationProperty);

			_borderStyleProperty = new EnumProperty("Border", FilterPropertyType.Enum, Enum.GetNames(typeof(FormBorderStyle))) { Value = FormBorderStyle.Sizable.ToString() };
			AddProperty(_borderStyleProperty);

			_zoomProperty = new IntegerProperty("Zoom", 5, 1000, 5) { Value = 100 };
			AddProperty(_zoomProperty);

			_inputPin = new InputPin("Image", PinMediaType.Image);
			AddPin(_inputPin);
		}

		#region Overrides of Filter

		public override Guid TypeGuid
		{
			get { return new Guid("39EF4F36-2239-4E02-92E2-D65E8B63B396");}
		}

		public override void Process()
		{
			FireProcessingStateChanged(ProcessingState.Started);

			lock(_)
			if (_viewer != null)
				_viewer.Invoke((Action)(() => _viewer.Close()));

			lock (_)
			{
				_viewer = new ImageViewer
					{
						StartPosition = FormStartPosition.Manual,
						FormBorderStyle = (FormBorderStyle)Enum.Parse(typeof(FormBorderStyle), _borderStyleProperty.Value.ToString()),
						Size = (Size) _sizeProperty.Value,
						Location = (Point) _locationProperty.Value,
						Text = _nameProperty.Value as string,
						Image = (IImage) _inputPin.GetData(),
						TopMost = true,
					};
				_viewer.ImageBox.SetZoomScale(_zoomProperty.Value / 100f, Point.Empty);
			}
			Application.Run(_viewer);
			_viewer = null;

			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}
