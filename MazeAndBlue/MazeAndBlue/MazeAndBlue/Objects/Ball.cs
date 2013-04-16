using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class Ball : Sprite
    {
        public int playerId { get; set; }
        public bool mouseSelected { get; set; }
        int prevId;
        Color color;

        public Ball(Point pos, Color c) : base(pos)
        {
            color = c;
            playerId = -1;
            prevId = -1;
            mouseSelected = false;
        }

        public void loadContent()
        {
            loadContent("ball");
        }

        public override void draw(SpriteBatch spriteBatch)
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
                    if (i != prevId && Program.game.players[i].visible && Program.game.players[i].overlaps(this))
                    {
                        playerId = i;
                        Program.game.players[playerId].visible = false;
                    }
                }
            }
            else if (Program.game.players[playerId].selecting())
            {
                prevId = playerId;
                Program.game.players[playerId].visible = true;
                playerId = -1;
            }

            if (prevId >= 0 && !Program.game.players[prevId].overlaps(this))
                prevId = -1;
        }

        public void moveBall(Point pos)
        {
            pos.X -= width / 2;
            pos.Y -= height / 2;
            position = pos;
        }
    }
}
