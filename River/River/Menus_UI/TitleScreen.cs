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
    class TitleScreen
    {
        public enum ScreenType
        {
            Title,
            CharacterSelect,
            Closed,
            GameExited,
        }

        private enum TitleSelectionType
        {
            NewGame,
            LoadGame,
            Quit
        }

        private EntityType CharacterSelection = EntityType.Magician;
        private TitleSelectionType TitleSelection;

        private Texture2D Background;
        private Texture2D CharacterSelect;
        private Texture2D HorizontalSelect;
        public static Texture2D VerticalSelect;

        private bool IsLoading = false;

        public ScreenType CurrentScreen;

        private Main MainPTR;

        public TitleScreen(Main MainPTR)
        {
            this.MainPTR = MainPTR;
            CurrentScreen = TitleScreen.ScreenType.Title;

            //TODO: Default to load if there is save data?
            TitleSelection = TitleSelectionType.NewGame;
        }

        private float SelectionDelay = 0f; //In ms
        public void Update(GameTime GameTime)
        {
            SelectionDelay -= GameTime.ElapsedGameTime.Milliseconds;

            switch (CurrentScreen)
            {
                //UPDATE TITLE SCREEN
                case ScreenType.Title:

                    //Set timer to 0 if new time is below or if player reset thumbstick in Y
                    if (SelectionDelay < 0f || (Math.Abs(Main.GamePadState.ThumbSticks.Left.Y) <= .2f && Main.LastGamePadState.ThumbSticks.Left.Y >= 0.2f) ||
                        (Main.KeyboardState.IsKeyUp(Keys.Up) && Main.LastKeyboardState.IsKeyDown(Keys.Up)) ||
                        (Main.KeyboardState.IsKeyUp(Keys.Down) && Main.LastKeyboardState.IsKeyDown(Keys.Down)))
                        SelectionDelay = 0f;

                    // Up
                    if ((Main.GamePadState.ThumbSticks.Left.Y > 0.5f || Main.KeyboardState.IsKeyDown(Keys.Up)) &&
                        SelectionDelay == 0f)
                    {
                        SelectionDelay = Main.StandardDelay;
                        switch (TitleSelection)
                        {
                            case TitleSelectionType.NewGame:
                                TitleSelection = TitleSelectionType.Quit;
                                break;
                            case TitleSelectionType.LoadGame:
                                TitleSelection = TitleSelectionType.NewGame;
                                break;
                            case TitleSelectionType.Quit:
                                TitleSelection = TitleSelectionType.LoadGame;
                                break;
                        }
                    }

                    // Down
                    if ((Main.GamePadState.ThumbSticks.Left.Y < -0.5f || Main.KeyboardState.IsKeyDown(Keys.Down)) &&
                        SelectionDelay == 0f)
                    {
                        SelectionDelay = Main.StandardDelay;
                        switch (TitleSelection)
                        {
                            case TitleSelectionType.NewGame:
                                TitleSelection = TitleSelectionType.LoadGame;
                                break;
                            case TitleSelectionType.LoadGame:
                                TitleSelection = TitleSelectionType.Quit;
                                break;
                            case TitleSelectionType.Quit:
                                TitleSelection = TitleSelectionType.NewGame;
                                break;
                        }
                    }

                    if ((Main.GamePadState.IsButtonDown(Buttons.A) && !Main.LastGamePadState.IsButtonDown(Buttons.A)) ||
                        (Main.KeyboardState.IsKeyDown(Keys.Enter) && !Main.LastKeyboardState.IsKeyDown(Keys.Enter)))
                    {

                        switch (TitleSelection)
                        {
                            case TitleSelectionType.NewGame:
                                IsLoading = false;
                                CurrentScreen = ScreenType.CharacterSelect;
                                break;
                            case TitleSelectionType.LoadGame:
                                IsLoading = true;
                                CurrentScreen = ScreenType.CharacterSelect;
                                break;
                            case TitleSelectionType.Quit:
                                CurrentScreen = ScreenType.GameExited;
                                break;
                        }

                    }
                    else if ((Main.GamePadState.IsButtonDown(Buttons.B) && !Main.LastGamePadState.IsButtonDown(Buttons.B)) ||
                        (Main.KeyboardState.IsKeyDown(Keys.Back) && !Main.LastKeyboardState.IsKeyDown(Keys.Back)))
                    {
                        CurrentScreen = ScreenType.Closed;
                        MainPTR.GameEditorStart();
                    }
                    else if ((Main.GamePadState.IsButtonDown(Buttons.Y) && !Main.LastGamePadState.IsButtonDown(Buttons.Y)) ||
                        (Main.KeyboardState.IsKeyDown(Keys.Tab) && !Main.LastKeyboardState.IsKeyDown(Keys.Tab)))
                    {
                        MainPTR.DataBaseEditorStart();
                    }
                    break;

                //UPDATE CHARACTER SELECT
                case ScreenType.CharacterSelect:

                    //Set timer to 0 if new time is below or if player reset thumbstick in X
                    if (SelectionDelay < 0f || (Math.Abs(Main.GamePadState.ThumbSticks.Left.X) <= .2f && Main.LastGamePadState.ThumbSticks.Left.X >= 0.2f) ||
                        (Main.KeyboardState.IsKeyUp(Keys.Up) && Main.LastKeyboardState.IsKeyDown(Keys.Up)) ||
                        (Main.KeyboardState.IsKeyUp(Keys.Down) && Main.LastKeyboardState.IsKeyDown(Keys.Down)))
                        SelectionDelay = 0f;

                    if ((Main.GamePadState.IsButtonDown(Buttons.A) && !Main.LastGamePadState.IsButtonDown(Buttons.A)) ||
                        (Main.KeyboardState.IsKeyDown(Keys.Enter) && !Main.LastKeyboardState.IsKeyDown(Keys.Enter)))
                    {
                        CurrentScreen = ScreenType.Closed;
                        if (!IsLoading)
                        {
                            MainPTR.NewGame(CharacterSelection);
                        }
                        else
                        {
                            MainPTR.LoadGame(CharacterSelection);
                        }
                        return;
                    }
                    else if ((Main.GamePadState.IsButtonDown(Buttons.B) && !Main.LastGamePadState.IsButtonDown(Buttons.B)) ||
                        (Main.KeyboardState.IsKeyDown(Keys.Back) && !Main.LastKeyboardState.IsKeyDown(Keys.Back)))
                    {
                        CurrentScreen = ScreenType.Title;
                        return;
                    }


                    // Left
                    if ((Main.GamePadState.ThumbSticks.Left.X < -0.5f || Main.KeyboardState.IsKeyDown(Keys.Left)) && SelectionDelay == 0f)
                    {
                        SelectionDelay = Main.StandardDelay;
                        switch (CharacterSelection)
                        {
                            case EntityType.Magician:
                                CharacterSelection = EntityType.Bandit;
                                break;
                            case EntityType.Warrior:
                                CharacterSelection = EntityType.Magician;
                                break;
                            case EntityType.Bandit:
                                CharacterSelection = EntityType.Warrior;
                                break;
                        }
                    }

                    // Right
                    if ((Main.GamePadState.ThumbSticks.Left.X > 0.5f || Main.KeyboardState.IsKeyDown(Keys.Right)) && SelectionDelay == 0f)
                    {
                        SelectionDelay = Main.StandardDelay;
                        switch (CharacterSelection)
                        {
                            case EntityType.Magician:
                                CharacterSelection = EntityType.Warrior;
                                break;
                            case EntityType.Warrior:
                                CharacterSelection = EntityType.Bandit;
                                break;
                            case EntityType.Bandit:
                                CharacterSelection = EntityType.Magician;
                                break;
                        }
                    }

                    break;
            }
        }

        public void LoadContent(ContentManager Content)
        {
            Background = Content.Load<Texture2D>("Textures/MainMenu/Background");
            CharacterSelect = Content.Load<Texture2D>("Textures/MainMenu/CharacterSelect");
            HorizontalSelect = Content.Load<Texture2D>("Textures/MainMenu/HorizontalSelect");
            VerticalSelect = Content.Load<Texture2D>("Textures/MainMenu/VerticalSelect");
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Begin();

            switch (CurrentScreen)
            {
                case TitleScreen.ScreenType.Title:
                    SpriteBatch.Draw(Background, Vector2.Zero, Color.White);

                    Vector2 TitleCursorPosition = Vector2.Zero;
                    switch (TitleSelection)
                    {
                        case TitleSelectionType.NewGame:
                            TitleCursorPosition = new Vector2(Main.BackBufferWidth / 2 - 160, 390);
                            break;
                        case TitleSelectionType.LoadGame:
                            TitleCursorPosition = new Vector2(Main.BackBufferWidth / 2 - 160, 470);
                            break;
                        case TitleSelectionType.Quit:
                            TitleCursorPosition = new Vector2(Main.BackBufferWidth / 2 - 100, 558);
                            break;
                    }
                    SpriteBatch.Draw(VerticalSelect, TitleCursorPosition, Color.White);
                    break;
                case ScreenType.CharacterSelect:
                    SpriteBatch.Draw(CharacterSelect, Vector2.Zero, Color.White);

                    Vector2 CharacterCursorPosition = Vector2.Zero;
                    switch (CharacterSelection)
                    {
                        case EntityType.Magician:
                            CharacterCursorPosition = new Vector2(340, 240);
                            break;
                        case EntityType.Warrior:
                            CharacterCursorPosition = new Vector2(460, 440);
                            break;
                        case EntityType.Bandit:
                            CharacterCursorPosition = new Vector2(810, 240);
                            break;
                    }
                    SpriteBatch.Draw(HorizontalSelect, CharacterCursorPosition, Color.White);
                    break;
            }

            SpriteBatch.End();
        }

    }
}
