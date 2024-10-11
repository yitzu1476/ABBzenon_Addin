namespace Gateway_EditorTool
{
    partial class Form3
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
            this.label1 = new System.Windows.Forms.Label();
            this.Update_All = new System.Windows.Forms.Button();
            this.Update_DB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(708, 109);
            this.label1.TabIndex = 1;
            this.label1.Text = "Do you want to create xml and ini files with station info, or merely update the d" +
    "atabase content?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Update_All
            // 
            this.Update_All.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Update_All.Location = new System.Drawing.Point(83, 198);
            this.Update_All.Name = "Update_All";
            this.Update_All.Size = new System.Drawing.Size(237, 81);
            this.Update_All.TabIndex = 2;
            this.Update_All.Text = "Update All";
            this.Update_All.UseVisualStyleBackColor = true;
            this.Update_All.Click += new System.EventHandler(this.Update_All_Click);
            // 
            // Update_DB
            // 
            this.Update_DB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Update_DB.Location = new System.Drawing.Point(418, 198);
            this.Update_DB.Name = "Update_DB";
            this.Update_DB.Size = new System.Drawing.Size(237, 81);
            this.Update_DB.TabIndex = 3;
            this.Update_DB.Text = "Update Database";
            this.Update_DB.UseVisualStyleBackColor = true;
            this.Update_DB.Click += new System.EventHandler(this.Update_DB_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 375);
            this.Controls.Add(this.Update_DB);
            this.Controls.Add(this.Update_All);
            this.Controls.Add(this.label1);
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Update_All;
        private System.Windows.Forms.Button Update_DB;
    }
}