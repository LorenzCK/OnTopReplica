namespace OnTopReplica.SidePanels {
    partial class GroupSwitchPanel {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonEnable = new System.Windows.Forms.Button();
            this.listWindows = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelStatus);
            this.groupBox1.Controls.Add(this.buttonCancel);
            this.groupBox1.Controls.Add(this.buttonEnable);
            this.groupBox1.Controls.Add(this.listWindows);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 592);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Group switch mode:";
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.Location = new System.Drawing.Point(3, 543);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(952, 17);
            this.labelStatus.TabIndex = 3;
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(885, 563);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(70, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = global::OnTopReplica.Strings.GroupSwitchModeDisableButton;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.Cancel_click);
            // 
            // buttonEnable
            // 
            this.buttonEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEnable.Image = global::OnTopReplica.Properties.Resources.xiao_ok;
            this.buttonEnable.Location = new System.Drawing.Point(745, 563);
            this.buttonEnable.Name = "buttonEnable";
            this.buttonEnable.Size = new System.Drawing.Size(134, 23);
            this.buttonEnable.TabIndex = 1;
            this.buttonEnable.Text = global::OnTopReplica.Strings.GroupSwitchModeEnableButton;
            this.buttonEnable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonEnable.UseVisualStyleBackColor = true;
            this.buttonEnable.Click += new System.EventHandler(this.Enable_click);
            // 
            // listWindows
            // 
            this.listWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listWindows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
            this.listWindows.FullRowSelect = true;
            this.listWindows.GridLines = true;
            this.listWindows.HideSelection = false;
            this.listWindows.LabelWrap = false;
            this.listWindows.Location = new System.Drawing.Point(6, 19);
            this.listWindows.Name = "listWindows";
            this.listWindows.Size = new System.Drawing.Size(949, 521);
            this.listWindows.TabIndex = 0;
            this.listWindows.UseCompatibleStateImageBehavior = false;
            this.listWindows.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = global::OnTopReplica.Strings.GroupSwitchModeWindows;
            this.colName.Width = 220;
            // 
            // GroupSwitchPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(220, 220);
            this.Name = "GroupSwitchPanel";
            this.Size = new System.Drawing.Size(961, 592);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonEnable;
        private System.Windows.Forms.ListView listWindows;
        private System.Windows.Forms.ColumnHeader colName;
    }
}
