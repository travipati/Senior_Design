using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class StatsScreen
    {
        Rectangle window;
        Texture2D background;
        //Rectangle avatar;
        Button totalButton, easyButton, hardButton, resetButton, mainMenuButton, createdMazesButton, singlePlayerButton, coopModeButton;
        int hTopRow, hOtherRows, smallButtonWidth, smallButtonHeight, largeButtonWidth, largeButtonHeight;
        int screenWidth = Program.game.screenWidth;
        int screenHeight = Program.game.screenHeight;
        string totalGameTime;
        enum StatsState {TOTAL, SINGLESIMPLE, SINGLEHARD, COOPSIMPLE, COOPHARD, CREATED};
        StatsState statsState;

        public StatsScreen()
        {
            calcTotalGameTime();
            window = new Rectangle(screenWidth * 7 / 25 , screenHeight / 3 + 50, 8 * screenWidth / 15, 7 * screenHeight / 10);
            hTopRow = screenHeight / 5;
            hOtherRows = screenHeight / 15 * 2;
            smallButtonHeight = 72;
            smallButtonWidth = 136;
            largeButtonWidth = 170;
            largeButtonHeight = 92;
            
            int row1 = screenHeight / 9 - largeButtonHeight / 2;
            int row2 = screenHeight / 2 - 3 * smallButtonHeight;
            int row3 = screenHeight / 2 - 3 * smallButtonHeight / 2 - 10;

            resetButton = new Button(new Point(5 * screenWidth / 6 - smallButtonWidth / 2, row1), largeButtonWidth, 
                largeButtonHeight, "Reset", "Buttons/reset");
            mainMenuButton = new Button(new Point(screenWidth / 6 - smallButtonWidth / 2, row1), largeButtonWidth, 
                largeButtonHeight, "Main", "Buttons/mainMenuButton");
            totalButton = new Button(new Point(screenWidth / 2 - smallButtonWidth * 3, row2), smallButtonWidth, 
                smallButtonHeight, "Total", "Buttons/statistics");
            singlePlayerButton = new Button(new Point(screenWidth / 2 - (int)(smallButtonWidth * 3/2) + 20, row2), smallButtonWidth, 
                smallButtonHeight, "Total", "Buttons/singlePlayer");
            coopModeButton = new Button(new Point(screenWidth / 2 + (int)(smallButtonWidth / 2) - 20, row2), smallButtonWidth, 
                smallButtonHeight, "Total", "Buttons/coopMode");
            createdMazesButton = new Button(new Point(screenWidth / 2 + smallButtonWidth * 2, row2), smallButtonWidth, 
                smallButtonHeight, "Total", "Buttons/createMaze");
            easyButton = new Button(new Point(screenWidth / 2 - 2 * smallButtonWidth, row3), smallButtonWidth, 
                smallButtonHeight, "Easy", "Buttons/easy");
            hardButton = new Button(new Point(screenWidth / 2 + smallButtonWidth, row3), smallButtonWidth, 
                smallButtonHeight, "Hard", "Buttons/hard");
            int levelX = window.Right - window.Width / 3 - smallButtonWidth / 2;
            statsState = StatsState.TOTAL;
        }

        public void loadContent()
        {
            background = Program.game.Content.Load<Texture2D>("Backgrounds/statsScreen");
            totalButton.loadContent();
            easyButton.loadContent();
            hardButton.loadContent();
            resetButton.loadContent();
            mainMenuButton.loadContent();
            singlePlayerButton.loadContent();
            coopModeButton.loadContent();
            createdMazesButton.loadContent();
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
                int x = screenWidth / 2;
                int y = window.Top + (int)(0.08 * screenHeight * (i + 2)) - 55;
                Point pos = new Point(x, y);
                Program.game.drawText(textArray[i], pos);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
           /* Vector2 position;
            position.X = screenWidth / 1000;
            position.Y = screenHeight / 10 - 80;
            spriteBatch.Draw(background, position, Color.White);*/
            totalButton.draw(spriteBatch);
            easyButton.draw(spriteBatch);
            hardButton.draw(spriteBatch);
            resetButton.draw(spriteBatch);
            mainMenuButton.draw(spriteBatch);
            singlePlayerButton.draw(spriteBatch);
            coopModeButton.draw(spriteBatch);
            createdMazesButton.draw(spriteBatch);
           
            int x = screenWidth / 2;
            int y = screenHeight / 9;
            totalButton.selected = false;
            easyButton.selected = false;
            hardButton.selected = false;
            singlePlayerButton.selected = false;
            coopModeButton.selected = false;
            createdMazesButton.selected = false;

            switch (statsState)
            {
                case StatsState.TOTAL:
                    totalButton.selected = true;
                    Program.game.drawText("Game Statistics", new Point(x, y));
                    List<string> totalBlock = new List<string>();
                    totalBlock.Add("Total Game Time: " + totalGameTime + " seconds.");
                    totalBlock.Add("Total Score: " + Program.game.gameStats.data.totalScore + " pts.");
                    drawBlock(totalBlock);
                    break;
                case StatsState.SINGLESIMPLE:
                    easyButton.selected = true;
                    singlePlayerButton.selected = true;
                    Program.game.drawText("Single Easy Levels Stats", new Point(x, y));
                    List<string> singleSimpleBlock = new List<string>();
                    singleSimpleBlock.Add("                         Score      Time      Wall Hits    Stars");
                    for (int i = 0; i < 6; i++)
                        singleSimpleBlock.Add("Level " + (i + 1) + ":             " + Program.game.gameStats.data.levelData[i].score +
                            "              " + Program.game.gameStats.data.levelData[i].time + "              " +
                            Program.game.gameStats.data.levelData[i].hits + "              " + Program.game.gameStats.data.levelData[i].numStars);
                    drawBlock(singleSimpleBlock);
                    break;
                case StatsState.SINGLEHARD:
                    hardButton.selected = true;
                    singlePlayerButton.selected = true;
                    Program.game.drawText("coop Hard Levels Stats", new Point(x, y));
                    List<string> singleHardBlock = new List<string>();
                    for (int i = 0; i < 6; i++)
                        singleHardBlock.Add("Level " + (i + 1) + " Score: " + Program.game.gameStats.data.levelData[i + 18].score);
                    drawBlock(singleHardBlock);
                    break;
                case StatsState.COOPSIMPLE:
                    easyButton.selected = true;
                    coopModeButton.selected = true;
                    Program.game.drawText("Coop Easy Levels Stats", new Point(x, y));
                    List<string> coopSimpleBlock = new List<string>();
                    for (int i = 0; i < 6; i++)
                        coopSimpleBlock.Add("Level " + (i + 1) + ":   Score: " + Program.game.gameStats.data.levelData[i].score +
                            "\n           Time: " + Program.game.gameStats.data.levelData[i].time + "\n           Wall Hits: " + 
                            Program.game.gameStats.data.levelData[i].hits + "\n           Stars: " + Program.game.gameStats.data.levelData[i].numStars);
                    drawBlock(coopSimpleBlock);
                    break;
                case StatsState.COOPHARD:
                    hardButton.selected = true;
                    coopModeButton.selected = true;
                    Program.game.drawText("Coop Hard Levels Stats", new Point(x, y));
                    List<string> coopHardBlock = new List<string>();
                    for (int i = 0; i < 6; i++)
                        coopHardBlock.Add("Level " + (i + 1) + " Score: " + Program.game.gameStats.data.levelData[i + 6].score);
                    drawBlock(coopHardBlock);
                    break;
                case StatsState.CREATED:
                    createdMazesButton.selected = true;
                    Program.game.drawText("Created Mazes Stats", new Point(x, y));
                    List<string> createdBlock = new List<string>();
                    for (int i = 0; i < 6; i++)
                        createdBlock.Add("Level " + (i + 1) + " Score: " + Program.game.gameStats.data.levelData[i].score);
                    drawBlock(createdBlock);
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("error in drawTitle, StatsScreen", "Error @ StatsScreen");
                    break;
            }
        }

        public void update()
        {
            if (mainMenuButton.isSelected())
                Program.game.startMainMenu();
            
            if (resetButton.isSelected())
            {
                //System.Windows.Forms.MessageBox.Show("Resetting your stats", "Warning");
                Program.game.gameStats.resetData();
                Program.game.gameStats.saveStats();
                calcTotalGameTime();
            }

            if (totalButton.isSelected())
                statsState = StatsState.TOTAL;
            else if (singlePlayerButton.isSelected() && !hardButton.isSelected())
                statsState = StatsState.SINGLESIMPLE;
            else if (singlePlayerButton.isSelected())
                statsState = StatsState.SINGLEHARD;
            else if (coopModeButton.isSelected() && !hardButton.isSelected())
                statsState = StatsState.COOPSIMPLE;
            else if (coopModeButton.isSelected())
                statsState = StatsState.COOPHARD;
            else if (createdMazesButton.isSelected())
                statsState = StatsState.CREATED;
        }
    }
}
