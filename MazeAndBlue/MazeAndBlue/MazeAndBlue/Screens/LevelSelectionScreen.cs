using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{

    public class LevelSelectionScreen 
    {        
        List<Button> levelButtons;
        Button menuButton, left, right;
        Texture2D background;

        public LevelSelectionScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            //int labelWidth = screenWidth / 2;
            //int labelHeight = screenHeight / 5;
            int levelButtonWidth = screenWidth / 8;
            int levelButtonHeight = screenHeight / 8;
            int menuButtonWidth = screenWidth / 8;
            int menuButtonHeight = screenHeight / 8;

            int levelButtony = screenHeight / 2;
            int level1x = screenWidth / 12 * 2;
            int level2x = screenWidth / 12 * 5;
            int level3x = screenWidth / 12 * 8;

            int menuButtony = screenHeight /6;
            int leftx = screenWidth / 12;
            int menuButtonx = screenWidth / 12 * 5;
            int rightx = screenWidth / 12 * 9;

            menuButton = new Button(new Point(menuButtonx, menuButtony), menuButtonWidth, menuButtonHeight, "Main Menu", "Buttons/button");
            left = new Button(new Point(leftx, menuButtony), menuButtonWidth, menuButtonHeight, "prev", "Buttons/button");
            right = new Button(new Point(rightx, menuButtony), menuButtonWidth, menuButtonHeight, "next", "Buttons/button");

            levelButtons = new List<Button>();
            levelButtons.Add(new Button(new Point(level1x, levelButtony), levelButtonWidth, levelButtonHeight, "1", "Buttons/button"));
            levelButtons.Add(new Button(new Point(level2x, levelButtony), levelButtonWidth, levelButtonHeight, "2", "Buttons/button"));
            levelButtons.Add(new Button(new Point(level3x, levelButtony), levelButtonWidth, levelButtonHeight, "3", "Buttons/button"));
        }
        
        public void loadContent()
        {
            background = Program.game.Content.Load<Texture2D>("Backgrounds/simple1");
            menuButton.loadContent();
            left.loadContent();
            right.loadContent();
            foreach (Button button in levelButtons)
                button.loadContent();
        }
        
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            menuButton.draw(spriteBatch);
            left.draw(spriteBatch);
            right.draw(spriteBatch);
            foreach (Button button in levelButtons)
                button.draw(spriteBatch);
            string text = "Title";
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
            else if (left.isSelected())
                undefinedButtonPress();
            else if (right.isSelected())
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
