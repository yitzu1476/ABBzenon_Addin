namespace OPC_VariableCreator
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox_DeviceName = new System.Windows.Forms.ComboBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBox_DeviceDescription = new System.Windows.Forms.ComboBox();
            this.comboBox_DeviceStationNum = new System.Windows.Forms.ComboBox();
            this.comboBox_VariableName = new System.Windows.Forms.ComboBox();
            this.comboBox_VariableDescription = new System.Windows.Forms.ComboBox();
            this.comboBox_VariableDataType = new System.Windows.Forms.ComboBox();
            this.comboBox_SymbolicAdd = new System.Windows.Forms.ComboBox();
            this.comboBox_VariableDataLen = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(24, 61);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(896, 26);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(950, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 49);
            this.button1.TabIndex = 1;
            this.button1.Text = "Get Sheets";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(24, 199);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox1.Size = new System.Drawing.Size(259, 444);
            this.listBox1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(768, 600);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(233, 44);
            this.button2.TabIndex = 3;
            this.button2.Text = "Collect Points";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1127, 37);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(403, 606);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "File path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(729, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Device name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(692, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Device description:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(663, 308);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Device station number:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(719, 352);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Variable name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(682, 396);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Variable description:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(701, 440);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 20);
            this.label7.TabIndex = 11;
            this.label7.Text = "Variable datatype";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(638, 484);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(196, 20);
            this.label8.TabIndex = 12;
            this.label8.Text = "Variable symbolic address:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(679, 528);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(155, 20);
            this.label9.TabIndex = 13;
            this.label9.Text = "Variable data length:";
            // 
            // comboBox_DeviceName
            // 
            this.comboBox_DeviceName.FormattingEnabled = true;
            this.comboBox_DeviceName.Location = new System.Drawing.Point(849, 217);
            this.comboBox_DeviceName.Name = "comboBox_DeviceName";
            this.comboBox_DeviceName.Size = new System.Drawing.Size(226, 28);
            this.comboBox_DeviceName.TabIndex = 14;
            // 
            // listBox2
            // 
            this.listBox2.Enabled = false;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 20;
            this.listBox2.Location = new System.Drawing.Point(324, 199);
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox2.Size = new System.Drawing.Size(274, 444);
            this.listBox2.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(26, 148);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(148, 29);
            this.label10.TabIndex = 16;
            this.label10.Text = "Sheet name:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(319, 148);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(288, 29);
            this.label11.TabIndex = 17;
            this.label11.Text = "Header of selected sheet:";
            // 
            // comboBox_DeviceDescription
            // 
            this.comboBox_DeviceDescription.FormattingEnabled = true;
            this.comboBox_DeviceDescription.Location = new System.Drawing.Point(849, 261);
            this.comboBox_DeviceDescription.Name = "comboBox_DeviceDescription";
            this.comboBox_DeviceDescription.Size = new System.Drawing.Size(226, 28);
            this.comboBox_DeviceDescription.TabIndex = 18;
            // 
            // comboBox_DeviceStationNum
            // 
            this.comboBox_DeviceStationNum.FormattingEnabled = true;
            this.comboBox_DeviceStationNum.Location = new System.Drawing.Point(849, 305);
            this.comboBox_DeviceStationNum.Name = "comboBox_DeviceStationNum";
            this.comboBox_DeviceStationNum.Size = new System.Drawing.Size(226, 28);
            this.comboBox_DeviceStationNum.TabIndex = 19;
            // 
            // comboBox_VariableName
            // 
            this.comboBox_VariableName.FormattingEnabled = true;
            this.comboBox_VariableName.Location = new System.Drawing.Point(849, 349);
            this.comboBox_VariableName.Name = "comboBox_VariableName";
            this.comboBox_VariableName.Size = new System.Drawing.Size(226, 28);
            this.comboBox_VariableName.TabIndex = 20;
            // 
            // comboBox_VariableDescription
            // 
            this.comboBox_VariableDescription.FormattingEnabled = true;
            this.comboBox_VariableDescription.Location = new System.Drawing.Point(849, 393);
            this.comboBox_VariableDescription.Name = "comboBox_VariableDescription";
            this.comboBox_VariableDescription.Size = new System.Drawing.Size(226, 28);
            this.comboBox_VariableDescription.TabIndex = 21;
            // 
            // comboBox_VariableDataType
            // 
            this.comboBox_VariableDataType.FormattingEnabled = true;
            this.comboBox_VariableDataType.Location = new System.Drawing.Point(849, 437);
            this.comboBox_VariableDataType.Name = "comboBox_VariableDataType";
            this.comboBox_VariableDataType.Size = new System.Drawing.Size(226, 28);
            this.comboBox_VariableDataType.TabIndex = 22;
            // 
            // comboBox_SymbolicAdd
            // 
            this.comboBox_SymbolicAdd.FormattingEnabled = true;
            this.comboBox_SymbolicAdd.Location = new System.Drawing.Point(849, 481);
            this.comboBox_SymbolicAdd.Name = "comboBox_SymbolicAdd";
            this.comboBox_SymbolicAdd.Size = new System.Drawing.Size(226, 28);
            this.comboBox_SymbolicAdd.TabIndex = 23;
            // 
            // comboBox_VariableDataLen
            // 
            this.comboBox_VariableDataLen.FormattingEnabled = true;
            this.comboBox_VariableDataLen.Location = new System.Drawing.Point(849, 525);
            this.comboBox_VariableDataLen.Name = "comboBox_VariableDataLen";
            this.comboBox_VariableDataLen.Size = new System.Drawing.Size(226, 28);
            this.comboBox_VariableDataLen.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(779, 148);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(238, 29);
            this.label12.TabIndex = 25;
            this.label12.Text = "SPA Content Column";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1554, 669);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.comboBox_VariableDataLen);
            this.Controls.Add(this.comboBox_SymbolicAdd);
            this.Controls.Add(this.comboBox_VariableDataType);
            this.Controls.Add(this.comboBox_VariableDescription);
            this.Controls.Add(this.comboBox_VariableName);
            this.Controls.Add(this.comboBox_DeviceStationNum);
            this.Controls.Add(this.comboBox_DeviceDescription);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.comboBox_DeviceName);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox_DeviceName;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBox_DeviceDescription;
        private System.Windows.Forms.ComboBox comboBox_DeviceStationNum;
        private System.Windows.Forms.ComboBox comboBox_VariableName;
        private System.Windows.Forms.ComboBox comboBox_VariableDescription;
        private System.Windows.Forms.ComboBox comboBox_VariableDataType;
        private System.Windows.Forms.ComboBox comboBox_SymbolicAdd;
        private System.Windows.Forms.ComboBox comboBox_VariableDataLen;
        private System.Windows.Forms.Label label12;
    }
}