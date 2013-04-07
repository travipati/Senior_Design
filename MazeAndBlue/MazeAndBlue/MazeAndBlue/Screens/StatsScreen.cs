using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class StatsScreen
    {
        Rectangle window;
        Texture2D background;
        Rectangle avatar;
        Button upper1, upper2, upper3, empty, lower1, lower2;
        int hTopRow, hOtherRows, buttonWidth;
        int screenWidth = Program.game.screenWidth;
        int screenHeight = Program.game.screenHeight;
        string totalGameTime;
        enum StatsState {TOTAL, SIMPLE, HARD};
        StatsState statsState;

        public StatsScreen()
        {
            calcTotalGameTime();
            window = new Rectangle(screenWidth * 7 / 25 , screenHeight / 3, 8 * screenWidth / 15, 7 * screenHeight / 10);
            hTopRow = screenHeight / 5;
            hOtherRows = screenHeight / 15 * 2;
            buttonWidth = screenWidth / 8;
            upper1 = new Button(new Point(0, hTopRow), buttonWidth, hOtherRows, "Total", "Buttons/statistics");
            upper2 = new Button(new Point(0, hTopRow + hOtherRows), buttonWidth, hOtherRows, "Simple", "Buttons/easy");
            upper3 = new Button(new Point(0, hTopRow + hOtherRows * 2), buttonWidth, hOtherRows, "Hard", "Buttons/hard");
            lower1 = new Button(new Point(0, hTopRow + hOtherRows * 4), buttonWidth, hOtherRows, "Reset", "Buttons/reset");
            lower2 = new Button(new Point(0, hTopRow + hOtherRows * 5), buttonWidth, hOtherRows, "Main", "Buttons/mainMenuButton");
            int levelX = window.Right - window.Width / 3 - buttonWidth / 2;
            statsState = StatsState.TOTAL;
        }

        public void loadContent()
        {
            //background = Program.game.Content.Load<Texture2D>("Backgrounds/simple0");
            background = Program.game.Content.Load<Texture2D>("Backgrounds/statsScreen");
            upper1.loadContent();
            upper2.loadContent();
            upper3.loadContent();
            lower1.loadContent();
            lower2.loadContent();
        }

        private void calcTotalGameTime()
        {
            int sec = Program.game.gameStats.data.totalGameTime;
            int hour = sec / 3600;
            sec = sec - hour * 3600;
            int min = sec / 60;
            sec = sec - min * 60;
            totalGameTime = hour + " : " + min + " : " + sec;
        }

        void drawBlock(List<string> textArray)
        {
            for (int i = 0; i < textArray.Count; i++)
            {
                int x = window.Left + window.Width / 2;
                int y = window.Top + (int)(0.1 * window.Height * (i + 1.5));
                Point pos = new Point(x, y);
                Program.game.draw(textArray[i], pos);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            upper1.draw(spriteBatch);
            upper2.draw(spriteBatch);
            upper3.draw(spriteBatch);
            lower1.draw(spriteBatch);
            lower2.draw(spriteBatch);
            switch (statsState)
            {
                case StatsState.TOTAL:

                    Program.game.draw("Game Statistics", new Point(screenWidth * 10 / 15, screenHeight / 10));
                    List<string> totalBlock = new List<string>();
                    totalBlock.Add("Total Game Time: " + totalGameTime + " seconds.");
                    totalBlock.Add("High score: " + Program.game.gameStats.data.totalScore + " pts.");
                    drawBlock(totalBlock);
                    break;

                case StatsState.SIMPLE:
                    Program.game.draw("Easy Levels Stats", new Point(screenWidth * 10 / 15, screenHeight / 10));
                    List<string> simpleBlock = new List<string>();
                    for (int i = 0; i < 6; i++)
                        simpleBlock.Add("Level " + (i + 1) + " Score: " + Program.game.gameStats.data.levelData[i].score);
                    drawBlock(simpleBlock);
                    break;
                case StatsState.HARD:
                    Program.game.draw("Hard Levels Stats", new Point(screenWidth * 10 / 15, screenHeight / 10));
                    List<string> hardBlock = new List<string>();
                    for (int i = 0; i < 6; i++)
                        hardBlock.Add("Level " + (i + 1) + " Score: " + Program.game.gameStats.data.levelData[i+6].score);
                    drawBlock(hardBlock);
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("error in drawTitle, StatsScreen", "Error @ StatsScreen");
                    break;
            }
        }

        public void update()
        {
            if (lower2.isSelected())
                Program.game.startMainMenu();
            
            if (lower1.isSelected())
            {
                System.Windows.Forms.MessageBox.Show("Resetting your stats", "Warning");
                Program.game.gameStats.resetData();
                Program.game.gameStats.saveStats();
                calcTotalGameTime();
            }

            if (upper1.isSelected())
                statsState = StatsState.TOTAL;
            else if (upper2.isSelected())
                statsState = StatsState.SIMPLE;
            else if (upper3.isSelected())
                statsState = StatsState.HARD;


        }
    }
}
