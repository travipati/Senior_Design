using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Threading;
using Microsoft.Kinect;
using Microsoft.Speech;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

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
        
        Kinect kinect;
        Maze maze;
        Player player1, player2;
        ScoreScreen scoreScreen;

        MouseState prevMouseState;

        voiceControl VC;
        keyboardSelect keyboard;

        public enum GameState { GAME, SCORE };
        public static GameState state { get; set; }
        
        public static SpriteFont font { get; set; }
        
        public int screenWidth { get { return GraphicsDevice.Viewport.Width; } }
        
        public int screenHeight { get { return GraphicsDevice.Viewport.Height; } }

        public MazeAndBlue()
        {
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 576;
            prevMouseState = Mouse.GetState();
        }

        protected override void Initialize()
        {
            kinect = new Kinect();
            startLevel();

            base.Initialize();

            VC = new voiceControl();
            if (kinect.getSensorReference() != null)
            {
                VC.recognizeSpeech(kinect.getSensorReference());
            }
            keyboard = new keyboardSelect(ref(VC.states));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            keyboard.grabInput(Keyboard.GetState());

            player1.update(kinect.playerSkeleton[0], maze, VC);
            player2.update(kinect.playerSkeleton[1], maze, VC);

            if (state == GameState.GAME)
                maze.update(player1, player2);

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

            maze.draw(spriteBatch);
            player2.drawBall(spriteBatch);
            player1.drawBall(spriteBatch);

            if (state == GameState.SCORE)
            {
                scoreScreen.draw(spriteBatch);
            }

            player2.drawHand(spriteBatch);
            player1.drawHand(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void startLevel()
        {
            state = GameState.GAME;
            maze = new Maze();
            maze.loadContent(GraphicsDevice);
            player1 = new Player(new Vector2(108, 75), new Vector2(108, 0), -0.5f, 0f, Color.Blue, 0);
            player1.loadContent(Content);
            player2 = new Player(new Vector2(750, 92), new Vector2(750, 0), 0f, 0.5f, Color.Yellow, 1);
            player2.loadContent(Content);
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

        private void OnLeftClick(Point point)
        {
            switch (state)
            {
                case GameState.GAME:
                    player1.onLeftClick(point);
                    player2.onLeftClick(point);
                    break;
                case GameState.SCORE:
                    scoreScreen.onLeftClick(point);
                    break;
            }
        }

    }
}
