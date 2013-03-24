using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class InstructionScreen
    {
        Texture2D texture;
        Rectangle screenRectangle;
        Sprite backButton;

        public InstructionScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            int buttonWidth = screenWidth / 7;
            int buttonHeight = screenHeight / 8;
            int X = 40;
            int Y = 40;
            backButton = new Sprite(new Point(X, Y), buttonWidth, buttonHeight);
        }

        public void loadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture = content.Load<Texture2D>("Backgrounds/instrs");
            backButton.loadContent(content, "Buttons/back");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenRectangle, Color.White);
            backButton.draw(spriteBatch);
        }

        public void onLeftClick(Point point)
        {
            if (backButton.contains(point))
                onBackButtonPress();
        }

        public void onBackButtonPress()
        {
            Program.game.startMainMenu();
        }
    }
}