using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using System.IO;

namespace MazeAndBlue
{
    
    public class SettingsScreen
    {
        Texture2D texture;
        Rectangle screenRectangle;
        Button menuButton, plRHand, plLHand, p2RHand, p2LHand, roomQuiet, roomAver, roomLoud, soundsOn, soundsOff,
            unlockOn, unlockOff, setGoalImage, deleteGoalImage, calibrateKinect;
        List<Button> buttons;
        Sprite redX;
        bool drawImage;

        public SettingsScreen()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            int smallButtonWidth = 136;
            int smallButtonHeight = 72;
            int largeButtonWidth = 170;
            int largeButtonHeight = 92;

            menuButton = new Button(new Point(screenWidth / 8, 50), smallButtonWidth,
                smallButtonHeight, "Main Menu", "Buttons/mainMenuButton");
            plLHand = new Button(new Point(screenWidth / 2 + smallButtonWidth, 150),
                smallButtonWidth, smallButtonHeight, "player one left", "Buttons/left");
            plRHand = new Button(new Point(screenWidth / 2 + 3 * smallButtonWidth, 150),
                smallButtonWidth, smallButtonHeight, "player one right", "Buttons/right");
            p2LHand = new Button(new Point(screenWidth / 2 + smallButtonWidth, 245),
                smallButtonWidth, smallButtonHeight, "player two left", "Buttons/left");
            p2RHand = new Button(new Point(screenWidth / 2 + 3 * smallButtonWidth, 245),
                smallButtonWidth, smallButtonHeight, "player two right", "Buttons/right");
            roomQuiet = new Button(new Point(screenWidth / 2 - smallButtonWidth, 340),
                smallButtonWidth, smallButtonHeight, "room low", "Buttons/low");
            roomAver = new Button(new Point(screenWidth / 2 + smallButtonWidth, 340),
                smallButtonWidth, smallButtonHeight, "room medium", "Buttons/medium");
            roomLoud = new Button(new Point(screenWidth / 2 + 3 * smallButtonWidth, 340),
                smallButtonWidth, smallButtonHeight, "room high", "Buttons/high");
            soundsOn = new Button(new Point(screenWidth / 2 + smallButtonWidth, 435),
                smallButtonWidth, smallButtonHeight, "sounds on", "Buttons/on");
            soundsOff = new Button(new Point(screenWidth / 2 + 3 * smallButtonWidth, 435),
                smallButtonWidth, smallButtonHeight, "sounds off", "Buttons/off");
            unlockOn = new Button(new Point(screenWidth / 2 + smallButtonWidth, 530),
                smallButtonWidth, smallButtonHeight, "unlock on", "Buttons/on");
            unlockOff = new Button(new Point(screenWidth / 2 + 3 * smallButtonWidth, 530),
                smallButtonWidth, smallButtonHeight, "unlock off", "Buttons/off");
            setGoalImage = new Button(new Point(screenWidth / 2 - 2 * largeButtonWidth, screenHeight - largeButtonHeight - 20), 
                largeButtonWidth, largeButtonHeight, "set goal image", "Buttons/setGoalImage");
            calibrateKinect = new Button(new Point(screenWidth / 2 + largeButtonWidth, screenHeight - largeButtonHeight - 20),
                largeButtonWidth, largeButtonHeight, "calibrate kinect", "Buttons/calibrateKinect");
            deleteGoalImage = new Button(new Point(screenWidth / 2 - largeButtonWidth + 10, screenHeight - largeButtonHeight - 20),
                largeButtonHeight, largeButtonHeight, "delete goal image", "", true);

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
            buttons.Add(unlockOn);
            buttons.Add(unlockOff);
            buttons.Add(setGoalImage);
            buttons.Add(calibrateKinect);

            redX = new Sprite(new Point(screenWidth / 2 - largeButtonWidth + 10, screenHeight - largeButtonHeight - 20),
                largeButtonHeight, largeButtonHeight);
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture = Program.game.Content.Load<Texture2D>("Backgrounds/settingsScreen");
            
            foreach (Button button in buttons)
                button.loadContent();

            drawImage = false;
            string goalImage = Program.game.goalImage;
            if (goalImage != "null" && File.Exists(goalImage))
            {
                deleteGoalImage.path = goalImage;
                deleteGoalImage.loadContent();
                drawImage = true;
            }

            redX.loadContent("redX");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenRectangle, Color.White);
            string text1 = "Player One Primary Hand";
            string text2 = "Player Two Primary Hand";
            string text3 = "Voice Sensitivity";
            string text4 = "Game Sounds";
            string text5 = "Unlock Levels";
            int x = 150;
            int y1 = 160;
            int y2 = 255;
            int y3 = 350;
            int y4 = 445;
            int y5 = 540;
            Vector2 text1Pos = new Vector2(x, y1);
            Vector2 text2Pos = new Vector2(x, y2);
            Vector2 text3Pos = new Vector2(x, y3);
            Vector2 text4Pos = new Vector2(x, y4);
            Vector2 text5Pos = new Vector2(x, y5);
            spriteBatch.DrawString(MazeAndBlue.font, text1, text1Pos, Color.Black);
            spriteBatch.DrawString(MazeAndBlue.font, text2, text2Pos, Color.Black); 
            spriteBatch.DrawString(MazeAndBlue.font, text3, text3Pos, Color.Black); 
            spriteBatch.DrawString(MazeAndBlue.font, text4, text4Pos, Color.Black);
            spriteBatch.DrawString(MazeAndBlue.font, text5, text5Pos, Color.Black);

            plRHand.selected = Program.game.players[0].rightHanded;
            plLHand.selected = !plRHand.selected;

            p2RHand.selected = Program.game.players[1].rightHanded;
            p2LHand.selected = !p2RHand.selected;

            roomQuiet.selected = false;
            roomAver.selected = false;
            roomLoud.selected = false;

            switch (Program.game.settings.getVolume())
            {
                case 0:
                    roomQuiet.selected = true;
                    break;
                case 1:
                    roomAver.selected = true;
                    break;
                case 2:
                    roomLoud.selected = true;
                    break;
            }

            soundsOn.selected = Program.game.soundEffectPlayer.soundsOn;
            soundsOff.selected = !soundsOn.selected;

            unlockOn.selected = Program.game.unlockOn;
            unlockOff.selected = !unlockOn.selected;
           
            foreach (Button button in buttons)
                button.draw(spriteBatch);

            if (drawImage)
            {
                deleteGoalImage.draw(spriteBatch);
                if (deleteGoalImage.contains(Program.game.ms.point))
                    redX.draw(spriteBatch);
            }
        }

        public void update()
        {
            if (menuButton.isSelected())
                Program.game.startMainMenu();
            else if (plRHand.isSelected())
                Program.game.settings.updateP1PrimaryHand(true);
            else if (plLHand.isSelected())
                Program.game.settings.updateP1PrimaryHand(false);
            else if (p2RHand.isSelected())
                Program.game.settings.updateP2PrimaryHand(true);
            else if (p2LHand.isSelected())
                Program.game.settings.updateP2PrimaryHand(false);
            else if (roomQuiet.isSelected())
                Program.game.settings.updateVolume(2);
            else if (roomAver.isSelected())
                Program.game.settings.updateVolume(1);
            else if (roomLoud.isSelected())
                Program.game.settings.updateVolume(0);
            else if (soundsOn.isSelected())
                Program.game.settings.updateSound(true);
            else if (soundsOff.isSelected())
                Program.game.settings.updateSound(false);
            else if (unlockOn.isSelected())
                Program.game.settings.updateUnlock(true);
            else if (unlockOff.isSelected())
                Program.game.settings.updateUnlock(false);
            else if (calibrateKinect.isSelected())
                Program.game.startCalibrationScreen();
            else if (setGoalImage.isSelected())
                setImage();
            else if (deleteGoalImage.isSelected())
                deleteImage();
        }

        void deleteImage()
        {
            drawImage = false;
            Program.game.settings.updateGoalFile("null");
        }

        void setImage()
        {
            bool wasFullScreen = Program.game.isFullScreen();
            
            if (wasFullScreen)
                Program.game.toggleFullScreen();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Supported files (GIF, JPEG, PNG)|*.GIF;*.JPG;*.PNG;*.JPEG";
            ofd.CheckFileExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string file = ofd.FileName;
                string ext = Path.GetExtension(file).ToUpper();
                if (ext == ".GIF" || ext == ".JPG" || ext == ".PNG" || ext == ".JPEG")
                {
                    deleteGoalImage.path = file;
                    deleteGoalImage.loadContent();
                    drawImage = true;
                }
                else
                {
                    file = "null";
                    drawImage = false;
                }

                Program.game.settings.updateGoalFile(file);
            }

            if (wasFullScreen)
                Program.game.toggleFullScreen();
        }

    }
}
