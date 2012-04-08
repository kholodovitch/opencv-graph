using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataStructures;

namespace Visualizer
{
	public partial class GraphControl : UserControl
	{
		private IGraph _graph;
		private Dictionary<Guid, GraphNode> _nodes = new Dictionary<Guid, GraphNode>();
		private Color ColorConnection = Color.FromArgb(192, 192, 192);

		public GraphControl()
		{
			InitializeComponent();
		}

		public void LoadGraph(IGraph graph)
		{
			_nodes.Clear();
			_graph = graph;

			var location = new Point(16, 16);
			foreach (IFilter filter in graph.Filters)
			{
				var graphNode = new GraphNode(filter) {Location = location};
				graphNode.LocationChanged += Node_LocationChanged;
				_nodes[filter.NodeGuid] = graphNode;
				Controls.Add(graphNode);
				location.Offset(new Point(64, 64));
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			foreach (IFilter filter in _graph.Filters)
			{
				var array = filter.Pins.Where(x => x.IsOutput).ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					IPin connectedTo = array[i].ConnectedTo;
					IFilter connectedToFilter = connectedTo.Filter;
					int index = connectedToFilter.Pins.Where(x => !x.IsOutput).ToList().IndexOf(connectedTo);
					GraphNode connectedToNode = _nodes[connectedToFilter.NodeGuid];
					GraphNode node = _nodes[filter.NodeGuid];

					var output = node.GetPinPort(i, true);
					output.Offset(node.Location);
					output.Offset(0, 1);

					var input = connectedToNode.GetPinPort(index, false);
					input.Offset(connectedToNode.Location);
					input.Offset(0, 1);

					e.Graphics.DrawBezier(new Pen(ColorConnection), output.X, output.Y, output.X + 64, output.Y, input.X - 64, input.Y, input.X - 8, input.Y);
					e.Graphics.FillPolygon(new SolidBrush(ColorConnection), new[] { input, new Point(input.X - 8, input.Y - 4), new Point(input.X - 8, input.Y + 4) });
				}
			}
		}

		private void Node_LocationChanged(object sender, System.EventArgs e)
		{
			Invalidate();
		}
	}
}
