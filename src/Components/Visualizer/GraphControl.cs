﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataStructures;

namespace Visualizer
{
	public partial class GraphControl : UserControl
	{
		private const int ArrowSize = 8;
		private readonly Color ColorConnection = Color.FromArgb(192, 192, 192);
		private readonly Dictionary<string, Connection> _connections = new Dictionary<string, Connection>();
		private readonly Dictionary<Guid, GraphNode> _nodes = new Dictionary<Guid, GraphNode>();
		private IGraphBundle _bundle;
		private int x;
		private int y;

		private GraphNode _outputNode;
		private int _outputIndex;

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
			
			foreach (IFilter filter in _bundle.Graph.Filters)
			{
				AddFilter(filter);
			}
		}

		public void AddFilter(IFilter filter)
		{
			Guid nodeGuid = filter.NodeGuid;
			var graphNode = new GraphNode(filter);
			graphNode.LocationChanged += Node_LocationChanged;
			graphNode.MouseDown += GraphNodeOnMouseDown;
			graphNode.MouseMove += GraphNodeOnMouseMove;
			_nodes[nodeGuid] = graphNode;
			if (_bundle.Locations.ContainsKey(nodeGuid))
				graphNode.Location = _bundle.Locations[nodeGuid];
			Controls.Add(graphNode);
		}

		private void GraphNodeOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
		{
			if (mouseEventArgs.Button != MouseButtons.Left)
				return;

			var node = (GraphNode) sender;
			node.Left = (node.Left + mouseEventArgs.X) - x;
			node.Top = (node.Top + mouseEventArgs.Y) - y;
			_bundle.Locations[node.Filter.NodeGuid] = node.Location;
		}

		private void GraphNodeOnMouseDown(object sender, MouseEventArgs mouseEventArgs)
		{
			var node = (GraphNode)sender;
			var output = node.Filter.Pins
				.Where(pin => pin.IsOutput)
				.ToArray();
			for (int i = 0; i < output.Length; i++)
			{
				var pinPort = node.GetPinPort(i, true);
				pinPort.Offset(GraphNode.PinPortSize / 2, GraphNode.PinPortSize / 2);
				if (pinPort.X - 3 < mouseEventArgs.X && pinPort.X + 3 > mouseEventArgs.X &&
					pinPort.Y - 3 < mouseEventArgs.Y && pinPort.Y + 3 > mouseEventArgs.Y )
				{
					_outputNode = node;
					_outputIndex = i;
				}
			}
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
				IPin[] array = filter.Pins.Where(x => x.IsOutput).ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					IPin connectedTo = array[i].ConnectedTo;
					if (connectedTo == null)
						continue;
					IFilter connectedToFilter = connectedTo.Filter;
					int index = connectedToFilter.Pins.Where(x => !x.IsOutput).ToList().IndexOf(connectedTo);
					GraphNode connectedToNode = _nodes[connectedToFilter.NodeGuid];
					GraphNode node = _nodes[filter.NodeGuid];

					Point output = node.GetPinPort(i, true);
					output.Offset(node.Location);
					output.Offset(0, 1);

					Point input = connectedToNode.GetPinPort(index, false);
					input.Offset(connectedToNode.Location);
					input.Offset(0, 1);

					string connectionKey = string.Format("{0}-{1}-{2}-{3}", filter.NodeGuid, i, connectedToFilter.NodeGuid, index);
					if (!_connections.ContainsKey(connectionKey))
						_connections[connectionKey] = new Connection(connectionKey, output, input);
					else
						_connections[connectionKey].SetPoints(output, input);
					DrawConnection(e.Graphics, _connections[connectionKey]);
				}
			}
		}

		private void DrawConnection(Graphics graphics, Connection connection)
		{
			Color colorConnection = connection.IsSelected ? Color.DarkRed : ColorConnection;
			graphics.DrawBezier(new Pen(colorConnection), connection.Points[0], connection.Points[1], connection.Points[2], connection.Points[3]);
			graphics.FillPolygon(new SolidBrush(colorConnection), connection.ArrowPath);
		}

		private void Node_LocationChanged(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void GraphControl_Click(object sender, EventArgs e)
		{
			Point point = PointToClient(Cursor.Position);
			_connections.ToList().ForEach(a => a.Value.IsSelected = false);

			var nearestConnection = _connections
				.Select(pair => new {Item = pair.Value, Point = GetNearestOnBezier(point, pair.Value.Points)})
				.Select(arg => new {arg.Item, Distance = GetDistance(point, arg.Point)})
				.OrderBy(arg => arg.Distance)
				.FirstOrDefault();
			if (nearestConnection != null && nearestConnection.Distance < 5)
				nearestConnection.Item.IsSelected = true;

			Invalidate();
		}

		private PointF GetNearestOnBezier(Point point, Point[] bezier)
		{
			double cx = 3.0*(bezier[1].X - bezier[0].X);
			double cy = 3.0*(bezier[1].Y - bezier[0].Y);
			double bx = 3.0*(bezier[2].X - bezier[1].X) - cx;
			double by = 3.0*(bezier[2].Y - bezier[1].Y) - cy;
			double ax = bezier[3].X - bezier[0].X - cx - bx;
			double ay = bezier[3].Y - bezier[0].Y - cy - by;

			PointF[] points = Enumerable
				.Range(0, 20)
				.Select(i => i/20.0)
				.Select(tg =>
				        	{
				        		double tsqr = tg*tg;
				        		double tcube = tsqr*tg;
				        		return new PointF(
				        			(float) ((ax*tcube) + (bx*tsqr) + (cx*tg) + bezier[0].X),
				        			(float) ((ay*tcube) + (by*tsqr) + (cy*tg) + bezier[0].Y)
				        			);
				        	})
				.ToArray();

			PointF nearest = PointF.Empty;
			float minDist = float.MaxValue;
			for (int i = 0; i < points.Length - 1; i++)
			{
				PointF current = GetNearestOnSegment(points[i], points[i + 1], point);
				float dist = GetDistance(point, current);
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

			float c1 = vx*wx + vy*wy;
			if (c1 <= 0)
				return p1;

			float c2 = vx*vx + vy*vy;
			if (c2 <= c1)
				return p2;

			float ratio = c1/c2;
			float nx = p1.X + ratio*vx;
			float ny = p1.Y + ratio*vy;
			return new PointF(nx, ny);
		}

		private static float GetDistance(PointF point, PointF current)
		{
			float dx = (current.X - point.X);
			float dy = (current.Y - point.Y);
			var dist = (float) Math.Sqrt(dx*dx + dy*dy);
			return dist;
		}

		#region Nested type: Connection

		private class Connection
		{
			private readonly string _connectionKey;
			private readonly Point[] _points = new Point[4];
			private Point _input;
			private Point _output;

			public Connection(string connectionKey, Point output, Point input)
			{
				_connectionKey = connectionKey;
				SetPoints(output, input);
			}

			public Point Input
			{
				get { return _input; }
			}

			public Point Output
			{
				get { return _output; }
			}

			public Point[] Points
			{
				get { return _points; }
			}

			public Point[] ArrowPath
			{
				get { return new[] {Input, new Point(Input.X - ArrowSize, Input.Y - ArrowSize/2), new Point(Input.X - ArrowSize, Input.Y + ArrowSize/2)}; }
			}

			public bool IsSelected { get; set; }

			internal void SetPoints(Point output, Point input)
			{
				_output = output;
				_input = input;

				_points[0] = new Point(output.X, output.Y);
				_points[1] = new Point(output.X + 32, output.Y);
				_points[2] = new Point(input.X - 32 - ArrowSize, input.Y);
				_points[3] = new Point(input.X - ArrowSize, input.Y);
			}
		}

		#endregion
	}
}