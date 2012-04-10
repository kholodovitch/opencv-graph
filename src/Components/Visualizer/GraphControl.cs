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

		private void GraphControl_Click(object sender, EventArgs e)
		{
			//var point = Cursor.Position;
			//var bezier = new Point[0];
			//var nearest = GetNearestOnBezier(point, bezier);
			//nearest.ToString();
		}

		private PointF GetNearestOnBezier(Point point, Point[] bezier)
		{
			var cx = 3*(bezier[1].X - bezier[0].X);
			var cy = 3*(bezier[1].Y - bezier[0].Y);
			var bx = 3*(bezier[2].X - bezier[1].X) - cx;
			var by = 3*(bezier[2].Y - bezier[1].Y) - cy;
			var ax = bezier[3].X - bezier[0].X - cx - bx;
			var ay = bezier[3].Y - bezier[0].Y - cy - @by;

			var points = Enumerable
				.Range(0, 1000)
				.Select(i => i/1000.0)
				.Select(tg =>
				        	{
				        		var tsqr = tg*tg;
				        		var tcube = tsqr*tg;
				        		return new PointF(
				        			(float) ((ax*tcube) + (bx*tsqr) + (cx*tg) + bezier[0].X),
				        			(float) ((ay*tcube) + (@by*tsqr) + (cy*tg) + bezier[0].Y)
				        			);
				        	})
				.ToArray();

			PointF nearest = PointF.Empty;
			float minDist = float.MaxValue;
			for (int i = 0; i < points.Length - 1; i++)
			{
				PointF current = GetNearestOnSegment(points[i], points[i + 1], point);
				var dx = (current.X - point.X);
				var dy = (current.Y - point.Y);
				var dist = (float) Math.Sqrt(dx*dx + dy*dy);
				if (minDist <= dist) 
					continue;

				minDist = dist;
				nearest = current;
			}
			return nearest;
		}

		private PointF GetNearestOnSegment(PointF p1, PointF p2, PointF p)
		{
			float vx = p2.X - p1.X;
			float vy = p2.Y - p1.Y;
			float wx = p.X - p1.X;
			float wy = p.Y - p1.Y;

			float c1 = vx * wx + vy * wy;
			if (c1 <= 0)
				return p1;

			float c2 = vx * vx + vy * vy;
			if (c2 <= c1)
				return p2;

			float ratio = c1 / c2;
			float nx = p1.X + ratio * vx;
			float ny = p1.Y + ratio * vy;
			return new PointF(nx, ny);
		}
	}
}
