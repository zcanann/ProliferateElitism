using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace River
{
    class IngameMenu : Menu
    {
        //Selection Type
        private enum IngameMenuSelectionType
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
                        MenuManager.OpenSaveMenu(true);
                        break;
                    case IngameMenuSelectionType.Options:
                        MenuManager.OpenOptionsMenu(true);
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