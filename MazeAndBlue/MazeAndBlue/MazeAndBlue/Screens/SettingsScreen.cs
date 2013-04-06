using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    
    public class SettingsScreen
    {
        Texture2D texture;
        Button menuButton, plRHand, plLHand, p2RHand, p2LHand, roomQuiet, roomAver, roomLoud, 
                soundsOn, soundsOff, setBackground, setGoalImage;
        List<Button> buttons;
        Rectangle screenRectangle;

        public SettingsScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            int smallButtonWidth = 136;
            int smallButtonHeight = 72;
            int largeButtonWidth = 170;
            int largeButtonHeight = 92;

            menuButton = new Button(new Point(screenWidth / 8, 50), smallButtonWidth, smallButtonHeight, "Main Menu", "Buttons/mainMenuButton");
            plLHand = new Button(new Point(screenWidth / 2 + smallButtonWidth, 170), smallButtonWidth, smallButtonHeight, "player one left", "Buttons/left");
            plRHand = new Button(new Point(screenWidth / 2 + 3 * smallButtonWidth, 170), smallButtonWidth, smallButtonHeight, "player one right", "Buttons/right");
            p2LHand = new Button(new Point(screenWidth / 2 + smallButtonWidth, 275), smallButtonWidth, smallButtonHeight, "player two left", "Buttons/left");
            p2RHand = new Button(new Point(screenWidth / 2 + 3 * smallButtonWidth, 275), smallButtonWidth, smallButtonHeight, "player two right", "Buttons/right");
            roomQuiet = new Button(new Point(screenWidth / 2 - smallButtonWidth, 380), smallButtonWidth, smallButtonHeight, "room low", "Buttons/low");
            roomAver = new Button(new Point(screenWidth / 2 + smallButtonWidth, 380), smallButtonWidth, smallButtonHeight, "room medium", "Buttons/medium");
            roomLoud = new Button(new Point(screenWidth / 2 + 3 * smallButtonWidth, 380), smallButtonWidth, smallButtonHeight, "room high", "Buttons/high");
            soundsOn = new Button(new Point(screenWidth / 2 + smallButtonWidth, 485), smallButtonWidth, smallButtonHeight, "sounds on", "Buttons/on");
            soundsOff = new Button(new Point(screenWidth / 2 + 3 * smallButtonWidth, 485), smallButtonWidth, smallButtonHeight, "sounds off", "Buttons/off");
            setBackground = new Button(new Point(screenWidth / 2 + largeButtonWidth, screenHeight - largeButtonHeight - 40), 
                largeButtonWidth, largeButtonHeight, "setBackground", "Buttons/setBackground");
            setGoalImage = new Button(new Point(screenWidth / 2 - 2 * largeButtonWidth, screenHeight - largeButtonHeight - 40), 
                largeButtonWidth, largeButtonHeight, "setGoalImage", "Buttons/setGoalImage");

            buttons = new List<Button>();
            buttons.Add(menuButton);
            buttons.Add(plRHand);
            buttons.Add(plLHand);
            buttons.Add(p2RHand);
            buttons.Add(p2LHand);
            buttons.Add(roomQuiet);
            buttons.Add(roomAver);
            buttons.Add(roomLoud);
            buttons.Add(soundsOn);
            buttons.Add(soundsOff);
//            buttons.Add(setBackground);
//            buttons.Add(setGoalImage);
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture = Program.game.Content.Load<Texture2D>("Backgrounds/settingsScreen");
            
            foreach (Button button in buttons)
                button.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenRectangle, Color.White);
            string text1 = "Player One Primary Hand";
            string text2 = "Player Two Primary Hand";
            string text3 = "Room Volume";
            string text4 = "Game Sounds";
            int x = 150;
            int y1 = 180;
            int y2 = 285;
            int y3 = 390;
            int y4 = 495;
            Vector2 text1Pos = new Vector2(x, y1);
            Vector2 text2Pos = new Vector2(x, y2);
            Vector2 text3Pos = new Vector2(x, y3);
            Vector2 text4Pos = new Vector2(x, y4);
            spriteBatch.DrawString(MazeAndBlue.font, text1, text1Pos, Color.Black);
            spriteBatch.DrawString(MazeAndBlue.font, text2, text2Pos, Color.Black); 
            spriteBatch.DrawString(MazeAndBlue.font, text3, text3Pos, Color.Black); 
            spriteBatch.DrawString(MazeAndBlue.font, text4, text4Pos, Color.Black);

            if (Program.game.players[0].rightHanded)
            {
                plRHand.selected = true;
                plLHand.selected = false;
            }
            else
            {
                plRHand.selected = false;
                plLHand.selected = true;
            }

            if (Program.game.players[1].rightHanded)
            {
                p2RHand.selected = true;
                p2LHand.selected = false;
            }
            else
            {
                p2RHand.selected = false;
                p2LHand.selected = true;
            }

            if (Program.game.vs.precision == 0.6)
            {
                roomQuiet.selected = true;
                roomAver.selected = false;
                roomLoud.selected = false;
            }
            else if (Program.game.vs.precision == 0.5)
            {
                roomQuiet.selected = false;
                roomAver.selected = true;
                roomLoud.selected = false;
            }
            else
            {
                roomQuiet.selected = false;
                roomAver.selected = false;
                roomLoud.selected = true;
            }

            if(Program.game.soundEffectPlayer.soundsOn)
            {
                soundsOn.selected = true;
                soundsOff.selected = false;
            }
            else
            {
                soundsOn.selected = false;
                soundsOff.selected = true;
            }

            foreach (Button button in buttons)
                button.draw(spriteBatch);
        }

        public void update()
        {
            foreach (Button button in buttons)
                button.update();

            if (menuButton.isSelected())
                Program.game.startMainMenu();
            else if (plRHand.isSelected())
            {
                Program.game.settings.updateP1PrimaryHand(1);
                Program.game.players[0].switchHand(true);
            }
            else if (plLHand.isSelected())
            {
                Program.game.settings.updateP1PrimaryHand(0);
                Program.game.players[0].switchHand(false);
            }
            else if (p2RHand.isSelected())
            {
                Program.game.settings.updateP2PrimaryHand(1);
                Program.game.players[1].switchHand(true);
            }
            else if (p2LHand.isSelected())
            {
                Program.game.settings.updateP2PrimaryHand(0);
                Program.game.players[1].switchHand(false);
            }
            else if (roomQuiet.isSelected())
            {
                Program.game.settings.updateVolume(0);
                Program.game.vs.precision = 0.6;
            }
            else if (roomAver.isSelected())
            {
                Program.game.settings.updateVolume(1);
                Program.game.vs.precision = 0.5;
            }
            else if (roomLoud.isSelected())
            {
                Program.game.settings.updateVolume(2);
                Program.game.vs.precision = 0.4;
            }
            else if (soundsOn.isSelected())
            {
                Program.game.settings.updateSound(1);
                Program.game.soundEffectPlayer.soundsOn = true;
            }
            else if (soundsOff.isSelected())
            {
                Program.game.settings.updateSound(0);
                Program.game.soundEffectPlayer.soundsOn = false;
            }
            Program.game.settings.saveStats();
        }
    }
}
