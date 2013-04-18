using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class ScoreScreen
    {
        Texture2D texture;
        Rectangle window;
        Fireworks fireworks;
        Button menuButton, levelButton, restartButton;
        List<Sprite> stars;
        int time, hits, score, numStars;

        public ScoreScreen(int _time, int _hits)
        {
            hits = _hits;
            time = _time;

            if (time == 0)
                time = 1;

            calcScore();
            calcNumStars();

            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;

            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = screenWidth / 8;
            int buttonHeight = screenHeight / 8;

            int menuY = window.Bottom - 3 * window.Height / 4 - buttonHeight / 2;
            int resumeY = window.Bottom - 2 * window.Height / 4 - buttonHeight / 2;
            int nextY = window.Bottom - window.Height / 4 - buttonHeight / 2;
            int x = window.Right - window.Width / 4 - buttonWidth / 2;

            menuButton = new Button(new Point(x, menuY), buttonWidth, buttonHeight, "Main Menu", "Buttons/mainMenuButton");
            restartButton = new Button(new Point(x, resumeY), buttonWidth, buttonHeight, "Replay", "Buttons/replay");
            if (Program.game.level != 5)
                levelButton = new Button(new Point(x, nextY), buttonWidth, buttonHeight, "Next Level", "Buttons/next");
            else
                levelButton = new Button(new Point(x, nextY), buttonWidth, buttonHeight, "Hard", "Buttons/hard");

            stars = new List<Sprite>();
            for (int i = 0; i < numStars; i++)
                stars.Add(new Sprite(new Point(i * 80 + window.Left + window.Width / 4 - 100, window.Bottom - window.Height / 5 - 20)));
            if (!Program.game.customLevel)
                Program.game.gameStats.updateLevelStats(time, hits, score, numStars);
            else
                Program.game.customStats.updateLevelStats(time, hits, score, numStars);
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            menuButton.loadContent();
            restartButton.loadContent();
            levelButton.loadContent();
            foreach (Sprite star in stars)
                star.loadContent("star");
            fireworks = new Fireworks();
            fireworks.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            fireworks.draw(spriteBatch);
            spriteBatch.Draw(texture, window, new Color(128, 128, 128, 200));
            menuButton.draw(spriteBatch);
            restartButton.draw(spriteBatch);
            if (Program.game.level != 11 && !Program.game.customLevel)
                levelButton.draw(spriteBatch);
            if (Program.game.level % 6 == 5 && !Program.game.customLevel)
            {
                string compText = string.Empty;
                if (Program.game.level == 5)
                    compText = "You have completed all the Easy Levels!";
                else if (Program.game.level == 11)
                    compText = "You have completed all the Hard Levels!";
                Program.game.drawText(compText, new Point(window.Left + window.Width / 2, window.Top + 45));
            }
            string timeTakenText = "Time taken: " + time + " seconds.";
            string wallHitsText = "Wall Hits: " + hits; 
            string scoreText = "Score: " + score;
            Program.game.drawText(timeTakenText, new Point(window.Left + window.Width / 4, window.Bottom - 4 * window.Height / 5));
            Program.game.drawText(wallHitsText, new Point(window.Left + window.Width / 4, window.Bottom - 3 * window.Height / 5));
            Program.game.drawText(scoreText, new Point(window.Left + window.Width / 4, window.Bottom - 2 * window.Height / 5));
            foreach (Sprite star in stars)
                star.draw(spriteBatch, Color.Yellow);
        }

        public void update()
        {
            fireworks.update();

            if (menuButton.isSelected())
                Program.game.startMainMenu();
            else if (restartButton.isSelected())
                Program.game.startLevel();
            else if (Program.game.level != 11 && !Program.game.customLevel && levelButton.isSelected())
                Program.game.nextLevel();
        }

        void calcScore()
        {
            double hitMultiplier = 1.4 - 0.02 * hits;
            if (hitMultiplier < 0)
                hitMultiplier = 0;
            
            double timeMultiplier = 1.4 - 0.015 * time;
            if (timeMultiplier < 0)
                timeMultiplier = 0;
            
            double multiplier = hitMultiplier + timeMultiplier;

            score = (int)(multiplier * 1000);

            if (score < 0)
                score = 100;

            if (score > 2500)
                score = 2500;
        }

        void calcNumStars()
        {
            if (score >= 2000)
                numStars = 3;
            else if (score >= 1000)
                numStars = 2;
            else
                numStars = 1;
        }

    }
}
