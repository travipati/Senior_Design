using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class LevelSelectionScreen 
    {
        Texture2D background;
        Button menuButton, easyButton, hardButton;
        List<Button> easyLevelButtons, hardLevelButtons;
        List<Sprite> easyLevelStars, hardLevelStars;
        bool singlePlayer;
        enum LevelsState { EASY, HARD };
        LevelsState levelsState;
        string[] levelNames = { "level one", "level two", "level three", "level four", "level five", "level six" };

        public LevelSelectionScreen(bool _singlePlayer)
        {
            singlePlayer = _singlePlayer;

            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            int levelButtonWidth = screenWidth / 8;
            int levelButtonHeight = screenHeight / 8;
            int menuButtonWidth = 170;
            int menuButtonHeight = 92;

            List<int> levely = new List<int>();
            levely.Add(screenHeight / 2 - 10);
            levely.Add(2 * screenHeight / 3 + 40);
            List<int> levelx = new List<int>();
            levelx.Add(screenWidth / 2 - 5 * levelButtonWidth / 2);
            levelx.Add(screenWidth / 2 - levelButtonWidth / 2);
            levelx.Add(screenWidth / 2 + 3 * levelButtonWidth / 2);

            int menuButtony = screenHeight / 5;
            int easyx = screenWidth / 8;
            int menuButtonx = screenWidth / 2 - menuButtonWidth / 2;
            int hardx = 3 * screenWidth / 4;

            menuButton = new Button(new Point(menuButtonx, menuButtony), menuButtonWidth, menuButtonHeight, "Main Menu", "Buttons/mainMenuButton");
            easyButton = new Button(new Point(easyx, menuButtony), menuButtonWidth, menuButtonHeight, "Easy", "Buttons/easy");
            hardButton = new Button(new Point(hardx, menuButtony), menuButtonWidth, menuButtonHeight, "Hard", "Buttons/hard");

            easyLevelButtons = new List<Button>();
            hardLevelButtons = new List<Button>();
            easyLevelStars = new List<Sprite>();
            hardLevelStars = new List<Sprite>();

            if (singlePlayer)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (Program.game.unlockOn && i > Program.game.gameStats.data.singleNextLevelToUnlock)
                    {
                        easyLevelButtons.Add(new Button(new Point(levelx[i % 3], levely[i / 3]), levelButtonWidth, levelButtonHeight, levelNames[i], "LevelThumbnails/lockColor"));
                        easyLevelButtons[i].selectable = false;
                    }
                    else
                    {
                        easyLevelButtons.Add(new Button(new Point(levelx[i % 3], levely[i / 3]), levelButtonWidth, levelButtonHeight, levelNames[i], "LevelThumbnails/level" + i));
                        for (int j = 0; j < Program.game.gameStats.data.levelData[i + 12].numStars; j++)
                            easyLevelStars.Add(new Sprite(new Point(j * 60 + levelx[i % 3] + levelButtonWidth / 2 - 80, levely[i / 3] + levelButtonHeight - 20)));
                    }
                    if (Program.game.unlockOn && i + 6 > Program.game.gameStats.data.singleNextLevelToUnlock)
                    {
                        hardLevelButtons.Add(new Button(new Point(levelx[i % 3], levely[i / 3]), levelButtonWidth, levelButtonHeight, levelNames[i], "LevelThumbnails/lockColor"));
                        hardLevelButtons[i].selectable = false;
                    }
                    else
                    {
                        hardLevelButtons.Add(new Button(new Point(levelx[i % 3], levely[i / 3]), levelButtonWidth, levelButtonHeight, levelNames[i], "LevelThumbnails/level" + (i + 6)));
                        for (int j = 0; j < Program.game.gameStats.data.levelData[i + 18].numStars; j++)
                            hardLevelStars.Add(new Sprite(new Point(j * 60 + levelx[i % 3] + levelButtonWidth / 2 - 80, levely[i / 3] + levelButtonHeight - 20)));
                    }
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    if (Program.game.unlockOn && i > Program.game.gameStats.data.coopNextLevelToUnlock)
                    {
                        easyLevelButtons.Add(new Button(new Point(levelx[i % 3], levely[i / 3]), levelButtonWidth, levelButtonHeight, levelNames[i], "LevelThumbnails/lockColor"));
                        easyLevelButtons[i].selectable = false;
                    }
                    else
                    {
                        easyLevelButtons.Add(new Button(new Point(levelx[i % 3], levely[i / 3]), levelButtonWidth, levelButtonHeight, levelNames[i], "LevelThumbnails/level" + i));
                        for (int j = 0; j < Program.game.gameStats.data.levelData[i].numStars; j++)
                            easyLevelStars.Add(new Sprite(new Point(j * 60 + levelx[i % 3] + levelButtonWidth / 2 - 80, levely[i / 3] + levelButtonHeight - 20)));
                    }
                    if (Program.game.unlockOn && i + 6 > Program.game.gameStats.data.coopNextLevelToUnlock)
                    {
                        hardLevelButtons.Add(new Button(new Point(levelx[i % 3], levely[i / 3]), levelButtonWidth, levelButtonHeight, levelNames[i], "LevelThumbnails/lockColor"));
                        hardLevelButtons[i].selectable = false;
                    }
                    else
                    {
                        hardLevelButtons.Add(new Button(new Point(levelx[i % 3], levely[i / 3]), levelButtonWidth, levelButtonHeight, levelNames[i], "LevelThumbnails/level" + (i + 6)));
                        for (int j = 0; j < Program.game.gameStats.data.levelData[i + 6].numStars; j++)
                            hardLevelStars.Add(new Sprite(new Point(j * 60 + levelx[i % 3] + levelButtonWidth / 2 - 80, levely[i / 3] + levelButtonHeight - 20)));
                    }
                }
            }

            levelsState = LevelsState.EASY;
        }
        
        public void loadContent()
        {
            background = Program.game.Content.Load<Texture2D>("Backgrounds/chooseLevel");
            menuButton.loadContent();
            easyButton.loadContent();
            hardButton.loadContent();
            foreach (Button button in easyLevelButtons)
                button.loadContent();
            foreach (Button button in hardLevelButtons)
                button.loadContent();
            foreach (Sprite star in easyLevelStars)
                star.loadContent("star");
            foreach (Sprite star in hardLevelStars)
                star.loadContent("star");
        }
        
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            menuButton.draw(spriteBatch);
            easyButton.draw(spriteBatch);
            hardButton.draw(spriteBatch);

            switch (levelsState)
            {
                case LevelsState.EASY:
                    for (int i = 0; i < easyLevelButtons.Count; i++)
                    {
                        if (easyLevelButtons[i].selectable)
                            easyLevelButtons[i].draw(spriteBatch);
                        else
                            easyLevelButtons[i].draw(spriteBatch, Color.Gray);
                    }
                    foreach (Sprite star in easyLevelStars)
                        star.draw(spriteBatch, Color.Yellow);
                    easyButton.selected = true;
                    hardButton.selected = false;
                    break;
                case LevelsState.HARD:
                    for (int i = 0; i < hardLevelButtons.Count; i++)
                    {
                        if (hardLevelButtons[i].selectable)
                            hardLevelButtons[i].draw(spriteBatch);
                        else
                            hardLevelButtons[i].draw(spriteBatch, Color.Gray);
                    }
                    foreach (Sprite star in hardLevelStars)
                        star.draw(spriteBatch, Color.Yellow);
                    easyButton.selected = false;
                    hardButton.selected = true;
                    break;
            }
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.startMainMenu();
            else if (easyButton.isSelected())
                levelsState = LevelsState.EASY;
            else if (hardButton.isSelected())
                levelsState = LevelsState.HARD;

            switch (levelsState)
            {
                case LevelsState.EASY:
                    for (int i = 0; i < easyLevelButtons.Count; i++)
                    {
                        if (easyLevelButtons[i].selectable && easyLevelButtons[i].isSelected())
                            Program.game.startLevel(i, singlePlayer);
                    }
                    break;
                case LevelsState.HARD:
                    for (int i = 0; i < hardLevelButtons.Count; i++)
                    {
                        if (hardLevelButtons[i].selectable && hardLevelButtons[i].isSelected())
                            Program.game.startLevel(i + 6, singlePlayer);
                    }
                    break;
            }
        }
    }
}
