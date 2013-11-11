namespace River
{
    partial class RoomEditor
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
            this.RoomWidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.RoomHeight = new System.Windows.Forms.NumericUpDown();
            this.WalkableCB = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.RoomSet = new System.Windows.Forms.NumericUpDown();
            this.LoadButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TileEventCBB = new System.Windows.Forms.ComboBox();
            this.AddSurfaceButton = new System.Windows.Forms.Button();
            this.SurfaceTilesListBox = new System.Windows.Forms.ListBox();
            this.DelSurfaceButton = new System.Windows.Forms.Button();
            this.DelHeightButton = new System.Windows.Forms.Button();
            this.AddHeightButton = new System.Windows.Forms.Button();
            this.HeightTilesListBox = new System.Windows.Forms.ListBox();
            this.SurfaceTile = new System.Windows.Forms.NumericUpDown();
            this.HeightTile = new System.Windows.Forms.NumericUpDown();
            this.BaseTile = new System.Windows.Forms.NumericUpDown();
            this.YPosition = new System.Windows.Forms.Label();
            this.XPosition = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LoadRoomItems = new System.Windows.Forms.ComboBox();
            this.SaveRoomItems = new System.Windows.Forms.ComboBox();
            this.SaveNewButton = new System.Windows.Forms.Button();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.RoomInfoButton = new System.Windows.Forms.Button();
            this.FlipCB = new System.Windows.Forms.CheckBox();
            this.NoHeightCB = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.RoomWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RoomHeight)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RoomSet)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SurfaceTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseTile)).BeginInit();
            this.SuspendLayout();
            // 
            // RoomWidth
            // 
            this.RoomWidth.Location = new System.Drawing.Point(64, 14);
            this.RoomWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RoomWidth.Name = "RoomWidth";
            this.RoomWidth.Size = new System.Drawing.Size(64, 20);
            this.RoomWidth.TabIndex = 0;
            this.RoomWidth.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Room W:";
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(341, 237);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 2;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(134, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Room H:";
            // 
            // RoomHeight
            // 
            this.RoomHeight.Location = new System.Drawing.Point(192, 14);
            this.RoomHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RoomHeight.Name = "RoomHeight";
            this.RoomHeight.Size = new System.Drawing.Size(64, 20);
            this.RoomHeight.TabIndex = 3;
            this.RoomHeight.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // WalkableCB
            // 
            this.WalkableCB.AutoSize = true;
            this.WalkableCB.Location = new System.Drawing.Point(6, 103);
            this.WalkableCB.Name = "WalkableCB";
            this.WalkableCB.Size = new System.Drawing.Size(71, 17);
            this.WalkableCB.TabIndex = 5;
            this.WalkableCB.Text = "Walkable";
            this.WalkableCB.UseVisualStyleBackColor = true;
            this.WalkableCB.CheckedChanged += new System.EventHandler(this.WalkableCB_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.RoomWidth);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.RoomSet);
            this.groupBox1.Controls.Add(this.RoomHeight);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 43);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Settings";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(262, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Room Set:";
            // 
            // RoomSet
            // 
            this.RoomSet.Location = new System.Drawing.Point(325, 14);
            this.RoomSet.Name = "RoomSet";
            this.RoomSet.Size = new System.Drawing.Size(79, 20);
            this.RoomSet.TabIndex = 3;
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(12, 204);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 5;
            this.LoadButton.Text = "Load Room";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 237);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 7;
            this.SaveButton.Text = "Save Room";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.NoHeightCB);
            this.groupBox2.Controls.Add(this.FlipCB);
            this.groupBox2.Controls.Add(this.TileEventCBB);
            this.groupBox2.Controls.Add(this.AddSurfaceButton);
            this.groupBox2.Controls.Add(this.SurfaceTilesListBox);
            this.groupBox2.Controls.Add(this.DelSurfaceButton);
            this.groupBox2.Controls.Add(this.DelHeightButton);
            this.groupBox2.Controls.Add(this.AddHeightButton);
            this.groupBox2.Controls.Add(this.HeightTilesListBox);
            this.groupBox2.Controls.Add(this.SurfaceTile);
            this.groupBox2.Controls.Add(this.HeightTile);
            this.groupBox2.Controls.Add(this.BaseTile);
            this.groupBox2.Controls.Add(this.YPosition);
            this.groupBox2.Controls.Add(this.XPosition);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.WalkableCB);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 61);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 139);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tile Settings";
            // 
            // TileEventCBB
            // 
            this.TileEventCBB.FormattingEnabled = true;
            this.TileEventCBB.Items.AddRange(new object[] {
            "None",
            "PlayerSpawn",
            "DungeonExit",
            "Waypoint",
            "Chest",
            "WeaponRack",
            "Shop",
            "Storage",
            "Enchanter"});
            this.TileEventCBB.Location = new System.Drawing.Point(6, 76);
            this.TileEventCBB.Name = "TileEventCBB";
            this.TileEventCBB.Size = new System.Drawing.Size(71, 21);
            this.TileEventCBB.TabIndex = 16;
            // 
            // AddSurfaceButton
            // 
            this.AddSurfaceButton.Location = new System.Drawing.Point(356, 30);
            this.AddSurfaceButton.Name = "AddSurfaceButton";
            this.AddSurfaceButton.Size = new System.Drawing.Size(48, 20);
            this.AddSurfaceButton.TabIndex = 13;
            this.AddSurfaceButton.Text = "Add";
            this.AddSurfaceButton.UseVisualStyleBackColor = true;
            this.AddSurfaceButton.Click += new System.EventHandler(this.AddSurfaceButton_Click);
            // 
            // SurfaceTilesListBox
            // 
            this.SurfaceTilesListBox.FormattingEnabled = true;
            this.SurfaceTilesListBox.Location = new System.Drawing.Point(286, 60);
            this.SurfaceTilesListBox.Name = "SurfaceTilesListBox";
            this.SurfaceTilesListBox.ScrollAlwaysVisible = true;
            this.SurfaceTilesListBox.Size = new System.Drawing.Size(66, 69);
            this.SurfaceTilesListBox.TabIndex = 12;
            // 
            // DelSurfaceButton
            // 
            this.DelSurfaceButton.Location = new System.Drawing.Point(356, 60);
            this.DelSurfaceButton.Name = "DelSurfaceButton";
            this.DelSurfaceButton.Size = new System.Drawing.Size(48, 20);
            this.DelSurfaceButton.TabIndex = 13;
            this.DelSurfaceButton.Text = "Del";
            this.DelSurfaceButton.UseVisualStyleBackColor = true;
            this.DelSurfaceButton.Click += new System.EventHandler(this.DelSurfaceButton_Click);
            // 
            // DelHeightButton
            // 
            this.DelHeightButton.Location = new System.Drawing.Point(234, 60);
            this.DelHeightButton.Name = "DelHeightButton";
            this.DelHeightButton.Size = new System.Drawing.Size(48, 20);
            this.DelHeightButton.TabIndex = 13;
            this.DelHeightButton.Text = "Del";
            this.DelHeightButton.UseVisualStyleBackColor = true;
            this.DelHeightButton.Click += new System.EventHandler(this.DelHeightButton_Click);
            // 
            // AddHeightButton
            // 
            this.AddHeightButton.Location = new System.Drawing.Point(234, 32);
            this.AddHeightButton.Name = "AddHeightButton";
            this.AddHeightButton.Size = new System.Drawing.Size(48, 20);
            this.AddHeightButton.TabIndex = 13;
            this.AddHeightButton.Text = "Add";
            this.AddHeightButton.UseVisualStyleBackColor = true;
            this.AddHeightButton.Click += new System.EventHandler(this.AddHeightButton_Click);
            // 
            // HeightTilesListBox
            // 
            this.HeightTilesListBox.FormattingEnabled = true;
            this.HeightTilesListBox.Location = new System.Drawing.Point(164, 60);
            this.HeightTilesListBox.Name = "HeightTilesListBox";
            this.HeightTilesListBox.ScrollAlwaysVisible = true;
            this.HeightTilesListBox.Size = new System.Drawing.Size(66, 69);
            this.HeightTilesListBox.TabIndex = 12;
            // 
            // SurfaceTile
            // 
            this.SurfaceTile.Location = new System.Drawing.Point(286, 32);
            this.SurfaceTile.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.SurfaceTile.Name = "SurfaceTile";
            this.SurfaceTile.Size = new System.Drawing.Size(64, 20);
            this.SurfaceTile.TabIndex = 11;
            // 
            // HeightTile
            // 
            this.HeightTile.Location = new System.Drawing.Point(164, 32);
            this.HeightTile.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.HeightTile.Name = "HeightTile";
            this.HeightTile.Size = new System.Drawing.Size(64, 20);
            this.HeightTile.TabIndex = 11;
            // 
            // BaseTile
            // 
            this.BaseTile.Location = new System.Drawing.Point(81, 32);
            this.BaseTile.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.BaseTile.Name = "BaseTile";
            this.BaseTile.Size = new System.Drawing.Size(64, 20);
            this.BaseTile.TabIndex = 11;
            // 
            // YPosition
            // 
            this.YPosition.AutoSize = true;
            this.YPosition.Location = new System.Drawing.Point(29, 32);
            this.YPosition.Name = "YPosition";
            this.YPosition.Size = new System.Drawing.Size(16, 13);
            this.YPosition.TabIndex = 10;
            this.YPosition.Text = "-1";
            // 
            // XPosition
            // 
            this.XPosition.AutoSize = true;
            this.XPosition.Location = new System.Drawing.Point(29, 16);
            this.XPosition.Name = "XPosition";
            this.XPosition.Size = new System.Drawing.Size(16, 13);
            this.XPosition.TabIndex = 10;
            this.XPosition.Text = "-1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(283, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Surface Tiles:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(164, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Height Tiles:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(78, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Base Tile:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "X:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Y:";
            // 
            // LoadRoomItems
            // 
            this.LoadRoomItems.FormattingEnabled = true;
            this.LoadRoomItems.Location = new System.Drawing.Point(93, 206);
            this.LoadRoomItems.Name = "LoadRoomItems";
            this.LoadRoomItems.Size = new System.Drawing.Size(149, 21);
            this.LoadRoomItems.TabIndex = 9;
            // 
            // SaveRoomItems
            // 
            this.SaveRoomItems.FormattingEnabled = true;
            this.SaveRoomItems.Location = new System.Drawing.Point(93, 239);
            this.SaveRoomItems.Name = "SaveRoomItems";
            this.SaveRoomItems.Size = new System.Drawing.Size(149, 21);
            this.SaveRoomItems.TabIndex = 9;
            // 
            // SaveNewButton
            // 
            this.SaveNewButton.Location = new System.Drawing.Point(248, 237);
            this.SaveNewButton.Name = "SaveNewButton";
            this.SaveNewButton.Size = new System.Drawing.Size(75, 23);
            this.SaveNewButton.TabIndex = 10;
            this.SaveNewButton.Text = "Save New";
            this.SaveNewButton.UseVisualStyleBackColor = true;
            this.SaveNewButton.Click += new System.EventHandler(this.SaveNewButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(248, 206);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshButton.TabIndex = 11;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // RoomInfoButton
            // 
            this.RoomInfoButton.Location = new System.Drawing.Point(341, 206);
            this.RoomInfoButton.Name = "RoomInfoButton";
            this.RoomInfoButton.Size = new System.Drawing.Size(75, 23);
            this.RoomInfoButton.TabIndex = 12;
            this.RoomInfoButton.Text = "Room Info";
            this.RoomInfoButton.UseVisualStyleBackColor = true;
            this.RoomInfoButton.Click += new System.EventHandler(this.RoomInfoButton_Click);
            // 
            // FlipCB
            // 
            this.FlipCB.AutoSize = true;
            this.FlipCB.Location = new System.Drawing.Point(81, 78);
            this.FlipCB.Name = "FlipCB";
            this.FlipCB.Size = new System.Drawing.Size(42, 17);
            this.FlipCB.TabIndex = 17;
            this.FlipCB.Text = "Flip";
            this.FlipCB.UseVisualStyleBackColor = true;
            this.FlipCB.CheckedChanged += new System.EventHandler(this.FlipCB_CheckedChanged);
            // 
            // NoHeightCB
            // 
            this.NoHeightCB.AutoSize = true;
            this.NoHeightCB.Location = new System.Drawing.Point(81, 103);
            this.NoHeightCB.Name = "NoHeightCB";
            this.NoHeightCB.Size = new System.Drawing.Size(74, 17);
            this.NoHeightCB.TabIndex = 18;
            this.NoHeightCB.Text = "No Height";
            this.NoHeightCB.UseVisualStyleBackColor = true;
            this.NoHeightCB.CheckedChanged += new System.EventHandler(this.NoHeightCB_CheckedChanged);
            // 
            // RoomEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 269);
            this.Controls.Add(this.RoomInfoButton);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.SaveNewButton);
            this.Controls.Add(this.SaveRoomItems);
            this.Controls.Add(this.LoadRoomItems);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ApplyButton);
            this.Name = "RoomEditor";
            this.Text = "RoomEditor";
            this.Load += new System.EventHandler(this.RoomEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RoomWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RoomHeight)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RoomSet)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SurfaceTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HeightTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BaseTile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown RoomWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown RoomHeight;
        private System.Windows.Forms.CheckBox WalkableCB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label YPosition;
        private System.Windows.Forms.Label XPosition;
        private System.Windows.Forms.Button AddSurfaceButton;
        private System.Windows.Forms.ListBox SurfaceTilesListBox;
        private System.Windows.Forms.Button DelSurfaceButton;
        private System.Windows.Forms.Button DelHeightButton;
        private System.Windows.Forms.Button AddHeightButton;
        private System.Windows.Forms.ListBox HeightTilesListBox;
        private System.Windows.Forms.NumericUpDown SurfaceTile;
        private System.Windows.Forms.NumericUpDown HeightTile;
        private System.Windows.Forms.NumericUpDown BaseTile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown RoomSet;
        private System.Windows.Forms.ComboBox LoadRoomItems;
        private System.Windows.Forms.ComboBox SaveRoomItems;
        private System.Windows.Forms.Button SaveNewButton;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Button RoomInfoButton;
        private System.Windows.Forms.ComboBox TileEventCBB;
        private System.Windows.Forms.CheckBox NoHeightCB;
        private System.Windows.Forms.CheckBox FlipCB;
    }
}