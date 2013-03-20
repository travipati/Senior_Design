using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class PauseScreen
    {
        Texture2D texture;
        Rectangle window;
        Button resumeButton, menuButton;

        public PauseScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = screenWidth / 8;
            int buttonHeight = screenHeight / 8;
            int y = window.Bottom - window.Height / 3;
            int menuX = window.Left + window.Width / 2 - buttonWidth / 2;
            resumeButton = new Button(new Vector2(menuX, window.Bottom - window.Height / 2 - buttonHeight / 2), buttonWidth, buttonHeight, "Resume");
            menuButton = new Button(new Vector2(menuX, y), buttonWidth, buttonHeight, "Main Menu");
        }

        public void loadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            resumeButton.loadContent(content);
            menuButton.loadContent(content);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, window, new Color(128, 128, 128, 232));
            resumeButton.draw(spriteBatch);
            menuButton.draw(spriteBatch);
            string text = "Paused";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(window.X + (window.Width - textSize.X) / 2);
            int y = (int)(window.Top + window.Height / 3 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void onLeftClick(Point point)
        {
            if (resumeButton.contains(point))
                onResumeButtonPress();
            if (menuButton.contains(point))
                onMenuButtonPress();
        }

        private void onResumeButtonPress()
        {
            Program.game.resumeLevel();
        }

        private void onMenuButtonPress()
        {
            Program.game.startMainMenu();
        }
    }
}