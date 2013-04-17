using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class MazeAndBlue : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;

        public Kinect kinect;
        public GameStats gameStats;
        public GameSettings settings;
        Maze maze;
        MainMenu mainMenu;
        ScoreScreen scoreScreen;
        LevelSelectionScreen levelSelectionScreen;
        PauseScreen pauseScreen;
        SettingsScreen settingsScreen;
        StatsScreen statsScreen;
        InstructionScreen instructionScreen;
        CalibrationScreen calibrationScreen;
        Fireworks fireworks;
        CreateMazeSelection createMazeSelect;
        CreateMaze createMaze;

        public int level { get; set; }
        public bool singlePlayer { get; set; }
        public bool unlockOn { get; set; }
        bool vsSecondCycle = false;

        public List<string> deleteList;

        public List<Player> players { get; set; }
        public MouseSelect ms { get; set; }
        public VoiceSelect vs { get; set; }
        public KeyboardSelect ks { get; set; }
        public SoundEffectPlayer soundEffectPlayer { get; set; }

        public enum GameState { MAIN, LEVEL, GAME, SCORE, PAUSE, SETTING, STATS, INSTR, CALIBRATE, CREATESELECT, CREATE };
        public static GameState state { get; set; }
        
        public static SpriteFont font { get; set; }
        
        public int screenWidth { get { return GraphicsDevice.Viewport.Width; } }
        
        public int screenHeight { get { return GraphicsDevice.Viewport.Height; } }

        public MazeAndBlue()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;

            level = 0;
            unlockOn = true;
            deleteList = new List<string>();
        }

        protected override void Initialize()
        {
            players = new List<Player>();
            players.Add(new Player(-0.5f, 0f, Color.Blue, 0));
            players.Add(new Player(0f, 0.5f, Color.Yellow, 1));

            kinect = new Kinect();
            
            ms = new MouseSelect();
            ks = new KeyboardSelect();
            vs = new VoiceSelect();
            vs.recognizeSpeech(kinect.getSensorReference());
            
            soundEffectPlayer = new SoundEffectPlayer();

            gameStats = new GameStats();
            settings = new GameSettings();

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
            gameStats.saveStats();
        }

        public void startMainMenu()
        {
            mainMenu = new MainMenu();
            mainMenu.loadContent();
            state = GameState.MAIN;
        }

        public void startLevelSelectionScreen(bool singlePlayer)
        {
            levelSelectionScreen = new LevelSelectionScreen(singlePlayer);
            levelSelectionScreen.loadContent();
            state = GameState.LEVEL;
        }

        public void startLevel(int _level, bool _singlePlayer)
        {
            level = _level;
            singlePlayer = _singlePlayer;
            startLevel();
        }

        public void nextLevel()
        {
            level++;
            startLevel();
        }

        public void startLevel()
        {
            state = GameState.GAME;
            maze = new Maze(level, singlePlayer);
            maze.loadContent();
        }

        public void resumeLevel()
        {
            state = GameState.GAME;
        }

        public void startCustomLevel(int i)
        {
            state = GameState.GAME;
            maze = new Maze(i);
            maze.loadContent();
        }

        public void startScoreScreen(int time, int hits)
        {
            scoreScreen = new ScoreScreen(time, hits);
            scoreScreen.loadContent();
            fireworks = new Fireworks(); 
            fireworks.loadContent();
            state = GameState.SCORE;
        }

        public void startPauseSelectionScreen()
        {
            pauseScreen = new PauseScreen();
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

        public void startCalibrationScreen()
        {
            if (settingsScreen == null)
                startSettingsScreen();
            calibrationScreen = new CalibrationScreen();
            calibrationScreen.loadContent();
            state = GameState.CALIBRATE;
        }

        public void resumeSettings()
        {
            state = GameState.SETTING;
        }

        public void startCreateMazeSelect()
        {
            createMazeSelect = new CreateMazeSelection();
            createMazeSelect.loadContent();
            state = GameState.CREATESELECT;
        }
        
        public void startCreateMaze(bool hard, bool singleplayer)
        {
            createMaze = new CreateMaze(hard, singleplayer);
            createMaze.loadContent();
            state = GameState.CREATE;
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
                case GameState.CALIBRATE:
                    settingsScreen.draw(spriteBatch);
                    calibrationScreen.draw(spriteBatch);
                    break;
                case GameState.CREATESELECT:
                    createMazeSelect.draw(spriteBatch);
                    break;
                case GameState.CREATE:
                    createMaze.draw(spriteBatch);
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

            if (vs.word == "exit")
                Exit();

            if (ks.newKeyReady && ks.key == "Esc")
            {
                graphics.ToggleFullScreen(); 
                ks.newKeyReady = false;
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
                    fireworks.update();
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
                case GameState.CALIBRATE:
                    calibrationScreen.update();
                    break;
                case GameState.CREATESELECT:
                    createMazeSelect.update();
                    break;
                case GameState.CREATE:
                    createMaze.update();
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

        public void drawText(string text, Point pos)
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
