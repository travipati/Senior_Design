using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class DoorSwitch
    {
        Color color;
        Color doorColor;
        Rectangle dswitch;
        List<Rectangle> doors;
        Texture2D switchTexture;
        Texture2D doorTexture;

        static Color[] colorArray = new Color[] { Color.Orange, Color.Purple, Color.Cyan, Color.Silver, Color.Crimson, Color.Pink, Color.Maroon, Color.Lime };
        static int curColorIndex = 0;

        public DoorSwitch(Rectangle rec)
        {
            dswitch = rec;
            doors = new List<Rectangle>();
            color = doorColor = colorArray[curColorIndex++];
            curColorIndex %= colorArray.Length;
        }

        public void addDoor(Rectangle door)
        {
            doors.Add(door);
        }

        public void loadContent(GraphicsDevice graphicsDevice)
        {
            doorTexture = new Texture2D(graphicsDevice, 1, 1);
            doorTexture.SetData<Color>(new Color[] { Color.White });
            switchTexture = new Texture2D(graphicsDevice, 1, 1);
            switchTexture.SetData<Color>(new Color[] { Color.White });
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Rectangle rect in doors)
                spriteBatch.Draw(doorTexture, rect, doorColor);
            spriteBatch.Draw(switchTexture, dswitch, color);
        }

        public void update(List<Ball> balls, ref List<Rectangle> walls)
        {
            if (balls[0].overlaps(dswitch) || balls[1].overlaps(dswitch))
            {
                doorColor = Color.Transparent;
                foreach (Rectangle rect in doors)
                    walls.Remove(rect);
            }
            else
            {
                doorColor = color;
                foreach (Rectangle rect in doors)
                {
                    if (!walls.Contains(rect))
                        walls.Add(rect);
                }
            }
        }
    }
}
