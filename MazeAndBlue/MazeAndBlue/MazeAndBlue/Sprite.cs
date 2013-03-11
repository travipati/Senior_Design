using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class Sprite
    {
        public Vector2 position;
        public int width, height;
        protected Texture2D texture;

        public Sprite() : this(new Vector2(0, 0), 50, 50) { }

        public Sprite(Vector2 pos) : this(pos, 50, 50) { }

        public Sprite(Vector2 pos, int w, int h)
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
            int x = (int)position.X + width / 2;
            int y = (int)position.Y + height / 2;
            return rect.Contains(x, y);
        }

        public bool contains(Point point)
        {
            Rectangle rect = new Rectangle((int)position.X, (int)position.Y, width, height);
            return rect.Contains(point);
        }

    }
}
