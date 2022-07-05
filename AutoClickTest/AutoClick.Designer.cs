
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
            this.Test = new System.Windows.Forms.Button();
            this.MoveMouse = new System.Windows.Forms.Button();
            this.PointY = new System.Windows.Forms.TextBox();
            this.X = new System.Windows.Forms.Label();
            this.Y = new System.Windows.Forms.Label();
            this.PointX = new System.Windows.Forms.TextBox();
            this.ActionList = new System.Windows.Forms.TreeView();
            this.Clear = new System.Windows.Forms.Button();
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
            this.txtResult.Location = new System.Drawing.Point(841, 52);
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
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(841, 128);
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
            this.MoveMouse.Location = new System.Drawing.Point(841, 257);
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
            this.PointY.Location = new System.Drawing.Point(887, 220);
            this.PointY.Margin = new System.Windows.Forms.Padding(4);
            this.PointY.Name = "PointY";
            this.PointY.Size = new System.Drawing.Size(72, 27);
            this.PointY.TabIndex = 7;
            // 
            // X
            // 
            this.X.AutoSize = true;
            this.X.Location = new System.Drawing.Point(841, 180);
            this.X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(19, 19);
            this.X.TabIndex = 8;
            this.X.Text = "X";
            // 
            // Y
            // 
            this.Y.AutoSize = true;
            this.Y.Location = new System.Drawing.Point(841, 224);
            this.Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(18, 19);
            this.Y.TabIndex = 9;
            this.Y.Text = "Y";
            // 
            // PointX
            // 
            this.PointX.Location = new System.Drawing.Point(887, 176);
            this.PointX.Margin = new System.Windows.Forms.Padding(4);
            this.PointX.Name = "PointX";
            this.PointX.Size = new System.Drawing.Size(72, 27);
            this.PointX.TabIndex = 10;
            // 
            // ActionList
            // 
            this.ActionList.Location = new System.Drawing.Point(15, 52);
            this.ActionList.Name = "ActionList";
            this.ActionList.Size = new System.Drawing.Size(779, 432);
            this.ActionList.TabIndex = 11;
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
            // AutoClick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 570);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.ActionList);
            this.Controls.Add(this.PointX);
            this.Controls.Add(this.Y);
            this.Controls.Add(this.X);
            this.Controls.Add(this.PointY);
            this.Controls.Add(this.MoveMouse);
            this.Controls.Add(this.Test);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.Start);
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
        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.Button MoveMouse;
        private System.Windows.Forms.TextBox PointY;
        private System.Windows.Forms.Label X;
        private System.Windows.Forms.Label Y;
        private System.Windows.Forms.TextBox PointX;
        private System.Windows.Forms.TreeView ActionList;
        private System.Windows.Forms.Button Clear;
    }

    
}

