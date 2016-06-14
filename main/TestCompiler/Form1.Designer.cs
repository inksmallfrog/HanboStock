namespace TestCompiler
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.formulaBox = new System.Windows.Forms.RichTextBox();
            this.resultBox = new System.Windows.Forms.RichTextBox();
            this.compileButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // formulaBox
            // 
            this.formulaBox.Location = new System.Drawing.Point(12, 74);
            this.formulaBox.Name = "formulaBox";
            this.formulaBox.Size = new System.Drawing.Size(240, 239);
            this.formulaBox.TabIndex = 1;
            this.formulaBox.Text = "";
            // 
            // resultBox
            // 
            this.resultBox.Location = new System.Drawing.Point(303, 74);
            this.resultBox.Name = "resultBox";
            this.resultBox.Size = new System.Drawing.Size(246, 239);
            this.resultBox.TabIndex = 2;
            this.resultBox.Text = "";
            // 
            // compileButton
            // 
            this.compileButton.Location = new System.Drawing.Point(12, 28);
            this.compileButton.Name = "compileButton";
            this.compileButton.Size = new System.Drawing.Size(75, 23);
            this.compileButton.TabIndex = 3;
            this.compileButton.Text = "编译";
            this.compileButton.UseVisualStyleBackColor = true;
            this.compileButton.Click += new System.EventHandler(this.compileButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 325);
            this.Controls.Add(this.compileButton);
            this.Controls.Add(this.resultBox);
            this.Controls.Add(this.formulaBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox formulaBox;
        private System.Windows.Forms.RichTextBox resultBox;
        private System.Windows.Forms.Button compileButton;
    }
}

