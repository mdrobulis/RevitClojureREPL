namespace NavisworksClojureRepl
{
    partial class WPFHost
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WPFControl = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.WPFControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WPFControl.Location = new System.Drawing.Point(0, 0);
            this.WPFControl.Name = "WPFControl";
            this.WPFControl.Size = new System.Drawing.Size(150, 150);
            this.WPFControl.TabIndex = 0;
            this.WPFControl.Text = "elementHost1";
            this.WPFControl.Child = null;
            // 
            // WPFHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.WPFControl);
            this.Name = "WPFHost";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Integration.ElementHost WPFControl;
    }
}
