using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class InstructionScreen
    {
        Texture2D texture;
        Rectangle screenRectangle;
        Button menuButton;

        public InstructionScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            int buttonWidth = screenWidth / 7;
            int buttonHeight = screenHeight / 8;
            int X = 40;
            int Y = 40;
            menuButton = new Button(new Point(X, Y), buttonWidth, buttonHeight, "Main Menu", "Buttons/mainMenuButton");
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture = Program.game.Content.Load<Texture2D>("Backgrounds/instrs");
            menuButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenRectangle, Color.White);
            menuButton.draw(spriteBatch);
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.startMainMenu();
        }
    }
}