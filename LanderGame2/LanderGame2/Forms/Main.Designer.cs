using System.ComponentModel;

namespace LanderGame
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.tbCtrlMain = new System.Windows.Forms.TabControl();
            this.tbPgHome = new System.Windows.Forms.TabPage();
            this.btnPlay = new System.Windows.Forms.Button();
            this.cmbBoxMap = new System.Windows.Forms.ComboBox();
            this.cmbBoxLander = new System.Windows.Forms.ComboBox();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.btnChat = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.tbPgChat = new System.Windows.Forms.TabPage();
            this.btnChatDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.dataGridViewChat = new System.Windows.Forms.DataGridView();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtBoxChat = new System.Windows.Forms.TextBox();
            this.tbCtrlMain.SuspendLayout();
            this.tbPgHome.SuspendLayout();
            this.tbPgChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.dataGridViewChat)).BeginInit();
            this.SuspendLayout();
            // 
            // tbCtrlMain
            // 
            this.tbCtrlMain.Controls.Add(this.tbPgHome);
            this.tbCtrlMain.Controls.Add(this.tbPgChat);
            this.tbCtrlMain.Location = new System.Drawing.Point(12, 12);
            this.tbCtrlMain.Name = "tbCtrlMain";
            this.tbCtrlMain.SelectedIndex = 0;
            this.tbCtrlMain.Size = new System.Drawing.Size(577, 426);
            this.tbCtrlMain.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tbCtrlMain.TabIndex = 0;
            // 
            // tbPgHome
            // 
            this.tbPgHome.Controls.Add(this.btnPlay);
            this.tbPgHome.Controls.Add(this.cmbBoxMap);
            this.tbPgHome.Controls.Add(this.cmbBoxLander);
            this.tbPgHome.Controls.Add(this.btnAdmin);
            this.tbPgHome.Controls.Add(this.btnChat);
            this.tbPgHome.Controls.Add(this.label1);
            this.tbPgHome.Controls.Add(this.btnLogout);
            this.tbPgHome.Location = new System.Drawing.Point(4, 22);
            this.tbPgHome.Name = "tbPgHome";
            this.tbPgHome.Padding = new System.Windows.Forms.Padding(3);
            this.tbPgHome.Size = new System.Drawing.Size(569, 400);
            this.tbPgHome.TabIndex = 0;
            this.tbPgHome.Text = "Home";
            this.tbPgHome.UseVisualStyleBackColor = true;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(247, 176);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 49);
            this.btnPlay.TabIndex = 6;
            this.btnPlay.Text = "Play!";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // cmbBoxMap
            // 
            this.cmbBoxMap.FormattingEnabled = true;
            this.cmbBoxMap.Location = new System.Drawing.Point(206, 135);
            this.cmbBoxMap.Name = "cmbBoxMap";
            this.cmbBoxMap.Size = new System.Drawing.Size(148, 21);
            this.cmbBoxMap.TabIndex = 5;
            // 
            // cmbBoxLander
            // 
            this.cmbBoxLander.FormattingEnabled = true;
            this.cmbBoxLander.Location = new System.Drawing.Point(206, 108);
            this.cmbBoxLander.Name = "cmbBoxLander";
            this.cmbBoxLander.Size = new System.Drawing.Size(148, 21);
            this.cmbBoxLander.TabIndex = 4;
            // 
            // btnAdmin
            // 
            this.btnAdmin.Location = new System.Drawing.Point(488, 345);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(75, 49);
            this.btnAdmin.TabIndex = 3;
            this.btnAdmin.Text = "Admin Portal";
            this.btnAdmin.UseVisualStyleBackColor = true;
            this.btnAdmin.Visible = false;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // btnChat
            // 
            this.btnChat.Location = new System.Drawing.Point(6, 345);
            this.btnChat.Name = "btnChat";
            this.btnChat.Size = new System.Drawing.Size(75, 49);
            this.btnChat.TabIndex = 2;
            this.btnChat.Text = "Chat Room";
            this.btnChat.UseVisualStyleBackColor = true;
            this.btnChat.Click += new System.EventHandler(this.btnChat_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(87, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "New Game";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(6, 6);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 49);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "< Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // tbPgChat
            // 
            this.tbPgChat.Controls.Add(this.btnChatDelete);
            this.tbPgChat.Controls.Add(this.btnUpdate);
            this.tbPgChat.Controls.Add(this.dataGridViewChat);
            this.tbPgChat.Controls.Add(this.btnSend);
            this.tbPgChat.Controls.Add(this.txtBoxChat);
            this.tbPgChat.Location = new System.Drawing.Point(4, 22);
            this.tbPgChat.Name = "tbPgChat";
            this.tbPgChat.Padding = new System.Windows.Forms.Padding(3);
            this.tbPgChat.Size = new System.Drawing.Size(569, 400);
            this.tbPgChat.TabIndex = 1;
            this.tbPgChat.Text = "Chat";
            this.tbPgChat.UseVisualStyleBackColor = true;
            this.tbPgChat.Enter += new System.EventHandler(this.tbPgChat_Enter);
            // 
            // btnChatDelete
            // 
            this.btnChatDelete.Location = new System.Drawing.Point(110, 366);
            this.btnChatDelete.Name = "btnChatDelete";
            this.btnChatDelete.Size = new System.Drawing.Size(93, 24);
            this.btnChatDelete.TabIndex = 4;
            this.btnChatDelete.Text = "Delete";
            this.btnChatDelete.UseVisualStyleBackColor = true;
            this.btnChatDelete.Visible = false;
            this.btnChatDelete.Click += new System.EventHandler(this.btnChatDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(11, 366);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(93, 24);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Refresh";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // dataGridViewChat
            // 
            this.dataGridViewChat.AllowUserToAddRows = false;
            this.dataGridViewChat.AllowUserToDeleteRows = false;
            this.dataGridViewChat.AllowUserToResizeColumns = false;
            this.dataGridViewChat.AllowUserToResizeRows = false;
            this.dataGridViewChat.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewChat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewChat.Location = new System.Drawing.Point(11, 14);
            this.dataGridViewChat.MultiSelect = false;
            this.dataGridViewChat.Name = "dataGridViewChat";
            this.dataGridViewChat.ReadOnly = true;
            this.dataGridViewChat.RowHeadersVisible = false;
            this.dataGridViewChat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewChat.Size = new System.Drawing.Size(551, 341);
            this.dataGridViewChat.TabIndex = 2;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(470, 367);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(93, 24);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtBoxChat
            // 
            this.txtBoxChat.Location = new System.Drawing.Point(209, 370);
            this.txtBoxChat.Name = "txtBoxChat";
            this.txtBoxChat.Size = new System.Drawing.Size(255, 20);
            this.txtBoxChat.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 450);
            this.Controls.Add(this.tbCtrlMain);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tbCtrlMain.ResumeLayout(false);
            this.tbPgHome.ResumeLayout(false);
            this.tbPgChat.ResumeLayout(false);
            this.tbPgChat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.dataGridViewChat)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnChatDelete;

        private System.Windows.Forms.TextBox txtBoxChat;

        private System.Windows.Forms.DataGridView dataGridViewChat;

        private System.Windows.Forms.Button btnPlay;

        private System.Windows.Forms.ComboBox cmbBoxMap;

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.DataGridView dataGridView1;

        private System.Windows.Forms.TabPage tbPgHome;
        private System.Windows.Forms.TabControl tbCtrlMain;
        private System.Windows.Forms.TabPage tbPgChat;

        private System.Windows.Forms.ComboBox cmbBoxLander;

        private System.Windows.Forms.Button btnAdmin;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnUpdate;

        private System.Windows.Forms.Button btnChat;
        

        #endregion
    }
}