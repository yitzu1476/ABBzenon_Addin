namespace Archive2CSV
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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button_Daily = new System.Windows.Forms.Button();
            this.button_Custom = new System.Windows.Forms.Button();
            this.button_Yearly = new System.Windows.Forms.Button();
            this.button_Monthly = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ConfirmSelection = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.Preview = new System.Windows.Forms.Button();
            this.button_Summary = new System.Windows.Forms.Button();
            this.button_Average = new System.Windows.Forms.Button();
            this.button_Minimum = new System.Windows.Forms.Button();
            this.button_Maximum = new System.Windows.Forms.Button();
            this.button_Value = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.CSV_Folder = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(386, 350);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(415, 222);
            this.label6.TabIndex = 38;
            this.label6.Text = "Loading...";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(667, 603);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 46);
            this.label5.TabIndex = 37;
            this.label5.Text = "Folder:";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(669, 663);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(469, 39);
            this.textBox1.TabIndex = 36;
            this.textBox1.Text = "C:\\\\Temp";
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(1040, 875);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(102, 44);
            this.button_OK.TabIndex = 35;
            this.button_OK.Text = "Print";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker4.Location = new System.Drawing.Point(988, 97);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.ShowUpDown = true;
            this.dateTimePicker4.Size = new System.Drawing.Size(154, 26);
            this.dateTimePicker4.TabIndex = 34;
            this.dateTimePicker4.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(668, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 37);
            this.label4.TabIndex = 33;
            this.label4.Text = "EndTime:";
            this.label4.Visible = false;
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker3.Location = new System.Drawing.Point(828, 97);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(154, 26);
            this.dateTimePicker3.TabIndex = 32;
            this.dateTimePicker3.Visible = false;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(988, 47);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.ShowUpDown = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(154, 26);
            this.dateTimePicker2.TabIndex = 31;
            this.dateTimePicker2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(658, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 37);
            this.label3.TabIndex = 30;
            this.label3.Text = "DateTime:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(828, 47);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(154, 26);
            this.dateTimePicker1.TabIndex = 29;
            // 
            // button_Daily
            // 
            this.button_Daily.Location = new System.Drawing.Point(152, 26);
            this.button_Daily.Name = "button_Daily";
            this.button_Daily.Size = new System.Drawing.Size(111, 48);
            this.button_Daily.TabIndex = 28;
            this.button_Daily.Text = "Daily";
            this.button_Daily.UseVisualStyleBackColor = true;
            this.button_Daily.Click += new System.EventHandler(this.button_Daily_Click);
            // 
            // button_Custom
            // 
            this.button_Custom.Location = new System.Drawing.Point(503, 26);
            this.button_Custom.Name = "button_Custom";
            this.button_Custom.Size = new System.Drawing.Size(111, 48);
            this.button_Custom.TabIndex = 27;
            this.button_Custom.Text = "Custom";
            this.button_Custom.UseVisualStyleBackColor = true;
            this.button_Custom.Click += new System.EventHandler(this.button_Custom_Click);
            // 
            // button_Yearly
            // 
            this.button_Yearly.Location = new System.Drawing.Point(386, 26);
            this.button_Yearly.Name = "button_Yearly";
            this.button_Yearly.Size = new System.Drawing.Size(111, 48);
            this.button_Yearly.TabIndex = 26;
            this.button_Yearly.Text = "Yearly";
            this.button_Yearly.UseVisualStyleBackColor = true;
            this.button_Yearly.Click += new System.EventHandler(this.button_Yearly_Click);
            // 
            // button_Monthly
            // 
            this.button_Monthly.Location = new System.Drawing.Point(269, 26);
            this.button_Monthly.Name = "button_Monthly";
            this.button_Monthly.Size = new System.Drawing.Size(111, 48);
            this.button_Monthly.TabIndex = 25;
            this.button_Monthly.Text = "Monthly";
            this.button_Monthly.UseVisualStyleBackColor = true;
            this.button_Monthly.Click += new System.EventHandler(this.button_Monthly_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(661, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(340, 46);
            this.label2.TabIndex = 24;
            this.label2.Text = "Selected Archives";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 46);
            this.label1.TabIndex = 23;
            this.label1.Text = "Archives";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(669, 223);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(473, 344);
            this.listBox1.TabIndex = 22;
            // 
            // ConfirmSelection
            // 
            this.ConfirmSelection.Location = new System.Drawing.Point(460, 172);
            this.ConfirmSelection.Name = "ConfirmSelection";
            this.ConfirmSelection.Size = new System.Drawing.Size(169, 43);
            this.ConfirmSelection.TabIndex = 21;
            this.ConfirmSelection.Text = "Confirm Selection";
            this.ConfirmSelection.UseVisualStyleBackColor = true;
            this.ConfirmSelection.Click += new System.EventHandler(this.ConfirmSelection_Click);
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(21, 223);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(608, 696);
            this.treeView1.TabIndex = 20;
            // 
            // Preview
            // 
            this.Preview.Location = new System.Drawing.Point(925, 875);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(102, 44);
            this.Preview.TabIndex = 39;
            this.Preview.Text = "Preview";
            this.Preview.UseVisualStyleBackColor = true;
            this.Preview.Click += new System.EventHandler(this.Preview_Click);
            // 
            // button_Summary
            // 
            this.button_Summary.Location = new System.Drawing.Point(35, 104);
            this.button_Summary.Name = "button_Summary";
            this.button_Summary.Size = new System.Drawing.Size(111, 48);
            this.button_Summary.TabIndex = 40;
            this.button_Summary.Text = "Summary";
            this.button_Summary.UseVisualStyleBackColor = true;
            this.button_Summary.Click += new System.EventHandler(this.button_Summary_Click);
            // 
            // button_Average
            // 
            this.button_Average.Location = new System.Drawing.Point(152, 104);
            this.button_Average.Name = "button_Average";
            this.button_Average.Size = new System.Drawing.Size(111, 48);
            this.button_Average.TabIndex = 41;
            this.button_Average.Text = "Average";
            this.button_Average.UseVisualStyleBackColor = true;
            this.button_Average.Click += new System.EventHandler(this.button_Average_Click);
            // 
            // button_Minimum
            // 
            this.button_Minimum.Location = new System.Drawing.Point(269, 104);
            this.button_Minimum.Name = "button_Minimum";
            this.button_Minimum.Size = new System.Drawing.Size(111, 48);
            this.button_Minimum.TabIndex = 42;
            this.button_Minimum.Text = "Minimum";
            this.button_Minimum.UseVisualStyleBackColor = true;
            this.button_Minimum.Click += new System.EventHandler(this.button_Minimum_Click);
            // 
            // button_Maximum
            // 
            this.button_Maximum.Location = new System.Drawing.Point(386, 104);
            this.button_Maximum.Name = "button_Maximum";
            this.button_Maximum.Size = new System.Drawing.Size(111, 48);
            this.button_Maximum.TabIndex = 43;
            this.button_Maximum.Text = "Maximum";
            this.button_Maximum.UseVisualStyleBackColor = true;
            this.button_Maximum.Click += new System.EventHandler(this.button_Maximum_Click);
            // 
            // button_Value
            // 
            this.button_Value.Location = new System.Drawing.Point(503, 104);
            this.button_Value.Name = "button_Value";
            this.button_Value.Size = new System.Drawing.Size(111, 48);
            this.button_Value.TabIndex = 44;
            this.button_Value.Text = "Value";
            this.button_Value.UseVisualStyleBackColor = true;
            this.button_Value.Click += new System.EventHandler(this.button_Value_Click);
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(21, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(608, 65);
            this.label7.TabIndex = 45;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(139, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(490, 65);
            this.label8.TabIndex = 46;
            // 
            // CSV_Folder
            // 
            this.CSV_Folder.Location = new System.Drawing.Point(1036, 603);
            this.CSV_Folder.Name = "CSV_Folder";
            this.CSV_Folder.Size = new System.Drawing.Size(102, 44);
            this.CSV_Folder.TabIndex = 47;
            this.CSV_Folder.Text = "Folder";
            this.CSV_Folder.UseVisualStyleBackColor = true;
            this.CSV_Folder.Click += new System.EventHandler(this.CSV_Folder_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(661, 736);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(212, 46);
            this.label9.TabIndex = 49;
            this.label9.Text = "File Name:";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(665, 795);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(473, 39);
            this.textBox2.TabIndex = 48;
            this.textBox2.Text = "ReportExport.csv";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(21, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(104, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 50;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 936);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.CSV_Folder);
            this.Controls.Add(this.button_Value);
            this.Controls.Add(this.button_Maximum);
            this.Controls.Add(this.button_Minimum);
            this.Controls.Add(this.button_Average);
            this.Controls.Add(this.button_Summary);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.dateTimePicker4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePicker3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.button_Daily);
            this.Controls.Add(this.button_Custom);
            this.Controls.Add(this.button_Yearly);
            this.Controls.Add(this.button_Monthly);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.ConfirmSelection);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Archive to CSV Tool";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button_Daily;
        private System.Windows.Forms.Button button_Custom;
        private System.Windows.Forms.Button button_Yearly;
        private System.Windows.Forms.Button button_Monthly;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button ConfirmSelection;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button Preview;
        private System.Windows.Forms.Button button_Summary;
        private System.Windows.Forms.Button button_Average;
        private System.Windows.Forms.Button button_Minimum;
        private System.Windows.Forms.Button button_Maximum;
        private System.Windows.Forms.Button button_Value;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button CSV_Folder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}