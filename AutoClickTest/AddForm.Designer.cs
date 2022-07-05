
namespace AutoClickTest
{
    partial class AddForm
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
            this.Tool = new System.Windows.Forms.ComboBox();
            this.ToolName = new System.Windows.Forms.Label();
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.ActionLabel = new System.Windows.Forms.Label();
            this.ActionName = new System.Windows.Forms.ComboBox();
            this.KeyCode = new System.Windows.Forms.TextBox();
            this.KeyCodeLabel = new System.Windows.Forms.Label();
            this.MousePointX = new System.Windows.Forms.TextBox();
            this.MousePointLabel = new System.Windows.Forms.Label();
            this.MousePointY = new System.Windows.Forms.TextBox();
            this.SetMousePoint = new System.Windows.Forms.Button();
            this.DelayTitle = new System.Windows.Forms.Label();
            this.Delay = new System.Windows.Forms.TextBox();
            this.DelayLabel = new System.Windows.Forms.Label();
            this.MouseWheel = new System.Windows.Forms.TextBox();
            this.MouseWheelLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Tool
            // 
            this.Tool.FormattingEnabled = true;
            this.Tool.Location = new System.Drawing.Point(95, 20);
            this.Tool.Margin = new System.Windows.Forms.Padding(4);
            this.Tool.Name = "Tool";
            this.Tool.Size = new System.Drawing.Size(154, 27);
            this.Tool.TabIndex = 0;
            this.Tool.SelectedIndexChanged += new System.EventHandler(this.ActionID_SelectedIndexChanged);
            // 
            // ToolName
            // 
            this.ToolName.AutoSize = true;
            this.ToolName.Location = new System.Drawing.Point(17, 24);
            this.ToolName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ToolName.Name = "ToolName";
            this.ToolName.Size = new System.Drawing.Size(69, 19);
            this.ToolName.TabIndex = 1;
            this.ToolName.Text = "動作工具";
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(500, 118);
            this.Ok.Margin = new System.Windows.Forms.Padding(4);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(96, 29);
            this.Ok.TabIndex = 2;
            this.Ok.Text = "確定";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(604, 118);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(96, 29);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "取消";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ActionLabel
            // 
            this.ActionLabel.AutoSize = true;
            this.ActionLabel.Location = new System.Drawing.Point(37, 76);
            this.ActionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActionLabel.Name = "ActionLabel";
            this.ActionLabel.Size = new System.Drawing.Size(39, 19);
            this.ActionLabel.TabIndex = 4;
            this.ActionLabel.Text = "動作";
            // 
            // ActionName
            // 
            this.ActionName.FormattingEnabled = true;
            this.ActionName.Location = new System.Drawing.Point(95, 73);
            this.ActionName.Margin = new System.Windows.Forms.Padding(4);
            this.ActionName.Name = "ActionName";
            this.ActionName.Size = new System.Drawing.Size(154, 27);
            this.ActionName.TabIndex = 5;
            this.ActionName.SelectedIndexChanged += new System.EventHandler(this.ActionName_SelectedIndexChanged);
            // 
            // KeyCode
            // 
            this.KeyCode.Location = new System.Drawing.Point(267, 73);
            this.KeyCode.Name = "KeyCode";
            this.KeyCode.Size = new System.Drawing.Size(30, 27);
            this.KeyCode.TabIndex = 6;
            // 
            // KeyCodeLabel
            // 
            this.KeyCodeLabel.AutoSize = true;
            this.KeyCodeLabel.Location = new System.Drawing.Point(307, 77);
            this.KeyCodeLabel.Name = "KeyCodeLabel";
            this.KeyCodeLabel.Size = new System.Drawing.Size(24, 19);
            this.KeyCodeLabel.TabIndex = 7;
            this.KeyCodeLabel.Text = "鍵";
            // 
            // MousePointX
            // 
            this.MousePointX.Location = new System.Drawing.Point(267, 73);
            this.MousePointX.Name = "MousePointX";
            this.MousePointX.Size = new System.Drawing.Size(50, 27);
            this.MousePointX.TabIndex = 8;
            // 
            // MousePointLabel
            // 
            this.MousePointLabel.AutoSize = true;
            this.MousePointLabel.Location = new System.Drawing.Point(307, 77);
            this.MousePointLabel.Name = "MousePointLabel";
            this.MousePointLabel.Size = new System.Drawing.Size(12, 19);
            this.MousePointLabel.TabIndex = 9;
            this.MousePointLabel.Text = ",";
            // 
            // MousePointY
            // 
            this.MousePointY.Location = new System.Drawing.Point(325, 73);
            this.MousePointY.Name = "MousePointY";
            this.MousePointY.Size = new System.Drawing.Size(50, 27);
            this.MousePointY.TabIndex = 10;
            // 
            // SetMousePoint
            // 
            this.SetMousePoint.Location = new System.Drawing.Point(381, 72);
            this.SetMousePoint.Name = "SetMousePoint";
            this.SetMousePoint.Size = new System.Drawing.Size(108, 29);
            this.SetMousePoint.TabIndex = 11;
            this.SetMousePoint.Text = "選擇滑鼠座標";
            this.SetMousePoint.UseVisualStyleBackColor = true;
            this.SetMousePoint.Click += new System.EventHandler(this.SetMousePoint_Click);
            // 
            // DelayTitle
            // 
            this.DelayTitle.AutoSize = true;
            this.DelayTitle.Location = new System.Drawing.Point(37, 118);
            this.DelayTitle.Name = "DelayTitle";
            this.DelayTitle.Size = new System.Drawing.Size(39, 19);
            this.DelayTitle.TabIndex = 12;
            this.DelayTitle.Text = "延遲";
            // 
            // Delay
            // 
            this.Delay.Location = new System.Drawing.Point(95, 115);
            this.Delay.Name = "Delay";
            this.Delay.Size = new System.Drawing.Size(70, 27);
            this.Delay.TabIndex = 13;
            // 
            // DelayLabel
            // 
            this.DelayLabel.AutoSize = true;
            this.DelayLabel.Location = new System.Drawing.Point(181, 118);
            this.DelayLabel.Name = "DelayLabel";
            this.DelayLabel.Size = new System.Drawing.Size(39, 19);
            this.DelayLabel.TabIndex = 14;
            this.DelayLabel.Text = "毫秒";
            // 
            // MouseWheel
            // 
            this.MouseWheel.Location = new System.Drawing.Point(267, 74);
            this.MouseWheel.Name = "MouseWheel";
            this.MouseWheel.Size = new System.Drawing.Size(50, 27);
            this.MouseWheel.TabIndex = 15;
            // 
            // MouseWheelLabel
            // 
            this.MouseWheelLabel.AutoSize = true;
            this.MouseWheelLabel.Location = new System.Drawing.Point(323, 77);
            this.MouseWheelLabel.Name = "MouseWheelLabel";
            this.MouseWheelLabel.Size = new System.Drawing.Size(24, 19);
            this.MouseWheelLabel.TabIndex = 16;
            this.MouseWheelLabel.Text = "次";
            // 
            // AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 162);
            this.Controls.Add(this.MouseWheelLabel);
            this.Controls.Add(this.MouseWheel);
            this.Controls.Add(this.DelayLabel);
            this.Controls.Add(this.Delay);
            this.Controls.Add(this.DelayTitle);
            this.Controls.Add(this.SetMousePoint);
            this.Controls.Add(this.MousePointY);
            this.Controls.Add(this.MousePointLabel);
            this.Controls.Add(this.MousePointX);
            this.Controls.Add(this.KeyCodeLabel);
            this.Controls.Add(this.KeyCode);
            this.Controls.Add(this.ActionName);
            this.Controls.Add(this.ActionLabel);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.ToolName);
            this.Controls.Add(this.Tool);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AddForm";
            this.Text = "AddForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ComboBox Tool;
        private System.Windows.Forms.Label ToolName;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label ActionLabel;
        private System.Windows.Forms.ComboBox ActionName;
        private System.Windows.Forms.TextBox KeyCode;
        private System.Windows.Forms.Label KeyCodeLabel;
        private System.Windows.Forms.TextBox MousePointX;
        private System.Windows.Forms.Label MousePointLabel;
        private System.Windows.Forms.TextBox MousePointY;
        private System.Windows.Forms.Button SetMousePoint;
        private System.Windows.Forms.Label DelayTitle;
        private System.Windows.Forms.TextBox Delay;
        private System.Windows.Forms.Label DelayLabel;
        private System.Windows.Forms.TextBox MouseWheel;
        private System.Windows.Forms.Label MouseWheelLabel;
    }
}