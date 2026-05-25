
using System.Drawing;
using System.Windows.Forms;

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
            this.components = new System.ComponentModel.Container();
            this.Add = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.Label();
            this.lblLoopCount = new System.Windows.Forms.Label();
            this.numLoopCount = new System.Windows.Forms.NumericUpDown();
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
            this.btnNavTest = new System.Windows.Forms.Button();
            this.btnFishTest = new System.Windows.Forms.Button();
            this.RightPanel = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.numLoopCount)).BeginInit();
            this.SuspendLayout();

            // 視窗主體
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 720);
            this.Name = "AutoClick";
            this.Text = "AutoClick 腳本編輯器";

            // 連結 Load 事件 (這是讓左側動作回來的關鍵)
            this.Load += new System.EventHandler(this.AutoClick_Load);

            // Add 按鈕
            this.Add.Location = new System.Drawing.Point(15, 12);
            this.Add.Size = new System.Drawing.Size(96, 29);
            this.Add.Text = "增加動作";
            this.Add.Click += new System.EventHandler(this.AddAction);

            // ActionBlocksPanel (左側)
            this.ActionBlocksPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ActionBlocksPanel.Location = new System.Drawing.Point(15, 52);
            this.ActionBlocksPanel.Size = new System.Drawing.Size(550, 432);
            this.ActionBlocksPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ActionBlocksPanel.WrapContents = false;
            this.ActionBlocksPanel.AutoScroll = true;

            // ActionListPanel (中間)
            this.ActionListPanel.AllowDrop = true;
            this.ActionListPanel.AutoScroll = true;
            this.ActionListPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ActionListPanel.Location = new System.Drawing.Point(580, 52);
            this.ActionListPanel.Size = new System.Drawing.Size(580, 432);
            this.ActionListPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ActionListPanel.WrapContents = false;
            this.ActionListPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.ActionList_DragDrop);
            this.ActionListPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.ActionList_DragEnter);

            // RightPanel (右側)
            this.RightPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RightPanel.Location = new System.Drawing.Point(1175, 52);
            this.RightPanel.Size = new System.Drawing.Size(200, 432);
            this.RightPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.RightPanel.WrapContents = false;
            this.RightPanel.Padding = new Padding(10);

            // 右側按鈕與座標
            this.Test.Text = "滑鼠座標測試"; this.Test.Size = new Size(170, 30); this.Test.Click += Test_Click;
            this.btnNavTest.Text = "導航方位測試"; this.btnNavTest.Size = new Size(170, 30);
            this.MoveMouse.Text = "移動滑鼠座標"; this.MoveMouse.Size = new Size(170, 30); this.MoveMouse.Click += MoveMouse_Click;
            this.btnFishTest.Text = "方舟釣魚測試"; this.btnFishTest.Size = new Size(170, 30);
            this.X.Text = "X:"; this.Y.Text = "Y:"; this.PointX.Size = new Size(70, 25); this.PointY.Size = new Size(70, 25);
            this.txtResult.Text = "座標資訊";
            this.RightPanel.Controls.AddRange(new Control[] { this.Test, this.btnNavTest, this.MoveMouse, this.btnFishTest, this.X, this.PointX, this.Y, this.PointY, this.txtResult });

            // 底部按鈕
            int footerY = 510;
            this.btnRecord.Text = "🔴 錄製"; this.btnRecord.Location = new Point(15, footerY); this.btnRecord.Size = new Size(100, 35); this.btnRecord.Click += btnRecord_Click;
            this.Start.Text = "開始執行"; this.Start.Location = new Point(125, footerY); this.Start.Size = new Size(100, 35); this.Start.Click += Start_Click;
            this.Clear.Text = "清除清單"; this.Clear.Location = new Point(235, footerY); this.Clear.Size = new Size(100, 35); this.Clear.Click += Clear_Click;
            this.Stop.Text = "停止執行"; this.Stop.Location = new Point(345, footerY); this.Stop.Size = new Size(100, 35); this.Stop.Click += Stop_Click;
            this.btnExport.Text = "匯出腳本"; this.btnExport.Location = new Point(455, footerY); this.btnExport.Size = new Size(100, 35); this.btnExport.Click += btnExport_Click;
            this.btnImport.Text = "匯入腳本"; this.btnImport.Location = new Point(565, footerY); this.btnImport.Size = new Size(100, 35); this.btnImport.Click += btnImport_Click;

            this.lblLoopCount.Text = "重複次數 (0=無限):"; this.lblLoopCount.Location = new Point(15, footerY + 50); this.lblLoopCount.AutoSize = true;
            this.numLoopCount.Location = new Point(150, footerY + 48); this.numLoopCount.Size = new Size(80, 25);

            // 加入控制項
            this.Controls.AddRange(new Control[] { this.ActionBlocksPanel, this.ActionListPanel, this.RightPanel, this.Add, this.btnRecord, this.Start, this.Clear, this.Stop, this.btnExport, this.btnImport, this.lblLoopCount, this.numLoopCount });

            ((System.ComponentModel.ISupportInitialize)(this.numLoopCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Label txtResult;
        private System.Windows.Forms.Label lblLoopCount;
        private System.Windows.Forms.NumericUpDown numLoopCount;
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
        private System.Windows.Forms.Button btnNavTest;
        private System.Windows.Forms.Button btnFishTest;
        private System.Windows.Forms.FlowLayoutPanel RightPanel;
        private System.Windows.Forms.FlowLayoutPanel ActionBlocksPanel;
    }

    
}

