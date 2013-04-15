using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class CreateMazeSelection 
    {
        Button createMazeButton, singleButton, coOpButton, easyButton, hardButton, next, previous;
        List<Button> levelButtons;
        List<Button> buttons;
        bool singlePlayer, easy;

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

            createMazeButton = new Button
                (new Point(10, 10), 200, 100, "Create", "Buttons/button");
            singleButton = new Button
                (new Point(250, 10), 200, 100, "Single Player", "Buttons/button");
            coOpButton = new Button
                (new Point(500, 10), 200, 100, "Co Op", "Buttons/button");
            easyButton = new Button
                (new Point(250, 150), 200, 100, "easy", "Buttons/easy");
            hardButton = new Button
                (new Point(500, 150), 200, 100, "hard", "Buttons/hard");

            levelButtons = new List<Button>();
            int nameId = 0;
            string filename = "Mazes/temp" + nameId + ".maze";
            while (File.Exists(filename))
            {
                levelButtons.Add(new Button(new Point(levelx[nameId % 3], levely[nameId / 3]), levelButtonWidth, levelButtonHeight, nameId.ToString(), "star"));
                nameId++;
                filename = "Mazes/temp" + nameId + ".maze";
            }

            buttons = new List<Button>();
            buttons.Add(createMazeButton);
            buttons.Add(singleButton);
            buttons.Add(coOpButton);
            buttons.Add(easyButton);
            buttons.Add(hardButton);
            foreach (Button levelButton in levelButtons)
                buttons.Add(levelButton);
        }

        public void loadContent()
        {
            foreach (Button button in buttons)
                button.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            singleButton.selected = singlePlayer;
            coOpButton.selected = !singlePlayer;
            easyButton.selected = easy;
            hardButton.selected = !easy;

            foreach (Button button in buttons)
                button.draw(spriteBatch);
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
            for (int i = 0; i < levelButtons.Count; i++)
            {
                if (levelButtons[i].selectable && levelButtons[i].isSelected())
                    Program.game.startCustomLevel(i);
            }
        }
    }
}
