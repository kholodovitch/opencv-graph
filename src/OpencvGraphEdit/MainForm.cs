using System;
using System.Linq;
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
		public MainForm()
		{
			InitializeComponent();

			var graph = new Graph();
			var sourceFileImage = new SourceFileImage();
			sourceFileImage.Properties.First().Value = @"input.png";
			graph.AddFilter(sourceFileImage);
			
			Type first = FiltersHelper.GetFilterTypes().First();
			var filter = (Filter)first.Assembly.CreateInstance(first.FullName);
			graph.AddFilter(filter);

			var destFileImage = new DestFileImage();
			destFileImage.Properties.First().Value = @"output.png";
			graph.AddFilter(destFileImage);
			GraphLoader.Save(graph, "graph.xml", SaveOptions.AddComments);
		}
	}
}