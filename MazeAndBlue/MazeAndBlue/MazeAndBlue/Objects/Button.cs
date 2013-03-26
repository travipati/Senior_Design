using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class Button : Sprite
    {
        public string text;
        int originalW;
        int originalH;
        Point originalPos;
        public string path;

        private Button(Point pos, int w, int h, string s)
            : base(pos, w, h)
        {
            originalH = h;
            originalW = w;
            originalPos = pos;
            text = s;
        }

        public Button(Point pos, int w, int h, string s, string p) : base(pos, w, h)
        {
            originalH = h;
            originalW = w;
            originalPos = pos;
            text = s;
            path = p;
        }

        public void enlarge(double p)
        {
            int w = (int)(originalW * (1 + p));
            int h = (int)(originalH * (1 + p));
            this.position.X = originalPos.X - (int)(originalW * 0.5 * p);  
            this.position.Y = originalPos.Y - (int)(originalH * 0.5 * p);
            this.width = w;
            this.height = h;
        }

        public void resetSize()
        {
            this.position = originalPos;
            this.height = originalH;
            this.width = originalW;
        }

        public void reload()
        {
            resetSize();
            loadContent(Program.game.Content, path);
        }

        public void loadContent(ContentManager content)
        {
            loadContent(content, "Buttons/button");
        }


        public override void draw(SpriteBatch spriteBatch, Color textureColor)
        {
            draw(spriteBatch, textureColor, Color.Black);
        }

        public void draw(SpriteBatch spriteBatch, Color textureColor, Color fontColor)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), textureColor);
            Vector2 textSize = MazeAndBlue.font.MeasureString(text);
            int x = (int)(position.X + (width - textSize.X) / 2);
            int y = (int)(position.Y + (height - textSize.Y) / 2);
            Vector2 textPos = new Vector2(x, y);
            spriteBatch.DrawString(MazeAndBlue.font, text, textPos, fontColor);
        }

        public void update()
        {
            if (isOver())
            {
                enlarge(0.1);
                loadContent(Program.game.Content, "Buttons/Hover");
            }
            else
                reload();
        }

        public bool isSelected()
        {
            if (Program.game.ms.newPointReady && contains(Program.game.ms.point))
            {
                Program.game.ms.newPointReady = false;
                Program.game.soundEffectPlayer.playButton();
                return true;
            }
            else if (Program.game.vs.newWordReady && Program.game.vs.word == text)
            {
                Program.game.vs.newWordReady = false;
                Program.game.soundEffectPlayer.playButton();
                return true;
            }
            foreach (Player player in Program.game.players)
            {
                if (player.overlaps(this) && player.selecting())
                {
                    Program.game.soundEffectPlayer.playButton();
                    return true;
                }
            }

            return false;
        }

        public bool isOver()
        {
            if (contains(Program.game.ms.point))
                return true;
            foreach (Player player in Program.game.players)
            {
                if (player.overlaps(this))
                    return true;
            }

            return false;
        }
    }
}
