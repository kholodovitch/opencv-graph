using System;
using System.IO;
using System.Windows.Forms;

namespace Visualizer.PropertyEditors
{
	public partial class PathEditor : BasePropertyEditor
	{
		public PathEditor()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (mFolderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = mFolderBrowserDialog.FileName;
				FireValueChanged(textBox1.Text);
			}
		}
	}
}
