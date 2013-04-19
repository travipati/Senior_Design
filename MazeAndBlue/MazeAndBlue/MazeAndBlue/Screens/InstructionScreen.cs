﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class InstructionScreen
    {
        Texture2D texture;
        Rectangle screenRectangle;
        Button menuButton, systemsButton, selectButton, gameButton, createButton;

        public InstructionScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            int buttonWidth = screenWidth / 7;
            int buttonHeight = screenHeight / 8;
            int X = 40;
            int Y1 = 40;
            int Y2 = screenHeight / 2 + 5 * buttonHeight / 2;
            menuButton = new Button(new Point(X, Y1), buttonWidth, buttonHeight, "Main Menu", "Buttons/mainMenuButton");
            systemsButton = new Button(new Point(screenWidth / 5 - buttonWidth / 2, Y2), buttonWidth, buttonHeight, "Systems Requirements", "Buttons/system");
            selectButton = new Button(new Point(2 * screenWidth / 5 - buttonWidth / 2, Y2), buttonWidth, buttonHeight, "Select Options", "Buttons/selectOptions");
            gameButton = new Button(new Point(3 * screenWidth / 5 - buttonWidth / 2, Y2), buttonWidth, buttonHeight, "Game Play", "Buttons/gamePlay");
            createButton = new Button(new Point(4 * screenWidth / 5 - buttonWidth / 2, Y2), buttonWidth, buttonHeight, "Creating Mazes", "Buttons/creatingMazes");
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture = Program.game.Content.Load<Texture2D>("Backgrounds/blue");
            menuButton.loadContent();
            systemsButton.loadContent();
            selectButton.loadContent();
            gameButton.loadContent();
            createButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenRectangle, Color.White);
            menuButton.draw(spriteBatch);
            systemsButton.draw(spriteBatch);
            selectButton.draw(spriteBatch);
            gameButton.draw(spriteBatch);
            createButton.draw(spriteBatch);
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.startMainMenu();
        }
    }
}

/*
system/technical requirements
 * operating system
 * screen resolution
ways to select
 * kinect hover
 * mouse
 * voice control
 * during game play swipe
goal of game in both/switches 
 * single is just to make it through
 * coop is to both make it through
 * minimize wall hits and time
 * cooperate with switches
 * normal with one key
 * multiple with two keys
 * permanent with one key but stays open???
create your own
 * menu lets play and delete your own mazes
 * can create any type by just selecting buttons
 * how/where it saves to?
 * unlimited number
*/