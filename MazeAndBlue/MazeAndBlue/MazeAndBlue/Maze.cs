using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class Maze
    {
        Color goalColor;
        Rectangle goal;
        List<Rectangle> walls;
        Texture2D wallTexture, goalTexture;
        Timer timer;
   
        public Maze()
        {
            timer = new Timer();
            goalColor = Color.Red;
            goal = new Rectangle(825, 65, 135, 105);
            walls = new List<Rectangle>();
            walls.Add(new Rectangle(50, 50, 925, 15));
            walls.Add(new Rectangle(50, 525, 925, 15));
            walls.Add(new Rectangle(50, 50, 15, 475));
            walls.Add(new Rectangle(960, 50, 15, 475));
            walls.Add(new Rectangle(200, 50, 15, 325));
            walls.Add(new Rectangle(350, 200, 15, 325));
            walls.Add(new Rectangle(810, 50, 15, 325));
            walls.Add(new Rectangle(500, 170, 310, 15));
            walls.Add(new Rectangle(500, 170, 15, 205));
            walls.Add(new Rectangle(650, 320, 15, 205));
        }

        public void loadContent(GraphicsDevice graphicsDevice)
        {
            wallTexture = new Texture2D(graphicsDevice, 1, 1);
            wallTexture.SetData<Color>(new Color[] { Color.White });
            goalTexture = new Texture2D(graphicsDevice, 1, 1);
            goalTexture.SetData<Color>(new Color[] { Color.White });
        }

        public void draw(SpriteBatch spriteBatch, Color color)
        {
            foreach(Rectangle rect in walls)
            spriteBatch.Draw(wallTexture, rect, color);
            spriteBatch.Draw(goalTexture, goal, goalColor);
            timer.draw(spriteBatch);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            draw(spriteBatch, Color.Black);
        }

        private void detectWall(Sprite sprite, Rectangle wall, ref Vector2 nextPosition)
        {
            int spriteTop = (int)sprite.position.Y;
            int spriteBottom = (int)sprite.position.Y + sprite.height;
            int spriteLeft = (int)sprite.position.X;
            int spriteRight = (int)sprite.position.X + sprite.width;

            int nextTop = (int)nextPosition.Y;
            int nextBottom = (int)nextPosition.Y + sprite.height;
            int nextLeft = (int)nextPosition.X;
            int nextRight = (int)nextPosition.X + sprite.width;

            if (spriteBottom >= wall.Top && spriteTop <= wall.Bottom)
            {
                if (spriteRight <= wall.Left && nextRight > wall.Left)
                    nextPosition.X = wall.Left - sprite.width;
                else if (spriteLeft >= wall.Right && nextLeft < wall.Right)
                    nextPosition.X = wall.Right;
            }
            if (spriteRight >= wall.Left && spriteLeft <= wall.Right)
            {
                if (spriteBottom <= wall.Top && nextBottom > wall.Top)
                    nextPosition.Y = wall.Top - sprite.height;
                else if (spriteTop >= wall.Bottom && nextTop < wall.Bottom)
                    nextPosition.Y = wall.Bottom;
            }
            if (spriteRight < wall.Left && spriteBottom < wall.Top && nextRight > wall.Left && nextBottom > wall.Top)
            {
                nextPosition.X = wall.Left - sprite.width;
                nextPosition.Y = wall.Top - sprite.height;
            }
            else if (spriteRight < wall.Left && spriteTop > wall.Bottom && nextRight > wall.Left && nextTop < wall.Bottom)
            {
                nextPosition.X = wall.Left - sprite.width;
                nextPosition.Y = wall.Bottom;
            }
            else if (spriteLeft > wall.Right && spriteBottom < wall.Top && nextLeft < wall.Right && nextBottom > wall.Top)
            {
                nextPosition.X = wall.Right;
                nextPosition.Y = wall.Top - sprite.height;
            }
            else if (spriteLeft > wall.Right && spriteTop > wall.Bottom && nextLeft < wall.Right && nextTop < wall.Bottom)
            {
                nextPosition.X = wall.Right;
                nextPosition.Y = wall.Bottom;
            }
        }

        public void detectWalls(Sprite sprite, ref Vector2 nextPosition)
        {
            foreach (Rectangle wall in walls)
                detectWall(sprite, wall, ref nextPosition);
        }

        public void update(Player player1, Player player2)
        {
            if (player1.selected || player2.selected || player1.mouseSelected || player2.mouseSelected)
                timer.start();

            if (player1.reachedGoal(goal) && player2.reachedGoal(goal))
            {
                goalColor = Color.Green;
                timer.stop();
                Program.game.startScoreScreen(timer.time);
            }
        }
    }
}
