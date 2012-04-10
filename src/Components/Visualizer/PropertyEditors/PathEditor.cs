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
			get { return linkLabel1.Tag; }
			set
			{
				linkLabel1.Text = Path.GetFileName((string)value);
				linkLabel1.Tag = value;
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (mFolderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = mFolderBrowserDialog.FileName;
				linkLabel1.Tag = fileName;
				linkLabel1.Text = Path.GetFileName(fileName);
				FireValueChanged(linkLabel1.Tag);
			}
		}
	}
}
