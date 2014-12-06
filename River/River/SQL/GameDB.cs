using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace River
{
    static class GameDB
    {

        #region Variables/Initialization

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

                CommandMain = MainConnection.CreateCommand();
                CommandLocal = LocalConnection.CreateCommand();

                MainConnection.Open();
                LocalConnection.Open();

                InitializeStaticData();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        public static void CloseDatabase()
        {
            try
            {
                if (MainConnection != null)
                    MainConnection.Close();
                if (LocalConnection != null)
                    LocalConnection.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }

        private static void InitializeStaticData()
        {
            RemoveAllEnemies();
            RemoveAllObjects();
            RemoveUnusedInventories();

            //RefreshDB();

            // Create equipment

            CreateStaticInventories();
        }

        private static void RefreshDB()
        {
            ExecuteCommand("DELETE FROM ObjectManager", QueryType.NonExecute);
            ExecuteCommand("DELETE FROM Player", QueryType.NonExecute);
            ExecuteCommand("DELETE FROM InventoryManager", QueryType.NonExecute);
            ExecuteCommand("DELETE FROM EnemyManager", QueryType.NonExecute);
            ExecuteCommand("DELETE FROM Inventory", QueryType.NonExecute);
        }

        private static void CreateStaticInventories()
        {
            if (!ExecuteCommand("SELECT COUNT(*) FROM ObjectManager WHERE [Identifier] = 'EquipmentM'", QueryType.Scalar))
            {
                AddObjectToManager(GetObjectID("EquipmentM"), "'EquipmentM'");
            }

            if (!ExecuteCommand("SELECT COUNT(*) FROM ObjectManager WHERE [Identifier] = 'EquipmentW'", QueryType.Scalar))
            {
                AddObjectToManager(GetObjectID("EquipmentW"), "'EquipmentW'");
            }

            if (!ExecuteCommand("SELECT COUNT(*) FROM ObjectManager WHERE [Identifier] = 'EquipmentB'", QueryType.Scalar))
            {
                AddObjectToManager(GetObjectID("EquipmentB"), "'EquipmentB'");
            }

            // Create storage chest
            if (!ExecuteCommand("SELECT COUNT(*) FROM ObjectManager WHERE [Identifier] = 'Storage'", QueryType.Scalar))
            {
                AddObjectToManager(GetObjectID("Storage"), "'Storage'");
            }

            // Create shop
            if (!ExecuteCommand("SELECT COUNT(*) FROM ObjectManager WHERE [Identifier] = 'Shop'", QueryType.Scalar))
            {
                AddObjectToManager(GetObjectID("Shop"), "'Shop'");
            }

            // Create Enchanter
            if (!ExecuteCommand("SELECT COUNT(*) FROM ObjectManager WHERE [Identifier] = 'Enchanter'", QueryType.Scalar))
            {
                AddObjectToManager(GetObjectID("Enchanter"), "'Enchanter'");
            }
        }

        #endregion

        #region Key finding functions

        ///////////////////////////////////////
        // KEY FINDING
        ///////////////////////////////////////

        private static Int32 GetNewPlayerID()
        {
            Int32 NewPlayerID = 0;
            while (ExecuteCommand("SELECT COUNT(*) FROM Player WHERE [PlayerID] = " + (++NewPlayerID).ToString(), QueryType.Scalar)) ;

            return NewPlayerID;
        }

        private static Int32 LastObjectKey = 0;
        private static Int32 GetNewObjectID()
        {

            Int32 NewKey = LastObjectKey;

            // Get a new key
            while (ExecuteCommand("SELECT COUNT(*) FROM GameObject WHERE [ObjectID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            LastObjectKey = NewKey;

            return NewKey;
        }

        private static Int32 LastEnemyKey = 0;
        private static Int32 GetNewEnemyID()
        {
            Int32 NewKey = LastEnemyKey;

            // Get a new key
            while (ExecuteCommand("SELECT COUNT(*) FROM Enemy WHERE [EnemyID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            LastEnemyKey = NewKey;

            return NewKey;
        }

        private static Int32 LastEnemyManagerKey = 0;
        private static Int32 GetNewEnemyManagerID()
        {
            Int32 NewKey = LastEnemyManagerKey;

            // Get a new key
            while (ExecuteCommand("SELECT COUNT(*) FROM EnemyManager WHERE [UniqueID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            LastEnemyManagerKey = NewKey;

            return NewKey;
        }

        private static Int32 LastInventory = 0;
        private static Int32 GetNewInventoryID()
        {
            Int32 NewKey = LastInventory;

            // Get new inventory ID
            while (ExecuteCommand("SELECT COUNT(*) FROM Inventory WHERE [InventoryID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            LastInventory = NewKey;

            return NewKey;
        }

        // Use this with caution
        public static void InsertInventory(Int32 InventoryID)
        {
            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(" + InventoryID.ToString() + ")", QueryType.NonExecute);
        }

        private static Int32 LastInventoryManager = 0;
        private static Int32 GetNewInventoryManagerID()
        {
            Int32 NewKey = LastInventoryManager;

            // Get new inventory ID
            while (ExecuteCommand("SELECT COUNT(*) FROM InventoryManager WHERE [UniqueID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            LastInventoryManager = NewKey;

            return NewKey;
        }


        private static Int32 LastItemKey = 0;
        public static Int32 GetNewItemID()
        {
            Int32 NewKey = LastItemKey;

            // Get a new key
            while (ExecuteCommand("SELECT COUNT(*) FROM Item WHERE [ItemID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            LastItemKey = NewKey;

            return NewKey;
        }

        private static Int32 LastObjectManagerKey = 0;
        public static Int32 GetNewObjectManagerID()
        {
            Int32 NewKey = LastObjectManagerKey;

            while (ExecuteCommand("SELECT COUNT(*) FROM ObjectManager WHERE [UniqueID] = " + (++NewKey).ToString(), QueryType.Scalar)) ;

            LastObjectManagerKey = NewKey;

            return NewKey;
        }

        #endregion

        #region Data insertion

        ///////////////////////////////////////
        // ADDING ENTRIES
        ///////////////////////////////////////

        public static void CreatePlayer(String Class)
        {
            // Only allow one of each class
            ExecuteCommand("DELETE FROM Player WHERE Class = '" + Class + "'", QueryType.NonExecute);


            if (Class == EntityType.Magician.ToString())
            {
                ExecuteCommand("DELETE FROM ObjectManager WHERE [Identifier] = 'EquipmentM'", QueryType.NonExecute);
            }
            else if (Class == EntityType.Warrior.ToString())
            {
                ExecuteCommand("DELETE FROM ObjectManager WHERE [Identifier] = 'EquipmentW'", QueryType.NonExecute);
            }
            else if (Class == EntityType.Bandit.ToString())
            {
                ExecuteCommand("DELETE FROM ObjectManager WHERE [Identifier] = 'EquipmentB'", QueryType.NonExecute);
            }

            CreateStaticInventories();

            // Get new unique IDs
            Int32 NewInventory = AddNewInventoryToDataBase();
            Int32 NewPlayerID = GetNewPlayerID();

            // Insert player
            ExecuteCommand("INSERT INTO Player ([PlayerID], [InventoryID], [Class], [Level], [Experience], [Progress], [Gold]) " +
                "Values(" + NewPlayerID.ToString() + ", " + NewInventory.ToString() + ", '" + Class + "', 0, 0, '0_0', 0)", QueryType.NonExecute);
        }

        public static void CreateLevelEnemies(Int32 Count, Int32[] IDPool)
        {
            Int32 NewID;
            Int32 IDIndex;

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

        public static void CreateLevelObjects(Int32 Count, Int32[] IDPool)
        {
            Int32 NewID;
            Int32 IDIndex;

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

        public static bool AddItemToDataBase(String Slot, Int32 Armor, Int32 Primary, Int32 Vitality,
            String Name, Int32 Level, Int32 Attack, Int32 AttackSpeedBonus)
        {
            Int32 NewKey = GetNewItemID();

            return ExecuteCommand("INSERT INTO Item ([Slot], [Armor], [Primary], [Vitality], [Name], [Level], [Attack], [AttackSpeedBonus], [ItemID]) " +
                "Values('" + Slot.ToString() + "', " + Armor.ToString() + ", " + Primary.ToString() + ", " + Vitality.ToString() + ", '" + Name + "', "
                 + Level.ToString() + ", " + Attack.ToString() + ", " + AttackSpeedBonus.ToString() + ", " + NewKey.ToString() + ")", QueryType.NonExecute);
        }

        private static Int32 AddNewInventoryToDataBase()
        {
            Int32 NewInventory = GetNewInventoryID();

            ExecuteCommand("INSERT INTO Inventory ([InventoryID]) " + "Values(" + NewInventory.ToString() + ")", QueryType.NonExecute);

            return NewInventory;
        }

        public static bool AddObjectToDataBase(String Name)
        {
            Int32 NewKey = GetNewObjectID();

            return ExecuteCommand("INSERT INTO GameObject ([ObjectID], [ObjectName]) " + "Values(" + NewKey.ToString() + ", '" + Name + "')", QueryType.NonExecute);

        }

        public static bool AddEnemyToDataBase(String Name)
        {
            Int32 NewKey = GetNewEnemyID();

            return ExecuteCommand("INSERT INTO Enemy ([EnemyID], [EnemyName]) " + "Values(" + NewKey.ToString() + ", '" + Name + "')", QueryType.NonExecute);
        }

        public static bool AddEnemyToManager(Int32 ID)
        {
            Int32 NewKey = GetNewEnemyManagerID();
            Int32 NewInventory = AddNewInventoryToDataBase();

            return ExecuteCommand("INSERT INTO EnemyManager ([EnemyID], [InventoryID], [UniqueID]) " + "Values(" + ID.ToString() + ", " + NewInventory.ToString() + ", " + NewKey.ToString() + ")", QueryType.NonExecute);
        }

        public static bool AddObjectToManager(Int32 ID, String Identifier = "''")
        {
            Int32 NewKey = GetNewObjectManagerID();
            Int32 NewInventory = AddNewInventoryToDataBase();

            return ExecuteCommand("INSERT INTO ObjectManager ([ObjectID], [InventoryID], [UniqueID], [Identifier]) " +
                "Values(" + ID.ToString() + ", " + NewInventory.ToString() + ", " + NewKey.ToString() + ", " + Identifier.ToString() + ")", QueryType.NonExecute);
        }

        public static bool AddItemToInventoryManager(Int32 ItemID, Int32 InventoryID)
        {
            Int32 NewKey = GetNewInventoryManagerID();

            return ExecuteCommand("INSERT INTO InventoryManager ([InventoryID], [ItemID], [UniqueID]) " +
                "Values(" + InventoryID.ToString() + ", " + ItemID.ToString() + ", " + NewKey.ToString() + ")", QueryType.NonExecute);
        }

        #endregion

        #region Removing entries

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

        public static bool DeleteInventoryFromDataBase(Int32 ID)
        {
            return ExecuteCommand("DELETE FROM Inventory WHERE [InventoryID] = " + ID.ToString(), QueryType.NonExecute);
        }

        public static bool RemoveItemFromInventory(Int32 ItemID, Int32 InventoryID)
        {
            return ExecuteCommand("REMOVE FROM InventoryManager WHERE [InventoryID] = " + ItemID.ToString() + " AND [ItemID] = " + InventoryID.ToString() + ")", QueryType.NonExecute);
        }

        public static void RemoveAllEnemies()
        {
            LastEnemyManagerKey = 0;
            ExecuteCommand("DELETE FROM EnemyManager", QueryType.NonExecute);
        }

        public static void RemoveAllObjects()
        {
            LastObjectManagerKey = 0;
            ExecuteCommand("DELETE FROM ObjectManager WHERE Identifier = ''", QueryType.NonExecute);
        }

        public static void RemoveUnusedInventories()
        {
            ExecuteCommand("DELETE FROM Inventory " +
                "WHERE InventoryID NOT IN (SELECT InventoryID FROM Player) " +
                "AND InventoryID NOT IN (SELECT InventoryID FROM EnemyManager) " +
                "AND InventoryID NOT IN (SELECT InventoryID FROM ObjectManager)", QueryType.NonExecute);

            ExecuteCommand("DELETE FROM InventoryManager " +
                "WHERE InventoryID NOT IN (SELECT InventoryID FROM Inventory)", QueryType.NonExecute);
        }

        #endregion

        public static void UpdateOwnerShip(Int32 OldInventoryID, Int32 NewInventoryID, Int32 ItemID)
        {
            List<Object> UniqueID;

            // Grab the first item that matches the description of the item being traded
            ExecuteCommand("SELECT UniqueID FROM InventoryManager WHERE InventoryID = " + OldInventoryID.ToString() + " " +
                "AND ItemID = " + ItemID.ToString(), QueryType.Reader, out UniqueID);

            if (UniqueID.Count <= 0)
            {
                throw new Exception("Something went wrong");
            }

            // Update ownership for this entry
            ExecuteCommand("UPDATE InventoryManager SET InventoryID = " + NewInventoryID.ToString() + " " +
                "WHERE UniqueID = " + UniqueID[0].ToString(), QueryType.NonExecute);
        }

        public static Int32 GetPlayerInventoryID(String Class)
        {
            List<Object> InventoryID;

            ExecuteCommand("SELECT InventoryID FROM Player WHERE Class = '" + Class + "'", QueryType.Reader, out InventoryID);

            if (InventoryID.Count <= 0)
            {
                throw new Exception("Something went wrong");
            }

            return (Int32)InventoryID[0];
        }

        private static Int32 GetObjectID(String ObjectName)
        {
            List<Object> ObjectID;

            ExecuteCommand("SELECT ObjectID FROM GameObject WHERE ObjectName = '" + ObjectName + "'", QueryType.Reader, out ObjectID);

            if (ObjectID.Count <= 0)
            {
                throw new Exception("Something went wrong");
            }

            return (Int32)ObjectID[0];
        }

        public static Int32 GetEnemyInventoryID(Int32 EnemyIndex)
        {
            List<Object> InventoryID;

            ExecuteCommand("SELECT InventoryID FROM EnemyManager WHERE UniqueID = " + EnemyIndex.ToString(), QueryType.Reader, out InventoryID);

            if (InventoryID.Count <= 0)
            {
                throw new Exception("Something went wrong");
            }

            return (Int32)InventoryID[0];
        }

        public static Int32 GetObjectInventoryID(Int32 ObjectIndex)
        {
            List<Object> InventoryID;

            ExecuteCommand("SELECT InventoryID FROM ObjectManager WHERE UniqueID = " + ObjectIndex.ToString(), QueryType.Reader, out InventoryID);

            if (InventoryID.Count <= 0)
            {
                throw new Exception("Something went wrong");
            }

            return (Int32)InventoryID[0];
        }

        public static Int32 GetEnchanterInventoryID()
        {
            return GetSpecialInventoryID("Enchanter");
        }

        public static Int32 GetShopInventoryID()
        {
            return GetSpecialInventoryID("Shop");
        }

        public static Int32 GetStorageInventoryID()
        {
            return GetSpecialInventoryID("Storage");
        }

        public static Int32 GetEquipmentInventoryID(EntityType Entity)
        {
            switch (Entity)
            {
                case EntityType.Magician:
                    return GetSpecialInventoryID("EquipmentM");
                case EntityType.Warrior:
                    return GetSpecialInventoryID("EquipmentW");
                case EntityType.Bandit:
                    return GetSpecialInventoryID("EquipmentB");
                default:
                    throw new Exception("Invalid entity type to retrieve inventory for");
            }

        }

        private static Int32 GetSpecialInventoryID(String Identifier)
        {
            List<Object> InventoryID;

            ExecuteCommand("SELECT InventoryID FROM ObjectManager WHERE Identifier = '" + Identifier.ToString() + "'", QueryType.Reader, out InventoryID);

            if (InventoryID.Count <= 0)
            {
                throw new Exception("Something went wrong");
            }

            return (Int32)InventoryID[0];
        }

        public static Item AddRandomItem(Int32 InventoryID, Int32 BaseLevel)
        {
            List<Object> PossibleItemIDs;

            // Generate a random item with level from BaseLevel - 3 to BaseLevel
            ExecuteCommand("SELECT ItemID FROM Item WHERE Level <= " + BaseLevel.ToString() + " " +
                "AND Level >= " + (BaseLevel - 3).ToString(), QueryType.Reader, out PossibleItemIDs);

            if (PossibleItemIDs.Count == 0)
            {
                throw new Exception("Something went wrong. Not enough items in DB");
            }

            Int32 SelectedItemID = (Int32)PossibleItemIDs[Random.Next(0, PossibleItemIDs.Count)];

            AddItemToInventoryManager(SelectedItemID, InventoryID);

            return ReadItemFromDataBase(SelectedItemID);
        }

        public static Item ReadItemFromDataBase(Int32 ItemID)
        {
            Item ObtainedItem = null;
            List<Object> Result;

            ExecuteCommand("SELECT * FROM Item WHERE [ItemID] = " + ItemID.ToString(), QueryType.Reader, out Result);

            if (Result.Count > 0)
            {
                Int32 Armor = (Int32)Result[1];
                Int32 Primary = (Int32)Result[2];
                Int32 Vitality = (Int32)Result[3];
                String Name = (String)Result[4];
                Int32 Level = (Int32)Result[5];
                Int32 Attack = (Int32)Result[6];
                float AttackSpeedBonus = (float)((double)Result[7]);
                //Int32 ItemID = (Int32)Result[8];

                switch ((String)Result[0])
                {
                    case "Amulet":
                        ObtainedItem = new Items.Amulet(Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID);
                        break;
                    case "Chest":
                        ObtainedItem = new Items.Chest(Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID);
                        break;
                    case "Feet":
                        ObtainedItem = new Items.Feet(Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID);
                        break;
                    case "Hands":
                        ObtainedItem = new Items.Hands(Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID);
                        break;
                    case "Head":
                        ObtainedItem = new Items.Head(Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID);
                        break;
                    case "Legs":
                        ObtainedItem = new Items.Legs(Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID);
                        break;
                    case "Offhand":
                        ObtainedItem = new Items.Offhand(Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID);
                        break;
                    case "Ring":
                        ObtainedItem = new Items.Ring(Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID);
                        break;
                    case "Weapon":
                        ObtainedItem = new Items.Weapon(Armor, Primary, Vitality, Name, Level, Attack, AttackSpeedBonus, ItemID);
                        break;

                    default:
                    case "None":
                        throw new Exception("I dont think there should be any items in the database without a specified slot.");
                        break;

                }
            }

            return ObtainedItem;
        }

        public static void LoadPlayer(EntityType Class, out Int32 PlayerID, out Int32 PlayerInventoryID, out Int32 PlayerLevel,
            out Int32 PlayerExperience, out Int32 PlayerGold, out String PlayerProgress)
        {
            List<Object> Result;

            ExecuteCommand("SELECT * FROM Player WHERE [Class] = '" + Class.ToString() + "'", QueryType.Reader, out Result);

            if (Result.Count > 0)
            {
                PlayerID = (Int32)Result[0];
                PlayerInventoryID = (Int32)Result[1];
                PlayerLevel = (Int32)Result[3];
                PlayerExperience = (Int32)Result[4];
                PlayerProgress = (String)Result[5];
                PlayerGold = (Int32)Result[6];

                return;
            }

            throw new Exception("Player could not be loaded from database");
        }

        public static void UpdatePlayer(EntityType Class, Int32 PlayerLevel, Int32 PlayerExperience, Int32 PlayerGold, String PlayerProgress)
        {
            ExecuteCommand("UPDATE Player SET Level = " + PlayerLevel.ToString() + " " +
                 "WHERE Class = '" + Class.ToString() + "'", QueryType.NonExecute);
            ExecuteCommand("UPDATE Player SET Experience = " + PlayerExperience.ToString() + " " +
                 "WHERE Class = '" + Class.ToString() + "'", QueryType.NonExecute);
            ExecuteCommand("UPDATE Player SET Gold = " + PlayerGold.ToString() + " " +
                 "WHERE Class = '" + Class.ToString() + "'", QueryType.NonExecute);
            ExecuteCommand("UPDATE Player SET Progress = '" + PlayerProgress + "' " +
                 "WHERE Class = '" + Class.ToString() + "'", QueryType.NonExecute);
            
        }

        #region Query entries

        ///////////////////////////////////////
        // ACCESS ENTRIES
        ///////////////////////////////////////

        public static List<Object> GetItemsFromInventory(String InventoryID)
        {
            List<Object> Result;

            ExecuteCommand("SELECT [ItemID] FROM InventoryManager WHERE [InventoryID] = " + InventoryID.ToString(), QueryType.Reader, out Result);

            return Result;
        }

        public static List<Object> GetEnemies()
        {
            List<Object> Result;

            ExecuteCommand("SELECT [EnemyID] FROM EnemyManager", QueryType.Reader, out Result);

            return Result;
        }

        public static List<Object> GetObjects()
        {
            List<Object> Result;

            ExecuteCommand("SELECT [ObjectID] FROM ObjectManager WHERE Identifier = ''", QueryType.Reader, out Result);

            return Result;
        }

        public static List<Object> GetObjectIDs()
        {
            List<Object> Result;

            ExecuteCommand("SELECT [UniqueID] FROM ObjectManager WHERE Identifier = ''", QueryType.Reader, out Result);

            return Result;
        }

        public static List<Object> GetEnemyIDs()
        {
            List<Object> Result;

            ExecuteCommand("SELECT [UniqueID] FROM EnemyManager", QueryType.Reader, out Result);

            return Result;
        }

        #endregion

        #region Execute command functions

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

        #endregion

    }
}
