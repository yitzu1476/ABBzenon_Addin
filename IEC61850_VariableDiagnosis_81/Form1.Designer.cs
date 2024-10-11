namespace IEC61850_VariableDiagnosis_81
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.DeleteSelected = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Profile_Manager = new System.Windows.Forms.Button();
            this.CreateProfile = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ColumnSelection = new System.Windows.Forms.Button();
            this.Refresh_var = new System.Windows.Forms.Button();
            this.SelectVar = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.Set_Selected_CO = new System.Windows.Forms.Button();
            this.Set_All_CO = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // DeleteSelected
            // 
            this.DeleteSelected.Location = new System.Drawing.Point(1528, 193);
            this.DeleteSelected.Name = "DeleteSelected";
            this.DeleteSelected.Size = new System.Drawing.Size(158, 49);
            this.DeleteSelected.TabIndex = 21;
            this.DeleteSelected.Text = "Delete Selected";
            this.DeleteSelected.UseVisualStyleBackColor = true;
            this.DeleteSelected.Click += new System.EventHandler(this.DeleteSelected_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1090, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 25);
            this.label2.TabIndex = 20;
            this.label2.Text = "Profile selection:";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1259, 22);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(263, 33);
            this.comboBox1.TabIndex = 19;
            // 
            // Profile_Manager
            // 
            this.Profile_Manager.Location = new System.Drawing.Point(1528, 905);
            this.Profile_Manager.Name = "Profile_Manager";
            this.Profile_Manager.Size = new System.Drawing.Size(158, 49);
            this.Profile_Manager.TabIndex = 18;
            this.Profile_Manager.Text = "Profile Manager";
            this.Profile_Manager.UseVisualStyleBackColor = true;
            this.Profile_Manager.Click += new System.EventHandler(this.Profile_Manager_Click);
            // 
            // CreateProfile
            // 
            this.CreateProfile.Location = new System.Drawing.Point(592, 15);
            this.CreateProfile.Name = "CreateProfile";
            this.CreateProfile.Size = new System.Drawing.Size(124, 49);
            this.CreateProfile.TabIndex = 17;
            this.CreateProfile.Text = "Save";
            this.CreateProfile.UseVisualStyleBackColor = true;
            this.CreateProfile.Click += new System.EventHandler(this.CreateProfile_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(162, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(412, 30);
            this.textBox1.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 25);
            this.label1.TabIndex = 15;
            this.label1.Text = "Profile name: ";
            // 
            // ColumnSelection
            // 
            this.ColumnSelection.Location = new System.Drawing.Point(1528, 850);
            this.ColumnSelection.Name = "ColumnSelection";
            this.ColumnSelection.Size = new System.Drawing.Size(158, 49);
            this.ColumnSelection.TabIndex = 14;
            this.ColumnSelection.Text = "Column Selection";
            this.ColumnSelection.UseVisualStyleBackColor = true;
            this.ColumnSelection.Click += new System.EventHandler(this.ColumnSelection_Click);
            // 
            // Refresh_var
            // 
            this.Refresh_var.Location = new System.Drawing.Point(1528, 138);
            this.Refresh_var.Name = "Refresh_var";
            this.Refresh_var.Size = new System.Drawing.Size(158, 49);
            this.Refresh_var.TabIndex = 13;
            this.Refresh_var.Text = "Refresh";
            this.Refresh_var.UseVisualStyleBackColor = true;
            this.Refresh_var.Click += new System.EventHandler(this.Refresh_var_Click);
            // 
            // SelectVar
            // 
            this.SelectVar.Location = new System.Drawing.Point(1528, 83);
            this.SelectVar.Name = "SelectVar";
            this.SelectVar.Size = new System.Drawing.Size(158, 49);
            this.SelectVar.TabIndex = 12;
            this.SelectVar.Text = "Select Variable";
            this.SelectVar.UseVisualStyleBackColor = true;
            this.SelectVar.Click += new System.EventHandler(this.SelectVar_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(30, 83);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(1492, 431);
            this.dataGridView1.TabIndex = 11;
            // 
            // dataGridView2
            // 
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(30, 527);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 62;
            this.dataGridView2.RowTemplate.Height = 28;
            this.dataGridView2.Size = new System.Drawing.Size(1492, 427);
            this.dataGridView2.TabIndex = 22;
            // 
            // Set_Selected_CO
            // 
            this.Set_Selected_CO.Location = new System.Drawing.Point(1528, 527);
            this.Set_Selected_CO.Name = "Set_Selected_CO";
            this.Set_Selected_CO.Size = new System.Drawing.Size(158, 49);
            this.Set_Selected_CO.TabIndex = 23;
            this.Set_Selected_CO.Text = "Set Selected CO";
            this.Set_Selected_CO.UseVisualStyleBackColor = true;
            this.Set_Selected_CO.Click += new System.EventHandler(this.Set_Selected_CO_Click);
            // 
            // Set_All_CO
            // 
            this.Set_All_CO.Location = new System.Drawing.Point(1528, 582);
            this.Set_All_CO.Name = "Set_All_CO";
            this.Set_All_CO.Size = new System.Drawing.Size(158, 49);
            this.Set_All_CO.TabIndex = 24;
            this.Set_All_CO.Text = "Set All CO";
            this.Set_All_CO.UseVisualStyleBackColor = true;
            this.Set_All_CO.Click += new System.EventHandler(this.Set_All_CO_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(346, 253);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(845, 460);
            this.richTextBox1.TabIndex = 25;
            this.richTextBox1.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1591, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(93, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 30;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1696, 967);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Set_All_CO);
            this.Controls.Add(this.Set_Selected_CO);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.DeleteSelected);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Profile_Manager);
            this.Controls.Add(this.CreateProfile);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ColumnSelection);
            this.Controls.Add(this.Refresh_var);
            this.Controls.Add(this.SelectVar);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "IEC 61850 Diagnosis Tool";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DeleteSelected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Profile_Manager;
        private System.Windows.Forms.Button CreateProfile;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ColumnSelection;
        private System.Windows.Forms.Button Refresh_var;
        private System.Windows.Forms.Button SelectVar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button Set_Selected_CO;
        private System.Windows.Forms.Button Set_All_CO;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}