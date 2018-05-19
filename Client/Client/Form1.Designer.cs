namespace Client
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.getScrn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.statusLabel = new System.Windows.Forms.Label();
            this.netPing = new System.Windows.Forms.Button();
            this.reboot = new System.Windows.Forms.Button();
            this.shutdown = new System.Windows.Forms.Button();
            this.screenBox = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteТекущуюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenBox)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // getScrn
            // 
            this.getScrn.Location = new System.Drawing.Point(6, 6);
            this.getScrn.Name = "getScrn";
            this.getScrn.Size = new System.Drawing.Size(248, 33);
            this.getScrn.TabIndex = 0;
            this.getScrn.Text = "Получить скриншот";
            this.getScrn.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1051, 479);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.statusLabel);
            this.tabPage1.Controls.Add(this.netPing);
            this.tabPage1.Controls.Add(this.reboot);
            this.tabPage1.Controls.Add(this.shutdown);
            this.tabPage1.Controls.Add(this.getScrn);
            this.tabPage1.Controls.Add(this.screenBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1043, 453);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // statusLabel
            // 
            this.statusLabel.Location = new System.Drawing.Point(3, 381);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(251, 66);
            this.statusLabel.TabIndex = 5;
            this.statusLabel.Text = "label1";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // netPing
            // 
            this.netPing.Location = new System.Drawing.Point(6, 161);
            this.netPing.Name = "netPing";
            this.netPing.Size = new System.Drawing.Size(248, 33);
            this.netPing.TabIndex = 4;
            this.netPing.Text = "Включить NetPing";
            this.netPing.UseVisualStyleBackColor = true;
            // 
            // reboot
            // 
            this.reboot.Location = new System.Drawing.Point(6, 105);
            this.reboot.Name = "reboot";
            this.reboot.Size = new System.Drawing.Size(248, 33);
            this.reboot.TabIndex = 3;
            this.reboot.Text = "Перезагрузить";
            this.reboot.UseVisualStyleBackColor = true;
            // 
            // shutdown
            // 
            this.shutdown.Location = new System.Drawing.Point(6, 66);
            this.shutdown.Name = "shutdown";
            this.shutdown.Size = new System.Drawing.Size(248, 33);
            this.shutdown.TabIndex = 2;
            this.shutdown.Text = "Выключить";
            this.shutdown.UseVisualStyleBackColor = true;
            // 
            // screenBox
            // 
            this.screenBox.Location = new System.Drawing.Point(260, 6);
            this.screenBox.Name = "screenBox";
            this.screenBox.Size = new System.Drawing.Size(777, 441);
            this.screenBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.screenBox.TabIndex = 1;
            this.screenBox.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.deleteТекущуюToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(172, 70);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.addToolStripMenuItem.Text = "Добавить";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // deleteТекущуюToolStripMenuItem
            // 
            this.deleteТекущуюToolStripMenuItem.Name = "deleteТекущуюToolStripMenuItem";
            this.deleteТекущуюToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.deleteТекущуюToolStripMenuItem.Text = "Удалить текущую";
            this.deleteТекущуюToolStripMenuItem.Click += new System.EventHandler(this.deleteТекущуюToolStripMenuItem_Click);
            // 
            // settingButton
            // 
            this.settingButton.FlatAppearance.BorderSize = 0;
            this.settingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingButton.Image = ((System.Drawing.Image)(resources.GetObject("settingButton.Image")));
            this.settingButton.Location = new System.Drawing.Point(1032, 4);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(18, 18);
            this.settingButton.TabIndex = 3;
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.settingButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1055, 486);
            this.Controls.Add(this.settingButton);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Управление станциями";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.screenBox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button getScrn;
        private System.Windows.Forms.PictureBox screenBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button settingButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteТекущуюToolStripMenuItem;
        private System.Windows.Forms.Button netPing;
        private System.Windows.Forms.Button reboot;
        private System.Windows.Forms.Button shutdown;
        private System.Windows.Forms.Label statusLabel;
    }
}

