using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeAndBlue
{
    public class MazeAndBlue : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;

        Kinect kinect;
        Maze maze;
        MainMenu mainMenu;
        ScoreScreen scoreScreen;
        LevelSelectionScreen levelSelectionScreen;
        PauseScreen pauseScreen;
        InstructionScreen instructionScreen;
        int level, numLevels = 6;

        MouseState prevMouseState;

        public List<Player> players { get; set; }
        public MouseSelect ms { get; set; }
        public VoiceSelect vs { get; set; }
        public KeyboardSelect ks { get; set; }

        public enum GameState { MAIN, LEVEL, GAME, SCORE, PAUSE, INSTR };
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

            var form = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;// -8;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;// -30;
            //graphics.IsFullScreen = true;

            players = new List<Player>();
            players.Add(new Player(-0.5f, 0f, Color.Blue, 0));
            players.Add(new Player(0f, 0.5f, Color.Yellow, 1));

            prevMouseState = Mouse.GetState();
            level = 0;
        }

        protected override void Initialize()
        {
            kinect = new Kinect();
            //startLevel(level);
            startMainMenu();

            ms = new MouseSelect();
            vs = new VoiceSelect();
            vs.recognizeSpeech(kinect.getSensorReference());
            ks = new KeyboardSelect();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Backgrounds/blue");        
            font = Content.Load<SpriteFont>("font");

            foreach (Player player in players)
                player.loadContent(Content);
        }

        protected override void UnloadContent()
        {
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
                case GameState.GAME:
                    maze.update();
                    break;
            }

            players[0].update(kinect.playerSkeleton[0], maze);
            players[1].update(kinect.playerSkeleton[1], maze);

            MouseState mouseState = Mouse.GetState();
            if (prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                OnLeftClick(new Point(mouseState.X, mouseState.Y));
            prevMouseState = mouseState;

            base.Update(gameTime);
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
                    players[1].drawBall(spriteBatch);
                    players[0].drawBall(spriteBatch);
                    break;
                case GameState.SCORE:
                    scoreScreen.draw(spriteBatch);
                    break;
                case GameState.PAUSE:
                    maze.draw(spriteBatch);
                    pauseScreen.draw(spriteBatch);
                    break;
                case GameState.INSTR:
                    instructionScreen.draw(spriteBatch);
                    break;
            }

            players[1].drawHand(spriteBatch);
            players[0].drawHand(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void startMainMenu()
        {
            mainMenu = new MainMenu();
            mainMenu.loadContent(GraphicsDevice, Content);
            state = GameState.MAIN;
            /*
            player1.selected = false;
            player1.mouseSelected = false;
            player2.selected = false;
            player2.mouseSelected = false;
             * */
        }

        public void startLevel(int selectedLevel)
        {
            level = selectedLevel;
            nextLevel();
        }

        public void resumeLevel()
        {
            state = GameState.GAME;
        }

        public void nextLevel()
        {
            state = GameState.GAME;
            level %= numLevels;
            maze = new Maze("Mazes\\" + level++ + ".maze");
            maze.loadContent(GraphicsDevice, Content);
            players[0].setBallPos(maze.p1StartPosition);
            players[1].setBallPos(maze.p2StartPosition);
        }

        public void startScoreScreen(int time)
        {
            scoreScreen = new ScoreScreen(time);
            scoreScreen.loadContent(GraphicsDevice, Content);
            state = GameState.SCORE;
            foreach (Player player in players)
            {
                player.selected = false;
                player.mouseSelected = false;
            }
        }

        public void startLevelSelectionScreen()
        {
            levelSelectionScreen = new LevelSelectionScreen();
            levelSelectionScreen.loadContent(GraphicsDevice, Content);
            state = GameState.LEVEL;
/*            player1.selected = false;
            player1.mouseSelected = false;
            player2.selected = false;
            player2.mouseSelected = false;
 */
        }

        public void startPauseSelectionScreen()
        {
            pauseScreen = new PauseScreen();
            pauseScreen.loadContent(GraphicsDevice, Content);
            state = GameState.PAUSE;
            foreach (Player player in players)
            {
                player.selected = false;
                player.mouseSelected = false;
            }
        }

        public void startInstructionScreen()
        {
            instructionScreen = new InstructionScreen();
            instructionScreen.loadContent(GraphicsDevice, Content);
            state = GameState.INSTR;
        }

        private void OnLeftClick(Point point)
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
                    foreach (Player player in players)
                        player.onLeftClick(point);
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
        }

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
