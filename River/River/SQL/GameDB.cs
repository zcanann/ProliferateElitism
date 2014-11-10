using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Windows.Forms;

namespace River
{
    class GameDB
    {
        private SqlCeConnection MainConnection = null;
        private SqlCeConnection LocalConnection = null;
        private SqlCeCommand CommandMain;
        private SqlCeCommand CommandLocal;

        public GameDB()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                MainConnection = new SqlCeConnection("Data Source = ..\\..\\..\\GameDatabase.sdf;Password=''");
                LocalConnection = new SqlCeConnection("Data Source = GameDatabase.sdf;Password=''");
                MainConnection.Open();
                LocalConnection.Open();

                CommandMain = MainConnection.CreateCommand();
                CommandLocal = LocalConnection.CreateCommand();

                InitializeEnemyDatabase();

                MainConnection.Close();
                LocalConnection.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void InitializeEnemyDatabase()
        {
            ExecuteCommand("DELETE FROM Enemy WHERE 1=1");
            ExecuteCommand("INSERT INTO Enemy ([ID], [Name]) Values(1, 'Skeleton')");
            ExecuteCommand("INSERT INTO Enemy ([ID], [Name]) Values(2, 'Goblin')");
        }

        private bool ExecuteCommand(String QueryText)
        {
            int AffectedRowsMain;
            int AffectedRowsLocal;

            CommandMain.CommandText = QueryText;
            CommandLocal.CommandText = QueryText;

            AffectedRowsMain = CommandMain.ExecuteNonQuery();
            AffectedRowsLocal = CommandLocal.ExecuteNonQuery();

            if (AffectedRowsMain != AffectedRowsLocal)
                throw new Exception("Mismatch on main and local database copies!");

            if (AffectedRowsMain > 0)
                return true;

            return false;
        }
    }
}
