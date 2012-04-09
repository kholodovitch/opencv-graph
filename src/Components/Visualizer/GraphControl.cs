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
		private const int ArrowSize = 8;
		private int x = 0;
		private int y = 0;

		private IGraphBundle _bundle;
		private Dictionary<Guid, GraphNode> _nodes = new Dictionary<Guid, GraphNode>();
		private readonly Color ColorConnection = Color.FromArgb(192, 192, 192);

		public GraphControl()
		{
			InitializeComponent();
		}

		public IGraphBundle GraphBundle
		{
			get { return _bundle; }
		}

		public void LoadGraph(IGraphBundle bundle)
		{
			_nodes.Clear();
			_bundle = bundle;

			var location = new Point(16, 16);
			foreach (IFilter filter in _bundle.Graph.Filters)
			{
				Guid nodeGuid = filter.NodeGuid;
				var graphNode = new GraphNode(filter);
				graphNode.LocationChanged += Node_LocationChanged;
				graphNode.MouseDown += GraphNodeOnMouseDown;
				graphNode.MouseMove += GraphNodeOnMouseMove;
				_nodes[nodeGuid] = graphNode;
				if (_bundle.Locations.ContainsKey(nodeGuid))
				{
					graphNode.Location = _bundle.Locations[nodeGuid];
				}
				else
				{
					graphNode.Location = location;
					location.Offset(new Point(64, 64));
				}
				Controls.Add(graphNode);
			}
		}

		private void GraphNodeOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
		{
			if (mouseEventArgs.Button != MouseButtons.Left) 
				return;
			
			var node = (GraphNode)sender;
			node.Left = (node.Left + mouseEventArgs.X) - x;
			node.Top = (node.Top + mouseEventArgs.Y) - y;
			_bundle.Locations[node.Filter.NodeGuid] = node.Location;
		}

		private void GraphNodeOnMouseDown(object sender, MouseEventArgs mouseEventArgs)
		{
			x = mouseEventArgs.X;
			y = mouseEventArgs.Y;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (DesignMode)
				return;

			foreach (IFilter filter in _bundle.Graph.Filters)
			{
				var array = filter.Pins.Where(x => x.IsOutput).ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					IPin connectedTo = array[i].ConnectedTo;
					if (connectedTo == null) 
						continue;
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

					e.Graphics.DrawBezier(new Pen(ColorConnection), output.X, output.Y, output.X + 32, output.Y, input.X - 32 - ArrowSize, input.Y, input.X - ArrowSize, input.Y);
					e.Graphics.FillPolygon(new SolidBrush(ColorConnection), new[] {input, new Point(input.X - ArrowSize, input.Y - ArrowSize/2), new Point(input.X - ArrowSize, input.Y + ArrowSize/2)});
				}
			}
		}

		private void Node_LocationChanged(object sender, System.EventArgs e)
		{
			Invalidate();
		}
	}
}
