using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class ProgressCircle
    {
        public void drawCircle(SpriteBatch spriteBatch, Color c, Rectangle rec, double percent)
        {
            int radius = 20;

            int outerRadius = radius * 2 + 2;
            Texture2D texture = new Texture2D(Program.game.GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2 * percent; angle += angleStep)
            {
                int x = (int)Math.Round(radius + radius * Math.Cos(angle + 3 * Math.PI / 2));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle + 3 * Math.PI / 2));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            spriteBatch.Draw(texture, rec, c);
        }

        public void draw(SpriteBatch spriteBatch, int val, Point pos)
        {
            double percent = (double)val / 3000;
            drawCircle(spriteBatch, Color.Black, new Rectangle(pos.X - 5, pos.Y - 5, 50, 50), 1);
            drawCircle(spriteBatch, Color.Black, new Rectangle(pos.X - 10, pos.Y - 10, 60, 60), 1);
            for (int i = 1; i < 5; i++)
                drawCircle(spriteBatch, Color.White, new Rectangle(pos.X - (5 + i), pos.Y - (5 + i), 50 + 2 * i, 50 + 2 * i), percent);
        }
    }
}
