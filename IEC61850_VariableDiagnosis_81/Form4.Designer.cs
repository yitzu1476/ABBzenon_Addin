namespace IEC61850_VariableDiagnosis_81
{
    partial class Form4
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
            this.DeleteVariables = new System.Windows.Forms.Button();
            this.DeleteProfile = new System.Windows.Forms.Button();
            this.SeeContent = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // DeleteVariables
            // 
            this.DeleteVariables.Location = new System.Drawing.Point(381, 433);
            this.DeleteVariables.Name = "DeleteVariables";
            this.DeleteVariables.Size = new System.Drawing.Size(223, 51);
            this.DeleteVariables.TabIndex = 9;
            this.DeleteVariables.Text = "Delete Selected Variables";
            this.DeleteVariables.UseVisualStyleBackColor = true;
            this.DeleteVariables.Click += new System.EventHandler(this.DeleteVariables_Click);
            // 
            // DeleteProfile
            // 
            this.DeleteProfile.Location = new System.Drawing.Point(15, 433);
            this.DeleteProfile.Name = "DeleteProfile";
            this.DeleteProfile.Size = new System.Drawing.Size(223, 51);
            this.DeleteProfile.TabIndex = 8;
            this.DeleteProfile.Text = "Delete Selected Profile";
            this.DeleteProfile.UseVisualStyleBackColor = true;
            this.DeleteProfile.Click += new System.EventHandler(this.DeleteProfile_Click);
            // 
            // SeeContent
            // 
            this.SeeContent.Location = new System.Drawing.Point(244, 181);
            this.SeeContent.Name = "SeeContent";
            this.SeeContent.Size = new System.Drawing.Size(131, 59);
            this.SeeContent.TabIndex = 7;
            this.SeeContent.Text = ">>";
            this.SeeContent.UseVisualStyleBackColor = true;
            this.SeeContent.Click += new System.EventHandler(this.SeeContent_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.HorizontalScrollbar = true;
            this.listBox2.ItemHeight = 20;
            this.listBox2.Location = new System.Drawing.Point(381, 12);
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox2.Size = new System.Drawing.Size(223, 404);
            this.listBox2.TabIndex = 6;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(15, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(223, 404);
            this.listBox1.TabIndex = 5;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 496);
            this.Controls.Add(this.DeleteVariables);
            this.Controls.Add(this.DeleteProfile);
            this.Controls.Add(this.SeeContent);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Name = "Form4";
            this.Text = "Profile Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DeleteVariables;
        private System.Windows.Forms.Button DeleteProfile;
        private System.Windows.Forms.Button SeeContent;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox1;
    }
}