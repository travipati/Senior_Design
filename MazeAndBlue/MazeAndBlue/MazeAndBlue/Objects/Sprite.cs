using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class Sprite
    {
        public Point position;
        public int width, height;
        protected Texture2D texture;

        public Sprite() : this(new Point()) { }

        //public Sprite(Point pos) : this(pos, Program.game.sx(40), Program.game.sy(40)) { }
        public Sprite(Point pos) : this(pos, 40, 40) { }

        public Sprite(Point pos, int w, int h)
        {
            position = pos;
            width = w;
            height = h;
        }

        public void loadContent(ContentManager content, string name)
        {
            texture = content.Load<Texture2D>(name);
        }

        public void draw(SpriteBatch spriteBatch)
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
