using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class StatsScreen
    {
        Rectangle window, confWindow;
        Texture2D background, confTexture;
        //Rectangle avatar;
        Button totalButton, easyButton, hardButton, resetButton, mainMenuButton, createdMazesButton, singlePlayerButton, coopModeButton,
            nextButton, prevButton, yesButton, noButton;
        int hTopRow, hOtherRows, smallButtonWidth, smallButtonHeight, largeButtonWidth, largeButtonHeight;
        int screenWidth = Program.game.screenWidth;
        int screenHeight = Program.game.screenHeight;
        string totalGameTime;
        enum StatsState {TOTAL, SINGLEEASY, SINGLEHARD, COOPEASY, COOPHARD, CREATED};
        StatsState statsState;
        List<Button> buttons;
        bool conformation;
        int custPage;

        public StatsScreen()
        {
            conformation = false;
            custPage = 0;

            calcTotalGameTime();
            window = new Rectangle(screenWidth * 7 / 25 , screenHeight / 3 + 50, 8 * screenWidth / 15, 7 * screenHeight / 10);
            confWindow = new Rectangle(3 * screenWidth / 8, 3 * screenHeight / 8, screenWidth / 4, screenHeight / 4);
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
                smallButtonHeight, "Total", "Buttons/totalButton");
            singlePlayerButton = new Button(new Point(screenWidth / 2 - (int)(smallButtonWidth * 3/2) + 20, row2), smallButtonWidth, 
                smallButtonHeight, "Total", "Buttons/singlePlayer");
            coopModeButton = new Button(new Point(screenWidth / 2 + (int)(smallButtonWidth / 2) - 20, row2), smallButtonWidth, 
                smallButtonHeight, "Total", "Buttons/coopMode");
            createdMazesButton = new Button(new Point(screenWidth / 2 + smallButtonWidth * 2, row2), smallButtonWidth, 
                smallButtonHeight, "Total", "Buttons/createdMazes");
            easyButton = new Button(new Point(screenWidth / 2 - 2 * smallButtonWidth, row3), smallButtonWidth, 
                smallButtonHeight, "Easy", "Buttons/easy");
            hardButton = new Button(new Point(screenWidth / 2 + smallButtonWidth, row3), smallButtonWidth, 
                smallButtonHeight, "Hard", "Buttons/hard");
            nextButton = new Button
                (new Point(screenWidth - 220, screenHeight - 120), smallButtonWidth, smallButtonHeight, "Next", "Buttons/next");
            prevButton = new Button
                (new Point(20, screenHeight - 120), smallButtonWidth, smallButtonHeight, "Previous", "Buttons/previous");
            yesButton = new Button(new Point(screenWidth / 2 - 120, confWindow.Bottom - 100), 80, 50, "Yes", "Buttons/yes");
            noButton = new Button(new Point(screenWidth / 2 + 40, confWindow.Bottom - 100), 80, 50, "No", "Buttons/no");
            int levelX = window.Right - window.Width / 3 - smallButtonWidth / 2;
            statsState = StatsState.TOTAL;

            buttons = new List<Button>();
            buttons.Add(totalButton);
            buttons.Add(easyButton);
            buttons.Add(hardButton);
            buttons.Add(resetButton);
            buttons.Add(mainMenuButton);
            buttons.Add(singlePlayerButton);
            buttons.Add(coopModeButton);
            buttons.Add(createdMazesButton);
            buttons.Add(nextButton);
            buttons.Add(prevButton);
        }

        public void loadContent()
        {
            background = Program.game.Content.Load<Texture2D>("Backgrounds/statsScreen");
            confTexture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            confTexture.SetData<Color>(new Color[] { Color.White });

            foreach (Button button in buttons)
                button.loadContent();
            yesButton.loadContent();
            noButton.loadContent();
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

        void drawBlock(List<string> textArray, Point pos, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textArray.Count; i++)
            {
                string[] words = textArray[i].Split('\t');
                int y = pos.Y + 50 * i;
                for (int j = 0; j < words.Length; j++)
                {
                    int x = pos.X + 200 * j;
                    //Point pos = new Point(x, y);
                    spriteBatch.DrawString(MazeAndBlue.font, words[j], new Vector2(x, y), Color.Black);
                    //Program.game.drawText(textArray[i], pos);
                }
            }
        }

        void drawBlock(List<string> textArray, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textArray.Count; i++)
            {
                int x = screenWidth / 6;
                int y = window.Top + (int)(0.07 * screenHeight * (i + 2)) - 50;
                //Point pos = new Point(x, y);
                Vector2 textPos = new Vector2(x, y);
                spriteBatch.DrawString(MazeAndBlue.font, textArray[i], textPos, Color.Black);
                //Program.game.drawText(textArray[i], pos);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
           /* Vector2 position;
            position.X = screenWidth / 1000;
            position.Y = screenHeight / 10 - 80;
            spriteBatch.Draw(background, position, Color.White);*/
            totalButton.draw(spriteBatch);
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
                    drawBlock(totalBlock, spriteBatch);
                    break;
                case StatsState.SINGLEEASY:
                    easyButton.selected = true;
                    singlePlayerButton.selected = true;
                    easyButton.draw(spriteBatch);
                    hardButton.draw(spriteBatch);
                    Program.game.drawText("Single Easy Levels Stats", new Point(x, y));
                    List<string> singleSimpleBlock = new List<string>();
                    singleSimpleBlock.Add("\tScore\tTime\tWall Hits\tStars");
                    for (int i = 0; i < 6; i++)
                        singleSimpleBlock.Add("Level " + (i + 1) + ":\t" + Program.game.gameStats.data.levelData[i + 12].score +
                            "\t" + Program.game.gameStats.data.levelData[i + 12].time + "\t" +
                            Program.game.gameStats.data.levelData[i + 12].hits + "\t" + Program.game.gameStats.data.levelData[i + 12].numStars);
                    drawBlock(singleSimpleBlock, new Point(screenWidth / 5 - 50, screenHeight / 3 + 120), spriteBatch);
                    break;
                case StatsState.SINGLEHARD:
                    hardButton.selected = true;
                    singlePlayerButton.selected = true;
                    easyButton.draw(spriteBatch);
                    hardButton.draw(spriteBatch);
                    Program.game.drawText("coop Hard Levels Stats", new Point(x, y));
                    List<string> singleHardBlock = new List<string>();
                    for (int i = 0; i < 6; i++)
                        singleHardBlock.Add("Level " + (i + 1) + " Score: " + Program.game.gameStats.data.levelData[i + 18].score);
                    drawBlock(singleHardBlock, spriteBatch);
                    break;
                case StatsState.COOPEASY:
                    easyButton.selected = true;
                    coopModeButton.selected = true;
                    easyButton.draw(spriteBatch);
                    hardButton.draw(spriteBatch);
                    Program.game.drawText("Coop Easy Levels Stats", new Point(x, y));
                    List<string> coopSimpleBlock = new List<string>();
                    for (int i = 0; i < 6; i++)
                        coopSimpleBlock.Add("Level " + (i + 1) + ":   Score: " + Program.game.gameStats.data.levelData[i].score +
                            "\n           Time: " + Program.game.gameStats.data.levelData[i].time + "\n           Wall Hits: " + 
                            Program.game.gameStats.data.levelData[i].hits + "\n           Stars: " + Program.game.gameStats.data.levelData[i].numStars);
                    drawBlock(coopSimpleBlock, spriteBatch);
                    break;
                case StatsState.COOPHARD:
                    hardButton.selected = true;
                    coopModeButton.selected = true;
                    easyButton.draw(spriteBatch);
                    hardButton.draw(spriteBatch);
                    Program.game.drawText("Coop Hard Levels Stats", new Point(x, y));
                    List<string> coopHardBlock = new List<string>();
                    for (int i = 0; i < 6; i++)
                        coopHardBlock.Add("Level " + (i + 1) + " Score: " + Program.game.gameStats.data.levelData[i + 6].score);
                    drawBlock(coopHardBlock, spriteBatch);
                    break;
                case StatsState.CREATED:
                    createdMazesButton.selected = true;
                    Program.game.drawText("Created Mazes Stats", new Point(x, y));
                    List<string> createdBlock = new List<string>();
                    createdBlock.Add("\tScore\tTime\tWall Hits\tStars");
                    if (Program.game.customStats.data.customLevelIDs.Count <= (custPage + 1) * 6)
                    {
                        for (int i = custPage * 6; i < Program.game.customStats.data.customLevelIDs.Count; i++)
                        {
                            int level = Program.game.customStats.data.customLevelIDs[i];
                            LevelData levelData = Program.game.customStats.data.customData[level];
                            createdBlock.Add("Level " + level + ":\t" + levelData.score + "\t" + levelData.time + "\t" +
                                levelData.hits + "\t" + levelData.numStars);
                        }
                    }
                    else
                    {
                        for (int i = custPage * 6; i < (custPage + 1) * 6; i++)
                        {
                            int level = Program.game.customStats.data.customLevelIDs[i];
                            LevelData levelData = Program.game.customStats.data.customData[level];
                            createdBlock.Add("Level " + level + ":\t" + levelData.score + "\t" + levelData.time + "\t" +
                                levelData.hits + "\t" + levelData.numStars);
                        }
                        nextButton.draw(spriteBatch);
                    }
                    if (custPage > 0)
                        prevButton.draw(spriteBatch);
                    drawBlock(createdBlock, new Point(screenWidth / 5 - 50, screenHeight / 3 + 60), spriteBatch);
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("error in drawTitle, StatsScreen", "Error @ StatsScreen");
                    break;
            }

            if (conformation)
            {
                spriteBatch.Draw(confTexture, confWindow, new Color(128, 128, 128, 200));
                Program.game.drawText("Reset stat?", new Point(screenWidth / 2, confWindow.Top + 55));
                yesButton.draw(spriteBatch);
                noButton.draw(spriteBatch);
            }
        }

        public void update()
        {
            if (!conformation)
            {
                if (mainMenuButton.isSelected())
                    Program.game.startMainMenu();
                else if (resetButton.isSelected())
                {
                    //System.Windows.Forms.MessageBox.Show("Resetting your stats", "Warning");
                    conformation = true;
                    foreach (Button button in buttons)
                        button.selectable = false;
                }
                else if (totalButton.isSelected())
                    statsState = StatsState.TOTAL;
                else if (singlePlayerButton.isSelected())
                    statsState = StatsState.SINGLEEASY;
                else if (coopModeButton.isSelected())
                    statsState = StatsState.COOPEASY;
                else if (createdMazesButton.isSelected())
                    statsState = StatsState.CREATED;
                else if (singlePlayerButton.selected && easyButton.isSelected())
                    statsState = StatsState.SINGLEEASY;
                else if (singlePlayerButton.selected && hardButton.isSelected())
                    statsState = StatsState.SINGLEHARD;
                else if (coopModeButton.selected && easyButton.isSelected())
                    statsState = StatsState.COOPEASY;
                else if (coopModeButton.selected && hardButton.isSelected())
                    statsState = StatsState.COOPHARD;
            }
            else
            {
                if (yesButton.isSelected())
                {
                    Program.game.gameStats.resetData();
                    Program.game.gameStats.saveStats();
                    Program.game.customStats.resetData();
                    Program.game.customStats.saveStats();
                    calcTotalGameTime();
                    conformation = false;
                }
                if (noButton.isSelected())
                    conformation = false;

                if (!conformation)
                {
                    foreach (Button button in buttons)
                        button.selectable = true;
                }
            }
        }
    }
}
