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

        /*public DoorSwitch(Rectangle rec, List<Rectangle> dwalls, Color col, ref List<Rectangle> walls)
        {
            dswitch = rec;
            doors = dwalls;
            color = col;
            doorColor = col;
            foreach (Rectangle door in dwalls)
                walls.Add(door);
        }*/

        public DoorSwitch(Rectangle rec, Color col)
        {
            dswitch = rec;
            color = col;
            doorColor = col;
            doors = new List<Rectangle>();
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

        public void update(Player player1, Player player2, ref List<Rectangle> walls)
        {
            if (player1.overlaps(dswitch) || player2.overlaps(dswitch))
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
