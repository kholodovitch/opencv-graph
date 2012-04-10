using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FilterImplementation;
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

			var t= FiltersHelper.GetFilterTypes()
				.Select(x => new {TypeId = x.Key, x.Value.FullName});

			TreeNode lastNode = null;
			string subPathAgg;
			foreach (var path in t)
			{
				subPathAgg = string.Empty;
				foreach (string subPath in path.FullName.Split('.'))
				{
					subPathAgg += subPath + '.';
					TreeNode[] nodes = treeView1.Nodes.Find(subPathAgg, true);
					if (nodes.Length == 0)
						if (lastNode == null)
							lastNode = treeView1.Nodes.Add(subPathAgg, subPath);
						else
							lastNode = lastNode.Nodes.Add(subPathAgg, subPath);
					else
						lastNode = nodes[0];
				}
				if (lastNode != null) 
					lastNode.Tag = path.TypeId;
			}


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