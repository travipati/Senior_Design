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

        public ScoreScreen(int _time)
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = screenWidth / 8;
            int buttonHeight = screenHeight / 8;

            int y = window.Bottom - window.Height / 2;
            int menuX = window.Left + window.Width / 2 - 5 * buttonWidth / 2;
            int nextX = window.Left + window.Width / 2 - buttonWidth / 2;
            int resumeX = window.Left + window.Width / 2 + 3 * buttonWidth / 2;
            menuButton = new Button(new Point(menuX, y), buttonWidth, buttonHeight, "Main Menu", "Buttons/mainMenuButton");
            levelButton = new Button(new Point(nextX, y), buttonWidth, buttonHeight, "Next Level", "Buttons/next");
            restartButton = new Button(new Point(resumeX, y), buttonWidth, buttonHeight, "Restart Level", "Buttons/restartLevel");

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
            if (Program.game.level % 6 != 0)
                levelButton.draw(spriteBatch);
            else
            {
                string compText = string.Empty;
                if (Program.game.level == 6)
                    compText = "Congratulations! You have completed all the Easy Levels!";
                else if (Program.game.level == 12)
                    compText = "Congratulations! You have completed all the Hard Levels!";
                Vector2 compSize = MazeAndBlue.font.MeasureString(compText);
                int compX = (int)(window.X + (window.Width - compSize.X) / 2);
                int compY = (int)(window.Top + window.Height / 3 - compSize.Y / 2);
                Vector2 compPos = new Vector2(compX, compY);
                spriteBatch.DrawString(MazeAndBlue.font, compText, compPos, Color.Black);
            }
            restartButton.draw(spriteBatch);
            string timeTakenText = "Time taken: " + time + " seconds.";
            Vector2 textSize = MazeAndBlue.font.MeasureString(timeTakenText);
            int xT = (int)(window.X + (window.Width - textSize.X) / 2);
            int yT = (int)(window.Top + window.Height / 6 - textSize.Y / 2);
            Vector2 textPos = new Vector2(xT, yT);
            spriteBatch.DrawString(MazeAndBlue.font, timeTakenText, textPos, Color.Black);
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
