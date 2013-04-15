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
        }
    }
}
