namespace Jazz2TAS
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
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemNew = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItemEdit = new System.Windows.Forms.MenuItem();
            this.menuItemOffset = new System.Windows.Forms.MenuItem();
            this.groupBoxLevels = new System.Windows.Forms.GroupBox();
            this.dataGridViewLevels = new System.Windows.Forms.DataGridView();
            this.ColumnLevelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMoveUp = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnMoveDown = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBoxInputs = new System.Windows.Forms.GroupBox();
            this.dataGridViewInputs = new System.Windows.Forms.DataGridView();
            this.ColumnFrame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLeft = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnRight = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnUp = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnDown = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnJump = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnShoot = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnRun = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnGun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFiller = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.groupBoxPositionHistory = new System.Windows.Forms.GroupBox();
            this.dataGridViewPositionHistory = new System.Windows.Forms.DataGridView();
            this.ColumnPositionFrame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPositionX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPositionY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPositionFiller = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonPlayPause = new System.Windows.Forms.Button();
            this.buttonFrameAdvance = new System.Windows.Forms.Button();
            this.groupBoxLevels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLevels)).BeginInit();
            this.groupBoxInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInputs)).BeginInit();
            this.groupBoxPositionHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPositionHistory)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemEdit});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNew,
            this.menuItemOpen,
            this.menuItemSave,
            this.menuItemSaveAs});
            this.menuItemFile.Text = "File";
            // 
            // menuItemNew
            // 
            this.menuItemNew.Index = 0;
            this.menuItemNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.menuItemNew.Text = "New";
            this.menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 1;
            this.menuItemOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.menuItemOpen.Text = "Open...";
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Index = 2;
            this.menuItemSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuItemSave.Text = "Save";
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemSaveAs
            // 
            this.menuItemSaveAs.Index = 3;
            this.menuItemSaveAs.Text = "Save as...";
            this.menuItemSaveAs.Click += new System.EventHandler(this.menuItemSaveAs_Click);
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.Index = 1;
            this.menuItemEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOffset});
            this.menuItemEdit.Text = "Edit";
            // 
            // menuItemOffset
            // 
            this.menuItemOffset.Index = 0;
            this.menuItemOffset.Text = "Offset selected inputs...";
            this.menuItemOffset.Click += new System.EventHandler(this.menuItemOffset_Click);
            // 
            // groupBoxLevels
            // 
            this.groupBoxLevels.Controls.Add(this.dataGridViewLevels);
            this.groupBoxLevels.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBoxLevels.Location = new System.Drawing.Point(4, 4);
            this.groupBoxLevels.Name = "groupBoxLevels";
            this.groupBoxLevels.Size = new System.Drawing.Size(240, 416);
            this.groupBoxLevels.TabIndex = 0;
            this.groupBoxLevels.TabStop = false;
            this.groupBoxLevels.Text = "Levels";
            // 
            // dataGridViewLevels
            // 
            this.dataGridViewLevels.AllowUserToResizeRows = false;
            this.dataGridViewLevels.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewLevels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLevels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnLevelName,
            this.ColumnMoveUp,
            this.ColumnMoveDown,
            this.ColumnDelete});
            this.dataGridViewLevels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLevels.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewLevels.MultiSelect = false;
            this.dataGridViewLevels.Name = "dataGridViewLevels";
            this.dataGridViewLevels.RowHeadersVisible = false;
            this.dataGridViewLevels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLevels.Size = new System.Drawing.Size(234, 397);
            this.dataGridViewLevels.TabIndex = 0;
            this.dataGridViewLevels.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLevels_CellClick);
            this.dataGridViewLevels.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewLevels_CellPainting);
            this.dataGridViewLevels.SelectionChanged += new System.EventHandler(this.dataGridViewLevels_SelectionChanged);
            // 
            // ColumnLevelName
            // 
            this.ColumnLevelName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnLevelName.DataPropertyName = "LevelName";
            this.ColumnLevelName.HeaderText = "Level name";
            this.ColumnLevelName.Name = "ColumnLevelName";
            // 
            // ColumnMoveUp
            // 
            this.ColumnMoveUp.HeaderText = "";
            this.ColumnMoveUp.Name = "ColumnMoveUp";
            this.ColumnMoveUp.Width = 24;
            // 
            // ColumnMoveDown
            // 
            this.ColumnMoveDown.HeaderText = "";
            this.ColumnMoveDown.Name = "ColumnMoveDown";
            this.ColumnMoveDown.Width = 24;
            // 
            // ColumnDelete
            // 
            this.ColumnDelete.HeaderText = "";
            this.ColumnDelete.Name = "ColumnDelete";
            this.ColumnDelete.Width = 24;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(244, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 416);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // groupBoxInputs
            // 
            this.groupBoxInputs.Controls.Add(this.dataGridViewInputs);
            this.groupBoxInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxInputs.Location = new System.Drawing.Point(247, 4);
            this.groupBoxInputs.Name = "groupBoxInputs";
            this.groupBoxInputs.Size = new System.Drawing.Size(450, 416);
            this.groupBoxInputs.TabIndex = 2;
            this.groupBoxInputs.TabStop = false;
            this.groupBoxInputs.Text = "Inputs";
            // 
            // dataGridViewInputs
            // 
            this.dataGridViewInputs.AllowUserToResizeRows = false;
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
            this.ColumnGun,
            this.ColumnFiller});
            this.dataGridViewInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewInputs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewInputs.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewInputs.Name = "dataGridViewInputs";
            this.dataGridViewInputs.RowHeadersVisible = false;
            this.dataGridViewInputs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewInputs.Size = new System.Drawing.Size(444, 397);
            this.dataGridViewInputs.TabIndex = 1;
            this.dataGridViewInputs.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridViewInputs_RowPrePaint);
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
            // ColumnGun
            // 
            this.ColumnGun.DataPropertyName = "Gun";
            this.ColumnGun.HeaderText = "Gun";
            this.ColumnGun.Name = "ColumnGun";
            this.ColumnGun.Width = 40;
            // 
            // ColumnFiller
            // 
            this.ColumnFiller.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnFiller.HeaderText = "";
            this.ColumnFiller.Name = "ColumnFiller";
            this.ColumnFiller.ReadOnly = true;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(697, 4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 416);
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // groupBoxPositionHistory
            // 
            this.groupBoxPositionHistory.Controls.Add(this.dataGridViewPositionHistory);
            this.groupBoxPositionHistory.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBoxPositionHistory.Location = new System.Drawing.Point(700, 4);
            this.groupBoxPositionHistory.Name = "groupBoxPositionHistory";
            this.groupBoxPositionHistory.Size = new System.Drawing.Size(240, 416);
            this.groupBoxPositionHistory.TabIndex = 2;
            this.groupBoxPositionHistory.TabStop = false;
            this.groupBoxPositionHistory.Text = "Position history";
            // 
            // dataGridViewPositionHistory
            // 
            this.dataGridViewPositionHistory.AllowUserToAddRows = false;
            this.dataGridViewPositionHistory.AllowUserToDeleteRows = false;
            this.dataGridViewPositionHistory.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewPositionHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPositionHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnPositionFrame,
            this.ColumnPositionX,
            this.ColumnPositionY,
            this.ColumnPositionFiller});
            this.dataGridViewPositionHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPositionHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewPositionHistory.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewPositionHistory.Name = "dataGridViewPositionHistory";
            this.dataGridViewPositionHistory.RowHeadersVisible = false;
            this.dataGridViewPositionHistory.Size = new System.Drawing.Size(234, 397);
            this.dataGridViewPositionHistory.TabIndex = 1;
            // 
            // ColumnPositionFrame
            // 
            this.ColumnPositionFrame.HeaderText = "Frame";
            this.ColumnPositionFrame.Name = "ColumnPositionFrame";
            this.ColumnPositionFrame.Width = 48;
            // 
            // ColumnPositionX
            // 
            this.ColumnPositionX.HeaderText = "X and speed";
            this.ColumnPositionX.Name = "ColumnPositionX";
            this.ColumnPositionX.Width = 72;
            // 
            // ColumnPositionY
            // 
            this.ColumnPositionY.HeaderText = "Y and speed";
            this.ColumnPositionY.Name = "ColumnPositionY";
            this.ColumnPositionY.Width = 72;
            // 
            // ColumnPositionFiller
            // 
            this.ColumnPositionFiller.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnPositionFiller.HeaderText = "";
            this.ColumnPositionFiller.Name = "ColumnPositionFiller";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.buttonFrameAdvance);
            this.panelBottom.Controls.Add(this.buttonPlayPause);
            this.panelBottom.Controls.Add(this.labelInfo);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(4, 420);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(936, 32);
            this.panelBottom.TabIndex = 2;
            // 
            // labelInfo
            // 
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelInfo.Location = new System.Drawing.Point(0, 0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(192, 32);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonPlayPause
            // 
            this.buttonPlayPause.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonPlayPause.Location = new System.Drawing.Point(192, 0);
            this.buttonPlayPause.Name = "buttonPlayPause";
            this.buttonPlayPause.Size = new System.Drawing.Size(64, 32);
            this.buttonPlayPause.TabIndex = 1;
            this.buttonPlayPause.Text = "Pause";
            this.buttonPlayPause.UseVisualStyleBackColor = true;
            this.buttonPlayPause.Click += new System.EventHandler(this.buttonPlayPause_Click);
            // 
            // buttonFrameAdvance
            // 
            this.buttonFrameAdvance.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonFrameAdvance.Location = new System.Drawing.Point(256, 0);
            this.buttonFrameAdvance.Name = "buttonFrameAdvance";
            this.buttonFrameAdvance.Size = new System.Drawing.Size(64, 32);
            this.buttonFrameAdvance.TabIndex = 2;
            this.buttonFrameAdvance.Text = ">>";
            this.buttonFrameAdvance.UseVisualStyleBackColor = true;
            this.buttonFrameAdvance.Click += new System.EventHandler(this.buttonFrameAdvance_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 456);
            this.Controls.Add(this.groupBoxInputs);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.groupBoxLevels);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.groupBoxPositionHistory);
            this.Controls.Add(this.panelBottom);
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowIcon = false;
            this.groupBoxLevels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLevels)).EndInit();
            this.groupBoxInputs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInputs)).EndInit();
            this.groupBoxPositionHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPositionHistory)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItemFile;
        private System.Windows.Forms.MenuItem menuItemNew;
        private System.Windows.Forms.MenuItem menuItemOpen;
        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemSaveAs;
        private System.Windows.Forms.GroupBox groupBoxLevels;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBoxInputs;
        private System.Windows.Forms.DataGridView dataGridViewLevels;
        private System.Windows.Forms.DataGridView dataGridViewInputs;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLevelName;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnMoveUp;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnMoveDown;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnDelete;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.GroupBox groupBoxPositionHistory;
        private System.Windows.Forms.DataGridView dataGridViewPositionHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPositionFrame;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPositionX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPositionY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPositionFiller;
        private System.Windows.Forms.MenuItem menuItemEdit;
        private System.Windows.Forms.MenuItem menuItemOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFrame;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnLeft;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnRight;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnUp;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnDown;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnJump;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnShoot;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGun;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFiller;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button buttonPlayPause;
        private System.Windows.Forms.Button buttonFrameAdvance;
    }
}