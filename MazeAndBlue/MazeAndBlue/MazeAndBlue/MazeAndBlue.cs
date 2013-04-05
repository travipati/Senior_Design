using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public GameSettings settings;
        MainMenu mainMenu;
        ScoreScreen scoreScreen;
        LevelSelectionScreen levelSelectionScreen;
        PauseScreen pauseScreen;
        SettingsScreen settingsScreen;
        StatsScreen statsScreen;
        InstructionScreen instructionScreen; 
        Fireworks fireworks;

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
            settings = new GameSettings();

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
            kinect.stop();
        }

        public void startMainMenu()
        {
            mainMenu = new MainMenu();
            mainMenu.loadContent();
            state = GameState.MAIN;
        }

        public void startLevelSelectionScreen()
        {
            levelSelectionScreen = new LevelSelectionScreen();
            levelSelectionScreen.loadContent();
            state = GameState.LEVEL;
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
            gameStats.updateLevelStats(level, time, maze.wallHits);
            scoreScreen = new ScoreScreen(time);
            scoreScreen.loadContent();
            fireworks = new Fireworks(); 
            fireworks.loadContent();
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
                    maze.draw(spriteBatch);
                    fireworks.draw(spriteBatch);
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
            {
                Exit();
            }

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
                    fireworks.update();
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

        public void draw(string text, Point pos)
        {
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(pos.X - textSize.X / 2);
            int y = (int)(pos.Y - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public int sx(int x)
        {
            return x + (screenWidth-Maze.width) / 2;
        }

        public int sy(int y)
        {
            return y + (screenHeight-Maze.height) / 2;
        }
    }
}
