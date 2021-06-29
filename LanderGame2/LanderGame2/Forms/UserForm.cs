using System;
using System.Windows.Forms;

namespace LanderGame
{
    public partial class UserForm : Form
    {
        private User _user;
        public UserForm()
        {
            InitializeComponent();
        }
    
        public bool ShowDialog(User user)
        {
            _user = user;
            if (_user.Username != null)
            {
                lblTitle.Text = "Edit User";
            }
            UpdateDisplay();
            return ShowDialog() == DialogResult.OK;
        }

        private void UpdateDisplay()
        {
            txtBoxUsername.Text = _user.Username;
            txtBoxPassword.Text = _user.Password;
            txtBoxHighScore.Text = _user.HighScore.ToString();
            txtBoxAttempts.Text = _user.LoginAttempts.ToString();
            chkBoxAdmin.Checked = _user.IsAdmin;
            chkBoxLocked.Checked = _user.Locked;
        }

        private void PushData()
        {
            _user.Username = txtBoxUsername.Text;
            _user.Password = txtBoxPassword.Text;
            _user.HighScore = Convert.ToInt32(txtBoxHighScore.Text);
            _user.LoginAttempts = Convert.ToInt32(txtBoxAttempts.Text);
            _user.Locked = chkBoxLocked.Checked;
            _user.IsAdmin = chkBoxAdmin.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PushData();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }
    }
}