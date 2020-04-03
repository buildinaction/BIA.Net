namespace BIA.ProjectCreatorWizard.UI
{
    partial class CompanyAndDesignOptionForm
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
            this.CompanyLabel = new System.Windows.Forms.Label();
            this.RemoteDesignAddressLabel = new System.Windows.Forms.Label();
            this.UseRemoteDesignLabel = new System.Windows.Forms.Label();
            this.AddressRemoteDesginTextbox = new System.Windows.Forms.TextBox();
            this.CompanyNameTextbox = new System.Windows.Forms.TextBox();
            this.UseRemoteDesignCheckbox = new System.Windows.Forms.CheckBox();
            this.ValidateButton = new System.Windows.Forms.Button();
            this.DivisionNameTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.UseSupportMailCheckbox = new System.Windows.Forms.CheckBox();
            this.SupportMailTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SupportMailLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CompanyLabel
            // 
            this.CompanyLabel.AutoSize = true;
            this.CompanyLabel.Location = new System.Drawing.Point(12, 12);
            this.CompanyLabel.Name = "CompanyLabel";
            this.CompanyLabel.Size = new System.Drawing.Size(88, 13);
            this.CompanyLabel.TabIndex = 0;
            this.CompanyLabel.Text = "Company Name :";
            // 
            // RemoteDesignAddressLabel
            // 
            this.RemoteDesignAddressLabel.AutoSize = true;
            this.RemoteDesignAddressLabel.Location = new System.Drawing.Point(12, 108);
            this.RemoteDesignAddressLabel.Name = "RemoteDesignAddressLabel";
            this.RemoteDesignAddressLabel.Size = new System.Drawing.Size(132, 13);
            this.RemoteDesignAddressLabel.TabIndex = 1;
            this.RemoteDesignAddressLabel.Text = "Address of remote design :";
            this.RemoteDesignAddressLabel.Visible = false;
            // 
            // UseRemoteDesignLabel
            // 
            this.UseRemoteDesignLabel.AutoSize = true;
            this.UseRemoteDesignLabel.Location = new System.Drawing.Point(12, 84);
            this.UseRemoteDesignLabel.Name = "UseRemoteDesignLabel";
            this.UseRemoteDesignLabel.Size = new System.Drawing.Size(101, 13);
            this.UseRemoteDesignLabel.TabIndex = 2;
            this.UseRemoteDesignLabel.Text = "Use remote design :";
            this.UseRemoteDesignLabel.Visible = false;
            // 
            // AddressRemoteDesginTextbox
            // 
            this.AddressRemoteDesginTextbox.Location = new System.Drawing.Point(179, 106);
            this.AddressRemoteDesginTextbox.Name = "AddressRemoteDesginTextbox";
            this.AddressRemoteDesginTextbox.Size = new System.Drawing.Size(100, 20);
            this.AddressRemoteDesginTextbox.TabIndex = 4;
            this.AddressRemoteDesginTextbox.Visible = false;
            // 
            // CompanyNameTextbox
            // 
            this.CompanyNameTextbox.Location = new System.Drawing.Point(179, 9);
            this.CompanyNameTextbox.Name = "CompanyNameTextbox";
            this.CompanyNameTextbox.Size = new System.Drawing.Size(100, 20);
            this.CompanyNameTextbox.TabIndex = 1;
            // 
            // UseRemoteDesignCheckbox
            // 
            this.UseRemoteDesignCheckbox.AutoSize = true;
            this.UseRemoteDesignCheckbox.Location = new System.Drawing.Point(179, 84);
            this.UseRemoteDesignCheckbox.Name = "UseRemoteDesignCheckbox";
            this.UseRemoteDesignCheckbox.Size = new System.Drawing.Size(15, 14);
            this.UseRemoteDesignCheckbox.TabIndex = 3;
            this.UseRemoteDesignCheckbox.UseVisualStyleBackColor = true;
            this.UseRemoteDesignCheckbox.Visible = false;
            this.UseRemoteDesignCheckbox.CheckedChanged += new System.EventHandler(this.UseRemoteDesignCheckbox_CheckedChanged);
            // 
            // ValidateButton
            // 
            this.ValidateButton.Location = new System.Drawing.Point(325, 199);
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.Size = new System.Drawing.Size(75, 23);
            this.ValidateButton.TabIndex = 7;
            this.ValidateButton.Text = "Valider";
            this.ValidateButton.UseVisualStyleBackColor = true;
            this.ValidateButton.Click += new System.EventHandler(this.ValidateButton_Click);
            // 
            // DivisionNameTextbox
            // 
            this.DivisionNameTextbox.Location = new System.Drawing.Point(179, 32);
            this.DivisionNameTextbox.Name = "DivisionNameTextbox";
            this.DivisionNameTextbox.Size = new System.Drawing.Size(100, 20);
            this.DivisionNameTextbox.TabIndex = 2;
            this.DivisionNameTextbox.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Division or Team Name :";
            this.label1.Visible = false;
            // 
            // UseSupportMailCheckbox
            // 
            this.UseSupportMailCheckbox.AutoSize = true;
            this.UseSupportMailCheckbox.Location = new System.Drawing.Point(179, 154);
            this.UseSupportMailCheckbox.Name = "UseSupportMailCheckbox";
            this.UseSupportMailCheckbox.Size = new System.Drawing.Size(15, 14);
            this.UseSupportMailCheckbox.TabIndex = 5;
            this.UseSupportMailCheckbox.UseVisualStyleBackColor = true;
            this.UseSupportMailCheckbox.Visible = false;
            this.UseSupportMailCheckbox.CheckedChanged += new System.EventHandler(this.UseSupportMailCheckbox_CheckedChanged);
            // 
            // SupportMailTextBox
            // 
            this.SupportMailTextBox.Location = new System.Drawing.Point(179, 175);
            this.SupportMailTextBox.Name = "SupportMailTextBox";
            this.SupportMailTextBox.Size = new System.Drawing.Size(100, 20);
            this.SupportMailTextBox.TabIndex = 6;
            this.SupportMailTextBox.Visible = false;
            this.SupportMailTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Report errors on support mailbox :";
            this.label2.Visible = false;
            // 
            // SupportMailLabel
            // 
            this.SupportMailLabel.AutoSize = true;
            this.SupportMailLabel.Location = new System.Drawing.Point(12, 176);
            this.SupportMailLabel.Name = "SupportMailLabel";
            this.SupportMailLabel.Size = new System.Drawing.Size(79, 13);
            this.SupportMailLabel.TabIndex = 9;
            this.SupportMailLabel.Text = "Mail of support:";
            this.SupportMailLabel.Visible = false;
            this.SupportMailLabel.Click += new System.EventHandler(this.label3_Click);
            // 
            // CompanyAndDesignOptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 234);
            this.Controls.Add(this.UseSupportMailCheckbox);
            this.Controls.Add(this.SupportMailTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SupportMailLabel);
            this.Controls.Add(this.DivisionNameTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ValidateButton);
            this.Controls.Add(this.UseRemoteDesignCheckbox);
            this.Controls.Add(this.CompanyNameTextbox);
            this.Controls.Add(this.AddressRemoteDesginTextbox);
            this.Controls.Add(this.UseRemoteDesignLabel);
            this.Controls.Add(this.RemoteDesignAddressLabel);
            this.Controls.Add(this.CompanyLabel);
            this.Name = "CompanyAndDesignOptionForm";
            this.Text = "CompanyAndDesignOptionForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CompanyLabel;
        private System.Windows.Forms.Label RemoteDesignAddressLabel;
        private System.Windows.Forms.Label UseRemoteDesignLabel;
        private System.Windows.Forms.TextBox AddressRemoteDesginTextbox;
        private System.Windows.Forms.TextBox CompanyNameTextbox;
        private System.Windows.Forms.CheckBox UseRemoteDesignCheckbox;
        private System.Windows.Forms.Button ValidateButton;
        private System.Windows.Forms.TextBox DivisionNameTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox UseSupportMailCheckbox;
        private System.Windows.Forms.TextBox SupportMailTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label SupportMailLabel;
    }
}