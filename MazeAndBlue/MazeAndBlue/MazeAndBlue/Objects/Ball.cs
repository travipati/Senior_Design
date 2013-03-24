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
    class Ball
    {
        Sprite ball;
        public int playerId { get; set; }
        public bool mouseSelected { get; set; }
        Color color;

        public Ball(Point pos, Color c)
        {
            ball = new Sprite();
            ball.position = pos;
            color = c;
            playerId = -1;
            mouseSelected = false;
        }

        public void loadContent(ContentManager content)
        {
            ball.loadContent(content, "ball");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            ball.draw(spriteBatch, color);
        }

        public void update(Maze maze)
        {
            if (Program.game.ms.newPointReady && ball.contains(Program.game.ms.point))
            {
                Program.game.ms.newPointReady = false;
                mouseSelected = !mouseSelected;
            }

            if (playerId < 0)
            {
                for (int i = 0; i < Program.game.players.Count; i++)
                {
                    if (Program.game.players[i].overlaps(ball) && Program.game.players[i].selecting())
                        playerId = i;
                }
            }
            else if (Program.game.players[playerId].selecting())
                playerId = -1;

            Point position;
            
            position = Program.game.ms.point;         
            if (playerId >= 0)
                position = Program.game.players[playerId].getPosition();

            if (playerId >= 0 || mouseSelected)
                moveBall(position, maze);
        }

        private void moveBall(Point pos, Maze maze)
        {
            pos.X -= ball.width / 2;
            pos.Y -= ball.height / 2;
            maze.detectWalls(ball, ref pos);
            ball.position = pos;
        }

        public bool overlaps(Sprite sprite)
        {
            return ball.overlaps(sprite);
        }

        public bool overlaps(Rectangle rect)
        {
            return ball.overlaps(rect);
        }
    }
}
