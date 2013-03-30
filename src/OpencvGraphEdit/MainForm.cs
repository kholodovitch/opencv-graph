using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DataStructures;
using FilterImplementation;
using FilterImplementation.Base;
using FilterImplementation.Serialization;
using FilterImplementation.Source;

namespace OpencvGraphEdit
{
	public partial class MainForm : Form
	{
		private const string PathToXml = @"..\..\sample_graph.xml";

		public MainForm()
		{
			InitializeComponent();


			this.treeView1.AllowDrop = true;
			this.graphControl1.AllowDrop = true;
			this.treeView1.MouseDown += new MouseEventHandler(listBox1_MouseDown);
			this.treeView1.DragOver += new DragEventHandler(listBox1_DragOver);

			this.graphControl1.DragEnter += new DragEventHandler(treeView1_DragEnter);
			this.graphControl1.DragDrop += new DragEventHandler(treeView1_DragDrop);

			IDictionary<Guid, Type> filterTypes = FiltersHelper.GetFilterTypes();
			UpdateTreeView(filterTypes);

			GraphBundle graphBundle = GraphLoader.Load(PathToXml);
			graphControl1.LoadGraph(graphBundle);
		}

		private void treeView1_DragDrop(object sender, DragEventArgs e)
		{
			object typeGuid = e.Data.GetData(typeof(Guid));
			if (typeGuid == null) 
				return;
			
			var filterType = FiltersHelper.GetFilterType((Guid)typeGuid);
			var filter = (IFilter)filterType.Assembly.CreateInstance(filterType.FullName);
			graphControl1.AddFilter(filter);
		}

		private void listBox1_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		private void treeView1_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		private void listBox1_MouseDown(object sender, MouseEventArgs e)
		{
			TreeNode newNode = treeView1.GetNodeAt(e.Location);
			if (treeView1.SelectedNode != newNode)
				treeView1.SelectedNode = newNode;
			
			object tag = treeView1.SelectedNode.Tag;
			if (tag is Guid)
				treeView1.DoDragDrop(((Guid) tag), DragDropEffects.Move);
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