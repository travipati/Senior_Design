using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

struct selectStates
{
    public bool [] select;
    public bool [] selectStated;
}

namespace MazeAndBlue
{
    public class MazeAndBlue : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        
        Kinect kinect;
        Maze maze;
        Player player1, player2;
        MainMenu mainMenu;
        ScoreScreen scoreScreen;
        LevelSelectionScreen levelSelectionScreen;
        PauseScreen pauseScreen;
        int level, numLevels = 6;

        MouseState prevMouseState;

        voiceControl vc;
        KeyboardSelect keyboard;

        public enum GameState { MAIN, LEVEL, GAME, SCORE, PAUSE };
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

            prevMouseState = Mouse.GetState();
            level = 0;
        }

        protected override void Initialize()
        {
            kinect = new Kinect();
            //startLevel(level);
            startMainMenu();

            vc = new voiceControl();
            vc.recognizeSpeech(kinect.getSensorReference());
            keyboard = new KeyboardSelect(ref(vc.states));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Backgrounds/blue");        

            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            keyboard.grabInput(keyboardState);

            switch (state)
            {
                case GameState.GAME:
                    player1.update(kinect.playerSkeleton[0], maze, vc);
                    player2.update(kinect.playerSkeleton[1], maze, vc);
                    maze.update(player1, player2);
                    break;
            }              

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
                    player2.drawBall(spriteBatch);
                    player1.drawBall(spriteBatch);
                    player2.drawHand(spriteBatch);
                    player1.drawHand(spriteBatch);
                    break;
                case GameState.SCORE:
                    scoreScreen.draw(spriteBatch);
                    break;
                case GameState.PAUSE:
                    pauseScreen.draw(spriteBatch);
                    break;
            }

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

        public void nextLevel()
        {
            state = GameState.GAME;
            level %= numLevels;
            maze = new Maze("Mazes\\" + level++ + ".maze");
            maze.loadContent(GraphicsDevice, Content);
            player1 = new Player(maze.p1StartPosition, -0.5f, 0f, Color.Blue, 0);
            player1.loadContent(Content);
            player2 = new Player(maze.p2StartPosition, 0f, 0.5f, Color.Yellow, 1);
            player2.loadContent(Content);
        }

        public void startLevel(int selectedLevel)
        {
            level = selectedLevel;
            nextLevel();
        }

        public void startScoreScreen(int time)
        {
            scoreScreen = new ScoreScreen(time);
            scoreScreen.loadContent(GraphicsDevice, Content);
            state = GameState.SCORE;
            player1.selected = false;
            player1.mouseSelected = false;
            player2.selected = false;
            player2.mouseSelected = false;
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
            player1.selected = false;
            player1.mouseSelected = false;
            player2.selected = false;
            player2.mouseSelected = false;
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
                    player1.onLeftClick(point);
                    player2.onLeftClick(point);
                    break;
                case GameState.SCORE:
                    scoreScreen.onLeftClick(point);
                    break;
                case GameState.PAUSE:
                    pauseScreen.onLeftClick(point);
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
