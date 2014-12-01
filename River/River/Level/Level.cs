using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace River
{
    class Level
    {
        private bool LevelEditorMode;

        private SpriteFont DebugFont;
        private Random Random = new Random();
        public TileMap LevelMap;
        public Player Player;
        public Enemy[] Enemies;
        public List<DamageEmitter> DamageEmitters = new List<DamageEmitter>();
        private ChestInventory[] Chests;
        public ShopInventory ShopInventory;
        public StorageInventory StorageInventory;
        public EnchantingInventory EnchantingInventory;

        public Enemy LastLootedEnemy;

        private List<LootInventory> LootPriorityInventories = new List<LootInventory>();

        public Enemy TargetEnemy; //Enemy to display UI elements for

        public Level()
        {
            //Create Player/Map
            LevelMap = new TileMap();
            EnchantingInventory = new EnchantingInventory(GameDB.GetEnchanterInventoryID());
            StorageInventory = new StorageInventory(GameDB.GetStorageInventoryID());
        }

        public void LoadLevel(bool LevelEditorMode, EntityType Class)
        {
            this.LevelEditorMode = LevelEditorMode;

            //Clear old inventories
            LootPriorityInventories.Clear();

            if (!LevelEditorMode)
            {
                LoadNextMap();
                Player = new Player(LevelMap.PlayerSpawn, this, Class);
                LoadNextMapPt2();
            }
            else
            {
                Enemies = new Enemy[0];
                Player = new Player(Vector2.Zero, this, EntityType.Warrior);
            }
        }


        public void LoadContent(ContentManager Content)
        {
            //Load misc content
            DebugFont = Content.Load<SpriteFont>(@"Fonts\Pericles6");
            //Load tile content
            Tile.LoadContent(Content);
            //Load map content
            LevelMap.LoadContent(Content);
        }

        public void LoadConditionalContent(ContentManager Content, IGraphicsDeviceService Graphics)
        {
            //Load player content
            Player.LoadConditionalContent(Content, Graphics);
            Skill.LoadConditionalContent(Content, Player.Class);
        }

        #region Public Methods

        public void OrganizeLootPriority(LootInventory Inventory)
        {

            //Find best quality item in new inventory
            int HighestQuality = 0;
            for (int ecx = 0; ecx < Inventory.Items.Length; ecx++)
            {
                //Set priority based on quality of the best item
                if (Inventory.Items[ecx] != Item.None &&
                    (int)Inventory.Items[ecx].Quality > HighestQuality)
                {
                    HighestQuality = (int)Inventory.Items[ecx].Quality;
                }
            }

            int FirstIndex = LootPriorityInventories.Count;
            int ComparedHighestQuality = 0;
            for (int ecx = 0; ecx < LootPriorityInventories.Count; ecx++)
            {

                //Find highest quality item in saved inventories
                for (int edi = 0; edi < LootPriorityInventories[ecx].Items.Length; edi++)
                {
                    if (LootPriorityInventories[ecx].Items[edi] != Item.None &&
                        (int)LootPriorityInventories[ecx].Items[edi].Quality > HighestQuality)
                    {
                        ComparedHighestQuality = (int)LootPriorityInventories[ecx].Items[edi].Quality;
                    }
                }

                if (HighestQuality >= ComparedHighestQuality)
                {
                    FirstIndex = ecx;
                    break;
                }
            }

            if (!Inventory.IsEmpty())
                LootPriorityInventories.Insert(FirstIndex, Inventory);

        }

        public void GivePlayerGoldExp(long Gold, int Experience)
        {
            Player.AddGoldExperience(Gold, Experience);
        }

        public float GetPlayerX()
        {
            return Player.Position.X;
        }

        public float GetPlayerY()
        {
            return Player.Position.Y;
        }

        public int GetSecondarySkillSelection()
        {
            return Player.SecondarySelection;
        }
        public int GetDefensiveSkillSelection()
        {
            return Player.DefensiveSelection;
        }
        public int GetSpecialSkillSelection()
        {
            return Player.SpecialSelection;
        }

        public void SetSecondarySkillSelection(int Selection)
        {
            Player.SecondarySelection = Selection;
        }
        public void SetDefensiveSkillSelection(int Selection)
        {
            Player.DefensiveSelection = Selection;
        }
        public void SetSpecialSkillSelection(int Selection)
        {
            Player.SpecialSelection = Selection;
        }

        #endregion

        public bool CheckForExitTile()
        {
            if (LevelMap.IsExitCell(Player.Position))
            {
                LoadNextMap();
                LoadNextMapPt2();
                return true;
            }
            return false;
        }

        private void LoadNextMap()
        {
            LevelMap.LoadNextMap();
            ShopInventory = new ShopInventory(this, GameDB.GetShopInventoryID());

            GameDB.RemoveAllObjects();
            GameDB.RemoveAllEnemies();
            GameDB.RemoveUnusedInventories();

            LoadObjects();
            LoadEnemies();

        }

        private void LoadObjects()
        {
            Int32 ObjectCount = LevelMap.EnemySpawn.Length;

            GameDB.CreateLevelObjects(ObjectCount, new Int32[] { 1, 2 });
            List<Object> ObjectTypes = GameDB.GetObjects();
            List<Object> ObjectIDs = GameDB.GetObjectIDs();

            // Should not happen, but will prevent errors if something weird occurs
            if (ObjectTypes.Count != ObjectCount && ObjectIDs.Count != ObjectCount)
            {
                throw new Exception("Something went wrong");
                //ObjectCount = ObjectIDs.Count;
            }

            //Create objects
            Chests = new ChestInventory[LevelMap.ChestSpawns.Length];

            ChestType NextChestType;

            for (int ecx = 0; ecx < LevelMap.ChestSpawns.Length; ecx++)
            {
                switch ((Int32)ObjectTypes[ecx])
                {
                    case 1:
                        NextChestType = ChestType.Chest;
                        break;
                    case 2:
                        NextChestType = ChestType.Lockbox;
                        break;
                    default:
                        NextChestType = ChestType.Chest;
                        break;
                }

                Chests[ecx] = new ChestInventory(NextChestType, GameDB.GetObjectInventoryID((Int32)ObjectIDs[ecx]));
            }
        }

        private void LoadEnemies()
        {
            Int32 EnemyCount = LevelMap.EnemySpawn.Length;

            GameDB.CreateLevelEnemies(EnemyCount, new Int32[] { 1, 2, 3, 4, 5, 6 });
            List<Object> EnemyTypes = GameDB.GetEnemies();
            List<Object> EnemyIDs = GameDB.GetEnemyIDs();

            // Should not happen, but will prevent errors if something weird occurs
            if (EnemyTypes.Count != EnemyCount && EnemyIDs.Count != EnemyCount)
            {
                throw new Exception("Something went wrong");
                //EnemyCount = EnemyIDs.Count;
            }

            EntityType NextEntityType;

            //Create enemies
            Enemies = new Enemy[EnemyCount];

            int CurrentExpPool = LevelMap.ExpPool[LevelMap.RoomSetID][LevelMap.RoomID];
            int ExpShare = 0;

            if (Enemies.Length > 0)
                ExpShare = CurrentExpPool / Enemies.Length;

            if (ExpShare == 0)
                ExpShare = 1;

            for (int ecx = 0; ecx < Enemies.Length; ecx++)
            {
                switch ((Int32)EnemyTypes[ecx])
                {
                    case 1:
                        NextEntityType = EntityType.Elemental;
                        break;
                    case 2:
                        NextEntityType = EntityType.Goblin;
                        break;
                    case 3:
                        NextEntityType = EntityType.Ogre;
                        break;
                    case 4:
                        NextEntityType = EntityType.Skeleton;
                        break;
                    case 5:
                        NextEntityType = EntityType.Slime;
                        break;
                    case 6:
                        NextEntityType = EntityType.Zombie;
                        break;
                    default:
                        NextEntityType = EntityType.Skeleton;
                        break;
                }

                Enemies[ecx] = new Enemy(LevelMap.EnemySpawn[ecx], this, NextEntityType, ExpShare, (Int32)EnemyIDs[ecx]);

            }
        }

        private void LoadNextMapPt2()
        {
            Player.Position = LevelMap.PlayerSpawn;
            if (Player.SpriteAnimation != null)
                Player.SpriteAnimation.Position = LevelMap.PlayerSpawn;
            ShopInventory.GenerateShopItems(Player.LevelValue);
            Player.UpdateCamera();
        }


        public void SendPrioritizedToBack(LootInventory Inv)
        {
            int Index = LootPriorityInventories.IndexOf(Inv);

            if (Index == -1)
                return;

            //Move the item to back of queue
            LootPriorityInventories.RemoveAt(Index);
            LootPriorityInventories.Insert(LootPriorityInventories.Count, Inv);
        }

        public LootInventory FindNearbyLoot()
        {

            //Enemies
            for (int ecx = 0; ecx < LootPriorityInventories.Count; ecx++)
            {
                for (int edi = 0; edi < Enemies.Length; edi++)
                {
                    if (!Enemies[edi].IsAlive)
                        if (!Enemies[edi].Inventory.IsEmpty())
                            if (Enemies[edi].Inventory == LootPriorityInventories[ecx])
                                if (Tile.IntersectionTest(Enemies[edi].Position, Player.Position, Tile.TileWidth))
                                {
                                    //Swap bottom item and this item
                                    Player.NearLootable = true;
                                    LastLootedEnemy = Enemies[edi];
                                    return Enemies[edi].Inventory;
                                }
                }
            }


            //Chests
            for (int ecx = 0; ecx < Chests.Length; ecx++)
            {
                if (!(Chests[ecx].IsGenerated() && Chests[ecx].IsEmpty()))
                    if (Tile.IntersectionTest(LevelMap.ChestSpawns[ecx], Player.Position, Tile.TileWidth))
                    {
                        Player.NearLootable = true;
                        return Chests[ecx];
                    }
            }

            Player.NearLootable = false;
            return null;
        }

        public void Update(GameTime GameTime)
        {
            //Update damage emitters
            UpdateEmitters(GameTime);

            Entity.UpdateStaticEmitters(GameTime);

            //Update Player
            Player.Update(GameTime);

            if (LevelMap.IsInTown())
            {
                CheckForShop();
                CheckForStorage();
                CheckForEnchanter();
            }

            //Update enemies
            for (int ecx = 0; ecx < Enemies.Length; ecx++)
            {
                //Only update near-ish enemies
                if (Tile.SquareTest(Enemies[ecx].Position, Player.Position, Main.BackBufferWidth * 2))
                    Enemies[ecx].Update(GameTime, Player);
            }
        }

        private void CheckForShop()
        {
            if (Tile.IntersectionTest(LevelMap.ShopSpawn, Player.Position, Tile.TileWidth))
            {
                Player.NearShop = true;

                if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) &&
                    !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
                {
                    MenuManager.OpenShopMenu(true);
                    MenuManager.OpenInventoryMenu(false);
                    //Pass in the inventories to the swap helper class
                    SwapHelper.Connect(Player.Inventory);
                    SwapHelper.Connect(ShopInventory);
                }

            }
        }

        private void CheckForStorage()
        {
            if (Tile.IntersectionTest(LevelMap.StorageSpawn, Player.Position, Tile.TileWidth))
            {
                Player.NearStorage = true;

                if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) &&
                    !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
                {
                    MenuManager.OpenStorageMenu(true);
                    MenuManager.OpenInventoryMenu(false);
                    //Pass in the inventories to the swap helper class
                    SwapHelper.Connect(Player.Inventory);
                    SwapHelper.Connect(StorageInventory);
                }

            }
        }

        private void CheckForEnchanter()
        {
            if (Tile.IntersectionTest(LevelMap.EnchanterSpawn, Player.Position, Tile.TileWidth))
            {
                Player.NearEnchanter = true;

                if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) &&
                    !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
                {
                    MenuManager.OpenEnchantingMenu(false);
                    MenuManager.OpenInventoryMenu(true);
                    //Pass in the inventories to the swap helper class
                    SwapHelper.Connect(Player.Inventory);
                    SwapHelper.Connect(EnchantingInventory);
                }

            }
        }

        private void UpdateEmitters(GameTime GameTime)
        {

            //Tile check
            for (int ecx = 0; ecx < DamageEmitters.Count; ecx++)
            {
                DamageEmitters[ecx].Update(GameTime);
                if (!DamageEmitters[ecx].IsMarkedDead())
                    if (!LevelMap.IsWalkableCell(DamageEmitters[ecx].Position))
                        DamageEmitters[ecx].Kill();
            }

            //Remove dead emitters
            int eax = 0;
        Loop:
            for (; eax < DamageEmitters.Count; eax++)
                if (!DamageEmitters[eax].GetIsAlive())
                {
                    //Removing an index in the middle of a loop is problematic.
                    //Using goto to escape and cycle through the leftovers is a good way to avoid issues
                    DamageEmitters.RemoveAt(eax);
                    goto Loop; //Don't increment loop counter -- just exit the loop
                }

            //UPDATE DAMAGE EMITTERS
            for (int ecx = 0; ecx < DamageEmitters.Count; ecx++)
            {

                //If player owns the emitter...
                if (DamageEmitters[ecx].IsPlayerOwned())
                {
                    //Test against all active enemies
                    for (int edi = 0; edi < Enemies.Length; edi++)
                    {
                        if (//Enemies[edi].Explored &&
                            Enemies[edi].IsAlive &&
                            !DamageEmitters[ecx].IsMarkedDead() &&
                            DamageEmitters[ecx].Intersects(Enemies[edi].Position, edi))
                        {
                            //Set up target enemy for UI display
                            TargetEnemy = Enemies[edi];

                            //Try weapon debuff
                            if (TryWeaponDebuff())
                                Enemies[edi].ActiveBuffs.Add(GetWeaponDebuff());

                            //Add any spell debuffs
                            if (DamageEmitters[ecx].Debuff != null)
                                Enemies[edi].ActiveBuffs.Add(DamageEmitters[ecx].GetDebuff());

                            //Damage enemy
                            Enemies[edi].ChangeHealth((int)Player.ComputeDamage(DamageEmitters[ecx].GetDamage()));

                        }
                    }
                }
                else
                {
                    //Owned by enemy, test vs player
                    if (!DamageEmitters[ecx].IsMarkedDead() &&
                        DamageEmitters[ecx].Intersects(Player.Position, -1))
                    {
                        //Add any spell debuffs to player
                        if (DamageEmitters[ecx].Debuff != null)
                            Player.ActiveBuffs.Add(DamageEmitters[ecx].GetDebuff());

                        //Damage player
                        Player.ChangeHealth((int)DamageEmitters[ecx].ParentEntity.ComputeDamage(DamageEmitters[ecx].GetDamage()));
                    }
                }
            }
        }

        private bool TryWeaponDebuff()
        {
            //Returns true (if success) based on whether or not debuff exists and random number gen results

            if (Player.Equipment.Items[(int)Item.SlotType.Weapon] != Item.None)
                if (Player.Equipment.Items[(int)Item.SlotType.Weapon].DebuffOnAttack != null)
                {

                    switch (Player.Equipment.Items[(int)Item.SlotType.Weapon].DebuffOnAttack.GetState())
                    {
                        case Buff.StateType.Burn:
                        case Buff.StateType.Poison:
                        case Buff.StateType.Chill:
                            if (Random.Next(0, 100) < 85)
                                return false;
                            break;
                        case Buff.StateType.Freeze:
                            if (Random.Next(0, 100) < 95)
                                return false;
                            break;
                    }

                    return true;
                }

            return false;
        }

        private Buff GetWeaponDebuff()
        {
            Buff BuffCopy = new Buff
              (Player.Equipment.Items[(int)Item.SlotType.Weapon].DebuffOnAttack.GetName(),
              Player.Equipment.Items[(int)Item.SlotType.Weapon].DebuffOnAttack.GetSpeed(),
              Player.Equipment.Items[(int)Item.SlotType.Weapon].DebuffOnAttack.GetState(),
              Player.Equipment.Items[(int)Item.SlotType.Weapon].DebuffOnAttack.GetDurationMax(),
              Player.Equipment.Items[(int)Item.SlotType.Weapon].DebuffOnAttack.GetTickDurationMax(),
                //Compute damage for the debuff tick
              Player.ComputeDamage(-Player.Equipment.Items[(int)Item.SlotType.Weapon].DebuffOnAttack.GetTickHealthOffset()),
              Player.Equipment.Items[(int)Item.SlotType.Weapon].DebuffOnAttack.GetSpeedMultiplier());

            return BuffCopy;
        }

        #region Draw

        //float EarthQuake = 0f;
        //bool dir = true;
        public void Draw(GameTime GameTime, SpriteBatch SpriteBatch)
        {

            SpriteEffects Flip = SpriteEffects.None;
            Color DrawColor = Color.White;

            Vector2 FirstSquare = new Vector2(Camera.Location.X / Tile.TileStepX, Camera.Location.Y / Tile.TileStepY);
            int FirstX = (int)MathHelper.Clamp(FirstSquare.X - 1, 0, LevelMap.MapWidth);
            int FirstY = (int)MathHelper.Clamp(FirstSquare.Y - 3, 0, LevelMap.MapHeight);

            Vector2 LastSquare = new Vector2((Camera.Location.X + Main.BackBufferWidth) / Tile.TileStepX,
                (Camera.Location.Y + Main.BackBufferHeight) / Tile.TileStepY);

            int LastX = (int)MathHelper.Clamp(LastSquare.X + 1, 0, LevelMap.MapWidth);
            int LastY = (int)MathHelper.Clamp(LastSquare.Y + 3, 0, LevelMap.MapHeight); //+ 3 is to allow for offscreen height tiles

            //Set player MapPoint to help with drawing
            Player.MapPoint = LevelMap.WorldToMapCell(new Point((int)Player.Position.X, (int)Player.Position.Y));
            //Set enemy map points too

            for (int ecx = 0; ecx < Enemies.Length; ecx++)
                Enemies[ecx].MapPoint = LevelMap.WorldToMapCell(new Point((int)Enemies[ecx].Position.X, (int)Enemies[ecx].Position.Y));

            for (int ecx = 0; ecx < DamageEmitters.Count; ecx++)
                DamageEmitters[ecx].MapPoint = LevelMap.WorldToMapCell(new Point((int)DamageEmitters[ecx].Position.X, (int)DamageEmitters[ecx].Position.Y));

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            int RowOffset;

            for (int Y = 0; Y < LastY; Y++)
            {
                //Determine if there is a row offset if it is an odd row

                if ((FirstY + Y) % 2 == 1)
                    RowOffset = Tile.OddRowXOffset;
                else
                    RowOffset = 0;

                for (int X = 0; X < LastX; X++)
                {
                    //Get the offset and keep it within bounds
                    int MapX = FirstX + X;
                    int MapY = FirstY + Y;

                    if ((MapX < 0) || (MapY < 0) ||
                        (MapX >= LastX) || (MapY >= LastY) ||
                        // (!LevelMap.MapCells[MapY, MapX].Explored && !LevelEditorMode) ||
                        LevelMap.MapCells[MapY, MapX].TileID == -1 ||
                        LevelMap.MapCells[MapY, MapX].TileID >= Tile.FloorTiles.Length) //TEMP
                        continue;

                    //1 - DRAW BASE TILE
                    SpriteBatch.Draw(
                        Tile.FloorTiles[LevelMap.MapCells[MapY, MapX].TileID],
                        Camera.WorldToScreen(
                            new Vector2((MapX * Tile.TileStepX) + RowOffset, MapY * Tile.TileStepY)),
                        Tile.FloorTiles[LevelMap.MapCells[MapY, MapX].TileID].Bounds,
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        1.0f);
                }
            }

            for (int Y = 0; Y < LastY; Y++)
            {
                int MapY = FirstY + Y;

                //Draw player
                if (MapY == Player.MapPoint.Y)
                    Player.Draw(SpriteBatch);

                //Draw enemies
                for (int ecx = 0; ecx < Enemies.Length; ecx++)
                    if ((MapY == Enemies[ecx].MapPoint.Y))
                        Enemies[ecx].Draw(SpriteBatch);

                //Draw emitters
                for (int ecx = 0; ecx < DamageEmitters.Count; ecx++)
                    if ((MapY == DamageEmitters[ecx].MapPoint.Y))
                        DamageEmitters[ecx].Draw(SpriteBatch, this);

                for (int X = 0; X < LastX; X++)
                {
                    //Get the offset and keep it within bounds
                    int MapX = FirstX + X;

                    //Determine if there is a row offset if it is an odd row
                    if ((FirstY + Y) % 2 == 1)
                        RowOffset = Tile.OddRowXOffset;
                    else
                        RowOffset = 0;

                    if ((MapX < 0) || (MapY < 0) ||
                        (MapX >= LastX) || (MapY >= LastY) ||
                        // (!LevelMap.MapCells[MapY, MapX].Explored && !LevelEditorMode) ||
                        LevelMap.MapCells[MapY, MapX].TileID == -1)
                        continue;

                    int HeightRow = 0;

                    //Adjust flip
                    if (LevelMap.MapCells[MapY, MapX].Flipped)
                        Flip = SpriteEffects.FlipHorizontally;
                    else
                        Flip = SpriteEffects.None;


                    //Set opacity if there are enemies/players
                    DrawColor.A = 255;
                    if (LevelMap.MapCells[MapY, MapX].HeightTiles.Count >= 2)
                        if (MapY >= Player.MapPoint.Y &&
                            MapY <= Player.MapPoint.Y + 4 &&
                            MapX >= Player.MapPoint.X - 1 &&
                            MapX <= Player.MapPoint.X + 1)
                            DrawColor.A = 96;


                    //2 - DRAW HEIGHT TILES
                    foreach (int TileID in LevelMap.MapCells[MapY, MapX].HeightTiles)
                    {
                        SpriteBatch.Draw(
                            Tile.HeightTiles[TileID],
                            Camera.WorldToScreen(
                                new Vector2(
                                    (MapX * Tile.TileStepX) + RowOffset + (Tile.TileWidth - Tile.HeightTiles[TileID].Bounds.Width) / 2,
                                    (MapY - 2) * Tile.TileStepY - (HeightRow * Tile.HeightTileOffset) + (Tile.TileHeight - Tile.HeightTiles[TileID].Bounds.Height) / 2)),
                            Tile.HeightTiles[TileID].Bounds, //LevelMap.MapCells[MapY, MapX].TileID].Bounds,
                           DrawColor,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            Flip,
                            1.0f);

                        //Increment the height so that the next object is drawn at a "higher" place
                        HeightRow++;
                    }

                    //DRAW SURFACE TILES
                    foreach (int TileID in LevelMap.MapCells[MapY, MapX].SurfaceTiles)
                    {
                        SpriteBatch.Draw(
                             Tile.SurfaceTiles[TileID],
                            Camera.WorldToScreen(
                                new Vector2(
                                    (MapX * Tile.TileStepX) + RowOffset + (Tile.TileWidth - Tile.SurfaceTiles[TileID].Bounds.Width) / 2,
                                    (MapY - 2) * Tile.TileStepY - (HeightRow * Tile.HeightTileOffset) + (Tile.TileHeight - Tile.SurfaceTiles[TileID].Bounds.Height) / 2)),
                           Tile.SurfaceTiles[TileID].Bounds,
                           Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            Flip,
                            1f);
                    }

                } //END X LOOP

            } //END Y LOOP

            //DRAW X,Y COORDS ON TILE (if const in Main is set)
            if (Main.DrawTileCoords)
                for (int Y = 0; Y < LastY; Y++)
                {
                    int MapY = FirstY + Y;

                    //Determine if there is a row offset if it is an odd row
                    for (int X = 0; X < LastX; X++)
                    {
                        //Get the offset and keep it within bounds
                        int MapX = FirstX + X;

                        if ((FirstY + Y) % 2 == 1)
                            RowOffset = Tile.OddRowXOffset;
                        else
                            RowOffset = 0;

                        SpriteBatch.DrawString(DebugFont, (MapX).ToString() + ", " + (MapY).ToString(),
                            Camera.WorldToScreen(
                                new Vector2((MapX * Tile.TileStepX) + RowOffset + 48,
                                    MapY * Tile.TileStepY + 24)),
                                 Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    }
                }

            //DEBUG DISPLAY
            SpriteBatch.DrawString(DebugFont, "Camera: " + Camera.Location.X.ToString() + ", " + Camera.Location.Y.ToString(), Vector2.Zero, Color.Red);
            SpriteBatch.DrawString(DebugFont, "Player: " + Player.Position.X.ToString() + ", " + Player.Position.Y.ToString(), new Vector2(0, 16), Color.Red);
            SpriteBatch.DrawString(DebugFont, "Exp: " + Player.Experience.ToString() + " / " + Player.ExpPerLevel.ToString(), new Vector2(0, 32), Color.Red);
            SpriteBatch.DrawString(DebugFont, "Level: " + Player.LevelValue.ToString(), new Vector2(0, 48), Color.Red);

            Player.DrawGoldFloatingText(SpriteBatch);
            Player.DrawExpFloatingText(SpriteBatch);

            SpriteBatch.End();

        } //END DRAW

        #endregion
    }
}