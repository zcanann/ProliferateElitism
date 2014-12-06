using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace River
{
    class IngameMenu : Menu
    {
        //Selection Type
        public enum IngameMenuSelectionType
        {
            Return,
            Inventory,
            Skills,
            Save,
            Options,
            Quit
        }

        private IngameMenuSelectionType MenuSelection;

        private Texture2D MenuBG;
        private Texture2D SubMenuBG;
        private Texture2D OptionsMenu;
        private Texture2D SaveMenu;

        private Vector2 SubmenuDrawPos;

        private Level LevelPTR;

        public IngameMenu(Level LevelPTR, MenuSideType MenuType)
            : base(MenuType)
        {
            this.LevelPTR = LevelPTR;
        }

        public override void Open()
        {
            MenuSelection = IngameMenuSelectionType.Return;
        }

        public void LoadContent(ContentManager Content)
        {
            MenuBG = Content.Load<Texture2D>(@"Textures\IngameMenu\IngameMenu");

            SubMenuBG = Content.Load<Texture2D>(@"Textures\IngameMenu\IngameMenu2");
            OptionsMenu = Content.Load<Texture2D>(@"Textures\IngameMenu\Options");
            SaveMenu = Content.Load<Texture2D>(@"Textures\IngameMenu\Save");
            SetPositions();
        }

        private void SetPositions()
        {
            SubmenuDrawPos = new Vector2(Main.BackBufferWidth / 2, 0);
        }

        float SelectionDelay = 0f;
        public override void Update(GameTime GameTime)
        {

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Start) &&
               !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Start))
            {
                MenuManager.CloseMenu(this);
                return;
            }

            SelectionDelay -= GameTime.ElapsedGameTime.Milliseconds;

            if (SelectionDelay < 0f)
                SelectionDelay = 0f;


            //B to exit menu
            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.B) &&
                !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.B))
            {
                MenuManager.CloseMenu(this);
                return;
            }

            if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) &&
               !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
                switch (MenuSelection)
                {
                    case IngameMenuSelectionType.Return:
                        MenuManager.CloseMenu(this);
                        break;
                    case IngameMenuSelectionType.Inventory:
                        //Open both inventory and equipment
                        MenuManager.OpenEquipmentMenu(false);
                        MenuManager.OpenInventoryMenu(true);
                        //Pass in the inventories to the swap helper class
                        SwapHelper.Connect(LevelPTR.Player.Inventory);
                        SwapHelper.Connect(LevelPTR.Player.Equipment);
                        break;
                    case IngameMenuSelectionType.Skills:
                        HUD.Editing = true;
                        break;
                    case IngameMenuSelectionType.Save:
                        //MenuManager.OpenSaveMenu(true);
                        GameDB.UpdatePlayer(LevelPTR.Player.Class, LevelPTR.Player.LevelValue, LevelPTR.Player.Experience, LevelPTR.Player.Gold,
                            LevelPTR.LevelMap.RoomSetID.ToString() + "_" + LevelPTR.LevelMap.RoomID.ToString());
                        MenuManager.CloseMenu(this);

                        //Temporary demo save
                        /*Random DynamicSaveRand = new Random((int)LevelPTR.Player.Gold);

                        //Get gold byte data
                        byte[] GoldBytes = BitConverter.GetBytes((int)LevelPTR.Player.Gold);

                        //Save junk data # 1
                        byte[] JunkData1 = new byte[DynamicSaveRand.Next(55, 155)];
                        for (int ecx = 0; ecx < JunkData1.Length; ecx++)
                        {
                            do
                            {
                                JunkData1[ecx] = (byte)DynamicSaveRand.Next(1, 254);
                            } while (JunkData1[ecx] == GoldBytes[0] ||
                                JunkData1[ecx] == GoldBytes[1] ||
                                JunkData1[ecx] == GoldBytes[2] ||
                                JunkData1[ecx] == GoldBytes[3]);
                        }

                        //Save gold amount
                        int GoldIndex = JunkData1.Length;
                        
                        int GoldByteLength = 0; //# of digits to save
                        for (int ecx = 0; ecx < GoldBytes.Length; ecx++)
                        {
                            if (GoldBytes[ecx] != 0)
                                GoldByteLength++;
                        }

                        //Save junk data #2
                        byte[] JunkData2 = new byte[DynamicSaveRand.Next(55, 155)];
                        for (int ecx = 0; ecx < JunkData2.Length; ecx++)
                        {
                            do
                            {
                                JunkData2[ecx] = (byte)DynamicSaveRand.Next(1, 254);
                            } while (JunkData2[ecx] == GoldBytes[0] ||
                                JunkData2[ecx] == GoldBytes[1] ||
                                JunkData2[ecx] == GoldBytes[2] ||
                                JunkData2[ecx] == GoldBytes[3]);
                        }

                         //Save index of gold
                        byte[] GoldIndexArray = BitConverter.GetBytes(GoldIndex);
                        byte[] GoldLengthArray = BitConverter.GetBytes(GoldByteLength);
                        //Anti-cheat save file length
                        byte[] FileLength = BitConverter.GetBytes(GoldLengthArray.Length + 4 + JunkData1.Length + JunkData2.Length + GoldByteLength + GoldIndexArray.Length);
                       

                        try
                        {
                            // Open file for reading
                            System.IO.FileStream _FileStream =
                               new System.IO.FileStream(@"C:\Users\Zachary\AppData\Roaming\Proliferate Elitism\save1.save", System.IO.FileMode.Create,
                                                        System.IO.FileAccess.Write);
                            _FileStream.SetLength(1);
                            // Writes a block of bytes to this stream using data from
                            // a byte array.
                            _FileStream.Write(FileLength, 0, FileLength.Length);
                            _FileStream.Write(GoldLengthArray, 0, GoldLengthArray.Length);
                            _FileStream.Write(GoldIndexArray, 0, GoldIndexArray.Length);
                            _FileStream.Write(JunkData1, 0, JunkData1.Length);
                            for (int ecx = 0; ecx < GoldBytes.Length; ecx++)
                            {
                                if (GoldBytes[ecx] != 0)
                                    _FileStream.Write(GoldBytes, ecx, 1);
                            }
                            _FileStream.Write(JunkData2, 0, JunkData2.Length);

                            // close file stream
                            _FileStream.Close();
                        }
                        catch (Exception _Exception)
                        {
                            // Error
                            Console.WriteLine("Exception caught in process: {0}",
                                              _Exception.ToString());
                        }*/

                        break;
                    case IngameMenuSelectionType.Options:
                        //MenuManager.OpenOptionsMenu(true);
                        MenuManager.CloseMenu(this);
                        break;
                    case IngameMenuSelectionType.Quit:
                        MenuManager.OpenExitMenu(true);
                        
                        break;
                }

            //Down
            if (Main.GamePadState.ThumbSticks.Left.Y < -0.5f &&
                SelectionDelay == 0f)
            {
                SelectionDelay = Main.StandardDelay;
                switch (MenuSelection)
                {
                    case IngameMenuSelectionType.Return:
                        MenuSelection = IngameMenuSelectionType.Inventory;
                        break;
                    case IngameMenuSelectionType.Inventory:
                        MenuSelection = IngameMenuSelectionType.Skills;
                        break;
                    case IngameMenuSelectionType.Skills:
                        MenuSelection = IngameMenuSelectionType.Save;
                        break;
                    case IngameMenuSelectionType.Save:
                        MenuSelection = IngameMenuSelectionType.Options;
                        break;
                    case IngameMenuSelectionType.Options:
                        MenuSelection = IngameMenuSelectionType.Quit;
                        break;
                    case IngameMenuSelectionType.Quit:
                        MenuSelection = IngameMenuSelectionType.Return;
                        break;
                }
            }

            //Up
            if (Main.GamePadState.ThumbSticks.Left.Y > 0.5f &&
                SelectionDelay == 0f)
            {
                SelectionDelay = Main.StandardDelay;
                switch (MenuSelection)
                {
                    case IngameMenuSelectionType.Return:
                        MenuSelection = IngameMenuSelectionType.Quit;
                        break;
                    case IngameMenuSelectionType.Inventory:
                        MenuSelection = IngameMenuSelectionType.Return;
                        break;
                    case IngameMenuSelectionType.Skills:
                        MenuSelection = IngameMenuSelectionType.Inventory;
                        break;
                    case IngameMenuSelectionType.Save:
                        MenuSelection = IngameMenuSelectionType.Skills;
                        break;
                    case IngameMenuSelectionType.Options:
                        MenuSelection = IngameMenuSelectionType.Save;
                        break;
                    case IngameMenuSelectionType.Quit:
                        MenuSelection = IngameMenuSelectionType.Options;
                        break;
                }
            }

        }


        private Vector2 CursorPosition = Vector2.Zero;
        public override void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Begin();

            //Black transparentish background
            SpriteBatch.Draw(MenuBG, Vector2.Zero, Color.White);
            //SpriteBatch.Draw(SubMenuBG, SubmenuDrawPos, Color.White);

            //Draw cursor at selection position
            switch (MenuSelection)
            {
                case IngameMenuSelectionType.Return:
                    CursorPosition = new Vector2(216, 110);
                    break;
                case IngameMenuSelectionType.Inventory:
                    CursorPosition = new Vector2(200, 194);
                    break;
                case IngameMenuSelectionType.Skills:
                    CursorPosition = new Vector2(218, 288);
                    break;
                case IngameMenuSelectionType.Save:
                    CursorPosition = new Vector2(232, 376);
                    break;
                case IngameMenuSelectionType.Options:
                    CursorPosition = new Vector2(204, 458);
                    break;
                case IngameMenuSelectionType.Quit:
                    CursorPosition = new Vector2(232, 542);
                    break;
            }
            SpriteBatch.Draw(TitleScreen.VerticalSelect, CursorPosition, Color.White);

            SpriteBatch.End();
        }
    }
}