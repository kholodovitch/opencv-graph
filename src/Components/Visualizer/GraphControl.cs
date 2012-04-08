using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataStructures;

namespace Visualizer
{
	public partial class GraphControl : UserControl
	{
		public GraphControl()
		{
			InitializeComponent();
		}

		public void LoadGraph(IGraph graph)
		{
			var graphNode = new GraphNode(graph.Filters.First());
			graphNode.Location = new Point(16, 16);
			Controls.Add(graphNode);
		}
	}
}
