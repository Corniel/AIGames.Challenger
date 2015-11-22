namespace AIGames.Challenger
{
	partial class ChallengerForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChallengerForm));
			this.opponentsTextBox = new System.Windows.Forms.TextBox();
			this.btnChallenge = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.intervalTextBox = new System.Windows.Forms.TextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			this.competitionsComboBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtOpponents
			// 
			this.opponentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.opponentsTextBox.Location = new System.Drawing.Point(0, 82);
			this.opponentsTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.opponentsTextBox.Multiline = true;
			this.opponentsTextBox.Name = "txtOpponents";
			this.opponentsTextBox.Size = new System.Drawing.Size(291, 216);
			this.opponentsTextBox.TabIndex = 0;
			// 
			// btnChallenge
			// 
			this.btnChallenge.Location = new System.Drawing.Point(9, 10);
			this.btnChallenge.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnChallenge.Name = "btnChallenge";
			this.btnChallenge.Size = new System.Drawing.Size(74, 34);
			this.btnChallenge.TabIndex = 1;
			this.btnChallenge.Text = "&Challenge";
			this.btnChallenge.UseVisualStyleBackColor = true;
			this.btnChallenge.Click += new System.EventHandler(this.btnChallenge_Click);
			// 
			// btnPause
			// 
			this.btnPause.Location = new System.Drawing.Point(87, 10);
			this.btnPause.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(71, 34);
			this.btnPause.TabIndex = 2;
			this.btnPause.Text = "&Pause";
			this.btnPause.UseVisualStyleBackColor = true;
			this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
			// 
			// txtInterval
			// 
			this.intervalTextBox.Location = new System.Drawing.Point(163, 10);
			this.intervalTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.intervalTextBox.Name = "txtInterval";
			this.intervalTextBox.Size = new System.Drawing.Size(76, 20);
			this.intervalTextBox.TabIndex = 3;
			this.intervalTextBox.Text = "310";
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(163, 30);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 13);
			this.label1.TabIndex = 4;
			// 
			// timer2
			// 
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// comboBox1
			// 
			this.competitionsComboBox.FormattingEnabled = true;
			this.competitionsComboBox.Location = new System.Drawing.Point(87, 49);
			this.competitionsComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.competitionsComboBox.Name = "comboBox1";
			this.competitionsComboBox.Size = new System.Drawing.Size(150, 21);
			this.competitionsComboBox.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 51);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(62, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Competition";
			// 
			// ChallengerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(285, 296);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.competitionsComboBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.intervalTextBox);
			this.Controls.Add(this.btnPause);
			this.Controls.Add(this.btnChallenge);
			this.Controls.Add(this.opponentsTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "ChallengerForm";
			this.Text = "Challenge bots";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChallengerForm_FormClosing);
			this.Load += new System.EventHandler(this.ChallengerForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox opponentsTextBox;
		private System.Windows.Forms.Button btnChallenge;
		private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.TextBox intervalTextBox;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Timer timer2;
		private System.Windows.Forms.ComboBox competitionsComboBox;
		private System.Windows.Forms.Label label2;
	}
}

