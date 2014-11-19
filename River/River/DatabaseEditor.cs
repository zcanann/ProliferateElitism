using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace River
{
    public partial class DatabaseEditor : Form
    {
        public DatabaseEditor()
        {
            this.Show();
            InitializeComponent();
        }

        private void DatabaseEditor_Load(object sender, EventArgs e)
        {

        }


        ///////////////////////////////////////
        // ADDING ENTRIES
        ///////////////////////////////////////
        private void addItemButton_Click(object sender, EventArgs e)
        {
            if (slotComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Select a slot type");
                return;
            }

            if (GameDB.AddItemToDataBase(slotComboBox.Items[slotComboBox.SelectedIndex].ToString(), (Int32)armorNUD.Value, (Int32)primaryNUD.Value, (Int32)vitalityNUD.Value,
                itemNameTB.Text, (Int32)levelNUD.Value, (Int32)attackNUD.Value, (Int32)attackSpeedNUD.Value))
            {
                MessageBox.Show("Item added!");
            }
            else
            {
                MessageBox.Show("Failed to insert item.");
            }
        }

        private void addGameObjectButton_Click(object sender, EventArgs e)
        {
            if (GameDB.AddObjectToDataBase(gameObjectNameTB.Text))
            {
                MessageBox.Show("Object added!");
            }
            else
            {
                MessageBox.Show("Failed to insert item.");
            }
        }

        private void addEnemyButton_Click(object sender, EventArgs e)
        {
            if (GameDB.AddEnemyToDataBase(enemyNameTB.Text))
            {
                MessageBox.Show("Enemy added!");
            }
            else
            {
                MessageBox.Show("Failed to insert item.");
            }
        }

        ///////////////////////////////////////
        // REMOVING ENTRIES
        ///////////////////////////////////////

        private void deleteItemButton_Click(object sender, EventArgs e)
        {
            if (GameDB.DeleteItemFromDataBase((Int32)itemIDNUD.Value))
            {
                MessageBox.Show("Item deleted!");
            }
            else
            {
                MessageBox.Show("ID not found!");
            }
        }

        private void deleteGameObjectButton_Click(object sender, EventArgs e)
        {
            if (GameDB.DeleteGameObjectFromDataBase((Int32)gameObjectIDNUD.Value))
            {
                MessageBox.Show("Object deleted!");
            }
            else
            {
                MessageBox.Show("ID not found!");
            }
        }

        private void deleteEnemyButton_Click(object sender, EventArgs e)
        {
            if (GameDB.DeleteEnemyFromDataBase((Int32)enemyIDNUD.Value))
            {
                MessageBox.Show("Enemy deleted!");
            }
            else
            {
                MessageBox.Show("ID not found!");
            }
        }
    }
}
