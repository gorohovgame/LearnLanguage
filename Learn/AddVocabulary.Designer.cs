
namespace Learn
{
    partial class gorm2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gorm2));
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fFile = new System.Windows.Forms.TextBox();
            this.fName = new System.Windows.Forms.TextBox();
            this.fDescription = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fLanguage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Bookman Old Style", 14.25F);
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(581, 145);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 43);
            this.button2.TabIndex = 1;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bookman Old Style", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bookman Old Style", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
            this.label2.Location = new System.Drawing.Point(15, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bookman Old Style", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
            this.label3.Location = new System.Drawing.Point(15, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Vocabulary";
            // 
            // fFile
            // 
            this.fFile.Font = new System.Drawing.Font("Bookman Old Style", 14.25F);
            this.fFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
            this.fFile.Location = new System.Drawing.Point(143, 102);
            this.fFile.Name = "fFile";
            this.fFile.Size = new System.Drawing.Size(512, 30);
            this.fFile.TabIndex = 5;
            this.fFile.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fFile_MouseClick);
            // 
            // fName
            // 
            this.fName.Font = new System.Drawing.Font("Bookman Old Style", 14.25F);
            this.fName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
            this.fName.Location = new System.Drawing.Point(143, 15);
            this.fName.MaxLength = 20;
            this.fName.Name = "fName";
            this.fName.Size = new System.Drawing.Size(225, 30);
            this.fName.TabIndex = 6;
            // 
            // fDescription
            // 
            this.fDescription.Font = new System.Drawing.Font("Bookman Old Style", 14.25F);
            this.fDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
            this.fDescription.Location = new System.Drawing.Point(143, 57);
            this.fDescription.MaxLength = 100;
            this.fDescription.Name = "fDescription";
            this.fDescription.Size = new System.Drawing.Size(553, 30);
            this.fDescription.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(661, 104);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 28);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // fLanguage
            // 
            this.fLanguage.Font = new System.Drawing.Font("Bookman Old Style", 14.25F);
            this.fLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
            this.fLanguage.Location = new System.Drawing.Point(623, 15);
            this.fLanguage.MaxLength = 2;
            this.fLanguage.Name = "fLanguage";
            this.fLanguage.Size = new System.Drawing.Size(73, 30);
            this.fLanguage.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bookman Old Style", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
            this.label4.Location = new System.Drawing.Point(400, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(217, 21);
            this.label4.TabIndex = 10;
            this.label4.Text = "Language [EN/PL/DE]";
            // 
            // gorm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 200);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.fLanguage);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.fDescription);
            this.Controls.Add(this.fName);
            this.Controls.Add(this.fFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //this.Name = "gorm2";
            this.Text = "Add Vocabulary";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fFile;
        private System.Windows.Forms.TextBox fName;
        private System.Windows.Forms.TextBox fDescription;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox fLanguage;
        private System.Windows.Forms.Label label4;
    }
}