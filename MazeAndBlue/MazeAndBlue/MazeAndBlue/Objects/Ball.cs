using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class Ball : Sprite
    {
        public int playerId { get; set; }
        public bool mouseSelected { get; set; }
        public Color color { get; set; }
        int prevId;

        public Ball(Point pos) : this(pos, Color.WhiteSmoke) { }

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
            if (playerId >= 0 && !Program.game.players[playerId].overlaps(this))
            {
                if (playerId == 0)
                    Program.game.players[playerId].draw(spriteBatch, new Color(0, 0, 255, 25));
                else if (playerId == 1)
                    Program.game.players[playerId].draw(spriteBatch, new Color(255, 255, 0, 25));
                Program.game.players[playerId].drawProgressCircle(spriteBatch);
            }
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
                        color = Program.game.players[i].color;
                        Program.game.players[playerId].visible = false;
                    }
                }
            }
            else if (Program.game.players[playerId].selecting() || 
                Program.game.players[playerId].position == new Point(-40, -40))
            {
                prevId = playerId;
                Program.game.players[playerId].visible = true;
                playerId = -1;
                color = Color.WhiteSmoke;
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
