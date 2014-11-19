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

        protected override void UnloadContent() { }

        //Start the game
        public void NewGame(EntityType Class)
        {
            Item.LoadConditionalContent(Content, Class);
            Level.LoadLevel(false, Class);
            HUD.LoadConditionalContent(Content, Class);
            Level.LoadConditionalContent(Content, Graphics);
        }

        public void LoadGame(EntityType Class)
        {
            Item.LoadConditionalContent(Content, Class);
            Level.LoadLevel(false, Class);
            HUD.LoadConditionalContent(Content, Class);
            Level.LoadConditionalContent(Content, Graphics);

            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(@"C:\Users\Zachary\AppData\Roaming\Proliferate Elitism\save1.save", System.IO.FileMode.Open,
                                            System.IO.FileAccess.Read);
                // Writes a block of bytes to this stream using data from
                // a byte array.

                int FileLength = -1;
                int GoldLength = -1;
                int GoldIndex = -1;
                int GoldValue = -1;
                byte[] FileLengthA = new byte[4];
                byte[] GoldLengthA = new byte[4];
                byte[] GoldIndexA = new byte[4];
                byte[] GoldValueA = new byte[4];

                _FileStream.Read(FileLengthA, 0, FileLengthA.Length);
                _FileStream.Read(GoldLengthA, 0, GoldLengthA.Length);
                _FileStream.Read(GoldIndexA, 0, GoldIndexA.Length);

                FileLength = BitConverter.ToInt32(FileLengthA, 0);
                GoldLength = BitConverter.ToInt32(GoldLengthA, 0);
                GoldIndex = BitConverter.ToInt32(GoldIndexA, 0);

                if (_FileStream.Length != FileLength)
                {
                    throw new Exception("Error! Save file corrupted!");
                }

                _FileStream.Seek(GoldIndex + 12, System.IO.SeekOrigin.Begin);
                for (int ecx = 0; ecx < GoldLength; ecx++)
                {
                    GoldValueA[ecx] = (byte)_FileStream.ReadByte();
                }

                GoldValue = BitConverter.ToInt32(GoldValueA, 0);

                Level.Player.Gold = GoldValue;

                // close file stream
                _FileStream.Close();
            }
            catch (Exception _Exception)
            {
                // Error
                MessageBox.Show(_Exception.ToString(), "Error!", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
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
