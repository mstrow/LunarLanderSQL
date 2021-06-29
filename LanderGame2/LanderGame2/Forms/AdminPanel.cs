using System;
using System.Data;
using System.Windows.Forms;

namespace LanderGame
{
    public partial class AdminPanel : Form
    {
        private LoginSession _session;
        private DataTable _users;
        private UserForm _userForm = new UserForm();
        public AdminPanel()
        {
            InitializeComponent();
        }

        public bool ShowDialog(LoginSession session)
        {
            _session = session;
            UpdateDisplay();
            return ShowDialog() == DialogResult.OK;
        }
        private void UpdateDisplay()
        {
            _users = _session.ListUsers();
            dataGridViewUsers.DataSource = null;
            dataGridViewUsers.DataSource = _users;
            dataGridViewUsers.Columns[8].Visible = false;
            foreach (DataGridViewColumn column in dataGridViewUsers.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string oldusername = _users.Rows[dataGridViewUsers.CurrentCell.RowIndex]["Username"].ToString();
            User user = _session.GetUserByNameAdmin(oldusername);
            if(_userForm.ShowDialog(user))
            {
                string newUsername = user.Username;
                user.Username = oldusername;
                _session.EditUserAdmin(user,newUsername);
            }
            UpdateDisplay();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            User user = new User();
            if(_userForm.ShowDialog(user))
            {
                _session.NewUserAdmin(user);
            }
            UpdateDisplay();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string username = _users.Rows[dataGridViewUsers.CurrentCell.RowIndex]["Username"].ToString();
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the user: " + username + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dialogResult == DialogResult.Yes)
            {
                _session.DeleteUserAdmin(username);
            }
            UpdateDisplay();
        }
    }
}