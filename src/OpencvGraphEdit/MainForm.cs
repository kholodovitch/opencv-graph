using System;
using System.Threading;
using System.Windows.Forms;
using FilterImplementation.Base;
using FilterImplementation.Serialization;

namespace OpencvGraphEdit
{
	public partial class MainForm : Form
	{
		private const string PathToXml = @"..\..\sample_graph.xml";

		public MainForm()
		{
			InitializeComponent();

			var graphBundle = GraphLoader.Load(PathToXml);
			graphControl1.LoadGraph(graphBundle);
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			GraphSaver.Save(graphControl1.GraphBundle, PathToXml, SaveOptions.AddComments);
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			foreach (Filter filter in graphControl1.GraphBundle.Graph.Filters)
			{
				Filter localFilter = filter;
				ThreadPool.QueueUserWorkItem(o => localFilter.Process());
			}
		}
	}
}