namespace River
{
    partial class DatabaseEditor
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.enemyNameTB = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gameObjectNameTB = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.levelNUD = new System.Windows.Forms.NumericUpDown();
            this.slotComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.itemNameTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.vitalityNUD = new System.Windows.Forms.NumericUpDown();
            this.primaryNUD = new System.Windows.Forms.NumericUpDown();
            this.attackSpeedNUD = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.attackNUD = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.armorNUD = new System.Windows.Forms.NumericUpDown();
            this.addItemButton = new System.Windows.Forms.Button();
            this.deleteItemButton = new System.Windows.Forms.Button();
            this.addGameObjectButton = new System.Windows.Forms.Button();
            this.addEnemyButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.itemIDNUD = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.enemyIDNUD = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.deleteEnemyButton = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.gameObjectIDNUD = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.deleteGameObjectButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.levelNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vitalityNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.primaryNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attackSpeedNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attackNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.armorNUD)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemIDNUD)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.enemyIDNUD)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameObjectIDNUD)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nameLabel);
            this.groupBox1.Controls.Add(this.enemyNameTB);
            this.groupBox1.Controls.Add(this.addEnemyButton);
            this.groupBox1.Location = new System.Drawing.Point(194, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(163, 79);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enemy";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(6, 22);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name:";
            // 
            // enemyNameTB
            // 
            this.enemyNameTB.Location = new System.Drawing.Point(50, 19);
            this.enemyNameTB.Name = "enemyNameTB";
            this.enemyNameTB.Size = new System.Drawing.Size(100, 20);
            this.enemyNameTB.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.gameObjectNameTB);
            this.groupBox2.Controls.Add(this.addGameObjectButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 174);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(165, 81);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Game Object";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // gameObjectNameTB
            // 
            this.gameObjectNameTB.Location = new System.Drawing.Point(50, 19);
            this.gameObjectNameTB.Name = "gameObjectNameTB";
            this.gameObjectNameTB.Size = new System.Drawing.Size(100, 20);
            this.gameObjectNameTB.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.addItemButton);
            this.groupBox3.Controls.Add(this.armorNUD);
            this.groupBox3.Controls.Add(this.attackNUD);
            this.groupBox3.Controls.Add(this.attackSpeedNUD);
            this.groupBox3.Controls.Add(this.primaryNUD);
            this.groupBox3.Controls.Add(this.vitalityNUD);
            this.groupBox3.Controls.Add(this.levelNUD);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.slotComboBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.itemNameTB);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(345, 156);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Item";
            // 
            // levelNUD
            // 
            this.levelNUD.Location = new System.Drawing.Point(54, 44);
            this.levelNUD.Name = "levelNUD";
            this.levelNUD.Size = new System.Drawing.Size(102, 20);
            this.levelNUD.TabIndex = 6;
            // 
            // slotComboBox
            // 
            this.slotComboBox.FormattingEnabled = true;
            this.slotComboBox.Items.AddRange(new object[] {
            "Ring",
            "Head",
            "Amulet",
            "Weapon",
            "Chest",
            "Offhand",
            "Hands",
            "Legs",
            "Feet",
            "None"});
            this.slotComboBox.Location = new System.Drawing.Point(213, 18);
            this.slotComboBox.Name = "slotComboBox";
            this.slotComboBox.Size = new System.Drawing.Size(102, 21);
            this.slotComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(179, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Slot:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name:";
            // 
            // itemNameTB
            // 
            this.itemNameTB.Location = new System.Drawing.Point(56, 18);
            this.itemNameTB.Name = "itemNameTB";
            this.itemNameTB.Size = new System.Drawing.Size(100, 20);
            this.itemNameTB.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Level:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Vitality:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Primary:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(162, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "AtkSpd:";
            // 
            // vitalityNUD
            // 
            this.vitalityNUD.Location = new System.Drawing.Point(54, 70);
            this.vitalityNUD.Name = "vitalityNUD";
            this.vitalityNUD.Size = new System.Drawing.Size(102, 20);
            this.vitalityNUD.TabIndex = 6;
            // 
            // primaryNUD
            // 
            this.primaryNUD.Location = new System.Drawing.Point(54, 96);
            this.primaryNUD.Name = "primaryNUD";
            this.primaryNUD.Size = new System.Drawing.Size(102, 20);
            this.primaryNUD.TabIndex = 6;
            // 
            // attackSpeedNUD
            // 
            this.attackSpeedNUD.Location = new System.Drawing.Point(213, 96);
            this.attackSpeedNUD.Name = "attackSpeedNUD";
            this.attackSpeedNUD.Size = new System.Drawing.Size(102, 20);
            this.attackSpeedNUD.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(166, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Attack:";
            // 
            // attackNUD
            // 
            this.attackNUD.Location = new System.Drawing.Point(213, 70);
            this.attackNUD.Name = "attackNUD";
            this.attackNUD.Size = new System.Drawing.Size(102, 20);
            this.attackNUD.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(170, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Armor:";
            // 
            // armorNUD
            // 
            this.armorNUD.Location = new System.Drawing.Point(213, 44);
            this.armorNUD.Name = "armorNUD";
            this.armorNUD.Size = new System.Drawing.Size(102, 20);
            this.armorNUD.TabIndex = 6;
            // 
            // addItemButton
            // 
            this.addItemButton.Location = new System.Drawing.Point(240, 122);
            this.addItemButton.Name = "addItemButton";
            this.addItemButton.Size = new System.Drawing.Size(75, 23);
            this.addItemButton.TabIndex = 4;
            this.addItemButton.Text = "Add Item";
            this.addItemButton.UseVisualStyleBackColor = true;
            this.addItemButton.Click += new System.EventHandler(this.addItemButton_Click);
            // 
            // deleteItemButton
            // 
            this.deleteItemButton.Location = new System.Drawing.Point(9, 39);
            this.deleteItemButton.Name = "deleteItemButton";
            this.deleteItemButton.Size = new System.Drawing.Size(75, 23);
            this.deleteItemButton.TabIndex = 4;
            this.deleteItemButton.Text = "Delete Item";
            this.deleteItemButton.UseVisualStyleBackColor = true;
            this.deleteItemButton.Click += new System.EventHandler(this.deleteItemButton_Click);
            // 
            // addGameObjectButton
            // 
            this.addGameObjectButton.Location = new System.Drawing.Point(77, 45);
            this.addGameObjectButton.Name = "addGameObjectButton";
            this.addGameObjectButton.Size = new System.Drawing.Size(75, 23);
            this.addGameObjectButton.TabIndex = 7;
            this.addGameObjectButton.Text = "Add Object";
            this.addGameObjectButton.UseVisualStyleBackColor = true;
            this.addGameObjectButton.Click += new System.EventHandler(this.addGameObjectButton_Click);
            // 
            // addEnemyButton
            // 
            this.addEnemyButton.Location = new System.Drawing.Point(73, 45);
            this.addEnemyButton.Name = "addEnemyButton";
            this.addEnemyButton.Size = new System.Drawing.Size(75, 23);
            this.addEnemyButton.TabIndex = 7;
            this.addEnemyButton.Text = "Add Enemy";
            this.addEnemyButton.UseVisualStyleBackColor = true;
            this.addEnemyButton.Click += new System.EventHandler(this.addEnemyButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.itemIDNUD);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.deleteItemButton);
            this.groupBox4.Location = new System.Drawing.Point(366, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(97, 71);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Item";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "ID:";
            // 
            // itemIDNUD
            // 
            this.itemIDNUD.Location = new System.Drawing.Point(33, 13);
            this.itemIDNUD.Name = "itemIDNUD";
            this.itemIDNUD.Size = new System.Drawing.Size(51, 20);
            this.itemIDNUD.TabIndex = 7;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.enemyIDNUD);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.deleteEnemyButton);
            this.groupBox5.Location = new System.Drawing.Point(366, 184);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(97, 71);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Enemy";
            // 
            // enemyIDNUD
            // 
            this.enemyIDNUD.Location = new System.Drawing.Point(33, 13);
            this.enemyIDNUD.Name = "enemyIDNUD";
            this.enemyIDNUD.Size = new System.Drawing.Size(51, 20);
            this.enemyIDNUD.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "ID:";
            // 
            // deleteEnemyButton
            // 
            this.deleteEnemyButton.Location = new System.Drawing.Point(9, 39);
            this.deleteEnemyButton.Name = "deleteEnemyButton";
            this.deleteEnemyButton.Size = new System.Drawing.Size(75, 23);
            this.deleteEnemyButton.TabIndex = 4;
            this.deleteEnemyButton.Text = "Delete Item";
            this.deleteEnemyButton.UseVisualStyleBackColor = true;
            this.deleteEnemyButton.Click += new System.EventHandler(this.deleteEnemyButton_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.gameObjectIDNUD);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.deleteGameObjectButton);
            this.groupBox6.Location = new System.Drawing.Point(366, 97);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(97, 71);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Game Object";
            // 
            // gameObjectIDNUD
            // 
            this.gameObjectIDNUD.Location = new System.Drawing.Point(33, 13);
            this.gameObjectIDNUD.Name = "gameObjectIDNUD";
            this.gameObjectIDNUD.Size = new System.Drawing.Size(51, 20);
            this.gameObjectIDNUD.TabIndex = 7;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "ID:";
            // 
            // deleteGameObjectButton
            // 
            this.deleteGameObjectButton.Location = new System.Drawing.Point(9, 42);
            this.deleteGameObjectButton.Name = "deleteGameObjectButton";
            this.deleteGameObjectButton.Size = new System.Drawing.Size(75, 23);
            this.deleteGameObjectButton.TabIndex = 4;
            this.deleteGameObjectButton.Text = "Delete Item";
            this.deleteGameObjectButton.UseVisualStyleBackColor = true;
            this.deleteGameObjectButton.Click += new System.EventHandler(this.deleteGameObjectButton_Click);
            // 
            // DatabaseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 267);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DatabaseEditor";
            this.Text = "DatabaseEditor";
            this.Load += new System.EventHandler(this.DatabaseEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.levelNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vitalityNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.primaryNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attackSpeedNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attackNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.armorNUD)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemIDNUD)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.enemyIDNUD)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameObjectIDNUD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox enemyNameTB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox gameObjectNameTB;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown levelNUD;
        private System.Windows.Forms.ComboBox slotComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox itemNameTB;
        private System.Windows.Forms.Button addGameObjectButton;
        private System.Windows.Forms.Button addItemButton;
        private System.Windows.Forms.NumericUpDown armorNUD;
        private System.Windows.Forms.NumericUpDown attackNUD;
        private System.Windows.Forms.NumericUpDown attackSpeedNUD;
        private System.Windows.Forms.NumericUpDown primaryNUD;
        private System.Windows.Forms.NumericUpDown vitalityNUD;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button deleteItemButton;
        private System.Windows.Forms.Button addEnemyButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown itemIDNUD;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown enemyIDNUD;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button deleteEnemyButton;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown gameObjectIDNUD;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button deleteGameObjectButton;
    }
}