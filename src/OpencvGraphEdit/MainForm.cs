using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FilterImplementation;
using FilterImplementation.Base;
using FilterImplementation.Destination;
using FilterImplementation.Serialization;
using FilterImplementation.Source;

namespace OpencvGraphEdit
{
	public partial class MainForm : Form
	{
		const string BlurName = "FilterImplementation.Filters.Image.Blur";
		const string CloneName = "FilterImplementation.Filters.Image.Clone";

		public MainForm()
		{
			InitializeComponent();

			var graph = new Graph();
			var sourceFileImage = new SourceFileImage();
			sourceFileImage.Properties.First().Value = @"input.png";
			graph.AddFilter(sourceFileImage);

			Type first = FiltersHelper.GetFilterTypes().First();
			var clone = (Filter)first.Assembly.CreateInstance(CloneName);
			clone.Properties.First().Value = 1;
			sourceFileImage.OutputPins.First().Connect(clone.InputPins.First());
			graph.AddFilter(clone);
			ThreadPool.QueueUserWorkItem(x => clone.Process());

			var filter0_0 = (Filter)first.Assembly.CreateInstance(BlurName);
			clone.OutputPins.ToArray()[0].Connect(filter0_0.InputPins.First());
			graph.AddFilter(filter0_0);
			ThreadPool.QueueUserWorkItem(x => filter0_0.Process());
			
			var filter0_1 = (Filter)first.Assembly.CreateInstance(BlurName);
			filter0_0.OutputPins.First().Connect(filter0_1.InputPins.First());
			graph.AddFilter(filter0_1);
			ThreadPool.QueueUserWorkItem(x => filter0_1.Process());

			var filter0_2 = (Filter)first.Assembly.CreateInstance(BlurName);
			filter0_1.OutputPins.First().Connect(filter0_2.InputPins.First());
			graph.AddFilter(filter0_2);
			ThreadPool.QueueUserWorkItem(x => filter0_2.Process());

			var filter1 = (Filter)first.Assembly.CreateInstance(BlurName);
			clone.OutputPins.ToArray()[1].Connect(filter1.InputPins.First());
			graph.AddFilter(filter1);
			ThreadPool.QueueUserWorkItem(x => filter1.Process());

			var destFileImage3 = new DestFileImage();
			destFileImage3.Properties.First().Value = @"output3.png";
			filter0_2.OutputPins.First().Connect(destFileImage3.InputPins.First());
			graph.AddFilter(destFileImage3);

			var destFileImage1 = new DestFileImage();
			destFileImage1.Properties.First().Value = @"output1.png";
			filter1.OutputPins.First().Connect(destFileImage1.InputPins.First());
			graph.AddFilter(destFileImage1);

			GraphLoader.Save(graph, "graph.xml", SaveOptions.AddComments);

			ThreadPool.QueueUserWorkItem(x => sourceFileImage.Process());
			ThreadPool.QueueUserWorkItem(x => destFileImage1.Process());
			ThreadPool.QueueUserWorkItem(x => destFileImage3.Process());
		}
	}
}