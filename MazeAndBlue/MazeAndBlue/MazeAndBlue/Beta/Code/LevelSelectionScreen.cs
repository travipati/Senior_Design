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
        bool singlePlayer;
        enum LevelsState { COOPEASY, COOPHARD };
        LevelsState levelsState;

        public LevelSelectionScreen(bool _singlePlayer)
        {
            singlePlayer = _singlePlayer;

            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            int levelButtonWidth = screenWidth / 8;
            int levelButtonHeight = screenHeight / 8;
            int menuButtonWidth = 170;
            int menuButtonHeight = 92;

            int levelButton1y = screenHeight / 2 - 10;
            int levelButton2y = 2 * screenHeight / 3 + 40;
            int level1x = screenWidth / 2 - 5 * levelButtonWidth / 2;
            int level2x = screenWidth / 2 - levelButtonWidth / 2;
            int level3x = screenWidth / 2 + 3 * levelButtonWidth / 2;

            int menuButtony = screenHeight / 5;
            int easyx = screenWidth / 8;
            int menuButtonx = screenWidth / 2 - menuButtonWidth / 2;
            int hardx = 3 * screenWidth / 4;

            menuButton = new Button(new Point(menuButtonx, menuButtony), menuButtonWidth, menuButtonHeight, "Main Menu", "Buttons/mainMenuButton");
            easyButton = new Button(new Point(easyx, menuButtony), menuButtonWidth, menuButtonHeight, "Easy", "Buttons/easy");
            hardButton = new Button(new Point(hardx, menuButtony), menuButtonWidth, menuButtonHeight, "Hard", "Buttons/hard");

            easyLevelButtons = new List<Button>();
            easyLevelButtons.Add(new Button(new Point(level1x, levelButton1y), levelButtonWidth, levelButtonHeight, "level one", "LevelThumbnails/level0"));
            easyLevelButtons.Add(new Button(new Point(level2x, levelButton1y), levelButtonWidth, levelButtonHeight, "level two", "LevelThumbnails/level1"));
            easyLevelButtons.Add(new Button(new Point(level3x, levelButton1y), levelButtonWidth, levelButtonHeight, "level three", "LevelThumbnails/level2"));
            easyLevelButtons.Add(new Button(new Point(level1x, levelButton2y), levelButtonWidth, levelButtonHeight, "level four", "LevelThumbnails/level3"));
            easyLevelButtons.Add(new Button(new Point(level2x, levelButton2y), levelButtonWidth, levelButtonHeight, "level five", "LevelThumbnails/level4"));
            easyLevelButtons.Add(new Button(new Point(level3x, levelButton2y), levelButtonWidth, levelButtonHeight, "level six", "LevelThumbnails/level5"));

            hardLevelButtons = new List<Button>();
            hardLevelButtons.Add(new Button(new Point(level1x, levelButton1y), levelButtonWidth, levelButtonHeight, "level one", "LevelThumbnails/level6"));
            hardLevelButtons.Add(new Button(new Point(level2x, levelButton1y), levelButtonWidth, levelButtonHeight, "level two", "LevelThumbnails/level7"));
            hardLevelButtons.Add(new Button(new Point(level3x, levelButton1y), levelButtonWidth, levelButtonHeight, "level three", "LevelThumbnails/level8"));
            hardLevelButtons.Add(new Button(new Point(level1x, levelButton2y), levelButtonWidth, levelButtonHeight, "level four", "LevelThumbnails/level9"));
            hardLevelButtons.Add(new Button(new Point(level2x, levelButton2y), levelButtonWidth, levelButtonHeight, "level five", "LevelThumbnails/level10"));
            hardLevelButtons.Add(new Button(new Point(level3x, levelButton2y), levelButtonWidth, levelButtonHeight, "level six", "LevelThumbnails/level11"));

            levelsState = LevelsState.COOPEASY;
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
        }
        
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            menuButton.draw(spriteBatch);
            easyButton.draw(spriteBatch);
            hardButton.draw(spriteBatch);

            switch (levelsState)
            {
                case LevelsState.COOPEASY:
                    foreach (Button button in easyLevelButtons)
                        button.draw(spriteBatch);
                    easyButton.selected = true;
                    hardButton.selected = false;
                    break;
                case LevelsState.COOPHARD:
                    foreach (Button button in hardLevelButtons)
                        button.draw(spriteBatch);
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
                levelsState = LevelsState.COOPEASY;
            else if (hardButton.isSelected())
                levelsState = LevelsState.COOPHARD;

            switch (levelsState)
            {
                case LevelsState.COOPEASY:
                    for (int i = 0; i < easyLevelButtons.Count; i++)
                    {
                        if (easyLevelButtons[i].isSelected())
                            Program.game.startLevel(i, singlePlayer);
                    }
                    break;
                case LevelsState.COOPHARD:
                    for (int i = 0; i < hardLevelButtons.Count; i++)
                    {
                        if (hardLevelButtons[i].isSelected())
                            Program.game.startLevel(i + 6, singlePlayer);
                    }
                    break;
            }
        }
    }
}