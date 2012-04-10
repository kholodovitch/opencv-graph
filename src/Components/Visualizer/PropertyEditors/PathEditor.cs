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

		public override object Value
		{
			get { return textBox1.Text; }
			set { textBox1.Text = (string) value; }
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
