namespace IEC61850_VariableDiagnosis_81
{
    partial class Form2
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
            this.Label_SelectedCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ClearAll = new System.Windows.Forms.Button();
            this.Filter_ClearFilter = new System.Windows.Forms.Button();
            this.SelectAll = new System.Windows.Forms.Button();
            this.Filter_notInclude = new System.Windows.Forms.Button();
            this.Filter_Include = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.confirm = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SelectAll_Selection = new System.Windows.Forms.Button();
            this.ShowAllSelected = new System.Windows.Forms.Button();
            this.Refresh_button = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Label_SelectedCount
            // 
            this.Label_SelectedCount.AutoSize = true;
            this.Label_SelectedCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_SelectedCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label_SelectedCount.Location = new System.Drawing.Point(1467, 25);
            this.Label_SelectedCount.Name = "Label_SelectedCount";
            this.Label_SelectedCount.Size = new System.Drawing.Size(23, 25);
            this.Label_SelectedCount.TabIndex = 27;
            this.Label_SelectedCount.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1302, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 25);
            this.label3.TabIndex = 26;
            this.label3.Text = "Selected count:";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(21, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1642, 480);
            this.label1.TabIndex = 17;
            // 
            // ClearAll
            // 
            this.ClearAll.Location = new System.Drawing.Point(1548, 234);
            this.ClearAll.Name = "ClearAll";
            this.ClearAll.Size = new System.Drawing.Size(115, 51);
            this.ClearAll.TabIndex = 24;
            this.ClearAll.Text = "Clear All";
            this.ClearAll.UseVisualStyleBackColor = true;
            this.ClearAll.Click += new System.EventHandler(this.ClearAll_Click);
            // 
            // Filter_ClearFilter
            // 
            this.Filter_ClearFilter.Location = new System.Drawing.Point(980, 17);
            this.Filter_ClearFilter.Name = "Filter_ClearFilter";
            this.Filter_ClearFilter.Size = new System.Drawing.Size(122, 44);
            this.Filter_ClearFilter.TabIndex = 23;
            this.Filter_ClearFilter.Text = "Clear Filter";
            this.Filter_ClearFilter.UseVisualStyleBackColor = true;
            this.Filter_ClearFilter.Click += new System.EventHandler(this.Filter_ClearFilter_Click);
            // 
            // SelectAll
            // 
            this.SelectAll.Location = new System.Drawing.Point(1548, 160);
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(115, 51);
            this.SelectAll.TabIndex = 22;
            this.SelectAll.Text = "Select All";
            this.SelectAll.UseVisualStyleBackColor = true;
            this.SelectAll.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // Filter_notInclude
            // 
            this.Filter_notInclude.Location = new System.Drawing.Point(852, 17);
            this.Filter_notInclude.Name = "Filter_notInclude";
            this.Filter_notInclude.Size = new System.Drawing.Size(122, 44);
            this.Filter_notInclude.TabIndex = 21;
            this.Filter_notInclude.Text = "Not Include";
            this.Filter_notInclude.UseVisualStyleBackColor = true;
            this.Filter_notInclude.Click += new System.EventHandler(this.Filter_notInclude_Click);
            // 
            // Filter_Include
            // 
            this.Filter_Include.Location = new System.Drawing.Point(724, 17);
            this.Filter_Include.Name = "Filter_Include";
            this.Filter_Include.Size = new System.Drawing.Size(122, 44);
            this.Filter_Include.TabIndex = 20;
            this.Filter_Include.Text = "Include";
            this.Filter_Include.UseVisualStyleBackColor = true;
            this.Filter_Include.Click += new System.EventHandler(this.Filter_Include_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(211, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(491, 30);
            this.textBox1.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 25);
            this.label2.TabIndex = 18;
            this.label2.Text = "Variable name filter:";
            // 
            // confirm
            // 
            this.confirm.Location = new System.Drawing.Point(1548, 85);
            this.confirm.Name = "confirm";
            this.confirm.Size = new System.Drawing.Size(115, 51);
            this.confirm.TabIndex = 15;
            this.confirm.Text = "Confirm";
            this.confirm.UseVisualStyleBackColor = true;
            this.confirm.Click += new System.EventHandler(this.confirm_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(21, 85);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(1503, 480);
            this.dataGridView1.TabIndex = 14;
            // 
            // SelectAll_Selection
            // 
            this.SelectAll_Selection.Location = new System.Drawing.Point(1548, 500);
            this.SelectAll_Selection.Name = "SelectAll_Selection";
            this.SelectAll_Selection.Size = new System.Drawing.Size(115, 65);
            this.SelectAll_Selection.TabIndex = 25;
            this.SelectAll_Selection.Text = "Check All Selection";
            this.SelectAll_Selection.UseVisualStyleBackColor = true;
            this.SelectAll_Selection.Click += new System.EventHandler(this.SelectAll_Selection_Click);
            // 
            // ShowAllSelected
            // 
            this.ShowAllSelected.Location = new System.Drawing.Point(1520, 17);
            this.ShowAllSelected.Name = "ShowAllSelected";
            this.ShowAllSelected.Size = new System.Drawing.Size(143, 44);
            this.ShowAllSelected.TabIndex = 28;
            this.ShowAllSelected.Text = "Show Selection";
            this.ShowAllSelected.UseVisualStyleBackColor = true;
            this.ShowAllSelected.Click += new System.EventHandler(this.ShowAllSelected_Click);
            // 
            // Refresh_button
            // 
            this.Refresh_button.Location = new System.Drawing.Point(766, 296);
            this.Refresh_button.Name = "Refresh_button";
            this.Refresh_button.Size = new System.Drawing.Size(106, 49);
            this.Refresh_button.TabIndex = 29;
            this.Refresh_button.Text = "Refresh";
            this.Refresh_button.UseVisualStyleBackColor = true;
            this.Refresh_button.Click += new System.EventHandler(this.Refresh_button_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(467, 172);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(806, 332);
            this.richTextBox1.TabIndex = 30;
            this.richTextBox1.Text = "";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1679, 580);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Refresh_button);
            this.Controls.Add(this.ShowAllSelected);
            this.Controls.Add(this.Label_SelectedCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ClearAll);
            this.Controls.Add(this.Filter_ClearFilter);
            this.Controls.Add(this.SelectAll);
            this.Controls.Add(this.Filter_notInclude);
            this.Controls.Add(this.Filter_Include);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.confirm);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.SelectAll_Selection);
            this.Name = "Form2";
            this.Text = "Variable Selection";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label_SelectedCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ClearAll;
        private System.Windows.Forms.Button Filter_ClearFilter;
        private System.Windows.Forms.Button SelectAll;
        private System.Windows.Forms.Button Filter_notInclude;
        private System.Windows.Forms.Button Filter_Include;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button confirm;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button SelectAll_Selection;
        private System.Windows.Forms.Button ShowAllSelected;
        private System.Windows.Forms.Button Refresh_button;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}