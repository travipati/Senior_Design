using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class DoorSwitch
    {
        Color color;
        Color doorColor;
        List<Sprite> switches;
        List<Rectangle> doors;
        bool permanent;
        int numRequired;
        Texture2D doorTexture;

        static Color[] colorArray = new Color[] { Color.Orange, Color.Lime, Color.Pink, Color.Crimson, Color.White, Color.Orchid, Color.SteelBlue, Color.Gray };
        static int curColorIndex = 0;

        public DoorSwitch() : this(false, 1) { }

        public DoorSwitch(bool _permanent, int _numRequired)
        {
            permanent = _permanent;
            numRequired = _numRequired;
            switches = new List<Sprite>();
            doors = new List<Rectangle>();
            color = doorColor = colorArray[curColorIndex++];
            curColorIndex %= colorArray.Length;
        }

        public void addSwitch(Sprite dswitch)
        {
            switches.Add(dswitch);
        }

        public void addDoor(Rectangle door)
        {
            doors.Add(door);
        }

        public void loadContent()
        {
            doorTexture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            doorTexture.SetData<Color>(new Color[] { Color.White });
            foreach (Sprite dswitch in switches)
            {
                if (numRequired > 1)
                    dswitch.loadContent("buttons/2keys");
                else if (permanent)
                    dswitch.loadContent("buttons/lockBnW");
                else
                    dswitch.loadContent("buttons/1key");
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Rectangle rect in doors)
                spriteBatch.Draw(doorTexture, rect, doorColor);
            foreach (Sprite dswitch in switches)
                dswitch.draw(spriteBatch, color);
        }

        public void update(List<Ball> balls, ref List<Rectangle> walls)
        {
            bool playsound = false;

            int count = 0;
            foreach (Sprite dswitch in switches)
            {
                foreach (Ball ball in balls)
                {
                    if (ball.overlaps(dswitch))
                        count++;
                }
            }

            if (count >= numRequired)
            {
                doorColor = Color.Transparent;
                foreach (Rectangle rect in doors)
                {
                    if (walls.Contains(rect))
                    {
                        walls.Remove(rect);
                        playsound = true;
                    }
                }
            }
            else if (!permanent)
            {
                doorColor = color;
                foreach (Rectangle rect in doors)
                {
                    if (!walls.Contains(rect))
                    {
                        walls.Add(rect);
                        playsound = true;
                    }
                }
            }

            if (playsound)
                Program.game.soundEffectPlayer.playDoor();
        }
    }
}
