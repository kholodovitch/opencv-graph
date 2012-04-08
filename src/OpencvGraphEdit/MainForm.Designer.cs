namespace OpencvGraphEdit
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.graphControl1 = new Visualizer.GraphControl();
			this.SuspendLayout();
			// 
			// graphControl1
			// 
			this.graphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphControl1.Location = new System.Drawing.Point(0, 0);
			this.graphControl1.Name = "graphControl1";
			this.graphControl1.Size = new System.Drawing.Size(784, 562);
			this.graphControl1.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 562);
			this.Controls.Add(this.graphControl1);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);

		}

		#endregion

		private Visualizer.GraphControl graphControl1;
	}
}

