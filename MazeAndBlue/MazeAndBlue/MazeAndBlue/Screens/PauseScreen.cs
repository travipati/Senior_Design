using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class PauseScreen
    {
        Texture2D texture;
        Rectangle window;
        Button resumeButton, restartButton, menuButton;
        List<Button> buttons;
        int level;

        public PauseScreen(int _level)
        {
            level = _level;
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = 170;
            int buttonHeight = 92;
            int y = window.Bottom - window.Height / 5 - buttonHeight / 2;
            int menuX = window.Left + window.Width / 2 - buttonWidth / 2;
            resumeButton = new Button(new Point(menuX, window.Top + 2 * window.Height / 5 - buttonHeight / 2), buttonWidth, buttonHeight, "Resume", "Buttons/resume");
            restartButton = new Button(new Point(menuX, window.Top + 3 * window.Height / 5 - buttonHeight / 2), buttonWidth, buttonHeight, "Restart", "Buttons/restartLevel");
            menuButton = new Button(new Point(menuX, y), buttonWidth, buttonHeight, "Main Menu", "Buttons/mainMenuButton");
            buttons = new List<Button>();
            buttons.Add(resumeButton);
            buttons.Add(restartButton);
            buttons.Add(menuButton);
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            resumeButton.loadContent();
            restartButton.loadContent();
            menuButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, window, new Color(128, 128, 128, 232));
            resumeButton.draw(spriteBatch);
            restartButton.draw(spriteBatch);
            menuButton.draw(spriteBatch);
            string text = "Paused";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(window.X + (window.Width - textSize.X) / 2);
            int y = (int)(window.Top + window.Height / 5 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void update()
        {
            foreach (Button button in buttons)
                button.update();

            if (resumeButton.isSelected())
                Program.game.resumeLevel();
            if (restartButton.isSelected())
                Program.game.startLevel(level);            
            if (menuButton.isSelected())
                Program.game.startMainMenu();
        }
    }
}