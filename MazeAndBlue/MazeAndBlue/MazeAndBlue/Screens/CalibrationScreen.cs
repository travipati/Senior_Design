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
        Button startButton;
        Button menuButton;
        List<Button> buttons;
        bool started;
        int countdown;
        Timer timer;

        public CalibrationScreen()
        {
            countdown = 5;
            timer = new Timer();
            

            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(0, 0, screenWidth, screenHeight);
            
            int buttonWidth = 170;
            int buttonHeight = 92;
            int y = window.Bottom - window.Height / 2;
            int menuX = window.Left + window.Width / 2 - buttonWidth / 2;
            startButton = new Button(new Point(menuX, y), buttonWidth, buttonHeight, "save", "Buttons/save");
            y = window.Bottom - window.Height / 3;
            menuButton = new Button(new Point(menuX, y), buttonWidth, buttonHeight, "settings", "Buttons/settings");
            buttons = new List<Button>();
            buttons.Add(menuButton);
        }

        public void loadContent()
        {
            background = Program.game.Content.Load<Texture2D>("Backgrounds/blue");
            startButton.loadContent();
            menuButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f, 
                Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            if (!started)
            {
                startButton.draw(spriteBatch);
            }
            menuButton.draw(spriteBatch);
            string text = "Calibrating the Kinect:\n\n" +
                "Fully outstretch both arms,\n" +
                "Press save and wait " + (int)(countdown - timer.time) +" seconds for the game to calibrate.\n";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(window.X + (window.Width - textSize.X) / 2);
            int y = (int)(window.Top + window.Height / 5 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void update()
        {
            if (!started && startButton.isSelected())
            {
                started = true;
                startButton.selectable = false;
                timer.start();
            }

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