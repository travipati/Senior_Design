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
        Texture2D background;
        Rectangle window;
        Button menuButton;
        List<Button> buttons;
        int countdown;
        Timer timer;

        public CalibrationScreen()
        {
            countdown = 5;
            timer = new Timer();
            timer.start();

            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(0, 0, screenWidth, screenHeight);
            
            int buttonWidth = 170;
            int buttonHeight = 92;
            int y = window.Bottom - window.Height / 2;
            int menuX = window.Left + window.Width / 2 - buttonWidth / 2;
            menuButton = new Button(new Point(menuX, y), buttonWidth, buttonHeight, "Main Menu", "Buttons/settings");
            buttons = new List<Button>();
            buttons.Add(menuButton);
        }

        public void loadContent()
        {
            background = Program.game.Content.Load<Texture2D>("Backgrounds/blue");
            menuButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            menuButton.draw(spriteBatch);
            string text = "Calibrating the Kinect:\n\n" +
                "Outstretch your controlling arm fully to your side,\n" +
                "Please wait " + (int)(countdown - timer.time) +" seconds for the game to calibrate.";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(window.X + (window.Width - textSize.X) / 2);
            int y = (int)(window.Top + window.Height / 5 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.resumeSettings();

            if (countdown - timer.time <= 0)
            {
                calibratePlayers();
                Program.game.resumeSettings();
            }
        }

        public void calibratePlayers()
        {
            for (int i = 0; i <Program.game.players.Count; i++)
            {
                if (Program.game.kinect.playerSkeleton[i] != null)
                    Program.game.players[i].setMovementRange(Program.game.kinect.playerSkeleton[i]);
            }
        }

    }
}