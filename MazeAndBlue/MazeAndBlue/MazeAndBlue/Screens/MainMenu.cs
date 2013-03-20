﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class MainMenu
    {
        Texture2D texture;
        Sprite singlePlayerButton, coopModeButton, settingsButton, instructionsButton,
             statisticsButton, exitButton;
        Rectangle screenRectangle;

        public MainMenu()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
           
            int buttonWidth = screenWidth / 7;
            int buttonHeight = screenHeight / 8;
            
            int Y1 = screenRectangle.Bottom - screenRectangle.Height / 3 - 45;
            int Y2 = screenRectangle.Bottom - screenRectangle.Height / 6 - 30;
            int X1 = screenRectangle.Left + screenRectangle.Width / 7;
            int X2 = screenRectangle.Left + 3 * screenRectangle.Width / 7;
            int X3 = screenRectangle.Left + 5 * screenRectangle.Width / 7;
            
            singlePlayerButton = new Sprite(new Vector2(X1, Y1), buttonWidth, buttonHeight);
            coopModeButton = new Sprite(new Vector2(X2, Y1), buttonWidth, buttonHeight);
            settingsButton = new Sprite(new Vector2(X3, Y1), buttonWidth, buttonHeight);
            instructionsButton = new Sprite(new Vector2(X1, Y2), buttonWidth, buttonHeight);
            statisticsButton = new Sprite(new Vector2(X2, Y2), buttonWidth, buttonHeight);
            exitButton = new Sprite(new Vector2(X3, Y2), buttonWidth, buttonHeight);
        }

        public void loadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture = content.Load<Texture2D>("Backgrounds/mainMenu");
            
            singlePlayerButton.loadContent(content, "Buttons/singlePlayer");
            coopModeButton.loadContent(content, "Buttons/coopMode");
            settingsButton.loadContent(content, "Buttons/settings");
            instructionsButton.loadContent(content, "Buttons/instructions");
            statisticsButton.loadContent(content, "Buttons/statistics");
            exitButton.loadContent(content, "Buttons/exit");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenRectangle, Color.White);
            singlePlayerButton.draw(spriteBatch);
            coopModeButton.draw(spriteBatch);
            settingsButton.draw(spriteBatch);
            instructionsButton.draw(spriteBatch);
            statisticsButton.draw(spriteBatch);
            exitButton.draw(spriteBatch);
        }

        public void onLeftClick(Point point)
        {
            if (singlePlayerButton.contains(point))
                onSinglePlayerButtonPress();
            else if (coopModeButton.contains(point))
                onCoopModeButtonPress();
        }

        private void onCoopModeButtonPress()
        {
            Program.game.startLevelSelectionScreen();
        }

        private void onSinglePlayerButtonPress()
        {
            //Program.game.startLevelSelectionScreen();
        }
    }
}
