using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIA.ProjectCreatorWizard.UI
{
    public partial class CompanyAndDesignOptionForm : Form
    {
        private CompanyAndDesignOptionViewModel _viewModel;

        public CompanyAndDesignOptionForm(CompanyAndDesignOptionViewModel viewModel)
        {
            InitializeComponent();

            AddressRemoteDesginTextbox.Visible = false;
            RemoteDesignAddressLabel.Visible = false;
            SupportMailLabel.Visible = false;
            SupportMailTextBox.Visible = false;

            this._viewModel = viewModel;
        }

        private void UseRemoteDesignCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (UseRemoteDesignCheckbox.Checked)
            {
                AddressRemoteDesginTextbox.Visible = true;
                RemoteDesignAddressLabel.Visible = true;
            }
            else
            {
                AddressRemoteDesginTextbox.Visible = false;
                RemoteDesignAddressLabel.Visible = false;
            }
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            _viewModel.CompanyName = CompanyNameTextbox.Text;
            _viewModel.DivisionName = DivisionNameTextbox.Text;
            _viewModel.UseRemoteDesign = UseRemoteDesignCheckbox.Checked;
            _viewModel.RemoteDesignAddress = AddressRemoteDesginTextbox.Text;

            _viewModel.SupportMail = SupportMailTextBox.Text;
            _viewModel.UseSupportMail = UseSupportMailCheckbox.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void UseSupportMailCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (UseSupportMailCheckbox.Checked)
            {
                SupportMailTextBox.Visible = true;
                SupportMailLabel.Visible = true;
            }
            else
            {
                SupportMailTextBox.Visible = false;
                SupportMailLabel.Visible = false;
            }
        }
    }
}
