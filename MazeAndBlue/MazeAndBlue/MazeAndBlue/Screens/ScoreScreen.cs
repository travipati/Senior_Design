using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class ScoreScreen
    {
        Texture2D texture;
        Rectangle window;
        Button menuButton, levelButton, restartButton;
        int time;
        Texture2D background;

        public ScoreScreen(int _time)
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = screenWidth / 8;
            int buttonHeight = screenHeight / 8;
            int y = window.Bottom - window.Height / 3 - buttonHeight / 2;
            int menuX = window.Left + window.Width / 3 - buttonWidth / 2;
            int levelX = window.Right - window.Width / 3 - buttonWidth / 2;
            menuButton = new Button(new Point(screenWidth / 2 - buttonWidth / 2, screenHeight/2 - buttonHeight), buttonWidth, buttonHeight, "Main Menu", "Buttons/mainMenuButton");
            levelButton = new Button(new Point(screenWidth / 2 - buttonWidth / 2, screenHeight / 2 + buttonHeight - screenHeight/16), buttonWidth, buttonHeight, "Next Level", "Buttons/next");
            restartButton = new Button(new Point(screenWidth / 2 - buttonWidth / 2, screenHeight / 2 + 2 * buttonHeight), buttonWidth, buttonHeight, "Restart", "Buttons/restartLevel");
            time = _time;
        }

        public void loadContent()
        {
            //background = Program.game.Content.Load<Texture2D>("Backgrounds/simple0");
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            menuButton.loadContent();
            levelButton.loadContent();
            restartButton.loadContent();

        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, window, new Color(128, 128, 128, 232));
            menuButton.draw(spriteBatch);
            levelButton.draw(spriteBatch);
            restartButton.draw(spriteBatch);
            string text = "Time taken: " + time + " seconds.";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(window.X + (window.Width - textSize.X) / 2);
            int y = (int)(window.Top + window.Height / 6 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.startMainMenu();
            else if (levelButton.isSelected())
                Program.game.nextLevel();
            else if (restartButton.isSelected())
            {
                Program.game.level--;
                Program.game.nextLevel();
            }
        }
    }
}
