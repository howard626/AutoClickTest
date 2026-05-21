
namespace AutoClickTest
{
    partial class AutoClick
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Add = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.Label();
            this.Start = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.btnRecord = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.Test = new System.Windows.Forms.Button();
            this.MoveMouse = new System.Windows.Forms.Button();
            this.PointY = new System.Windows.Forms.TextBox();
            this.X = new System.Windows.Forms.Label();
            this.Y = new System.Windows.Forms.Label();
            this.PointX = new System.Windows.Forms.TextBox();
            this.ActionListPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.Clear = new System.Windows.Forms.Button();
            this.ActionBlocksPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(15, 15);
            this.Add.Margin = new System.Windows.Forms.Padding(4);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(96, 29);
            this.Add.TabIndex = 0;
            this.Add.Text = "增加動作";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.AddAction);
            // 
            // txtResult
            // 
            this.txtResult.AutoSize = true;
            this.txtResult.Location = new System.Drawing.Point(1200, 52);
            this.txtResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(37, 19);
            this.txtResult.TabIndex = 2;
            this.txtResult.Text = "Test";
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(516, 526);
            this.Start.Margin = new System.Windows.Forms.Padding(4);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(96, 29);
            this.Start.TabIndex = 3;
            this.Start.Text = "開始";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(751, 526);
            this.Stop.Margin = new System.Windows.Forms.Padding(4);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(96, 29);
            this.Stop.TabIndex = 4;
            this.Stop.Text = "停止";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.Location = new System.Drawing.Point(400, 526);
            this.btnRecord.Margin = new System.Windows.Forms.Padding(4);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(96, 29);
            this.btnRecord.TabIndex = 16;
            this.btnRecord.Text = "🔴 錄製";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(860, 526);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(96, 29);
            this.btnExport.TabIndex = 14;
            this.btnExport.Text = "匯出腳本";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(970, 526);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(96, 29);
            this.btnImport.TabIndex = 15;
            this.btnImport.Text = "匯入腳本";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(1200, 128);
            this.Test.Margin = new System.Windows.Forms.Padding(4);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(120, 29);
            this.Test.TabIndex = 5;
            this.Test.Text = "滑鼠座標測試";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // MoveMouse
            // 
            this.MoveMouse.Location = new System.Drawing.Point(1200, 257);
            this.MoveMouse.Margin = new System.Windows.Forms.Padding(4);
            this.MoveMouse.Name = "MoveMouse";
            this.MoveMouse.Size = new System.Drawing.Size(120, 29);
            this.MoveMouse.TabIndex = 6;
            this.MoveMouse.Text = "移動滑鼠座標";
            this.MoveMouse.UseVisualStyleBackColor = true;
            this.MoveMouse.Click += new System.EventHandler(this.MoveMouse_Click);
            // 
            // PointY
            // 
            this.PointY.Location = new System.Drawing.Point(1250, 220);
            this.PointY.Margin = new System.Windows.Forms.Padding(4);
            this.PointY.Name = "PointY";
            this.PointY.Size = new System.Drawing.Size(72, 27);
            this.PointY.TabIndex = 7;
            // 
            // X
            // 
            this.X.AutoSize = true;
            this.X.Location = new System.Drawing.Point(1200, 180);
            this.X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(19, 19);
            this.X.TabIndex = 8;
            this.X.Text = "X";
            // 
            // Y
            // 
            this.Y.AutoSize = true;
            this.Y.Location = new System.Drawing.Point(1200, 224);
            this.Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(18, 19);
            this.Y.TabIndex = 9;
            this.Y.Text = "Y";
            // 
            // PointX
            // 
            this.PointX.Location = new System.Drawing.Point(1250, 176);
            this.PointX.Margin = new System.Windows.Forms.Padding(4);
            this.PointX.Name = "PointX";
            this.PointX.Size = new System.Drawing.Size(72, 27);
            this.PointX.TabIndex = 10;
            // 
            // ActionListPanel
            // 
            this.ActionListPanel.AllowDrop = true;
            this.ActionListPanel.AutoScroll = true;
            this.ActionListPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ActionListPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ActionListPanel.Location = new System.Drawing.Point(580, 52);
            this.ActionListPanel.Name = "ActionListPanel";
            this.ActionListPanel.Size = new System.Drawing.Size(600, 432);
            this.ActionListPanel.TabIndex = 11;
            this.ActionListPanel.WrapContents = false;
            this.ActionListPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.ActionList_DragDrop);
            this.ActionListPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.ActionList_DragEnter);
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(636, 526);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(94, 29);
            this.Clear.TabIndex = 12;
            this.Clear.Text = "清除";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // ActionBlocksPanel
            // 
            this.ActionBlocksPanel.Location = new System.Drawing.Point(15, 52);
            this.ActionBlocksPanel.Name = "ActionBlocksPanel";
            this.ActionBlocksPanel.Size = new System.Drawing.Size(550, 432);
            this.ActionBlocksPanel.TabIndex = 13;
            this.ActionBlocksPanel.WrapContents = false;
            this.ActionBlocksPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            // 
            // AutoClick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 650);
            this.Controls.Add(this.ActionBlocksPanel);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.ActionListPanel);
            this.Controls.Add(this.PointX);
            this.Controls.Add(this.Y);
            this.Controls.Add(this.X);
            this.Controls.Add(this.PointY);
            this.Controls.Add(this.MoveMouse);
            this.Controls.Add(this.Test);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.Add);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AutoClick";
            this.Text = "AutoClick";
            this.Load += new System.EventHandler(this.AutoClick_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Label txtResult;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.Button MoveMouse;
        private System.Windows.Forms.TextBox PointY;
        private System.Windows.Forms.Label X;
        private System.Windows.Forms.Label Y;
        private System.Windows.Forms.TextBox PointX;
        private System.Windows.Forms.FlowLayoutPanel ActionListPanel;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.FlowLayoutPanel ActionBlocksPanel;
    }

    
}

