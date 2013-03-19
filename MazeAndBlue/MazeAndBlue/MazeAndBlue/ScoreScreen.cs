using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
//for message box, debugging only
using System.Runtime.InteropServices;
using System;

namespace MazeAndBlue
{
    class ScoreScreen
    {
        //for message box, debugging only
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        //Texture2D texture;
        Rectangle window;
        Button menuButton, levelButton;
        int time;
        Texture2D background;

        public ScoreScreen(int _time)
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);
            int buttonWidth = screenWidth / 8;
            int buttonHeight = screenHeight / 8;
            int y = window.Bottom - window.Height / 3 - buttonHeight / 2;
            int menuX = window.Left + window.Width / 3 - buttonWidth / 2;
            int levelX = window.Right - window.Width / 3 - buttonWidth / 2;
            menuButton = new Button(new Vector2(menuX, y), buttonWidth, buttonHeight, "Main Menu");
            levelButton = new Button(new Vector2(levelX, y), buttonWidth, buttonHeight, "Next Level");
            time = _time;
        }

        public void loadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            background = content.Load<Texture2D>("simple0");
            //texture = new Texture2D(graphicsDevice, 1, 1);
            //texture.SetData<Color>(new Color[] { Color.White });
            menuButton.loadContent(content);
            levelButton.loadContent(content);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            menuButton.draw(spriteBatch);
            levelButton.draw(spriteBatch);
            string text = "Time taken: " + time + " seconds.";
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(window.X + (window.Width - textSize.X) / 2);
            int y = (int)(window.Top + window.Height / 3 - textSize.Y / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
        }

        public void onLeftClick(Point point)
        {
            if (menuButton.contains(point))
                onMenuButtonPress();
            else if (levelButton.contains(point))
                onLevelButtonPress();
        }

        private void onLevelButtonPress()
        {
            Program.game.nextLevel();
        }

        private void onMenuButtonPress()
        {
            MessageBox(new IntPtr(0), "No main menu yet, we go to level selection instead", "Error", 0);
            Program.game.startLevelSelectionScreen();
        }

    }
}
