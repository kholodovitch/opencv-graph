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

		public MainForm()
		{
			InitializeComponent();

			var graph = new Graph();
			var sourceFileImage = new SourceFileImage();
			sourceFileImage.Properties.First().Value = @"input.png";
			graph.AddFilter(sourceFileImage);
			
			Type first = FiltersHelper.GetFilterTypes().First();
			var filter0 = (Filter)first.Assembly.CreateInstance(BlurName);
			sourceFileImage.OutputPins.First().Connect(filter0.InputPins.First());
			graph.AddFilter(filter0);
			ThreadPool.QueueUserWorkItem(x => filter0.Process());
			
			var filter1 = (Filter)first.Assembly.CreateInstance(BlurName);
			filter0.OutputPins.First().Connect(filter1.InputPins.First());
			graph.AddFilter(filter1);
			ThreadPool.QueueUserWorkItem(x => filter1.Process());

			var filter2 = (Filter)first.Assembly.CreateInstance(BlurName);
			filter1.OutputPins.First().Connect(filter2.InputPins.First());
			graph.AddFilter(filter2);
			ThreadPool.QueueUserWorkItem(x => filter2.Process());

			var destFileImage = new DestFileImage();
			destFileImage.Properties.First().Value = @"output.png";
			filter2.OutputPins.First().Connect(destFileImage.InputPins.First());
			graph.AddFilter(destFileImage);
			GraphLoader.Save(graph, "graph.xml", SaveOptions.AddComments);

			ThreadPool.QueueUserWorkItem(x => sourceFileImage.Process());
			ThreadPool.QueueUserWorkItem(x => destFileImage.Process());
		}
	}
}