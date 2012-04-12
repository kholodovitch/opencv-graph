using System;
using System.Drawing;
using System.Windows.Forms;
using DataStructures;
using DataStructures.Enums;
using Emgu.CV;
using Emgu.CV.UI;
using FilterImplementation.Base;

namespace FilterImplementation.Destination
{
	internal class WinFormsRenderer : Filter
	{
		private readonly Property _nameProperty;
		private readonly InputPin _inputPin;
		private ImageViewer _viewer;
		private object _ = new object();

		public WinFormsRenderer()
		{
			_nameProperty = new Property("Name", FilterPropertyType.String);
			AddProperty(_nameProperty);

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
				_viewer = new ImageViewer {Size = new Size(800, 600)};
				_viewer.Name = _nameProperty.Value as string;
				_viewer.Image = (IImage) _inputPin.GetData();
			}
			Application.Run(_viewer);
			_viewer = null;

			FireProcessingStateChanged(ProcessingState.Finished);
		}

		#endregion
	}
}
