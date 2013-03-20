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
        Button menuButton;

        public PauseScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = screenWidth / 8;
            int buttonHeight = screenHeight / 8;
            int y = window.Bottom - window.Height / 3 - buttonHeight / 2;
            int menuX = window.Left + window.Width / 2 - buttonWidth / 2;
            menuButton = new Button(new Vector2(menuX, y), buttonWidth, buttonHeight, "Main Menu");
        }

        public void loadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            menuButton.loadContent(content);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, window, new Color(128, 128, 128, 232));
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
            if (menuButton.contains(point))
                onMenuButtonPress();
        }

        private void onMenuButtonPress()
        {
            Program.game.startMainMenu();
        }
    }
}