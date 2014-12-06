using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;

namespace River
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        //DEBUGGING
        public const bool DrawTileCoords = false;
        public const bool CollisionDetection = true;

        public const int BackBufferWidth = 1280;
        public const int BackBufferHeight = 720;

        public const float StandardDelay = 200f;

        public static KeyboardState KeyboardState;
        public static KeyboardState LastKeyboardState;
        public static GamePadState GamePadState;
        public static GamePadState LastGamePadState;

        public const float SpeedConst = 5.25f; //Used as a constant in speed "formulas" Higher = Slower default speed

        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;

        private TitleScreen TitleScreen;
        private Level Level;
        private LevelEditor LevelEditor;
        private DatabaseEditor DatabaseEditor;

        public Main()
        {
            this.IsMouseVisible = true;
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = BackBufferWidth;
            Graphics.PreferredBackBufferHeight = BackBufferHeight;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GameDB.InitializeDatabase();
            Level = new Level();
            HUD.Initialize(Level);
            TitleScreen = new TitleScreen(this);
            MenuManager.Initialize(Level);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            TitleScreen.LoadContent(Content);
            HUD.LoadContent(Content, Graphics);
            Level.LoadContent(Content);
            Enemy.LoadContent(Content, Graphics);
            Item.LoadContent(Content);
            Equipment.LoadContent(Content);
            DamageEmitter.LoadContent(Content, Graphics);
            MenuManager.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            GameDB.CloseDatabase();
        }

        //Start the game
        public void NewGame(EntityType Class)
        {
            GameDB.CreatePlayer(Class.ToString());
            Item.LoadConditionalContent(Content, Class);
            Level.LoadLevel(false, Class);
            HUD.LoadConditionalContent(Content, Class);
            Level.LoadConditionalContent(Content, Graphics);
        }

        public void LoadGame(EntityType Class)
        {
            Int32 PlayerID;
            Int32 PlayerInventoryID;
            Int32 PlayerLevel;
            Int32 PlayerExperience;
            Int32 PlayerGold;
            String PlayerProgress;

            GameDB.LoadPlayer(Class, out PlayerID, out PlayerInventoryID, out PlayerLevel, out PlayerExperience, out PlayerGold, out PlayerProgress);

            Item.LoadConditionalContent(Content, Class);
            Level.LoadLevel(false, Class);
            HUD.LoadConditionalContent(Content, Class);
            Level.LoadConditionalContent(Content, Graphics);

            Level.Player.LevelValue = PlayerLevel;
            Level.Player.Experience = PlayerExperience;
            Level.Player.Gold = PlayerGold;

            Level.LoadSpecificMap(Int32.Parse(PlayerProgress.Split('_')[0]), Int32.Parse(PlayerProgress.Split('_')[1]));

            GameDB.UpdatePlayer(Class, PlayerLevel, PlayerExperience, PlayerGold, PlayerProgress);
        }

        //Start the editor
        public void GameEditorStart()
        {
            Level.LoadLevel(true, EntityType.Warrior);
            Level.LoadConditionalContent(Content, Graphics);
            LevelEditor = new LevelEditor(Level);
            LevelEditor.LoadContent(Content);
        }

        public void DataBaseEditorStart()
        {
            DatabaseEditor = new DatabaseEditor();
        }

        protected override void Update(GameTime GameTime)
        {
            GamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState = Keyboard.GetState();

            //Exit if requested to do so by title screen or ingame menu
            if (TitleScreen.CurrentScreen == TitleScreen.ScreenType.GameExited)
                this.Exit();

            if (MenuManager.ExitMenu.HasExited == true)
                this.Exit();

            //Update title screen if open
            if (TitleScreen.CurrentScreen != TitleScreen.ScreenType.Closed)
                TitleScreen.Update(GameTime);

            //Update level editor
            else if (LevelEditor != null)
            {
                LevelEditor.Update(GameTime);
                MenuManager.Update(GameTime);
            }
            //Update level/menus
            else
            {
                HUD.Update(GameTime);

                if (!HUD.Editing)
                    MenuManager.Update(GameTime);

                //Update level if there are no menus that are updating
                if (!MenuManager.HasOpenMenu())
                    Level.Update(GameTime);

            }

            LastGamePadState = GamePadState;
            LastKeyboardState = KeyboardState;

            base.Update(GameTime);
        }

        protected override void Draw(GameTime GameTime)
        {
            if (LevelEditor != null)
                GraphicsDevice.Clear(Color.Gray);
            else
                GraphicsDevice.Clear(Color.Black);


            if (TitleScreen.CurrentScreen != TitleScreen.ScreenType.Closed)
                TitleScreen.Draw(SpriteBatch);
            else
                Level.Draw(GameTime, SpriteBatch);


            if (LevelEditor != null)
            {
                LevelEditor.Draw(SpriteBatch);
                MenuManager.Draw(SpriteBatch);
            }
            //Draw UI if no level editor/title screen
            else if (TitleScreen.CurrentScreen == TitleScreen.ScreenType.Closed)
            {
                HUD.Draw(SpriteBatch);

                if (!HUD.Editing)
                    MenuManager.Draw(SpriteBatch);
            }

            base.Draw(GameTime);
        }

    }
}
