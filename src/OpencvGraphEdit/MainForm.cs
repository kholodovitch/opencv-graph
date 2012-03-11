using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DataStructures;
using FilterImplementation;

namespace OpencvGraphEdit
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			var graph = new Graph();
			var name = FiltersHelper.GetFilterTypes().First();
            var y = (IFilter)name.Assembly.CreateInstance(name.FullName);
			graph.AddSourceFilter(y);
            GraphLoader.Save(graph, "graph.xml");
		}
	}
}
