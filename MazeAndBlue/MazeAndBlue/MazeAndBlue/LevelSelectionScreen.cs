using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
//for message box, debugging only
using System.Runtime.InteropServices;
using System;

namespace MazeAndBlue
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LevelSelectionScreen 
    {        
        //for message box, debugging only
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);
        Button L1Button, L2Button, L3Button, menuButton, left, right;
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

            menuButton = new Button(new Vector2(menuButtonx, menuButtony), menuButtonWidth, menuButtonHeight, "Main Menu");
            left = new Button(new Vector2(leftx, menuButtony), menuButtonWidth, menuButtonHeight, "prev");
            right = new Button(new Vector2(rightx, menuButtony), menuButtonWidth, menuButtonHeight, "next");

            L1Button = new Button(new Vector2(level1x, levelButtony), levelButtonWidth, levelButtonHeight, "1");
            L2Button = new Button(new Vector2(level2x, levelButtony), levelButtonWidth, levelButtonHeight, "2");
            L3Button = new Button(new Vector2(level3x, levelButtony), levelButtonWidth, levelButtonHeight, "3");
        }
        
        public void loadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            background = content.Load<Texture2D>("simple1");
            menuButton.loadContent(content);
            left.loadContent(content);
            right.loadContent(content);
            L1Button.loadContent(content);
            L2Button.loadContent(content);
            L3Button.loadContent(content);
        }
        
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            menuButton.draw(spriteBatch);
            left.draw(spriteBatch);
            right.draw(spriteBatch);
            L1Button.draw(spriteBatch);
            L2Button.draw(spriteBatch);
            L3Button.draw(spriteBatch);
            string text = "Title";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)((Program.game.screenWidth - textSize.X) / 2);
            int y = (int)(Program.game.screenHeight / 3 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void onLeftClick(Point point)
        {
            if (menuButton.contains(point))
                undefinedButtonPress();
            else if (left.contains(point))
                undefinedButtonPress();
            else if (right.contains(point))
                undefinedButtonPress();
            else if (L1Button.contains(point))
                onLevelButtonPress(1);
            else if (L2Button.contains(point))
                onLevelButtonPress(2);
            else if (L3Button.contains(point))
                onLevelButtonPress(3);
        }

        private void onLevelButtonPress(int levelButton)
        {
            Program.game.startLevel(levelButton);
        }
        
        private void undefinedButtonPress()
        {
            MessageBox(new IntPtr(0), "button undefined", "Error", 0);
        }

    }
}
