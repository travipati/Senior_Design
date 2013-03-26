using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeAndBlue
{
    class PauseScreen
    {
        Texture2D texture;
        Rectangle window;
        Button resumeButton, menuButton;
        List<Button> buttons;

        public PauseScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = screenWidth / 8;
            int buttonHeight = screenHeight / 8;
            int y = window.Bottom - window.Height / 3;
            int menuX = window.Left + window.Width / 2 - buttonWidth / 2;
            resumeButton = new Button(new Point(menuX, window.Bottom - window.Height / 2 - buttonHeight / 2), buttonWidth, buttonHeight, "Resume", "Buttons/button");
            menuButton = new Button(new Point(menuX, y), buttonWidth, buttonHeight, "Main Menu", "Buttons/button");
            buttons = new List<Button>();
            buttons.Add(resumeButton);
            buttons.Add(menuButton);
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

        public void update()
        {
            foreach (Button button in buttons)
            {
                if (button.isOver())
                {
                    button.enlarge(0.1);
                    button.loadContent(Program.game.Content, "Buttons/Hover");
                }
                else
                {
                    button.reload();
                }
            }

            if (resumeButton.isSelected())
                Program.game.resumeLevel();
            if (menuButton.isSelected())
                Program.game.startMainMenu();
        }
    }
}