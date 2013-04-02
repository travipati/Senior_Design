using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class DoorSwitch
    {
        Color color;
        Color doorColor;
        List<Rectangle> switches;
        List<Rectangle> doors;
        bool permanent;
        int numRequired;
        Texture2D switchTexture;
        Texture2D doorTexture;

        static Color[] colorArray = new Color[] { Color.Orange, Color.Purple, Color.Cyan, Color.Silver, Color.Crimson, Color.Pink, Color.Maroon, Color.Lime };
        static int curColorIndex = 0;

        public DoorSwitch() : this(false, 1) { }

        public DoorSwitch(bool _permanent, int _numRequired)
        {
            permanent = _permanent;
            numRequired = _numRequired;
            switches = new List<Rectangle>();
            doors = new List<Rectangle>();
            color = doorColor = colorArray[curColorIndex++];
            curColorIndex %= colorArray.Length;
        }

        public void addSwitch(Rectangle door)
        {
            switches.Add(door);
        }

        public void addDoor(Rectangle door)
        {
            doors.Add(door);
        }

        public void loadContent()
        {
            doorTexture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            doorTexture.SetData<Color>(new Color[] { Color.White });
            switchTexture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            switchTexture.SetData<Color>(new Color[] { Color.White });
        }

        public void draw(SpriteBatch spriteBatch)
        {
            string text = string.Empty;
            if (numRequired > 1)
                text = "M";
            else if (permanent)
                text = "P";
            foreach (Rectangle rect in doors)
                spriteBatch.Draw(doorTexture, rect, doorColor);
            foreach (Rectangle rect in switches)
            {
                spriteBatch.Draw(switchTexture, rect, color);
                if (numRequired > 1 || permanent)
                {
                    Vector2 textSize = MazeAndBlue.font.MeasureString(text);
                    int x = (int)(rect.X + (rect.Width - textSize.X) / 2);
                    int y = (int)(rect.Y + (rect.Height - textSize.Y) / 2);
                    Vector2 textPos = new Vector2(x, y);
                    spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
                }
            }
        }

        public void update(List<Ball> balls, ref List<Rectangle> walls)
        {
            bool playsound = false;

            int count = 0;
            foreach (Rectangle dswitch in switches)
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
