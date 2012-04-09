using System;
using System.Diagnostics;
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

			var graph = CreateTestGraph();
			foreach (Filter filter in graph.Filters)
			{
				Filter localFilter = filter;
				ThreadPool.QueueUserWorkItem(o => localFilter.Process());
			}

			var graphBundle = new GraphBundle(graph);
			GraphSaver.Save(graphBundle, "graph0.xml", SaveOptions.AddComments);
			//graphBundle = GraphLoader.Load("graph1.xml");
			//GraphSaver.Save(graph, "graph1.xml", SaveOptions.AddComments);
			graphControl1.LoadGraph(graphBundle);
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			GraphSaver.Save(graphControl1.GraphBundle, "graph1.xml", SaveOptions.AddComments);
		}

		private static Graph CreateTestGraph()
		{
			var graph = new Graph();
			var sourceFileImage = new SourceFileImage();
			sourceFileImage.Name = "FileImage";
			sourceFileImage.Properties.Values.First().Value = @"input.png";
			graph.AddFilter(sourceFileImage);

			var assembly = FiltersHelper.GetFilterTypes().First().Assembly;
			var clone = (Filter) assembly.CreateInstance(CloneName);
			Debug.Assert(clone != null);
			clone.Properties.Values.First().Value = 1;
			sourceFileImage.OutputPins.First().Connect(clone.InputPins.First());
			graph.AddFilter(clone);

			var filter0_0 = (Filter) assembly.CreateInstance(BlurName);
			Debug.Assert(filter0_0 != null);
			clone.OutputPins.ToArray()[0].Connect(filter0_0.InputPins.First());
			graph.AddFilter(filter0_0);

			var filter0_1 = (Filter) assembly.CreateInstance(BlurName);
			Debug.Assert(filter0_1 != null);
			filter0_0.OutputPins.First().Connect(filter0_1.InputPins.First());
			graph.AddFilter(filter0_1);

			var filter0_2 = (Filter) assembly.CreateInstance(BlurName);
			Debug.Assert(filter0_2 != null);
			filter0_1.OutputPins.First().Connect(filter0_2.InputPins.First());
			graph.AddFilter(filter0_2);

			var filter1 = (Filter) assembly.CreateInstance(BlurName);
			Debug.Assert(filter1 != null);
			clone.OutputPins.ToArray()[1].Connect(filter1.InputPins.First());
			graph.AddFilter(filter1);

			var destFileImage3 = new DestFileImage();
			destFileImage3.Properties.Values.First().Value = @"output3.png";
			filter0_2.OutputPins.First().Connect(destFileImage3.InputPins.First());
			graph.AddFilter(destFileImage3);

			var destFileImage1 = new DestFileImage();
			destFileImage1.Properties.Values.First().Value = @"output1.png";
			filter1.OutputPins.First().Connect(destFileImage1.InputPins.First());
			graph.AddFilter(destFileImage1);
			return graph;
		}
	}
}