using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class SettingsScreen
    {
        Button backButton, plRHand, plLHand, p2RHand, p2LHand, roomQuiet, roomAver, roomLoud;
        List<Button> buttons;

        public SettingsScreen()
        {
            backButton = new Button(new Point(150, 0), 100, 40, "", "Buttons/back");
            plRHand = new Button(new Point(0, 60), 100, 40, "plRHand", "Buttons/button");
            plLHand = new Button(new Point(150, 60), 100, 40, "plLHand", "Buttons/button");
            p2RHand = new Button(new Point(0, 120), 100, 40, "p2RHand", "Buttons/button");
            p2LHand = new Button(new Point(150, 120), 100, 40, "p2LHand", "Buttons/button");
            roomQuiet = new Button(new Point(0, 180), 100, 40, "roomQuiet", "Buttons/button");
            roomAver = new Button(new Point(150, 180), 100, 40, "roomAver", "Buttons/button");
            roomLoud = new Button(new Point(300, 180), 100, 40, "roomLoud", "Buttons/button");

            buttons = new List<Button>();
            buttons.Add(backButton);
            buttons.Add(plRHand);
            buttons.Add(plLHand);
            buttons.Add(p2RHand);
            buttons.Add(p2LHand);
            buttons.Add(roomQuiet);
            buttons.Add(roomAver);
            buttons.Add(roomLoud);
        }

        public void loadContent()
        {
            foreach (Button button in buttons)
                button.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
                button.draw(spriteBatch);
        }

        public void update()
        {
            foreach (Button button in buttons)
                button.update();

            if (backButton.isSelected())
                Program.game.startMainMenu();
            else if (plRHand.isSelected())
                Program.game.players[0].rightHanded = true;
            else if (plLHand.isSelected())
                Program.game.players[0].rightHanded = false;
            else if (p2RHand.isSelected())
                Program.game.players[1].rightHanded = true;
            else if (p2LHand.isSelected())
                Program.game.players[1].rightHanded = false;
            else if (roomQuiet.isSelected())
                Program.game.vs.precision = 0.8;
            else if (roomAver.isSelected())
                Program.game.vs.precision = 0.6;
            else if (roomLoud.isSelected())
                Program.game.vs.precision = 0.4;
        }
    }
}