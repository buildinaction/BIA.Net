namespace BIA.ProjectCreatorWizard.UI
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// CompanyAndDesignOption Form.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class CompanyAndDesignOptionForm : Form
    {
        private CompanyAndDesignOptionViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyAndDesignOptionForm"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public CompanyAndDesignOptionForm(CompanyAndDesignOptionViewModel viewModel)
        {
            this.InitializeComponent();

            this.AddressRemoteDesginTextbox.Visible = false;
            this.RemoteDesignAddressLabel.Visible = false;
            this.SupportMailLabel.Visible = false;
            this.SupportMailTextBox.Visible = false;

            this.viewModel = viewModel;
        }

        private void UseRemoteDesignCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.UseRemoteDesignCheckbox.Checked)
            {
                this.AddressRemoteDesginTextbox.Visible = true;
                this.RemoteDesignAddressLabel.Visible = true;
            }
            else
            {
                this.AddressRemoteDesginTextbox.Visible = false;
                this.RemoteDesignAddressLabel.Visible = false;
            }
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            this.viewModel.CompanyName = this.CompanyNameTextbox.Text;
            this.viewModel.DivisionName = this.DivisionNameTextbox.Text;
            this.viewModel.UseRemoteDesign = this.UseRemoteDesignCheckbox.Checked;
            this.viewModel.RemoteDesignAddress = this.AddressRemoteDesginTextbox.Text;

            this.viewModel.SupportMail = this.SupportMailTextBox.Text;
            this.viewModel.UseSupportMail = this.UseSupportMailCheckbox.Checked;

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
            if (this.UseSupportMailCheckbox.Checked)
            {
                this.SupportMailTextBox.Visible = true;
                this.SupportMailLabel.Visible = true;
            }
            else
            {
                this.SupportMailTextBox.Visible = false;
                this.SupportMailLabel.Visible = false;
            }
        }
    }
}
