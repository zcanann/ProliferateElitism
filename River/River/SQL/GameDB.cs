using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Windows.Forms;

namespace River
{
    static class GameDB
    {
        private enum QueryType
        {
            NonExecute,
            Scalar,
            ResultSet,
            Reader
        }

        private static SqlCeConnection MainConnection = null;
        private static SqlCeConnection LocalConnection = null;
        private static SqlCeCommand CommandMain;
        private static SqlCeCommand CommandLocal;
        private static Random Random = new Random();

        public static void InitializeDatabase()
        {
            try
            {
                MainConnection = new SqlCeConnection("Data Source = ..\\..\\..\\GameDatabase.sdf;Password=''");
                LocalConnection = new SqlCeConnection("Data Source = GameDatabase.sdf;Password=''");
                MainConnection.Open();
                LocalConnection.Open();

                CommandMain = MainConnection.CreateCommand();
                CommandLocal = LocalConnection.CreateCommand();

                InsertTestData();

                //MainConnection.Close();1
                //LocalConnection.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private static void InsertTestData()
        {
            // Create players
            CreatePlayers();

            // Create test items
            ExecuteCommand("DELETE FROM Item WHERE 1 = 1", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Item ([Slot], [Armor], [Primary], [Vitality], [Name], [Level], [Attack], [AttackSpeedBonus], [ItemID]) " +
                "Values('Weapon', 5, 5, 5, 'Sword', 5, 5, 1, 1)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Item ([Slot], [Armor], [Primary], [Vitality], [Name], [Level], [Attack], [AttackSpeedBonus], [ItemID]) " +
                "Values('Weapon', 7, 7, 7, 'Dagger', 7, 7, 1, 2)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Item ([Slot], [Armor], [Primary], [Vitality], [Name], [Level], [Attack], [AttackSpeedBonus], [ItemID]) " +
                "Values('Ring', 3, 3, 3, 'Copper Ring', 3, 3, 1, 3)", QueryType.NonExecute);

            // Create test enemies
            ExecuteCommand("DELETE FROM Enemy WHERE 1 = 1", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Enemy ([EnemyID], [EnemyName]) " + "Values(1, 'Skeleton')", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Enemy ([EnemyID], [EnemyName]) " + "Values(2, 'Goblin')", QueryType.NonExecute);

            // Create test objects
            ExecuteCommand("DELETE FROM GameObject WHERE 1 = 1", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO GameObject ([ObjectID], [ObjectName]) " + "Values(1, 'Chest')", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO GameObject ([ObjectID], [ObjectName]) " + "Values(2, 'Lockbox')", QueryType.NonExecute);

            // Create test enemy manager
            ExecuteCommand("DELETE FROM EnemyManager WHERE 1 = 1", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO EnemyManager ([EnemyID], [InventoryID], [UniqueID]) " + "Values(1, 4, 1)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO EnemyManager ([EnemyID], [InventoryID], [UniqueID]) " + "Values(1, 5, 2)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO EnemyManager ([EnemyID], [InventoryID], [UniqueID]) " + "Values(2, 6, 3)", QueryType.NonExecute);

            // Create test object manager
            ExecuteCommand("DELETE FROM ObjectManager WHERE 1 = 1", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO ObjectManager ([ObjectID], [InventoryID], [UniqueID]) " + "Values(1, 7, 1)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO ObjectManager ([ObjectID], [InventoryID], [UniqueID]) " + "Values(2, 8, 2)", QueryType.NonExecute);

            // Initialize test inventories
            ExecuteCommand("DELETE FROM Inventory WHERE 1 = 1", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(1)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(2)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(3)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(4)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(5)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(6)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(7)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(8)", QueryType.NonExecute);

            // Create test items for the inventories
            ExecuteCommand("DELETE FROM InventoryManager WHERE 1 = 1", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID]) " + "Values(1, 1)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID]) " + "Values(2, 2)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID]) " + "Values(2, 2)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID]) " + "Values(2, 3)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID]) " + "Values(5, 1)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID]) " + "Values(6, 3)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID]) " + "Values(7, 2)", QueryType.NonExecute);
            ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID]) " + "Values(8, 2)", QueryType.NonExecute);


            GetItemsFromInventory("2");
        }

        public static void CreatePlayers()
        {
            // Only 3 default players. One for each class.
            ExecuteCommand("DELETE FROM Player WHERE 1=1", QueryType.NonExecute);

            // Create player 1
            Int32 NewInventory = CreateNewInventory();
            ExecuteCommand("INSERT INTO Player ([PlayerID], [InventoryID], [Class], [Level], [Experience], [Progress]) " +
                "Values(1, " + NewInventory.ToString() + ", 'Magician', 0, 0, 'New')", QueryType.NonExecute);

            // Create player 2
            NewInventory = CreateNewInventory();
            ExecuteCommand("INSERT INTO Player ([PlayerID], [InventoryID], [Class], [Level], [Experience], [Progress]) " +
                "Values(2, " + NewInventory.ToString() + ", 'Warrior', 0, 0, 'New')", QueryType.NonExecute);

            // Create player 3
            NewInventory = CreateNewInventory();
            ExecuteCommand("INSERT INTO Player ([PlayerID], [InventoryID], [Class], [Level], [Experience], [Progress]) " +
                "Values(3, " + NewInventory.ToString() + ", 'Bandit', 0, 0, 'New')", QueryType.NonExecute);
        }

        // Creates an inventory and returns the new ID
        private static Int32 CreateNewInventory()
        {
            Int32 NewInventory = 0;

            // Get new inventory ID
            while (ExecuteCommand("SELECT COUNT(*) FROM Inventory WHERE [InventoryID] = " + (++NewInventory).ToString(), QueryType.Scalar)) ;

            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(" + NewInventory.ToString() + ")", QueryType.NonExecute);

            return NewInventory;
        }

        ///////////////////////////////////////
        // ADDING ENTRIES (called by editor GUI)
        ///////////////////////////////////////

        public static bool AddItemToDataBase(String Slot, Int32 Armor, Int32 Primary, Int32 Vitality,
            String Name, Int32 Level, Int32 Attack, Int32 AttackSpeedBonus)
        {
            Int32 NewKey = 0;

            // Get a new key
            while (ExecuteCommand("SELECT COUNT(*) FROM Item WHERE [ItemID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            return ExecuteCommand("INSERT INTO Item ([Slot], [Armor], [Primary], [Vitality], [Name], [Level], [Attack], [AttackSpeedBonus], [ItemID]) " +
                "Values('" + Slot.ToString() + "', " + Armor.ToString() + ", " + Primary.ToString() + ", " + Vitality.ToString() + ", '" + Name + "', "
                 + Level.ToString() + ", " + Attack.ToString() + ", " + AttackSpeedBonus.ToString() + ", " + NewKey.ToString() + ")", QueryType.NonExecute);
        }

        public static bool AddObjectToDataBase(String Name)
        {
            Int32 NewKey = 0;

            // Get a new key
            while (ExecuteCommand("SELECT COUNT(*) FROM GameObject WHERE [ObjectID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            return ExecuteCommand("INSERT INTO GameObject ([ObjectID], [ObjectName]) " + "Values(" + NewKey.ToString() + ", '" + Name + "')", QueryType.NonExecute);

        }

        public static bool AddEnemyToDataBase(String Name)
        {
            Int32 NewKey = 0;

            // Get a new key
            while (ExecuteCommand("SELECT COUNT(*) FROM Enemy WHERE [EnemyID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            return ExecuteCommand("INSERT INTO Enemy ([EnemyID], [EnemyName]) " + "Values(" + NewKey.ToString() + ", '" + Name + "')", QueryType.NonExecute);
        }

        ///////////////////////////////////////
        // ADDING ENTRIES (called by "level" or "player")
        ///////////////////////////////////////

        public static bool AddEnemyToManager(Int32 ID)
        {
            Int32 NewKey = 0;
            Int32 NewInventory = CreateNewInventory();

            // Get a new key
            while (ExecuteCommand("SELECT COUNT(*) FROM EnemyManager WHERE [UniqueID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            return ExecuteCommand("INSERT INTO EnemyManager ([EnemyID], [InventoryID], [UniqueID]) " + "Values(" + ID.ToString() + ", " + NewInventory.ToString() + ", " + NewKey.ToString() + ")", QueryType.NonExecute);
        }

        public static bool AddObjectToManager(Int32 ID)
        {
            Int32 NewKey = 0;
            Int32 NewInventory = CreateNewInventory();

            // Get a new key
            while (ExecuteCommand("SELECT COUNT(*) FROM ObjectManager WHERE [UniqueID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            return ExecuteCommand("INSERT INTO ObjectManager ([ObjectID], [InventoryID], [UniqueID]) " + "Values(" + ID.ToString() + ", " + NewInventory.ToString() + ", " + NewKey.ToString() + ")", QueryType.NonExecute);
        }

        public static bool AddItemToInventory(Int32 ItemID, Int32 InventoryID)
        {
            return ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID]) " + "Values(" + ItemID.ToString() + ", " + InventoryID.ToString() + ")", QueryType.NonExecute);
        }

        public static bool RemoveItemFromInventory(Int32 ItemID, Int32 InventoryID)
        {
            return ExecuteCommand("REMOVE FROM InventoryManager WHERE [InventoryID] = " + ItemID.ToString() + " AND [ItemID] = " + InventoryID.ToString() + ")", QueryType.NonExecute);
        }

        ///////////////////////////////////////
        // REMOVING ENTRIES (called by editor GUI)
        ///////////////////////////////////////

        public static bool DeleteItemFromDataBase(Int32 ID)
        {
            return ExecuteCommand("DELETE FROM Item WHERE [ItemID] = " + ID.ToString(), QueryType.NonExecute);
        }

        public static bool DeleteGameObjectFromDataBase(Int32 ID)
        {
            return ExecuteCommand("DELETE FROM GameObject WHERE [ObjectID] = " + ID.ToString(), QueryType.NonExecute);
        }

        public static bool DeleteEnemyFromDataBase(Int32 ID)
        {
            return ExecuteCommand("DELETE FROM Enemy WHERE [EnemyID] = " + ID.ToString(), QueryType.NonExecute);
        }

        private static bool DeleteUnusedInventories()
        {
            // TODO!
            return false;
        }

        ///////////////////////////////////////
        // ACCESS ENTRIES
        ///////////////////////////////////////

        public static void CreateEnemies(Int32 Count, Int32[] IDPool)
        {
            Int32 NewID;
            Int32 IDIndex;

            // Delete old enemies
            ExecuteCommand("DELETE FROM EnemyManager WHERE 1 = 1", QueryType.NonExecute);

            for (int Index = 0; Index < Count; Index++)
            {
                // Randomly choose from pool
                IDIndex = Random.Next(0, IDPool.Length);

                // Grab the ID from this pool
                NewID = IDPool[IDIndex];

                // Add this enemy
                AddEnemyToManager(NewID);
            }
        }

        public static void CreateObjects(Int32 Count, Int32[] IDPool)
        {
            Int32 NewID;
            Int32 IDIndex;

            // Delete old enemies
            ExecuteCommand("DELETE FROM ObjectManager WHERE 1 = 1", QueryType.NonExecute);

            for (int Index = 0; Index < Count; Index++)
            {
                // Randomly choose from pool
                IDIndex = Random.Next(0, IDPool.Length);

                // Grab the ID from this pool
                NewID = IDPool[IDIndex];

                // Add this object
                AddObjectToManager(NewID);
            }
        }

        public static List<Object> GetItemsFromInventory(String InventoryID)
        {
            List<Object> Result;

            ExecuteCommand("SELECT [ItemID] FROM InventoryManager WHERE [InventoryID] = " + InventoryID.ToString(), QueryType.Reader, out Result) ;

            return Result;
        }

        // Executes an SQL command on both the local copy (temporary) and main (permenant) database.
        // Returns true if the SQL command affected any rows.
        private static bool ExecuteCommand(String QueryText, QueryType QueryType)
        {
            int AffectedRowsMain = 0;
            int AffectedRowsLocal = 0;

            CommandMain.CommandText = QueryText;
            CommandLocal.CommandText = QueryText;

            switch (QueryType)
            {
                case GameDB.QueryType.NonExecute:
                    AffectedRowsMain = CommandMain.ExecuteNonQuery();
                    AffectedRowsLocal = CommandLocal.ExecuteNonQuery();
                    break;

                case GameDB.QueryType.Scalar:
                    AffectedRowsMain = (int)CommandMain.ExecuteScalar();
                    AffectedRowsLocal = (int)CommandLocal.ExecuteScalar();
                    break;

                case GameDB.QueryType.ResultSet:
                    AffectedRowsMain = CommandMain.ExecuteResultSet(ResultSetOptions.None).RecordsAffected;
                    AffectedRowsLocal = CommandLocal.ExecuteResultSet(ResultSetOptions.None).RecordsAffected;
                    break;

                case GameDB.QueryType.Reader:
                    AffectedRowsMain = CommandMain.ExecuteReader().RecordsAffected;
                    AffectedRowsLocal = CommandLocal.ExecuteReader().RecordsAffected;
                    break;
            }


            if (AffectedRowsMain != AffectedRowsLocal)
                throw new Exception("Mismatch on main and local database copies!");

            if (AffectedRowsMain > 0)
                return true;

            return false;
        }

        private static bool ExecuteCommand(String QueryText, QueryType QueryType, out List<Object> Results)
        {
            Results = new List<object>();
            SqlCeDataReader ReaderMain;
            SqlCeDataReader ReaderLocal;
            int AffectedRowsMain = 0;
            int AffectedRowsLocal = 0;

            CommandMain.CommandText = QueryText;
            CommandLocal.CommandText = QueryText;



            switch (QueryType)
            {
                case GameDB.QueryType.Reader:
                    ReaderMain = CommandMain.ExecuteReader();
                    ReaderLocal = CommandLocal.ExecuteReader();
                    AffectedRowsMain = ReaderMain.RecordsAffected;
                    AffectedRowsLocal = ReaderLocal.RecordsAffected;
                    break;
                default:
                    return false;
            }

            if (AffectedRowsMain != AffectedRowsLocal)
                throw new Exception("Mismatch on main and local database copies!");

            try
            {
                while (ReaderMain.Read())
                {
                    Object[] Values = new Object[ReaderMain.FieldCount];
                    int FieldCount = ReaderMain.GetValues(Values);
                    for (int Index = 0; Index < FieldCount; Index++)
                        Results.Add(Values[Index]);
                }
            }
            finally
            {
                ReaderMain.Close();
                ReaderLocal.Close();
            }

            if (AffectedRowsMain > 0)
                return true;

            return false;
        }
    }
}
