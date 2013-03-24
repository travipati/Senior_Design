using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeAndBlue
{
    public class Ball : Sprite
    {
        public int playerId { get; set; }
        public bool mouseSelected { get; set; }
        Color color;

        public Ball(Point pos, Color c)
        {
            position = pos;
            color = c;
            playerId = -1;
            mouseSelected = false;
        }

        public void loadContent(ContentManager content)
        {
            loadContent(content, "ball");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            draw(spriteBatch, color);
        }

        public void select()
        {
            if (Program.game.ms.newPointReady && contains(Program.game.ms.point))
            {
                Program.game.ms.newPointReady = false;
                mouseSelected = !mouseSelected;
            }

            if (playerId < 0)
            {
                for (int i = 0; i < Program.game.players.Count; i++)
                {
                    if (Program.game.players[i].overlaps(this) && Program.game.players[i].selecting())
                        playerId = i;
                }
            }
            else if (Program.game.players[playerId].selecting())
                playerId = -1;
        }

        public void moveBall(Point pos)
        {
            pos.X -= width / 2;
            pos.Y -= height / 2;
            position = pos;
        }
    }
}
