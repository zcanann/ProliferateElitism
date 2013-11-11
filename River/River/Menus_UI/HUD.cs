using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectMercury.Emitters;
using ProjectMercury;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;

namespace River
{
    static class HUD
    {
        public enum SkillChangeSelectionType
        {
            None,
            Primary,
            Secondary,
            Defensive,
            Special
        }
        private enum SubSelectionType
        {
            First = 0,
            Second,
            Third,
        }

        private static SubSelectionType SubSelection;
        public static SkillChangeSelectionType SkillChangeSelection = SkillChangeSelectionType.None;
        public static bool Editing = false; //True if changing the selected skill
        private static bool Selected = false;

        private static Texture2D EnemyHPBar;
        private static Texture2D EnemyHPFrame;
        private static Vector2 EnemyHPBarPos;

        private static Texture2D HealthGlobeFront;
        private static Texture2D ManaGlobeFront;
        private static Texture2D HealthGlobeBack;
        private static Texture2D ManaGlobeBack;
        private static Texture2D StartButton;
        private static Texture2D BackButton;

        private static Texture2D BlackPane;
        private static Texture2D GreenPane;
        private static Texture2D BluePane;
        private static Texture2D RedPane;
        private static Texture2D YellowPane;

        private static Texture2D LootIcon;
        private static Texture2D ReturnIcon;
        private static Texture2D MapIcon;
        private static Texture2D InventoryIcon;
        private static Texture2D PrimaryAttack;
        private static Texture2D SkillLockSmall;
        private static Texture2D SkillLockLarge;
        private static Texture2D SkillSelectSmall;
        private static Texture2D SkillSelectLarge;
        private static Texture2D[] SecondaryAttack = new Texture2D[3];
        private static Texture2D[] DefensiveAttack = new Texture2D[3];
        private static Texture2D[] SpecialAttack = new Texture2D[3];
        private static Texture2D MainHUD;

        private static Vector2 HealthGlobePos;
        private static Vector2 ManaGlobePos;

        private static Vector2 GreenPanePos;
        private static Vector2 BluePanePos;
        private static Vector2 RedPanePos;
        private static Vector2 YellowPanePos;

        private static Vector2[] BluePaneEditsPos = new Vector2[3];
        private static Vector2[] RedPaneEditsPos = new Vector2[3];
        private static Vector2[] YellowPaneEditsPos = new Vector2[3];

        private static Vector2 StartButtonPos;
        private static Vector2 BackButtonPos;

        private static Vector2 MapPanePos;
        private static Vector2 MapIconPos;
        private static Vector2 InventoryPanePos;
        private static Vector2 InventoryIconPos;
        private static Vector2 MainHudPos;

        private static Rectangle BlackBGRectangle;

        private static Level LevelPTR;

        private static CircleEmitter HealthEmitterBase = null;
        private static LineEmitter HealthEmitterDetail = null;
        private static CircleEmitter SecondaryEmitterBase = null;
        private static LineEmitter SecondaryEmitterDetail = null;
        private static SpriteBatchRenderer ParticleRenderer = null;

        private static Vector2 HealthEmitterPosition = new Vector2(230, 604);
        private static Vector2 SecondaryEmitterPosition = new Vector2(1044, 604);

        public static void Initialize(Level _LevelPTR)
        {
            LevelPTR = _LevelPTR;
        }

        public static void LoadContent(ContentManager Content, IGraphicsDeviceService Graphics)
        {
            EnemyHPBar = Content.Load<Texture2D>(@"Textures\UI\EnemyHPBar");
            EnemyHPFrame = Content.Load<Texture2D>(@"Textures\UI\EnemyHPFrame");

            HealthGlobeFront = Content.Load<Texture2D>(@"Textures\UI\HealthGlobe");
            ManaGlobeFront = Content.Load<Texture2D>(@"Textures\UI\ManaGlobe");
            HealthGlobeBack = Content.Load<Texture2D>(@"Textures\UI\HealthGlobe");
            ManaGlobeBack = Content.Load<Texture2D>(@"Textures\UI\ManaGlobe");
            StartButton = Content.Load<Texture2D>(@"Textures\UI\Buttons\StartButton");
            BackButton = Content.Load<Texture2D>(@"Textures\UI\Buttons\BackButton");
            BlackPane = Content.Load<Texture2D>(@"Textures\UI\Panes\BlackPane");
            GreenPane = Content.Load<Texture2D>(@"Textures\UI\Panes\GreenPane");
            BluePane = Content.Load<Texture2D>(@"Textures\UI\Panes\BluePane");
            RedPane = Content.Load<Texture2D>(@"Textures\UI\Panes\RedPane");
            YellowPane = Content.Load<Texture2D>(@"Textures\UI\Panes\YellowPane");
            MainHUD = Content.Load<Texture2D>(@"Textures\UI\MainHUD");

            SkillLockSmall = Content.Load<Texture2D>(@"Textures\UI\SkillLockSmall");
            SkillLockLarge = Content.Load<Texture2D>(@"Textures\UI\SkillLockLarge");
            SkillSelectSmall = Content.Load<Texture2D>(@"Textures\UI\SkillSelectSmall");
            SkillSelectLarge = Content.Load<Texture2D>(@"Textures\UI\SkillSelectLarge");
            SkillLockLarge = Content.Load<Texture2D>(@"Textures\UI\SkillLockLarge");

            LootIcon = Content.Load<Texture2D>(@"Textures\UI\LootIcon");
            ReturnIcon = Content.Load<Texture2D>(@"Textures\UI\ReturnIcon");
            MapIcon = Content.Load<Texture2D>(@"Textures\UI\MapIcon");
            InventoryIcon = Content.Load<Texture2D>(@"Textures\UI\InventoryIcon");

            SetPositions();

            if (HealthEmitterBase == null)
            {
                HealthEmitterBase = new CircleEmitter();
                HealthEmitterBase.Radius = 64;
                HealthEmitterBase.ReleaseColour = Color.Red.ToVector3();
                HealthEmitterBase.ReleaseQuantity = 200;
                HealthEmitterBase.ReleaseRotation = new VariableFloat { Value = 0f, Variation = 0.0f };
                HealthEmitterBase.ReleaseScale = new VariableFloat { Value = 12f, Variation = 4f };
                HealthEmitterBase.ReleaseSpeed = new VariableFloat { Value = 0f, Variation = 0f };
                HealthEmitterBase.ReleaseOpacity = new VariableFloat { Value = .2f, Variation = 0.0f };

                HealthEmitterDetail = new LineEmitter();
                HealthEmitterDetail.Length = 112;
                HealthEmitterDetail.ReleaseColour = Color.DarkRed.ToVector3();
                HealthEmitterDetail.ReleaseQuantity = 8;
                HealthEmitterDetail.ReleaseRotation = new VariableFloat { Value = 0f, Variation = 0.0f };
                HealthEmitterDetail.ReleaseScale = new VariableFloat { Value = 8f, Variation = 4f };
                HealthEmitterDetail.ReleaseSpeed = new VariableFloat { Value = 0f, Variation = 0f };
                HealthEmitterDetail.ReleaseOpacity = new VariableFloat { Value = .4f, Variation = 0.0f };

                OpacityModifier OM = new OpacityModifier();
                OM.Initial = 0.4f;
                OM.Ultimate = 0.05f;
                //HealthEmitterBase.Modifiers.Add(OM);
                HealthEmitterDetail.Modifiers.Add(OM);

                LinearGravityModifier LGM = new LinearGravityModifier();
                LGM.Gravity = new Vector2(0, -48);
                HealthEmitterDetail.Modifiers.Add(LGM);

                ParticleRenderer = new SpriteBatchRenderer();
                ParticleRenderer.GraphicsDeviceService = Graphics;

                HealthEmitterBase.ParticleTexture = Content.Load<Texture2D>(@"Textures\UI\AttributeParticle");
                HealthEmitterBase.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
                HealthEmitterDetail.ParticleTexture = Content.Load<Texture2D>(@"Textures\UI\AttributeParticle");
                HealthEmitterDetail.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";

                HealthEmitterBase.Initialise(4000, 25);
                HealthEmitterDetail.Initialise(4000, 5);

                //ParticleRenderer.LoadContent(Content);
            }


        }


        public static void LoadConditionalContent(ContentManager Content, EntityType Class)
        {
            SecondaryEmitterBase = new CircleEmitter();
            SecondaryEmitterBase.Radius = 64;
            SecondaryEmitterBase.ReleaseQuantity = 200;
            SecondaryEmitterBase.ReleaseRotation = new VariableFloat { Value = 0f, Variation = 0.0f };
            SecondaryEmitterBase.ReleaseScale = new VariableFloat { Value = 12f, Variation = 4f };
            SecondaryEmitterBase.ReleaseSpeed = new VariableFloat { Value = 0f, Variation = 0f };
            SecondaryEmitterBase.ReleaseOpacity = new VariableFloat { Value = .2f, Variation = 0.0f };

            SecondaryEmitterDetail = new LineEmitter();
            SecondaryEmitterDetail.Length = 112;
            SecondaryEmitterDetail.ReleaseQuantity = 10;
            SecondaryEmitterDetail.ReleaseRotation = new VariableFloat { Value = 0f, Variation = 0.0f };
            SecondaryEmitterDetail.ReleaseScale = new VariableFloat { Value = 8f, Variation = 4f };
            SecondaryEmitterDetail.ReleaseSpeed = new VariableFloat { Value = 0f, Variation = 0f };
            SecondaryEmitterDetail.ReleaseOpacity = new VariableFloat { Value = 0f, Variation = 0f };

            switch (Class)
            {
                case EntityType.Warrior:
                    SecondaryEmitterBase.ReleaseColour = Color.DarkRed.ToVector3();
                    SecondaryEmitterDetail.ReleaseColour = Color.IndianRed.ToVector3();
                    PrimaryAttack = Content.Load<Texture2D>(@"Textures\UI\Skills\Slash");
                    SecondaryAttack[0] = Content.Load<Texture2D>(@"Textures\UI\Skills\Strike");
                    SecondaryAttack[1] = Content.Load<Texture2D>(@"Textures\UI\Skills\Cleave");
                    SecondaryAttack[2] = Content.Load<Texture2D>(@"Textures\UI\Skills\TeleportStrike");
                    DefensiveAttack[0] = Content.Load<Texture2D>(@"Textures\UI\Skills\Shield");
                    DefensiveAttack[1] = Content.Load<Texture2D>(@"Textures\UI\Skills\Absorb");
                    DefensiveAttack[2] = Content.Load<Texture2D>(@"Textures\UI\Skills\Fear");
                    SpecialAttack[0] = Content.Load<Texture2D>(@"Textures\UI\Skills\Whirlwind");
                    SpecialAttack[1] = Content.Load<Texture2D>(@"Textures\UI\Skills\Rage");
                    SpecialAttack[2] = Content.Load<Texture2D>(@"Textures\UI\Skills\Crush");
                    break;
                case EntityType.Bandit:
                    SecondaryEmitterBase.ReleaseColour = Color.Yellow.ToVector3();
                    SecondaryEmitterDetail.ReleaseColour = Color.White.ToVector3();
                    PrimaryAttack = Content.Load<Texture2D>(@"Textures\UI\Skills\Shoot");
                    SecondaryAttack[0] = Content.Load<Texture2D>(@"Textures\UI\Skills\Multishot");
                    SecondaryAttack[1] = Content.Load<Texture2D>(@"Textures\UI\Skills\FlamingArrow");
                    SecondaryAttack[2] = Content.Load<Texture2D>(@"Textures\UI\Skills\PiercingShot");
                    DefensiveAttack[0] = Content.Load<Texture2D>(@"Textures\UI\Skills\Vanish");
                    DefensiveAttack[1] = Content.Load<Texture2D>(@"Textures\UI\Skills\Bandage");
                    DefensiveAttack[2] = Content.Load<Texture2D>(@"Textures\UI\Skills\Blind");
                    SpecialAttack[0] = Content.Load<Texture2D>(@"Textures\UI\Skills\PoisonArrow");
                    SpecialAttack[1] = Content.Load<Texture2D>(@"Textures\UI\Skills\AoeShot");
                    SpecialAttack[2] = Content.Load<Texture2D>(@"Textures\UI\Skills\Haste");
                    break;
                case EntityType.Magician:
                    SecondaryEmitterBase.ReleaseColour = Color.Purple.ToVector3();
                    SecondaryEmitterDetail.ReleaseColour = Color.LightPink.ToVector3();
                    PrimaryAttack = Content.Load<Texture2D>(@"Textures\UI\Skills\Arcanebolt");
                    SecondaryAttack[0] = Content.Load<Texture2D>(@"Textures\UI\Skills\Frostbolt");
                    SecondaryAttack[1] = Content.Load<Texture2D>(@"Textures\UI\Skills\Firebolt");
                    SecondaryAttack[2] = Content.Load<Texture2D>(@"Textures\UI\Skills\Lightningbolts");
                    DefensiveAttack[0] = Content.Load<Texture2D>(@"Textures\UI\Skills\IceShield");
                    DefensiveAttack[1] = Content.Load<Texture2D>(@"Textures\UI\Skills\Cauterize");
                    DefensiveAttack[2] = Content.Load<Texture2D>(@"Textures\UI\Skills\LightningShield");
                    SpecialAttack[0] = Content.Load<Texture2D>(@"Textures\UI\Skills\FrostExplosion");
                    SpecialAttack[1] = Content.Load<Texture2D>(@"Textures\UI\Skills\Firewalk");
                    SpecialAttack[2] = Content.Load<Texture2D>(@"Textures\UI\Skills\Storm");
                    break;
            }

            OpacityModifier OM = new OpacityModifier();
            OM.Initial = 0.06f;
            OM.Ultimate = 0.02f;
            SecondaryEmitterDetail.Modifiers.Add(OM);

            LinearGravityModifier LGM = new LinearGravityModifier();
            LGM.Gravity = new Vector2(0, -32);
            SecondaryEmitterDetail.Modifiers.Add(LGM);

            SecondaryEmitterBase.ParticleTexture = Content.Load<Texture2D>(@"Textures\UI\AttributeParticle");
            SecondaryEmitterBase.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";
            SecondaryEmitterDetail.ParticleTexture = Content.Load<Texture2D>(@"Textures\UI\AttributeParticle");
            SecondaryEmitterDetail.ParticleTextureAssetName = @"Content\Textures\UI\AttributeParticle";

            SecondaryEmitterBase.Initialise(4000, 25);
            SecondaryEmitterDetail.Initialise(4000, 5);

            ParticleRenderer.LoadContent(Content);

        }

        private const int TitleSafeOffset = 48;
        private static void SetPositions()
        {
            EnemyHPBarPos.X = (Main.BackBufferWidth - EnemyHPFrame.Width) / 2;

            MainHudPos.Y = Main.BackBufferHeight - MainHUD.Height;

            //Start from middle and set positions for left half of screen
            BluePanePos = new Vector2(Main.BackBufferWidth / 2 - BluePane.Width,
                Main.BackBufferHeight - BluePane.Height - TitleSafeOffset);

            for (int ecx = 0; ecx <= 2; ecx++)
            {
                BluePaneEditsPos[ecx].X = BluePanePos.X;
                BluePaneEditsPos[ecx].Y = BluePanePos.Y - BluePane.Height * (ecx + 1) - TitleSafeOffset;
            }

            GreenPanePos = new Vector2(Main.BackBufferWidth / 2 - BluePane.Width - GreenPane.Width,
                Main.BackBufferHeight - GreenPane.Height - TitleSafeOffset);

            MapPanePos = new Vector2(GreenPanePos.X - BlackPane.Width,
                Main.BackBufferHeight - BlackPane.Height - TitleSafeOffset);
            MapIconPos = new Vector2(MapPanePos.X + BlackPane.Width / 2 - MapIcon.Width / 2,
                MapPanePos.Y + 12);
            BackButtonPos = new Vector2(MapPanePos.X + BlackPane.Width / 2 - BackButton.Width / 2,
                Main.BackBufferHeight - BackButton.Height - TitleSafeOffset);

            //Start from middle and set positions for right half of screen
            RedPanePos = new Vector2(Main.BackBufferWidth / 2,
                Main.BackBufferHeight - RedPane.Height - TitleSafeOffset);

            for (int ecx = 0; ecx <= 2; ecx++)
            {
                RedPaneEditsPos[ecx].X = RedPanePos.X;
                RedPaneEditsPos[ecx].Y = RedPanePos.Y - RedPane.Height * (ecx + 1) - TitleSafeOffset;
            }

            YellowPanePos = new Vector2(Main.BackBufferWidth / 2 + RedPane.Width,
                Main.BackBufferHeight - YellowPane.Height - TitleSafeOffset);

            for (int ecx = 0; ecx <= 2; ecx++)
            {
                YellowPaneEditsPos[ecx].X = YellowPanePos.X;
                YellowPaneEditsPos[ecx].Y = YellowPanePos.Y - YellowPane.Height * (ecx + 1) - TitleSafeOffset;
            }

            InventoryPanePos = new Vector2(YellowPanePos.X + YellowPane.Width,
               Main.BackBufferHeight - BlackPane.Height - TitleSafeOffset);
            InventoryIconPos = new Vector2(InventoryPanePos.X + BlackPane.Width / 2 - InventoryIcon.Width / 2,
                InventoryPanePos.Y + 12);

            StartButtonPos = new Vector2(InventoryPanePos.X + BlackPane.Width / 2 - StartButton.Width / 2,
                 Main.BackBufferHeight - StartButton.Height - TitleSafeOffset);

            //HEALTH/MANA GLOBES
            HealthGlobePos = new Vector2(MapPanePos.X - HealthGlobeFront.Width,
                Main.BackBufferHeight - HealthGlobeFront.Height - TitleSafeOffset);
            ManaGlobePos = new Vector2(InventoryPanePos.X + BlackPane.Width,
                Main.BackBufferHeight - ManaGlobeFront.Height - TitleSafeOffset);

            //Transparent black BG
            BlackBGRectangle = new Rectangle(0, 0, Main.BackBufferWidth, Main.BackBufferHeight);

        }

        private static float SelectionDelay = 0f;
        public static void Update(GameTime GameTime)
        {
            //Update emitters
            HealthEmitterBase.Trigger(HealthEmitterPosition);
            HealthEmitterDetail.Trigger(new Vector2(HealthEmitterPosition.X, HealthEmitterPosition.Y + 64));

            SecondaryEmitterBase.Trigger(SecondaryEmitterPosition);
            SecondaryEmitterDetail.Trigger(new Vector2(SecondaryEmitterPosition.X, SecondaryEmitterPosition.Y + 64));

            HealthEmitterBase.Update((float)GameTime.ElapsedGameTime.TotalSeconds);
            HealthEmitterDetail.Update((float)GameTime.ElapsedGameTime.TotalSeconds);

            SecondaryEmitterBase.Update((float)GameTime.ElapsedGameTime.TotalSeconds);
            SecondaryEmitterDetail.Update((float)GameTime.ElapsedGameTime.TotalSeconds);

            //Emitter bounds
            float Dist = 0;
            for (int ecx = 0; ecx < HealthEmitterDetail.Particles.Length; ecx++)
            {
                Dist = HealthEmitterDetail.Particles[ecx].Position.X - HealthEmitterPosition.X;

                //Outer Section
                if (Dist <= -46 || Dist >= 46)
                {
                    if (HealthEmitterDetail.Particles[ecx].Position.Y <= HealthEmitterPosition.Y - 40)
                        HealthEmitterDetail.Particles[ecx].Scale = 0;
                }
                //MiddleOuterSection
                else if (Dist <= -32 || Dist >= 32)
                {
                    if (HealthEmitterDetail.Particles[ecx].Position.Y <= HealthEmitterPosition.Y - 52)
                        HealthEmitterDetail.Particles[ecx].Scale = 0;
                }
                //Middle Section
                else if (Dist <= -24 || Dist >= 24)
                {
                    if (HealthEmitterDetail.Particles[ecx].Position.Y <= HealthEmitterPosition.Y - 58)
                        HealthEmitterDetail.Particles[ecx].Scale = 0;
                }
                //Center Section
                if (HealthEmitterDetail.Particles[ecx].Position.Y <= HealthEmitterPosition.Y - 62)
                    HealthEmitterDetail.Particles[ecx].Scale = 0;

            }

            for (int ecx = 0; ecx < SecondaryEmitterDetail.Particles.Length; ecx++)
            {
                Dist = SecondaryEmitterDetail.Particles[ecx].Position.X - SecondaryEmitterPosition.X;

                //Outer Section
                if (Dist <= -46 || Dist >= 46)
                {
                    if (SecondaryEmitterDetail.Particles[ecx].Position.Y <= SecondaryEmitterPosition.Y - 40)
                        SecondaryEmitterDetail.Particles[ecx].Scale = 0;
                }
                //MiddleOuterSection
                else if (Dist <= -32 || Dist >= 32)
                {
                    if (SecondaryEmitterDetail.Particles[ecx].Position.Y <= SecondaryEmitterPosition.Y - 52)
                        SecondaryEmitterDetail.Particles[ecx].Scale = 0;
                }
                //Middle Section
                else if (Dist <= -24 || Dist >= 24)
                {
                    if (SecondaryEmitterDetail.Particles[ecx].Position.Y <= SecondaryEmitterPosition.Y - 58)
                        SecondaryEmitterDetail.Particles[ecx].Scale = 0;
                }
                //Center Section
                if (SecondaryEmitterDetail.Particles[ecx].Position.Y <= SecondaryEmitterPosition.Y - 62)
                    SecondaryEmitterDetail.Particles[ecx].Scale = 0;
            }


            //EDITING SKILLS
            if (Editing)
            {
                SelectionDelay -= GameTime.ElapsedGameTime.Milliseconds;

                //Set timer to 0 if new time is below or if player reset thumbstick in Y

                if (SkillChangeSelection == SkillChangeSelectionType.None)
                    SkillChangeSelection = SkillChangeSelectionType.Secondary;

                if (!Selected)
                {
                    if (SelectionDelay < 0f ||
                        Math.Abs(Main.GamePadState.ThumbSticks.Left.X) <= .2f)
                        SelectionDelay = 0f;

                    //Left
                    if (Main.GamePadState.ThumbSticks.Left.X < -0.5f &&
                        SelectionDelay == 0f)
                    {
                        SelectionDelay = Main.StandardDelay;
                        switch (SkillChangeSelection)
                        {
                            case SkillChangeSelectionType.Primary:
                                SkillChangeSelection = SkillChangeSelectionType.Special;
                                break;
                            case SkillChangeSelectionType.Secondary:
                                SkillChangeSelection = SkillChangeSelectionType.Primary;
                                break;
                            case SkillChangeSelectionType.Defensive:
                                SkillChangeSelection = SkillChangeSelectionType.Secondary;
                                break;
                            case SkillChangeSelectionType.Special:
                                SkillChangeSelection = SkillChangeSelectionType.Defensive;
                                break;
                        }
                    }

                    //Right
                    if (Main.GamePadState.ThumbSticks.Left.X > 0.5f &&
                        SelectionDelay == 0f)
                    {
                        SelectionDelay = Main.StandardDelay;
                        switch (SkillChangeSelection)
                        {
                            case SkillChangeSelectionType.Primary:
                                SkillChangeSelection = SkillChangeSelectionType.Secondary;
                                break;
                            case SkillChangeSelectionType.Secondary:
                                SkillChangeSelection = SkillChangeSelectionType.Defensive;
                                break;
                            case SkillChangeSelectionType.Defensive:
                                SkillChangeSelection = SkillChangeSelectionType.Special;
                                break;
                            case SkillChangeSelectionType.Special:
                                SkillChangeSelection = SkillChangeSelectionType.Primary;
                                break;
                        }
                    }
                }
                else
                {
                    if (SelectionDelay < 0f ||
                        Math.Abs(Main.GamePadState.ThumbSticks.Left.Y) <= .2f)
                        SelectionDelay = 0f;

                    //Down
                    if (Main.GamePadState.ThumbSticks.Left.Y < -0.5f &&
                        SelectionDelay == 0f)
                    {
                        SelectionDelay = Main.StandardDelay;

                        switch (SubSelection)
                        {
                            case SubSelectionType.First:
                                bool LoopAround = false;

                                switch (SkillChangeSelection)
                                {
                                    case SkillChangeSelectionType.Secondary:
                                        if (LevelPTR.GetSecondarySkillSelection() != -1)
                                            LoopAround = true;
                                        break;
                                    case SkillChangeSelectionType.Defensive:
                                        if (LevelPTR.GetDefensiveSkillSelection() != -1)
                                            LoopAround = true;
                                        break;
                                    case SkillChangeSelectionType.Special:
                                        if (LevelPTR.GetSpecialSkillSelection() != -1)
                                            LoopAround = true;
                                        break;
                                }
                                if (LoopAround)
                                    SubSelection = SubSelectionType.First;
                                else
                                    SubSelection = SubSelectionType.Third;
                                break;
                            case SubSelectionType.Second:
                                SubSelection = SubSelectionType.First;
                                break;
                            case SubSelectionType.Third:
                                SubSelection = SubSelectionType.Second;
                                break;
                        }
                    }

                    //Up
                    if (Main.GamePadState.ThumbSticks.Left.Y > 0.5f &&
                        SelectionDelay == 0f)
                    {
                        SelectionDelay = Main.StandardDelay;
                        switch (SubSelection)
                        {
                            case SubSelectionType.First:
                                SubSelection = SubSelectionType.Second;
                                break;
                            case SubSelectionType.Second:
                                bool LoopAround = false;

                                switch (SkillChangeSelection)
                                {
                                    case SkillChangeSelectionType.Secondary:
                                        if (LevelPTR.GetSecondarySkillSelection() != -1)
                                            LoopAround = true;
                                        break;
                                    case SkillChangeSelectionType.Defensive:
                                        if (LevelPTR.GetDefensiveSkillSelection() != -1)
                                            LoopAround = true;
                                        break;
                                    case SkillChangeSelectionType.Special:
                                        if (LevelPTR.GetSpecialSkillSelection() != -1)
                                            LoopAround = true;
                                        break;
                                }
                                if (LoopAround)
                                    SubSelection = SubSelectionType.First;
                                else
                                    SubSelection = SubSelectionType.Third;
                                break;
                            case SubSelectionType.Third:
                                SubSelection = SubSelectionType.First;
                                break;
                        }
                    }
                }

                if (Main.GamePadState.IsButtonDown(Buttons.B) && !Main.LastGamePadState.IsButtonDown(Buttons.B))
                {
                    if (!Selected)
                    {
                        Editing = false;
                        //SubSelection = SubSelectionType.First;
                        SkillChangeSelection = SkillChangeSelectionType.None;
                    }
                    else
                        Selected = false;
                }

                if (Main.GamePadState.IsButtonDown(Buttons.A) && !Main.LastGamePadState.IsButtonDown(Buttons.A))
                {

                    //Pressing return
                    if (SkillChangeSelection == SkillChangeSelectionType.Primary)
                    {
                        Editing = false;
                        SkillChangeSelection = SkillChangeSelectionType.None;
                        Main.LastGamePadState = Main.GamePadState;
                        return;
                    }

                    int CurrentSkill;
                    int SelectionOffset = 0;

                    if (Selected)
                    {
                        switch (SkillChangeSelection)
                        {
                            case SkillChangeSelectionType.Secondary:
                                //Adjustment to selection due to how they are shown on screen
                                CurrentSkill = LevelPTR.GetSecondarySkillSelection();
                                if (CurrentSkill == 0 && (int)SubSelection == CurrentSkill)
                                    SelectionOffset++;
                                else if (CurrentSkill == 0 && (int)SubSelection > CurrentSkill)
                                    SelectionOffset++;
                                else if (CurrentSkill == 1 && (int)SubSelection == CurrentSkill)
                                    SelectionOffset++;

                                LevelPTR.SetSecondarySkillSelection((int)SubSelection + SelectionOffset);
                                break;
                            case SkillChangeSelectionType.Defensive:

                                CurrentSkill = LevelPTR.GetDefensiveSkillSelection();
                                if (CurrentSkill == 0 && (int)SubSelection == CurrentSkill)
                                    SelectionOffset++;
                                else if (CurrentSkill == 0 && (int)SubSelection > CurrentSkill)
                                    SelectionOffset++;
                                else if (CurrentSkill == 1 && (int)SubSelection == CurrentSkill)
                                    SelectionOffset++;

                                LevelPTR.SetDefensiveSkillSelection((int)SubSelection + SelectionOffset);
                                break;
                            case SkillChangeSelectionType.Special:

                                CurrentSkill = LevelPTR.GetSpecialSkillSelection();
                                if (CurrentSkill == 0 && (int)SubSelection == CurrentSkill)
                                    SelectionOffset++;
                                else if (CurrentSkill == 0 && (int)SubSelection > CurrentSkill)
                                    SelectionOffset++;
                                else if (CurrentSkill == 1 && (int)SubSelection == CurrentSkill)
                                    SelectionOffset++;

                                LevelPTR.SetSpecialSkillSelection((int)SubSelection + SelectionOffset);
                                break;
                        }
                    }
                    else
                        SubSelection = SubSelectionType.First;


                    Selected = !Selected;

                } //END 'A' BUTTON CHECK

                Main.LastGamePadState = Main.GamePadState;

            } //END TEST FOR EDITING SKILLS

        } //END UPDATE

        public static void Draw(SpriteBatch SpriteBatch)
        {
            //DRAW PARTICLES
            ParticleRenderer.RenderEmitter(HealthEmitterBase);
            ParticleRenderer.RenderEmitter(SecondaryEmitterBase);
            ParticleRenderer.RenderEmitter(HealthEmitterDetail);
            ParticleRenderer.RenderEmitter(SecondaryEmitterDetail);


            //DRAW EVERYTHING ELSE
            SpriteBatch.Begin();
            SpriteBatch.Draw(MainHUD, MainHudPos, Color.White);

            //Draw map pane/icon/button
            SpriteBatch.Draw(BlackPane, MapPanePos, Color.White);

            SpriteBatch.Draw(MapIcon, MapIconPos, Color.White);
            SpriteBatch.Draw(BackButton, BackButtonPos, Color.White);

            //Draw inventory pane/icon/button
            SpriteBatch.Draw(BlackPane, InventoryPanePos, Color.White);

            SpriteBatch.Draw(InventoryIcon, InventoryIconPos, Color.White);
            SpriteBatch.Draw(StartButton, StartButtonPos, Color.White);

            SpriteBatch.End();

            //Black BG
            if (Editing)
                MenuManager.DrawTransparentBlackBG(SpriteBatch);

            SpriteBatch.Begin();

            //PRIMARY SKILL
            SpriteBatch.Draw(GreenPane, GreenPanePos, Color.White);

            //SECONDARY SKILL
            SpriteBatch.Draw(BluePane, BluePanePos, Color.White);
            if (SkillChangeSelection == SkillChangeSelectionType.Secondary)
            {
                if (Selected)
                {
                    int CurrentSkill = LevelPTR.GetSecondarySkillSelection();
                    int DrawOffset = 0; //Used to shift panes if only showing 2 instead of 3 vertically
                    for (int ecx = 0; ecx <= 2; ecx++)
                    {
                        if (CurrentSkill + DrawOffset != ecx)
                        {
                            SpriteBatch.Draw(BluePane, BluePaneEditsPos[ecx + DrawOffset], Color.White);
                            SpriteBatch.Draw(SecondaryAttack[ecx], BluePaneEditsPos[ecx + DrawOffset], Color.White);
                            if (ecx + DrawOffset == (int)SubSelection)
                                SpriteBatch.Draw(SkillSelectLarge, BluePaneEditsPos[ecx + DrawOffset], Color.White);
                        }
                        else
                            DrawOffset--;

                    }
                }
                else
                    SpriteBatch.Draw(SkillSelectLarge, BluePanePos, Color.White);
            }

            //DEFENSIVE SKILL
            SpriteBatch.Draw(RedPane, RedPanePos, Color.White);

            if (SkillChangeSelection == SkillChangeSelectionType.Defensive)
            {
                if (Selected)
                {
                    int CurrentSkill = LevelPTR.GetDefensiveSkillSelection();
                    int DrawOffset = 0;
                    for (int ecx = 0; ecx <= 2; ecx++)
                    {
                        if (CurrentSkill + DrawOffset != ecx)
                        {
                            SpriteBatch.Draw(RedPane, RedPaneEditsPos[ecx + DrawOffset], Color.White);
                            SpriteBatch.Draw(DefensiveAttack[ecx], RedPaneEditsPos[ecx + DrawOffset], Color.White);
                            if (ecx + DrawOffset == (int)SubSelection)
                                SpriteBatch.Draw(SkillSelectLarge, RedPaneEditsPos[ecx + DrawOffset], Color.White);
                        }
                        else
                            DrawOffset--;

                    }
                }
                else
                    SpriteBatch.Draw(SkillSelectLarge, RedPanePos, Color.White);
            }

            //SPECIAL SKILL
            SpriteBatch.Draw(YellowPane, YellowPanePos, Color.White);

            if (SkillChangeSelection == SkillChangeSelectionType.Special)
            {
                if (Selected)
                {
                    int CurrentSkill = LevelPTR.GetSpecialSkillSelection();
                    int DrawOffset = 0;
                    for (int ecx = 0; ecx <= 2; ecx++)
                    {
                        if (CurrentSkill + DrawOffset != ecx)
                        {
                            SpriteBatch.Draw(YellowPane, YellowPaneEditsPos[ecx + DrawOffset], Color.White);
                            SpriteBatch.Draw(SpecialAttack[ecx], YellowPaneEditsPos[ecx + DrawOffset], Color.White);
                            if (ecx + DrawOffset == (int)SubSelection)
                                SpriteBatch.Draw(SkillSelectSmall, YellowPaneEditsPos[ecx + DrawOffset], Color.White);
                        }
                        else
                            DrawOffset--;
                    }
                }
                else
                    SpriteBatch.Draw(SkillSelectSmall, YellowPanePos, Color.White);
            }

            //DRAW ICONS
            if (SkillChangeSelection != SkillChangeSelectionType.None)
                SpriteBatch.Draw(ReturnIcon, GreenPanePos, Color.White);
            else
            {
                if (LevelPTR.Player.NearLootable)
                    SpriteBatch.Draw(LootIcon, GreenPanePos, Color.White);
                else
                    SpriteBatch.Draw(PrimaryAttack, GreenPanePos, Color.White);
            }


            if (SkillChangeSelection == SkillChangeSelectionType.Primary)
                SpriteBatch.Draw(SkillSelectSmall, GreenPanePos, Color.White);

            if (LevelPTR.GetSecondarySkillSelection() == -1)
                SpriteBatch.Draw(SkillLockLarge, BluePanePos, Color.White);
            else
                SpriteBatch.Draw(SecondaryAttack[LevelPTR.GetSecondarySkillSelection()], BluePanePos, Color.White);

            if (LevelPTR.GetDefensiveSkillSelection() == -1)
                SpriteBatch.Draw(SkillLockLarge, RedPanePos, Color.White);
            else
                SpriteBatch.Draw(DefensiveAttack[LevelPTR.GetDefensiveSkillSelection()], RedPanePos, Color.White);

            if (LevelPTR.GetSpecialSkillSelection() == -1)
                SpriteBatch.Draw(SkillLockSmall, YellowPanePos, Color.White);
            else
                SpriteBatch.Draw(SpecialAttack[LevelPTR.GetSpecialSkillSelection()], YellowPanePos, Color.White);

            //Draw enemy HP bar
            if (LevelPTR.TargetEnemy != null && LevelPTR.TargetEnemy.IsAlive)
            {
                float NewWidth = (float)LevelPTR.TargetEnemy.GetHealth() /
                    (float)LevelPTR.TargetEnemy.GetMaxHealth() * EnemyHPBar.Width;

                SpriteBatch.Draw(EnemyHPBar, new Rectangle((int)EnemyHPBarPos.X, (int)EnemyHPBarPos.Y, (int)NewWidth, EnemyHPBar.Height), Color.White);
                SpriteBatch.Draw(EnemyHPFrame, EnemyHPBarPos, Color.White);
            }

            SpriteBatch.End();
        }

    }
}
