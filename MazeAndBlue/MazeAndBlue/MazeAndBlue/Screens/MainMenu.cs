using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeAndBlue
{
    public class MainMenu
    {
        Texture2D texture;
        Button singlePlayerButton, coopModeButton, settingsButton, instructionsButton,
             statisticsButton, exitButton;
        List<Button> buttons;
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
            
            singlePlayerButton = new Button
                (new Point(X1, Y1), buttonWidth, buttonHeight, "single mode", "Buttons/singlePlayer");
            coopModeButton = new Button
                (new Point(X2, Y1), buttonWidth, buttonHeight, "co op mode", "Buttons/coopMode");
            settingsButton = new Button
                (new Point(X3, Y1), buttonWidth, buttonHeight, "settings", "Buttons/settings");
            instructionsButton = new Button
                (new Point(X1, Y2), buttonWidth, buttonHeight, "instructions", "Buttons/instructions");
            statisticsButton = new Button
                (new Point(X2, Y2), buttonWidth, buttonHeight, "statistics","Buttons/statistics");
            exitButton = new Button
                (new Point(X3, Y2), buttonWidth, buttonHeight, "exit","Buttons/exit");

            buttons = new List<Button>();
            buttons.Add(singlePlayerButton);
            buttons.Add(coopModeButton);
            buttons.Add(settingsButton);
            buttons.Add(instructionsButton);
            buttons.Add(statisticsButton);
            buttons.Add(exitButton);
        }

        public void loadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture = content.Load<Texture2D>("Backgrounds/mainMenu");
            
            foreach (Button button in buttons)
            {
                button.loadContent(content, button.path);
            }
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

        public void update()
        {
            foreach (Button button in buttons)
                button.update();
            
            if (coopModeButton.isSelected())
                Program.game.startLevelSelectionScreen();
            else if (instructionsButton.isSelected())
                Program.game.startInstructionScreen();
            else if (exitButton.isSelected())
                Program.game.Exit();
        }

/*        public void onLeftClick(Point point)
        {
            if (singlePlayerButton.contains(point))
                onSinglePlayerButtonPress();
//            else if (coopModeButton.contains(point))
//                onCoopModeButtonPress();
            else if (settingsButton.contains(point))
                onSettingsButtonPress();
            else if (statisticsButton.contains(point))
                onStatisticsButtonPress();
            else if (instructionsButton.contains(point))
                onInstructionButtonPress();
            else if (exitButton.contains(point))
                Program.game.Exit();
        }

        private void onSinglePlayerButtonPress()
        {
            //Program.game.startLevelSelectionScreen();
        }
        
        private void onCoopModeButtonPress()
        {
            Program.game.startLevelSelectionScreen();
        }

        private void onSettingsButtonPress()
        {
            //Program.game.startSettingsScreen();
        }

        private void onStatisticsButtonPress()
        {
            //Program.game.startStatisticsScreen();
        }

        private void onInstructionButtonPress()
        {
            Program.game.startInstructionScreen();
        }*/
    }
}
