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
			Type name = FiltersHelper.GetFilterTypes().First();
			var y = (Filter)name.Assembly.CreateInstance(name.FullName);
			graph.AddFilter(new SourceFileImage());
			graph.AddFilter(y);
			graph.AddFilter(new DestFileImage());
			GraphLoader.Save(graph, "graph.xml", SaveOptions.AddComments);
		}
	}
}