using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace MazeAndBlue
{
    public class MazeAndBlue : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;

        Kinect kinect;
        Maze maze;
        public GameStats gameStats;
        MainMenu mainMenu;
        ScoreScreen scoreScreen;
        LevelSelectionScreen levelSelectionScreen;
        PauseScreen pauseScreen;
        SettingsScreen settingsScreen;
        StatsScreen statsScreen;
        InstructionScreen instructionScreen;

        public int level { get; set; }
        int numLevels = 12;
        bool vsSecondCycle = false;

        public List<Player> players { get; set; }
        public MouseSelect ms { get; set; }
        public VoiceSelect vs { get; set; }
        public KeyboardSelect ks { get; set; }
        public SoundEffectPlayer soundEffectPlayer { get; set; }

        public enum GameState { MAIN, LEVEL, GAME, SCORE, PAUSE, SETTING, STATS, INSTR };
        public static GameState state { get; set; }
        
        public static SpriteFont font { get; set; }
        
        public int screenWidth { get { return GraphicsDevice.Viewport.Width; } }
        
        public int screenHeight { get { return GraphicsDevice.Viewport.Height; } }

        public MazeAndBlue()
        {
            // Whenever hard-coding screen coordinates or widths/height, the sx/sy functions must be used
            // Optimal screen resolution: 1366 x 768 (no scaling will occur at this resolution)

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            gameStats = new GameStats();

            var form = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;// -8;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;// -30;
            //graphics.IsFullScreen = true;

            players = new List<Player>();
            players.Add(new Player(-0.5f, 0f, Color.Blue, 0));
            players.Add(new Player(0f, 0.5f, Color.Yellow, 1));

            level = 0;
        }

        protected override void Initialize()
        {
            kinect = new Kinect();
            ms = new MouseSelect();
            vs = new VoiceSelect();
            vs.recognizeSpeech(kinect.getSensorReference());
            ks = new KeyboardSelect();

            soundEffectPlayer = new SoundEffectPlayer();

            startMainMenu();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Backgrounds/blue");
            font = Content.Load<SpriteFont>("font");

            foreach (Player player in players)
                player.loadContent();
        }

        protected override void UnloadContent()
        {
        }

        public void startMainMenu()
        {
            mainMenu = new MainMenu();
            mainMenu.loadContent();
            state = GameState.MAIN;
            /*
            player1.selected = false;
            player1.mouseSelected = false;
            player2.selected = false;
            player2.mouseSelected = false;
             * */
        }

        public void startLevelSelectionScreen()
        {
            levelSelectionScreen = new LevelSelectionScreen();
            levelSelectionScreen.loadContent();
            state = GameState.LEVEL;
            /*            player1.selected = false;
                        player1.mouseSelected = false;
                        player2.selected = false;
                        player2.mouseSelected = false;
             */
        }

        public void startLevel(int selectedLevel)
        {
            level = selectedLevel;
            nextLevel();
        }

        public void nextLevel()
        {
            state = GameState.GAME;
            level %= numLevels;
            maze = new Maze(level);
            level++;
            maze.loadContent();
        }

        public void resumeLevel()
        {
            state = GameState.GAME;
        }

        public void startScoreScreen(int time)
        {
            gameStats.updateLevelStats(level, time, 0);
            scoreScreen = new ScoreScreen(time);
            scoreScreen.loadContent();
            state = GameState.SCORE;
        }

        public void startPauseSelectionScreen(int level)
        {
            pauseScreen = new PauseScreen(level);
            pauseScreen.loadContent();
            state = GameState.PAUSE;
        }

        public void startSettingsScreen()
        {
            settingsScreen = new SettingsScreen();
            settingsScreen.loadContent();
            state = GameState.SETTING;
        }

        public void startStatsScreen()
        {
            statsScreen = new StatsScreen();
            statsScreen.loadContent();
            state = GameState.STATS;
        }

        public void startInstructionScreen()
        {
            instructionScreen = new InstructionScreen();
            instructionScreen.loadContent();
            state = GameState.INSTR;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            
            spriteBatch.Begin();

            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(background, screenRectangle, Color.White);

            switch (state)
            {
                case GameState.MAIN:
                    mainMenu.draw(spriteBatch);
                    break;
                case GameState.LEVEL:
                    levelSelectionScreen.draw(spriteBatch);
                    break;
                case GameState.GAME:
                    maze.draw(spriteBatch);
                    break;
                case GameState.SCORE:
                    scoreScreen.draw(spriteBatch);
                    break;
                case GameState.PAUSE:
                    maze.draw(spriteBatch);
                    pauseScreen.draw(spriteBatch);
                    break;
                case GameState.SETTING:
                    settingsScreen.draw(spriteBatch);
                    break;
                case GameState.STATS:
                    statsScreen.draw(spriteBatch);
                    break;
                case GameState.INSTR:
                    instructionScreen.draw(spriteBatch);
                    break;
            }

            if (state != GameState.GAME)
            {
                players[1].draw(spriteBatch);
                players[0].draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            ms.grabInput();
            ks.grabInput();

            if (vs.word == "exit" || ks.key == "Esc")
                Exit();

            switch (state)
            {
                case GameState.MAIN:
                    mainMenu.update();
                    break;
                case GameState.LEVEL:
                    levelSelectionScreen.update();
                    break;
                case GameState.GAME:
                    maze.update();
                    break;
                case GameState.SCORE:
                    scoreScreen.update();
                    break;
                case GameState.PAUSE:
                    pauseScreen.update();
                    break;
                case GameState.SETTING:
                    settingsScreen.update();
                    break;
                case GameState.STATS:
                    statsScreen.update();
                    break;
                case GameState.INSTR:
                    instructionScreen.update();
                    break;
            }

            players[0].update(kinect.playerSkeleton[0]);
            players[1].update(kinect.playerSkeleton[1]);

            /*MouseState mouseState = Mouse.GetState();
            if (prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                OnLeftClick(new Point(mouseState.X, mouseState.Y));
            prevMouseState = mouseState;*/

            ms.newPointReady = false;
            ks.newKeyReady = false;
            if (vsSecondCycle)
            {
                vs.newWordReady = false;
                vsSecondCycle = false;
            }
            if (vs.newWordReady)
                vsSecondCycle = true;

            base.Update(gameTime);
        }

        /*private void OnLeftClick(Point point)
        {
            switch (state)
            {
                case GameState.MAIN:
                    mainMenu.onLeftClick(point);
                    break;
                case GameState.LEVEL:
                    levelSelectionScreen.onLeftClick(point);
                    break;
                case GameState.GAME:
                    maze.onLeftClick(point);
                    break;
                case GameState.SCORE:
                    scoreScreen.onLeftClick(point);
                    break;
                case GameState.PAUSE:
                    pauseScreen.onLeftClick(point);
                    break;
                case GameState.INSTR:
                    instructionScreen.onLeftClick(point);
                    break;
            }
        }*/

        public int sx(int x)
        {
            //return x * screenWidth / 1358;
            return x + (screenWidth-Maze.width) / 2;
        }

        public int sy(int y)
        {
            //return y * screenHeight / 738;
            return y + (screenHeight-Maze.height) / 2;
        }
    }
}
