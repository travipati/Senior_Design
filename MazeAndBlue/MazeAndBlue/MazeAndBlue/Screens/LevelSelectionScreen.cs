using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{

    public class LevelSelectionScreen 
    {        
        List<Button> levelButtons;
        Button menuButton, previous, next;
        Texture2D background;

        public LevelSelectionScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            //int labelWidth = screenWidth / 2;
            //int labelHeight = screenHeight / 5;
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
            int previousx = screenWidth / 8;
            int menuButtonx = screenWidth / 2 - menuButtonWidth / 2;
            int nextx = 3 * screenWidth / 4;

            menuButton = new Button(new Point(menuButtonx, menuButtony), menuButtonWidth, menuButtonHeight, "Main Menu", "Buttons/mainMenuButton");
            previous = new Button(new Point(previousx, menuButtony), menuButtonWidth, menuButtonHeight, "prev", "Buttons/previous");
            next = new Button(new Point(nextx, menuButtony), menuButtonWidth, menuButtonHeight, "next", "Buttons/next");

            levelButtons = new List<Button>();
            levelButtons.Add(new Button(new Point(level1x, levelButton1y), levelButtonWidth, levelButtonHeight, "1", "LevelThumbnails/level0"));
            levelButtons.Add(new Button(new Point(level2x, levelButton1y), levelButtonWidth, levelButtonHeight, "2", "LevelThumbnails/level1"));
            levelButtons.Add(new Button(new Point(level3x, levelButton1y), levelButtonWidth, levelButtonHeight, "3", "LevelThumbnails/level2"));
            levelButtons.Add(new Button(new Point(level1x, levelButton2y), levelButtonWidth, levelButtonHeight, "4", "LevelThumbnails/level3"));
            levelButtons.Add(new Button(new Point(level2x, levelButton2y), levelButtonWidth, levelButtonHeight, "5", "LevelThumbnails/level4"));
            levelButtons.Add(new Button(new Point(level3x, levelButton2y), levelButtonWidth, levelButtonHeight, "6", "LevelThumbnails/level5"));
        }
        
        public void loadContent()
        {
            background = Program.game.Content.Load<Texture2D>("Backgrounds/chooseLevel");
            menuButton.loadContent();
            previous.loadContent();
            next.loadContent();
            foreach (Button button in levelButtons)
                button.loadContent();
        }
        
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            menuButton.draw(spriteBatch);
            previous.draw(spriteBatch);
            next.draw(spriteBatch);
            foreach (Button button in levelButtons)
                button.draw(spriteBatch);
            string text = "";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)((Program.game.screenWidth - textSize.X) / 2);
            int y = (int)(Program.game.screenHeight / 3 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.startMainMenu();
            else if (previous.isSelected())
                undefinedButtonPress();
            else if (next.isSelected())
                undefinedButtonPress();
            for (int i = 0; i < levelButtons.Count; i++)
            {
                if (levelButtons[i].isSelected())
                    Program.game.startLevel(i);
            }

            foreach (Button button in levelButtons)
                button.update();

        }

        private void undefinedButtonPress()
        {
            System.Windows.Forms.MessageBox.Show("Button Undefined","Error");
        }

    }
}
