namespace Jazz2TAS
{
    partial class SequenceForm
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelRepeats = new System.Windows.Forms.Label();
            this.textBoxRepeats = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.dataGridViewInputs = new System.Windows.Forms.DataGridView();
            this.ColumnFrame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLeft = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnRight = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnUp = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnDown = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnJump = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnShoot = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnRun = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnFiller = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelLength = new System.Windows.Forms.Label();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInputs)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelRepeats, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxRepeats, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonCancel, 2, 4);
            this.tableLayoutPanel.Controls.Add(this.dataGridViewInputs, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonOK, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelLength, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxLength, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxName, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(384, 361);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelRepeats
            // 
            this.labelRepeats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRepeats.AutoSize = true;
            this.labelRepeats.Location = new System.Drawing.Point(3, 58);
            this.labelRepeats.Name = "labelRepeats";
            this.labelRepeats.Size = new System.Drawing.Size(131, 13);
            this.labelRepeats.TabIndex = 5;
            this.labelRepeats.Text = "Repeats:";
            // 
            // textBoxRepeats
            // 
            this.textBoxRepeats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.textBoxRepeats, 2);
            this.textBoxRepeats.Location = new System.Drawing.Point(140, 55);
            this.textBoxRepeats.Name = "textBoxRepeats";
            this.textBoxRepeats.Size = new System.Drawing.Size(241, 20);
            this.textBoxRepeats.TabIndex = 6;
            this.textBoxRepeats.Text = "1";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(253, 335);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(128, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Clear sequence";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // dataGridViewInputs
            // 
            this.dataGridViewInputs.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewInputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnFrame,
            this.ColumnLeft,
            this.ColumnRight,
            this.ColumnUp,
            this.ColumnDown,
            this.ColumnJump,
            this.ColumnShoot,
            this.ColumnRun,
            this.ColumnFiller});
            this.tableLayoutPanel.SetColumnSpan(this.dataGridViewInputs, 3);
            this.dataGridViewInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewInputs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewInputs.Location = new System.Drawing.Point(3, 81);
            this.dataGridViewInputs.Name = "dataGridViewInputs";
            this.dataGridViewInputs.RowHeadersVisible = false;
            this.dataGridViewInputs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewInputs.Size = new System.Drawing.Size(378, 248);
            this.dataGridViewInputs.TabIndex = 0;
            // 
            // ColumnFrame
            // 
            this.ColumnFrame.DataPropertyName = "Frame";
            this.ColumnFrame.HeaderText = "Frame";
            this.ColumnFrame.Name = "ColumnFrame";
            this.ColumnFrame.Width = 40;
            // 
            // ColumnLeft
            // 
            this.ColumnLeft.DataPropertyName = "Left";
            this.ColumnLeft.HeaderText = "Left";
            this.ColumnLeft.Name = "ColumnLeft";
            this.ColumnLeft.Width = 40;
            // 
            // ColumnRight
            // 
            this.ColumnRight.DataPropertyName = "Right";
            this.ColumnRight.HeaderText = "Right";
            this.ColumnRight.Name = "ColumnRight";
            this.ColumnRight.Width = 40;
            // 
            // ColumnUp
            // 
            this.ColumnUp.DataPropertyName = "Up";
            this.ColumnUp.HeaderText = "Up";
            this.ColumnUp.Name = "ColumnUp";
            this.ColumnUp.Width = 40;
            // 
            // ColumnDown
            // 
            this.ColumnDown.DataPropertyName = "Down";
            this.ColumnDown.HeaderText = "Down";
            this.ColumnDown.Name = "ColumnDown";
            this.ColumnDown.Width = 40;
            // 
            // ColumnJump
            // 
            this.ColumnJump.DataPropertyName = "Jump";
            this.ColumnJump.HeaderText = "Jump";
            this.ColumnJump.Name = "ColumnJump";
            this.ColumnJump.Width = 40;
            // 
            // ColumnShoot
            // 
            this.ColumnShoot.DataPropertyName = "Shoot";
            this.ColumnShoot.HeaderText = "Shoot";
            this.ColumnShoot.Name = "ColumnShoot";
            this.ColumnShoot.Width = 40;
            // 
            // ColumnRun
            // 
            this.ColumnRun.DataPropertyName = "Run";
            this.ColumnRun.HeaderText = "Run";
            this.ColumnRun.Name = "ColumnRun";
            this.ColumnRun.Width = 40;
            // 
            // ColumnFiller
            // 
            this.ColumnFiller.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnFiller.HeaderText = "";
            this.ColumnFiller.Name = "ColumnFiller";
            this.ColumnFiller.ReadOnly = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(172, 335);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelLength
            // 
            this.labelLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(3, 32);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(131, 13);
            this.labelLength.TabIndex = 3;
            this.labelLength.Text = "Sequence length: (frames)";
            // 
            // textBoxLength
            // 
            this.textBoxLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.textBoxLength, 2);
            this.textBoxLength.Location = new System.Drawing.Point(140, 29);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.Size = new System.Drawing.Size(241, 20);
            this.textBoxLength.TabIndex = 4;
            this.textBoxLength.Text = "10";
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 6);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(131, 13);
            this.labelName.TabIndex = 7;
            this.labelName.Text = "Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.textBoxName, 2);
            this.textBoxName.Location = new System.Drawing.Point(140, 3);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(241, 20);
            this.textBoxName.TabIndex = 8;
            // 
            // SequenceForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "SequenceForm";
            this.ShowIcon = false;
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInputs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView dataGridViewInputs;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.Label labelRepeats;
        private System.Windows.Forms.TextBox textBoxRepeats;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFrame;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnLeft;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnRight;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnUp;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnDown;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnJump;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnShoot;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFiller;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
    }
}