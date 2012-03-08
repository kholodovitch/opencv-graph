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
			string name = FiltersHelper.GetFilterTypes().First().FullName;
			var y = (IFilter)Assembly.GetExecutingAssembly().CreateInstance(name);
			graph.AddSourceFilter(y);
			graph.Start();

			var t = GraphLoader.Load("");
		}
	}
}
