using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class ScoreScreen
    {
        Texture2D texture;
        Rectangle window;
        Button menuButton, levelButton, restartButton;
        int time, hits, score, stars;

        public ScoreScreen(int _time, int _hits)
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = screenWidth / 8;
            int buttonHeight = screenHeight / 8;

            int y = window.Bottom - window.Height / 2;
            int menuX = window.Left + window.Width / 2 - 5 * buttonWidth / 2;
            int nextX = window.Left + window.Width / 2 - buttonWidth / 2;
            int resumeX = window.Left + window.Width / 2 + 3 * buttonWidth / 2;
            menuButton = new Button(new Point(menuX, y), buttonWidth, buttonHeight, "Main Menu", "Buttons/mainMenuButton");
            if (Program.game.level != 6)
                levelButton = new Button(new Point(nextX, y), buttonWidth, buttonHeight, "Next Level", "Buttons/next");
            else
                levelButton = new Button(new Point(nextX, y), buttonWidth, buttonHeight, "Hard", "Buttons/hard");
            restartButton = new Button(new Point(resumeX, y), buttonWidth, buttonHeight, "Restart Level", "Buttons/restartLevel");

            time = _time;
            hits = _hits;
            score = calcScore();
            stars = calcNumStars();
            Program.game.gameStats.updateLevelStats(Program.game.level, time, hits, score, stars);
        }

        public void loadContent()
        {
            //background = Program.game.Content.Load<Texture2D>("Backgrounds/simple0");
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            menuButton.loadContent();
            levelButton.loadContent();
            restartButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, window, new Color(128, 128, 128, 232));
            menuButton.draw(spriteBatch);
            if (Program.game.level != 12)
                levelButton.draw(spriteBatch);
            else
            {
                string compText1 = "Congratulations!", compText2 = string.Empty;
                if (Program.game.level == 6)
                    compText2 = "You have completed all the Easy Levels!";
                else if (Program.game.level == 12)
                    compText2 = "You have completed all the Hard Levels!";
                Vector2 compSize1 = MazeAndBlue.font.MeasureString(compText1);
                Program.game.draw(compText1, new Point(window.Left + window.Width / 4, window.Top + window.Height / 2));
                Vector2 compSize2 = MazeAndBlue.font.MeasureString(compText2);
                Program.game.draw(compText2, new Point(window.Left + window.Width / 4, window.Top + window.Height / 2));
            }
            restartButton.draw(spriteBatch);
            string timeTakenText = "Time taken: " + time + " seconds.";
            string score = "Score: ";
            Vector2 text1Size = MazeAndBlue.font.MeasureString(timeTakenText);
            Vector2 text2Size = MazeAndBlue.font.MeasureString(score);
            Program.game.draw(timeTakenText, new Point(window.Left + window.Width / 4, window.Top + window.Height / 2));
            Program.game.draw(score, new Point(window.Left + window.Width / 4, window.Top + window.Height / 2));
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.startMainMenu();
            else if (levelButton.isSelected() && Program.game.level != 12)
                Program.game.nextLevel();
            else if (restartButton.isSelected())
            {
                Program.game.level--;
                Program.game.nextLevel();
            }
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
