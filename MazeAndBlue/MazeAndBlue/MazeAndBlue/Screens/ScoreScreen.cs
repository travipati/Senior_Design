using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class ScoreScreen
    {
        Texture2D texture;
        Rectangle window;
        Button menuButton, levelButton, restartButton;
        int time, hits, score, numStars;
        List<Sprite> stars;

        public ScoreScreen(int _time, int _hits)
        {
            time = _time;
            hits = _hits;
            score = calcScore();
            numStars = calcNumStars();

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
            restartButton = new Button(new Point(x, resumeY), buttonWidth, buttonHeight, "Restart Level", "Buttons/restartLevel");
            if (Program.game.level != 6)
                levelButton = new Button(new Point(x, nextY), buttonWidth, buttonHeight, "Next Level", "Buttons/next");
            else
                levelButton = new Button(new Point(x, nextY), buttonWidth, buttonHeight, "Hard", "Buttons/hard");

            stars = new List<Sprite>();
            for (int i = 0; i < numStars; i++)
                stars.Add(new Sprite(new Point(i * 80 + window.Left + window.Width / 4 - 100, window.Bottom - window.Height / 5 - 20)));

            Program.game.gameStats.updateLevelStats(Program.game.level, time, hits, score, numStars);
        }

        public void loadContent()
        {
            //background = Program.game.Content.Load<Texture2D>("Backgrounds/simple0");
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            menuButton.loadContent();
            restartButton.loadContent();
            levelButton.loadContent();
            foreach (Sprite star in stars)
                star.loadContent("star");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, window, new Color(128, 128, 128, 232));
            menuButton.draw(spriteBatch);
            restartButton.draw(spriteBatch);
            if (Program.game.level != 12)
                levelButton.draw(spriteBatch);
            if (Program.game.level % 6 == 0)
            {
                string compText = string.Empty;
                if (Program.game.level == 6)
                    compText = "You have completed all the Easy Levels!";
                else if (Program.game.level == 12)
                    compText = "You have completed all the Hard Levels!";
                Program.game.draw(compText, new Point(window.Left + window.Width / 2, window.Top + 45));
            }
            string timeTakenText = "Time taken: " + time + " seconds.";
            string wallHitsText = "Wall Hits: " + hits; 
            string scoreText = "Score: " + score;
            Program.game.draw(timeTakenText, new Point(window.Left + window.Width / 4, window.Bottom - 4 * window.Height / 5));
            Program.game.draw(wallHitsText, new Point(window.Left + window.Width / 4, window.Bottom - 3 * window.Height / 5));
            Program.game.draw(scoreText, new Point(window.Left + window.Width / 4, window.Bottom - 2 * window.Height / 5));
            foreach (Sprite star in stars)
                star.draw(spriteBatch, Color.Yellow);
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.startMainMenu();
            else if (restartButton.isSelected())
                Program.game.startLevel();
            else if (levelButton.isSelected() && Program.game.level != 12)
                Program.game.nextLevel();
        }

        public int calcScore()
        {
            double multiplier = 1.5 - 0.01 * hits;

            if (multiplier < 0.5)
                multiplier = 0.5;

            int baseScore = (int)(multiplier * 100.0 * 20.0 / time);

            return baseScore;
        }

        public int calcNumStars()
        {
            if (score >= 100)
                return 3;
            else if (score >= 50)
                return 2;
            else if (score > 0)
                return 1;
            else
                return 0;
        }
    }
}
