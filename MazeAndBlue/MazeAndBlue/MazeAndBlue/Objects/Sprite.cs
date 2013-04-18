using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MazeAndBlue
{
    public class Sprite
    {
        public Point position;
        public int width, height;
        protected Texture2D texture;

        public Sprite() : this(new Point(-40, -40)) { }

        public Sprite(Point pos) : this(pos, 40, 40) { }

        public Sprite(Point pos, int w, int h)
        {
            position = pos;
            width = w;
            height = h;
        }

        public void loadContent(string name)
        {
            texture = Program.game.Content.Load<Texture2D>(name);
        }

        public void loadContent(Stream stream)
        {
            texture = Texture2D.FromStream(Program.game.GraphicsDevice, stream);
            stream.Close();
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            draw(spriteBatch, Color.White);
        }

        public virtual void draw(SpriteBatch spriteBatch, Color textureColor)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), textureColor);
        }

        public bool overlaps(Rectangle rect)
        {
            Rectangle nrect = new Rectangle((int)position.X, (int)position.Y, width, height);
            return nrect.Intersects(rect);
        }

        public bool overlaps(Sprite sprite)
        {
            Rectangle rect = new Rectangle((int)position.X, (int)position.Y, width, height);
            return sprite.overlaps(rect);
        }

        public bool contains(Point point)
        {
            Rectangle rect = new Rectangle((int)position.X, (int)position.Y, width, height);
            return rect.Contains(point);
        }

    }
}
