using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class StatsScreen
    {
        Rectangle window;
        Button menuButton, resetButton;
        Texture2D background;
        string totalGameTime;

        public StatsScreen()
        {
            calcTotalGameTime();
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = screenWidth / 8;
            int buttonHeight = screenHeight / 8;
            int y = window.Bottom - window.Height / 3 - buttonHeight / 2;
            int menuX = window.Left + window.Width / 3 - buttonWidth / 2;
            int levelX = window.Right - window.Width / 3 - buttonWidth / 2;
            menuButton = new Button(new Point(menuX, y), buttonWidth, buttonHeight, "Main Menu", "Buttons/button");
            resetButton = new Button(new Point(levelX, y), buttonWidth, buttonHeight, "Reset Stats", "Buttons/button");
        }

        public void loadContent()
        {
            background = Program.game.Content.Load<Texture2D>("Backgrounds/simple0");
            //texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            //texture.SetData<Color>(new Color[] { Color.White });
            menuButton.loadContent();
            resetButton.loadContent();
        }

        private void calcTotalGameTime()
        {
            int sec = Program.game.gameStats.data.totalGameTime;
            int hour = sec / 3600;
            sec = sec - hour * 3600;
            int min = sec / 60;
            sec = sec - min * 60;
            totalGameTime = hour + " : " + min + " : " + sec;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            menuButton.draw(spriteBatch);
            resetButton.draw(spriteBatch);
            string text = "Total Game Time: " + totalGameTime + " seconds.";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(window.X + (window.Width - textSize.X) / 2);
            int y = (int)(window.Top + window.Height / 3 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.startMainMenu();
            else if (resetButton.isSelected())
            {
                System.Windows.Forms.MessageBox.Show("You are resetting", "Warning");
                Program.game.gameStats.resetData();
                Program.game.gameStats.saveStats();
            }
        }
    }
}
