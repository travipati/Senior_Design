using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
                (new Point(X2, Y2), buttonWidth, buttonHeight, "statistics", "Buttons/statistics");
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

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture = Program.game.Content.Load<Texture2D>("Backgrounds/mainMenu");
            
            foreach (Button button in buttons)
                button.loadContent();
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

            if (singlePlayerButton.isSelected())
                System.Windows.Forms.MessageBox.Show("Single Player Not Yet Implemented");
            else if (coopModeButton.isSelected())
                Program.game.startLevelSelectionScreen(false);
            else if (settingsButton.isSelected())
                Program.game.startSettingsScreen();
            else if (instructionsButton.isSelected())
                Program.game.startInstructionScreen();
            else if (statisticsButton.isSelected())
                Program.game.startStatsScreen();
            else if (exitButton.isSelected())
                Program.game.Exit();
        }

    }
}
