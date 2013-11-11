using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace River
{
    /// <summary>
    /// Manages every bloody menu on earth
    /// </summary>
    static class MenuManager
    {

        //Both sides
        private static Minimap Minimap;
        //Right Side
        private static InventoryMenu InventoryMenu;
        private static OptionsMenu OptionsMenu;
        private static SaveMenu SaveMenu;
        private static ExitMenu ExitMenu;
        //Left Side
        private static IngameMenu IngameMenu;
        private static EquipmentMenu EquipmentMenu;
        private static LootMenu LootMenu;
        private static EnchantingMenu EnchantingMenu;
        private static StorageMenu StorageMenu;
        private static ShopMenu ShopMenu;

        private static MenuSideType CurrentFocus;

        private static Menu LeftMenu; //Also holds menus that take up both screens
        private static Menu RightMenu;

        private static Rectangle BlackBGRectangle;
        public static Texture2D BlackBG;

        public static void Initialize(Level LevelPTR)
        {
            Minimap = new Minimap(LevelPTR, MenuSideType.Both);
            InventoryMenu = new InventoryMenu(MenuSideType.Right, LevelPTR);
            OptionsMenu = new OptionsMenu(MenuSideType.Right);
            SaveMenu = new SaveMenu(MenuSideType.Right);
            ExitMenu = new ExitMenu(MenuSideType.Right);
            IngameMenu = new IngameMenu(LevelPTR, MenuSideType.Left);
            EquipmentMenu = new EquipmentMenu(MenuSideType.Left, LevelPTR);
            LootMenu = new LootMenu(MenuSideType.Left, LevelPTR);
            EnchantingMenu = new EnchantingMenu(MenuSideType.Left, LevelPTR);
            StorageMenu = new StorageMenu(MenuSideType.Left, LevelPTR);
            ShopMenu = new ShopMenu(MenuSideType.Left, LevelPTR);
        }

        public static void LoadContent(ContentManager Content)
        {

            Minimap.LoadContent(Content);
            IngameMenu.LoadContent(Content);
            SwapHelper.LoadContent(Content);
            //OptionsMenu.LoadContent(Content);

            BlackBG = Content.Load<Texture2D>(@"Textures\IngameMenu\SolidBlackSquare");
            BlackBGRectangle = new Rectangle(0, 0, Main.BackBufferWidth, Main.BackBufferHeight);
        }

        public static bool HasOpenMenu()
        {
            if (LeftMenu != null || RightMenu != null)
                return true;

            return false;
        }

        public static void ChangeFocus()
        {
            if (CurrentFocus == MenuSideType.Left)
                CurrentFocus = MenuSideType.Right;
            else if (CurrentFocus == MenuSideType.Right)
                CurrentFocus = MenuSideType.Left;
            else
                throw new Exception("Hey jackass, you screwed up and somehow the current menu selection was both. (class MenuManager)");
        }

        public static bool HasFocus(Menu CallingMenu)
        {
            if (CallingMenu.GetMenuType() == CurrentFocus ||
                CallingMenu.GetMenuType() == MenuSideType.Both)
                return true;

            return false;
        }

        public static void CloseMenu(Menu CallingMenu)
        {
            if (LeftMenu == CallingMenu)
                LeftMenu = null;
            if (RightMenu == CallingMenu)
                RightMenu = null;
        }
        public static void CloseAll()
        {
            LeftMenu = null;
            RightMenu = null;
        }

        public static void ExitGame()
        {

        }

        private static void OpenMenu(Menu TargetMenu, bool GiveFocus)
        {
            if (TargetMenu.GetMenuType() == MenuSideType.Both)
            {
                LeftMenu = TargetMenu;
                RightMenu = null;
                if (GiveFocus)
                    CurrentFocus = MenuSideType.Left;
                LeftMenu.Open();
            }

            if (TargetMenu.GetMenuType() == MenuSideType.Left)
            {
                LeftMenu = TargetMenu;
                if (GiveFocus)
                    CurrentFocus = MenuSideType.Left;
                LeftMenu.Open();
            }

            if (TargetMenu.GetMenuType() == MenuSideType.Right)
            {
                RightMenu = TargetMenu;
                if (GiveFocus)
                    CurrentFocus = MenuSideType.Right;
                RightMenu.Open();
            }

        }

        public static void OpenIngameMenu(bool GiveFocus)
        {
            OpenMenu(IngameMenu, GiveFocus);
        }
        public static void OpenMinimapMenu(bool GiveFocus)
        {
            OpenMenu(Minimap, GiveFocus);
        }
        public static void OpenEquipmentMenu(bool GiveFocus)
        {
            OpenMenu(EquipmentMenu, GiveFocus);
        }
        public static void OpenInventoryMenu(bool GiveFocus)
        {
            OpenMenu(InventoryMenu, GiveFocus);
        }
        public static void OpenLootMenu(bool GiveFocus)
        {
            OpenMenu(LootMenu, GiveFocus);
        }
        public static void OpenSaveMenu(bool GiveFocus)
        {
            OpenMenu(SaveMenu, GiveFocus);
        }
        public static void OpenStorageMenu(bool GiveFocus)
        {
            OpenMenu(StorageMenu, GiveFocus);
        }
        public static void OpenShopMenu(bool GiveFocus)
        {
            OpenMenu(ShopMenu, GiveFocus);
        }
        public static void OpenEnchantingMenu(bool GiveFocus)
        {
            OpenMenu(EnchantingMenu, GiveFocus);
        }
        public static void OpenOptionsMenu(bool GiveFocus)
        {
            OpenMenu(OptionsMenu, GiveFocus);
        }
        public static void OpenExitMenu(bool GiveFocus)
        {
            OpenMenu(ExitMenu, GiveFocus);
        }

        public static void Update(GameTime GameTime)
        {

            if (LeftMenu == null)
            {
                if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Start) &&
                    !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Start))
                {
                    //LeftMenu = IngameMenu;
                    OpenIngameMenu(true);
                    return;
                }
                if (Main.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Back) &&
                     !Main.LastGamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Back))
                {
                    OpenMinimapMenu(true);
                }
            }

            switch (CurrentFocus)
            {
                case MenuSideType.Right:
                    if (RightMenu != null)
                        RightMenu.Update(GameTime);
                    break;

                default:
                case MenuSideType.Left:
                    if (LeftMenu != null)
                        LeftMenu.Update(GameTime);
                    break;
            }

        }

        public static void Draw(SpriteBatch SpriteBatch)
        {
            if (RightMenu != null || LeftMenu != null)
                if (!HUD.Editing)
                    DrawTransparentBlackBG(SpriteBatch);

            if (RightMenu != null)
                RightMenu.Draw(SpriteBatch);

            if (LeftMenu != null)
                LeftMenu.Draw(SpriteBatch);

        }

        public static void DrawTransparentBlackBG(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(BlackBG, BlackBGRectangle, Color.White);
            SpriteBatch.End();
        }

    }

    public enum MenuType
    {
        Minimap,
        InventoryMenu,
        OptionsMenu,
        IngameMenu,
        EquipmentMenu,
        EnchantingMenu,
        StorageMenu,
    }

    public enum MenuSideType
    {
        Left,
        Right,
        Both,
    }
}
