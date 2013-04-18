using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class CreateMazeSelection 
    {
        Rectangle confWindow;
        Texture2D confTexture;
        Button createMazeButton, singleButton, coOpButton, easyButton, hardButton, nextButton, prevButton,
            playButton, deleteButton, menuButton, yesButton, noButton;
        List<Button> levelButtons;
        List<Button> buttons;
        bool singlePlayer, easy, play, conformation;
        int page, delLevel;

        public CreateMazeSelection()
        {
            int screenWidth = Program.game.screenWidth;
            int screenHeight = Program.game.screenHeight;
            int levelButtonWidth = screenWidth / 8;
            int levelButtonHeight = screenHeight / 8;
            List<int> levely = new List<int>();
            levely.Add(screenHeight / 2 - 10);
            levely.Add(2 * screenHeight / 3 + 40);
            List<int> levelx = new List<int>();
            levelx.Add(screenWidth / 2 - 5 * levelButtonWidth / 2);
            levelx.Add(screenWidth / 2 - levelButtonWidth / 2);
            levelx.Add(screenWidth / 2 + 3 * levelButtonWidth / 2);

            singlePlayer = true;
            easy = true;
            play = true;
            conformation = false;
            page = 0;

            confWindow = new Rectangle(3 * screenWidth / 8, 3 * screenHeight / 8, screenWidth / 4, screenHeight / 4);

            createMazeButton = new Button
                (new Point(20, 20), 200, 100, "create maze", "Buttons/createMaze");
            singleButton = new Button
                (new Point(260, 20), 200, 100, "Single Player", "Buttons/singlePlayer");
            coOpButton = new Button
                (new Point(490, 20), 200, 100, "Co Op Mode", "Buttons/coopMode");
            easyButton = new Button
                (new Point(720, 20), 200, 100, "easy", "Buttons/easy");
            hardButton = new Button
                (new Point(950, 20), 200, 100, "hard", "Buttons/hard");
            playButton = new Button
                (new Point(300, 150), 200, 100, "Play", "Buttons/play");
            deleteButton = new Button
                (new Point(600, 150), 200, 100, "Delete", "Buttons/delete");
            menuButton = new Button
                (new Point(Program.game.screenWidth - 166, 30), 136, 72, "Main Menu", "Buttons/mainMenuButton");
            nextButton = new Button
                (new Point(screenWidth - 220, screenHeight - 120), 200, 100, "Next", "Buttons/next");
            prevButton = new Button
                (new Point(20, screenHeight - 120), 200, 100, "Previous", "Buttons/previous");
            yesButton = new Button(new Point(screenWidth / 2 - 120, confWindow.Bottom - 100), 80, 50, "Yes", "Buttons/yes");
            noButton = new Button(new Point(screenWidth / 2 + 40, confWindow.Bottom - 100), 80, 50, "No", "Buttons/no");

            int i = 0;
            levelButtons = new List<Button>();
            string[] files = Directory.GetFiles(@"Mazes\", "custom*.maze");
            foreach (string file in files)
            {
                string nameId = file.Substring(12, file.IndexOf(".") - 12);
                string imageName = "custom" + nameId + ".png";
                if (File.Exists(imageName))
                {
                    levelButtons.Add(new Button(new Point(levelx[i % 3], levely[(i / 3) % 2]), levelButtonWidth, levelButtonHeight, nameId.ToString(), imageName, true));
                    i++;
                }
            }

            Program.game.customStats.data.numCustomLevels = levelButtons.Count;

            buttons = new List<Button>();
            buttons.Add(createMazeButton);
            buttons.Add(singleButton);
            buttons.Add(coOpButton);
            buttons.Add(easyButton);
            buttons.Add(hardButton);
            buttons.Add(playButton);
            buttons.Add(deleteButton);
            buttons.Add(menuButton);
        }

        public void loadContent()
        {
            confTexture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            confTexture.SetData<Color>(new Color[] { Color.White });

            foreach (Button button in buttons)
                button.loadContent();
            nextButton.loadContent();
            prevButton.loadContent();
            foreach (Button levelButton in levelButtons)
                levelButton.loadContent();
            yesButton.loadContent();
            noButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            singleButton.selected = singlePlayer;
            coOpButton.selected = !singlePlayer;
            easyButton.selected = easy;
            hardButton.selected = !easy;
            playButton.selected = play;
            deleteButton.selected = !play;

            foreach (Button button in buttons)
                button.draw(spriteBatch);
            if (levelButtons.Count <= (page + 1) * 6)
            {
                for (int i = page * 6; i < levelButtons.Count; i++)
                    levelButtons[i].draw(spriteBatch);
            }
            else
            {
                for (int i = page * 6; i < (page + 1) * 6; i++)
                    levelButtons[i].draw(spriteBatch);                  
                nextButton.draw(spriteBatch);
            }
            if (page > 0)
                prevButton.draw(spriteBatch);

            if (conformation)
            {
                spriteBatch.Draw(confTexture, confWindow, new Color(128, 128, 128, 200));
                Program.game.drawText("Delete maze?", new Point(Program.game.screenWidth / 2, confWindow.Top + 55));
                yesButton.draw(spriteBatch);
                noButton.draw(spriteBatch);
            }
        }

        public void update()
        {
            if (!conformation)
            {
                if (createMazeButton.isSelected())
                    Program.game.startCreateMaze(!easy, singlePlayer);
                else if (singleButton.isSelected())
                    singlePlayer = true;
                else if (coOpButton.isSelected())
                    singlePlayer = false;
                else if (easyButton.isSelected())
                    easy = true;
                else if (hardButton.isSelected())
                    easy = false;
                else if (playButton.isSelected())
                    play = true;
                else if (deleteButton.isSelected())
                    play = false;
                else if (levelButtons.Count > (page + 1) * 6 && nextButton.isSelected())
                    page++;
                else if (page > 0 && prevButton.isSelected())
                    page--;
                else if (menuButton.isSelected())
                    Program.game.startMainMenu();
                
								for (int i = 0; i < levelButtons.Count; i++)
                {
                    if (play && levelButtons[i].selectable && levelButtons[i].isSelected())
                        Program.game.startCustomLevel(Convert.ToInt32(levelButtons[i].path.Substring(6, levelButtons[i].path.IndexOf(".") - 6)));
                    if (!play && levelButtons[i].selectable && levelButtons[i].isSelected())
                    {
                        delLevel = i;
                        conformation = true;
                        foreach (Button button in buttons)
                            button.selectable = false;
                        nextButton.selectable = false;
                        prevButton.selectable = false;
                        foreach (Button levelButton in levelButtons)
                            levelButton.selectable = false;
                    }
                }
            }
            else
            {
                if (yesButton.isSelected())
                {
                    string imageName = levelButtons[delLevel].path;
                    string nameId = imageName.Substring(6, imageName.IndexOf(".") - 6);
                    string mazeName = "Mazes\\custom" + nameId + ".maze";
                    levelButtons.Remove(levelButtons[delLevel]);
                    File.Delete(mazeName);
                    File.Delete(imageName);
                    conformation = false;
                    Program.game.customStats.deleteLevelData(Convert.ToInt32(nameId));
                }
                if (noButton.isSelected())
                    conformation = false;

                if (!conformation)
                {
                    foreach (Button button in buttons)
                        button.selectable = true;
                    nextButton.selectable = true;
                    prevButton.selectable = true;
                    foreach (Button levelButton in levelButtons)
                        levelButton.selectable = true;
                }
            }
        }
    }
}
