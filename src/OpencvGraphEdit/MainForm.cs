using System;
using System.Collections.Generic;
using System.Drawing;
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

			IDictionary<Guid, Type> filterTypes = FiltersHelper.GetFilterTypes();
			UpdateTreeView(filterTypes);

			GraphBundle graphBundle = GraphLoader.Load(PathToXml);
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

		private void UpdateTreeView(IEnumerable<KeyValuePair<Guid, Type>> filterTypes)
		{
			foreach (var typePath in filterTypes.Select(x => new { TypeId = x.Key, x.Value.FullName }))
			{
				TreeNode node = typePath.FullName.Substring("FilterImplementation.".Length).Split('.')
					.Aggregate<string, TreeNode>(null, (current, pathBits) => AddNode(pathBits, current != null ? current.Nodes : treeView1.Nodes));
				if (node == null)
					continue;

				node.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
				node.Tag = typePath.TypeId;
			}
			treeView1.ExpandAll();
		}

		private TreeNode AddNode(string key, TreeNodeCollection nodeCollection)
		{
			return nodeCollection.ContainsKey(key) ? nodeCollection[key] : nodeCollection.Add(key, key);
		}
	}
}