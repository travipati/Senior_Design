﻿using System.Collections.Generic;
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
        List<List<Sprite>> stars;
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
            nextButton = new Button(new Point(10 * screenWidth / 11 - smallButtonWidth / 2, 10 * screenHeight / 11 - smallButtonHeight / 2), 
                smallButtonWidth, smallButtonHeight, "Next", "Buttons/next");
            prevButton = new Button(new Point(screenWidth / 11 - smallButtonWidth / 2, 10 * screenHeight / 11 - smallButtonHeight / 2), 
                smallButtonWidth, smallButtonHeight, "Previous", "Buttons/previous");
            yesButton = new Button(new Point(confWindow.Left + confWindow.Width / 4 - 40, confWindow.Bottom - confWindow.Height / 3 - 25), 80, 50, "Yes", "Buttons/yes");
            noButton = new Button(new Point(confWindow.Right - confWindow.Width / 4 - 40, confWindow.Bottom - confWindow.Height / 3 - 25), 80, 50, "No", "Buttons/no");
            int levelX = window.Right - window.Width / 3 - smallButtonWidth / 2;
            statsState = StatsState.TOTAL;

            stars = new List<List<Sprite>>();
            stars.Add(new List<Sprite>());
            stars.Add(new List<Sprite>());
            stars.Add(new List<Sprite>());
            stars.Add(new List<Sprite>());
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < Program.game.gameStats.data.levelData[i + 12].numStars; j++)
                    stars[0].Add(new Sprite(new Point(screenWidth / 5 + 750 + 50 * j, screenHeight / 3 + 173 + 50 * i)));
                for (int j = 0; j < Program.game.gameStats.data.levelData[i + 18].numStars; j++)
                    stars[1].Add(new Sprite(new Point(screenWidth / 5 + 750 + 50 * j, screenHeight / 3 + 173 + 50 * i)));
                for (int j = 0; j < Program.game.gameStats.data.levelData[i].numStars; j++)
                    stars[2].Add(new Sprite(new Point(screenWidth / 5 + 750 + 50 * j, screenHeight / 3 + 173 + 50 * i)));
                for (int j = 0; j < Program.game.gameStats.data.levelData[i + 6].numStars; j++)
                    stars[3].Add(new Sprite(new Point(screenWidth / 5 + 750 + 50 * j, screenHeight / 3 + 173 + 50 * i)));
            }
            for (int i = 0; i < Program.game.customStats.data.customLevelIDs.Count; i++)
            {
                if (i % 6 == 0)
                    stars.Add(new List<Sprite>());
                int nameId = Program.game.customStats.data.customLevelIDs[i];
                for (int j = 0; j < Program.game.customStats.data.customData[nameId].numStars; j++)
                    stars[stars.Count - 1].Add(new Sprite(new Point(screenWidth / 5 + 750 + 50 * j, screenHeight / 3 + 113 + 50 * (i % 6))));
            }

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
            foreach (List<Sprite> list in stars)
            {
                foreach (Sprite star in list)
                    star.loadContent("star");
            }
        }

        private void calcTotalGameTime()
        {
            int sec = Program.game.gameStats.data.totalGameTime + Program.game.customStats.data.totalGameTime;
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
                int x = screenWidth / 2;
                int y = window.Top + (int)(0.07 * screenHeight * (i + 2)) - 50;
                Point pos = new Point(x, y);
                //Vector2 textPos = new Vector2(x, y);
                //spriteBatch.DrawString(MazeAndBlue.font, textArray[i], textPos, Color.Black);
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
                    totalBlock.Add("Total Game Time: " + totalGameTime);
                    totalBlock.Add("Total Score: " + (Program.game.gameStats.data.totalScore + Program.game.customStats.data.totalScore));
                    drawBlock(totalBlock, spriteBatch);
                    break;
                case StatsState.SINGLEEASY:
                    easyButton.selected = true;
                    singlePlayerButton.selected = true;
                    easyButton.draw(spriteBatch);
                    hardButton.draw(spriteBatch);
                    Program.game.drawText("Single Easy Levels Stats", new Point(x, y));
                    List<string> singleSimpleBlock = new List<string>();
                    singleSimpleBlock.Add("\tTime\tWall Hits\tScore\tStars");
                    for (int i = 0; i < 6; i++)
                    {
                        LevelData levelData = Program.game.gameStats.data.levelData[i + 12];
                        singleSimpleBlock.Add("Level " + (i + 1) + ":\t" + levelData.time + "\t" +
                            levelData.hits + "\t" + levelData.score);
                    }
                    drawBlock(singleSimpleBlock, new Point(screenWidth / 5 - 50, screenHeight / 3 + 120), spriteBatch);
                    foreach (Sprite star in stars[0])
                        star.draw(spriteBatch, Color.Yellow);
                    break;
                case StatsState.SINGLEHARD:
                    hardButton.selected = true;
                    singlePlayerButton.selected = true;
                    easyButton.draw(spriteBatch);
                    hardButton.draw(spriteBatch);
                    Program.game.drawText("Co-op Hard Levels Stats", new Point(x, y));
                    List<string> singleHardBlock = new List<string>();
                    singleHardBlock.Add("\tTime\tWall Hits\tScore\tStars");
                    for (int i = 0; i < 6; i++)
                    {
                        LevelData levelData = Program.game.gameStats.data.levelData[i + 18];
                        singleHardBlock.Add("Level " + (i + 1) + ":\t" + levelData.time + "\t" +
                            levelData.hits + "\t" + levelData.score);
                    }
                    drawBlock(singleHardBlock, new Point(screenWidth / 5 - 50, screenHeight / 3 + 120), spriteBatch);
                    foreach (Sprite star in stars[1])
                        star.draw(spriteBatch, Color.Yellow);
                    break;
                case StatsState.COOPEASY:
                    easyButton.selected = true;
                    coopModeButton.selected = true;
                    easyButton.draw(spriteBatch);
                    hardButton.draw(spriteBatch);
                    Program.game.drawText("Co-op Easy Levels Stats", new Point(x, y));
                    List<string> coopSimpleBlock = new List<string>();
                    coopSimpleBlock.Add("\tTime\tWall Hits\tScore\tStars");
                    for (int i = 0; i < 6; i++)
                    {
                        LevelData levelData = Program.game.gameStats.data.levelData[i];
                        coopSimpleBlock.Add("Level " + (i + 1) + ":\t" + levelData.time + "\t" +
                            levelData.hits + "\t" + levelData.score);
                    }
                    drawBlock(coopSimpleBlock, new Point(screenWidth / 5 - 50, screenHeight / 3 + 120), spriteBatch);
                    foreach (Sprite star in stars[2])
                        star.draw(spriteBatch, Color.Yellow);
                    break;
                case StatsState.COOPHARD:
                    hardButton.selected = true;
                    coopModeButton.selected = true;
                    easyButton.draw(spriteBatch);
                    hardButton.draw(spriteBatch);
                    Program.game.drawText("Co-op Hard Levels Stats", new Point(x, y));
                    List<string> coopHardBlock = new List<string>();
                    coopHardBlock.Add("\tTime\tWall Hits\tScore\tStars");
                    for (int i = 0; i < 6; i++)
                    {
                        LevelData levelData = Program.game.gameStats.data.levelData[i + 6];
                        coopHardBlock.Add("Level " + (i + 1) + ":\t" + levelData.time + "\t" +
                            levelData.hits + "\t" + levelData.score);
                    }
                    drawBlock(coopHardBlock, new Point(screenWidth / 5 - 50, screenHeight / 3 + 120), spriteBatch);
                    foreach (Sprite star in stars[3])
                        star.draw(spriteBatch, Color.Yellow);
                    break;
                case StatsState.CREATED:
                    createdMazesButton.selected = true;
                    Program.game.drawText("Created Mazes Stats", new Point(x, y));
                    List<string> createdBlock = new List<string>();
                    createdBlock.Add("\tTime\tWall Hits\tScore\tStars");
                    if (Program.game.customStats.data.customLevelIDs.Count <= (custPage + 1) * 6)
                    {
                        for (int i = custPage * 6; i < Program.game.customStats.data.customLevelIDs.Count; i++)
                        {
                            int level = Program.game.customStats.data.customLevelIDs[i];
                            LevelData levelData = Program.game.customStats.data.customData[level];
                            createdBlock.Add("Level " + (i + 1) + ":\t" + levelData.time + "\t" +
                                levelData.hits + "\t" + levelData.score);
                        }
                    }
                    else
                    {
                        for (int i = custPage * 6; i < (custPage + 1) * 6; i++)
                        {
                            int level = Program.game.customStats.data.customLevelIDs[i];
                            LevelData levelData = Program.game.customStats.data.customData[level];
                            createdBlock.Add("Level " + (i + 1) + ":\t" + levelData.time + "\t" +
                                levelData.hits + "\t" + levelData.score);
                        }
                        nextButton.draw(spriteBatch);
                    }
                    if (custPage > 0)
                        prevButton.draw(spriteBatch);
                    drawBlock(createdBlock, new Point(screenWidth / 5 - 50, screenHeight / 3 + 60), spriteBatch);
                    if (Program.game.customStats.data.customLevelIDs.Count > 0)
                    {
                        foreach (Sprite star in stars[custPage + 4])
                            star.draw(spriteBatch, Color.Yellow);
                    }
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("error in drawTitle, StatsScreen", "Error @ StatsScreen");
                    break;
            }

            if (conformation)
            {
                spriteBatch.Draw(confTexture, confWindow, new Color(128, 128, 128, 200));
                Program.game.drawText("Reset stats?", new Point(screenWidth / 2, confWindow.Top + confWindow.Height / 3));
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
                {
                    custPage = 0;
                    statsState = StatsState.CREATED;
                }
                else if (singlePlayerButton.selected && easyButton.isSelected())
                    statsState = StatsState.SINGLEEASY;
                else if (singlePlayerButton.selected && hardButton.isSelected())
                    statsState = StatsState.SINGLEHARD;
                else if (coopModeButton.selected && easyButton.isSelected())
                    statsState = StatsState.COOPEASY;
                else if (coopModeButton.selected && hardButton.isSelected())
                    statsState = StatsState.COOPHARD;
                else if (createdMazesButton.selected && Program.game.customStats.data.customLevelIDs.Count 
                    > (custPage + 1) * 6 && nextButton.isSelected())
                    custPage++;
                else if (createdMazesButton.selected && custPage > 0 && prevButton.isSelected())
                    custPage--;
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
