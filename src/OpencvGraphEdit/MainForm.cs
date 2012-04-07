using System;
using System.Linq;
using System.Windows.Forms;
using DataStructures;
using FilterImplementation;
using FilterImplementation.Base;

namespace OpencvGraphEdit
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			var graph = new Graph();
			Type name = FiltersHelper.GetFilterTypes().First();
			var y = (Filter) name.Assembly.CreateInstance(name.FullName);
			graph.Filters.Add(y);
			GraphLoader.Save(graph, "graph.xml");
		}
	}
}