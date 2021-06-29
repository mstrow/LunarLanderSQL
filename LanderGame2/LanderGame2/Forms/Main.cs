using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LanderGame
{
    public partial class Main : Form
    {
        private LoginSession session;
        private AdminPanel _adminPanel;
        private DataTable _chat;
        public Main()
        {
            InitializeComponent();
            session = new LoginSession("Server=localhost;Port=3306;Database=LanderGame;Uid=devuser;password=password;");
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Login lgn = new Login();
            while (!session.LoggedIn)
            {
                lgn.ShowDialog();
                if (lgn.DialogResult == DialogResult.OK)
                {
                    session.Login(lgn.username, lgn.password);
                    if (session.IsAdmin)
                    {
                        _adminPanel = new AdminPanel();
                        btnAdmin.Visible = true;
                        btnChatDelete.Visible = true;
                    }
                }
                else
                {
                    Close();
                }
            }
            cmbBoxLander.DataSource = session.ListLanders().AsEnumerable().Select(x => x["Name"].ToString()).ToArray();
            cmbBoxMap.DataSource = session.ListMaps().AsEnumerable().Select(x => x["Name"].ToString()).ToArray();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            session.Logout();
            Close();
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            tbCtrlMain.SelectedIndex = 1;
        }

        private void timerChat_Tick(object sender, EventArgs e)
        {
            UpdateChat();
        }

        private void UpdateChat()
        {
            _chat = session.GetChat();
            dataGridViewChat.DataSource = null;
            dataGridViewChat.DataSource = _chat;
            foreach (DataGridViewColumn column in dataGridViewChat.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtBoxChat.Text != "")
            {
                session.SendChat(txtBoxChat.Text);
            }
            UpdateChat();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            _adminPanel.ShowDialog(session);
        }

        private void btnChatDelete_Click(object sender, EventArgs e)
        {
            DataRow row = _chat.Rows[dataGridViewChat.CurrentCell.RowIndex];
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the message: " + row["Message"], "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dialogResult == DialogResult.Yes)
            {
                session.DeleteChatAdmin(_chat.Rows[dataGridViewChat.CurrentCell.RowIndex]["ID"].ToString());
                UpdateChat();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateChat();
        }

        private void tbPgChat_Enter(object sender, EventArgs e)
        {
            UpdateChat();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            session.NewMatch(cmbBoxLander.SelectedValue.ToString(), cmbBoxMap.SelectedValue.ToString());
        }
    }
}