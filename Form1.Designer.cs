namespace AppSheetTest
{
    partial class Form1
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
            this.btnGetData = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.listViewPeople = new System.Windows.Forms.ListView();
            this.Picture = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Age = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Phone = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Bio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.peoplePictures = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // btnGetData
            // 
            this.btnGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetData.Location = new System.Drawing.Point(27, 1202);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(142, 23);
            this.btnGetData.TabIndex = 0;
            this.btnGetData.Text = "Get Youngest 5 Users";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.BtnGetData_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(199, 1202);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // listViewPeople
            // 
            this.listViewPeople.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewPeople.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Picture,
            this.UserName,
            this.Age,
            this.Phone,
            this.Bio});
            this.listViewPeople.HideSelection = false;
            this.listViewPeople.Location = new System.Drawing.Point(27, 51);
            this.listViewPeople.Name = "listViewPeople";
            this.listViewPeople.Size = new System.Drawing.Size(843, 1128);
            this.listViewPeople.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewPeople.TabIndex = 2;
            this.listViewPeople.UseCompatibleStateImageBehavior = false;
            this.listViewPeople.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListViewPeople_ColumnClick);
            // 
            // Picture
            // 
            this.Picture.Text = "Picture";
            this.Picture.Width = 250;
            // 
            // UserName
            // 
            this.UserName.Text = "Name";
            this.UserName.Width = 80;
            // 
            // Age
            // 
            this.Age.Text = "Age";
            // 
            // Phone
            // 
            this.Phone.Text = "Phone";
            this.Phone.Width = 120;
            // 
            // Bio
            // 
            this.Bio.Text = "Bio";
            this.Bio.Width = 280;
            // 
            // peoplePictures
            // 
            this.peoplePictures.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.peoplePictures.ImageSize = new System.Drawing.Size(32, 32);
            this.peoplePictures.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 1237);
            this.Controls.Add(this.listViewPeople);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGetData);
            this.Name = "Form1";
            this.Text = "App Sheet Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView listViewPeople;
        private System.Windows.Forms.ColumnHeader UserName;
        private System.Windows.Forms.ColumnHeader Age;
        private System.Windows.Forms.ColumnHeader Phone;
        private System.Windows.Forms.ColumnHeader Bio;
        private System.Windows.Forms.ImageList peoplePictures;
        private System.Windows.Forms.ColumnHeader Picture;
    }
}

