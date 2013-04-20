using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class MainMenu
    {
        Texture2D texture;
        Button singlePlayerButton, coopModeButton, createMazeButton, instructionsButton,
             statisticsButton, settingsButton, exitButton;
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
                (new Point(X1, Y1), buttonWidth, buttonHeight, "single player", "Buttons/singlePlayer");
            coopModeButton = new Button
                (new Point(X2, Y1), buttonWidth, buttonHeight, "co op mode", "Buttons/coopMode");
            createMazeButton = new Button
                (new Point(X3, Y1), buttonWidth, buttonHeight, "create maze", "Buttons/createMaze");
            instructionsButton = new Button
                (new Point(X1, Y2), buttonWidth, buttonHeight, "instructions", "Buttons/instructions");
            statisticsButton = new Button
                (new Point(X2, Y2), buttonWidth, buttonHeight, "statistics", "Buttons/statistics");
            settingsButton = new Button
                (new Point(X3, Y2), buttonWidth, buttonHeight, "settings", "Buttons/settings");
            exitButton = new Button
                (new Point(Program.game.screenWidth - 170, 30), 136, 72, "exit", "Buttons/exit");

            buttons = new List<Button>();
            buttons.Add(singlePlayerButton);
            buttons.Add(coopModeButton);
            buttons.Add(createMazeButton);
            buttons.Add(instructionsButton);
            buttons.Add(statisticsButton);
            buttons.Add(settingsButton);
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
            foreach (Button button in buttons)
                button.draw(spriteBatch);
        }

        public void update()
        {
            if (singlePlayerButton.isSelected())
                Program.game.startLevelSelectionScreen(true);
            else if (coopModeButton.isSelected())
                Program.game.startLevelSelectionScreen(false);
            else if (createMazeButton.isSelected())
                Program.game.startCreateMazeSelect();
            else if (instructionsButton.isSelected())
                Program.game.startInstructionScreen();
            else if (statisticsButton.isSelected())
                Program.game.startStatsScreen();
            else if (settingsButton.isSelected())
                Program.game.startSettingsScreen();
            else if (exitButton.isSelected())
                Program.game.Exit();
         }
    }
}
