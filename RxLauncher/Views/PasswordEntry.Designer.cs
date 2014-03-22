namespace RxLauncher.Views
{
	partial class PasswordEntry
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
			this.Password = new System.Windows.Forms.TextBox();
			this.submit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// Password
			// 
			this.Password.AcceptsReturn = true;
			this.Password.Location = new System.Drawing.Point(12, 21);
			this.Password.Name = "Password";
			this.Password.PasswordChar = '*';
			this.Password.Size = new System.Drawing.Size(179, 20);
			this.Password.TabIndex = 1;
			this.Password.UseSystemPasswordChar = true;
			// 
			// submit
			// 
			this.submit.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.submit.Location = new System.Drawing.Point(197, 19);
			this.submit.Name = "submit";
			this.submit.Size = new System.Drawing.Size(75, 23);
			this.submit.TabIndex = 2;
			this.submit.Text = "OK";
			this.submit.UseVisualStyleBackColor = true;
			// 
			// PasswordEntry
			// 
			this.AcceptButton = this.submit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 53);
			this.Controls.Add(this.submit);
			this.Controls.Add(this.Password);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PasswordEntry";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Enter a password";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button submit;
		internal System.Windows.Forms.TextBox Password;

	}
}