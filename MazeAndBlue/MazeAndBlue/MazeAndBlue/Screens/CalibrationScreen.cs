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
        List<Vector2> selections;                   //topRight1, bottomLeft2, topRight2, bottomLeft2

        public CalibrationScreen()
        {
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
            selections = new List<Vector2>(4);
            for (int i = 0; i < selections.Capacity; i++)
            {
                Vector2 temp = new Vector2(-1, -1);
                selections.Add(temp);
            }
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
                "Put your hand to the top right and say: \"select one\" or \"select two\",\n" +
                "then move your hand to the bottom left and do the same.";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(window.X + (window.Width - textSize.X) / 2);
            int y = (int)(window.Top + window.Height / 5 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);

            if (selections[0].X >= 0)
            {
                string tempText = "top right player 1";
                Vector2 tempTextSize = MazeAndBlue.font.MeasureString(tempText);
                spriteBatch.DrawString(MazeAndBlue.font, tempText, tempTextSize, Color.Black);
            }
            if (selections[1].X >= 0)
            {
                string tempText = "bottom left player 1";
                Vector2 tempTextSize = MazeAndBlue.font.MeasureString(tempText);
                spriteBatch.DrawString(MazeAndBlue.font, tempText, tempTextSize, Color.Black);
            }
            if (selections[2].X >= 0)
            {
                string tempText = "top right player 2";
                Vector2 tempTextSize = MazeAndBlue.font.MeasureString(tempText);
                spriteBatch.DrawString(MazeAndBlue.font, tempText, tempTextSize, Color.Black);
            }
            if (selections[3].X >= 0)
            {
                string tempText = "bottom left player 2";
                Vector2 tempTextSize = MazeAndBlue.font.MeasureString(tempText);
                spriteBatch.DrawString(MazeAndBlue.font, tempText, tempTextSize, Color.Black);
            }

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

            if (Program.game.players[0].selecting())
            {
                if (selections[0].X < 0)
                {
                    selections[0] = new Vector2(Program.game.players[0].position.X,
                        Program.game.players[0].position.Y);
                }
                else if (selections[1].X < 0)
                {
                    selections[1] = new Vector2(Program.game.players[0].position.X,
                        Program.game.players[0].position.Y);
                }
            }
            if (Program.game.players[1].selecting())
            {
                if (selections[2].X < 0)
                {
                    selections[2] = new Vector2(Program.game.players[0].position.X,
                        Program.game.players[0].position.Y);
                }
                else if (selections[3].X < 0)
                {
                    selections[3] = new Vector2(Program.game.players[0].position.X,
                        Program.game.players[0].position.Y);
                }
            }
        }
    }
}