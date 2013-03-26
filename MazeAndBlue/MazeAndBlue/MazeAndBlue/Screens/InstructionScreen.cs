using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class InstructionScreen
    {
        Texture2D texture;
        Rectangle screenRectangle;
        Button backButton;

        public InstructionScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            int buttonWidth = screenWidth / 7;
            int buttonHeight = screenHeight / 8;
            int X = 40;
            int Y = 40;
            backButton = new Button(new Point(X, Y), buttonWidth, buttonHeight, "", "Buttons/back");
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture = Program.game.Content.Load<Texture2D>("Backgrounds/instrs");
            backButton.loadContent("Buttons/back");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenRectangle, Color.White);
            backButton.draw(spriteBatch);
        }

        public void update()
        {
            if (backButton.isSelected())
                Program.game.startMainMenu();
        }
    }
}