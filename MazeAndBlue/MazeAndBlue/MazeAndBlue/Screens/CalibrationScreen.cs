using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class CalibrationScreen
    {
        Texture2D texture;
        Rectangle window;
        Button menuButton;
        List<Button> buttons;
        GameTime time;

        public CalibrationScreen()
        {
            time = new GameTime();
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(0, 0, screenWidth, screenHeight);
            
            int buttonWidth = 170;
            int buttonHeight = 92;
            int y = window.Bottom - window.Height / 2;
            int menuX = window.Left + window.Width / 2 - buttonWidth / 2;
            menuButton = new Button(new Point(menuX, y), buttonWidth, buttonHeight, "Main Menu", "Buttons/mainMenuButton");
            buttons = new List<Button>();
            buttons.Add(menuButton);
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            menuButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, window, new Color(128, 128, 128, 232));
            menuButton.draw(spriteBatch);
            string text = "Calibrate the Kinect:\n\n" +
                "Both players outstretch your controlling hand a comfortable distance to your side,\n" +
                "Please wait " + (int)(3 - time.TotalGameTime.TotalSeconds) +" seconds for the game to calibrate";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(window.X + (window.Width - textSize.X) / 2);
            int y = (int)(window.Top + window.Height / 5 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void update()
        {
            foreach (Button button in buttons)
            {
                button.update();
            }
            if (menuButton.isSelected())
            {
                Program.game.startMainMenu();
            }

            if (3 - time.TotalGameTime.TotalSeconds <= 0)
            {
                //call the player methods to update settings
                Program.game.startMainMenu();
            }
        }
    }
}