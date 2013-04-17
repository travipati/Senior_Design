using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class CreateMazeSelection 
    {
        Button createMazeButton, singleButton, coOpButton, easyButton, hardButton, nextButton, prevButton,
            playButton, deleteButton, menuButton;
        List<Button> levelButtons;
        List<Button> buttons;
        bool singlePlayer, easy, play;
        int page;

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
            page = 0;

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
                (new Point(300, 150), 200, 100, "Play", "Buttons/button");
            deleteButton = new Button
                (new Point(600, 150), 200, 100, "Delete", "Buttons/button");
            menuButton = new Button
                (new Point(Program.game.screenWidth - 166, 30), 136, 72, "Main Menu", "Buttons/mainMenuButton");

            int i = 0;
            levelButtons = new List<Button>();
            string[] files = Directory.GetFiles(@"Mazes\", "temp*.maze");
            foreach (string file in files)
            {
                string nameId = file.Substring(10, file.IndexOf(".") - 10);
                string imageName = "test" + nameId + ".png";
                if (File.Exists(imageName))
                {
                    levelButtons.Add(new Button(new Point(levelx[i % 3], levely[(i / 3) % 2]), levelButtonWidth, levelButtonHeight, nameId.ToString(), imageName, true));
                    i++;
                }
            }

            nextButton = new Button
                (new Point(screenWidth - 220, screenHeight - 120), 200, 100, "Next", "Buttons/button");
            prevButton = new Button
                (new Point(20, screenHeight - 120), 200, 100, "Previous", "Buttons/button");

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
            foreach (Button button in buttons)
                button.loadContent();
            nextButton.loadContent();
            prevButton.loadContent();
            foreach (Button levelButton in levelButtons)
                levelButton.loadContent();
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
        }

        public void update()
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
            else if (mainMenuButton.isSelected())
                Program.game.startMainMenu();
            for (int i = 0; i < levelButtons.Count; i++)
            {
                if (play && levelButtons[i].selectable && levelButtons[i].isSelected())
                    Program.game.startCustomLevel(i);
                if (!play && levelButtons[i].selectable && levelButtons[i].isSelected())
                {
                    string imageName = levelButtons[i].path;
                    string nameId = imageName.Substring(4, imageName.IndexOf(".") - 4);
                    string mazeName = "Mazes\\temp" + nameId + ".maze";
                    System.Console.WriteLine(mazeName);
                    //levelButtons[i].unLoadContent();
                    levelButtons.Remove(levelButtons[i]);
                    File.Delete(mazeName);
                    Program.game.deleteList.Add(imageName);
                }
            }
        }
    }
}
