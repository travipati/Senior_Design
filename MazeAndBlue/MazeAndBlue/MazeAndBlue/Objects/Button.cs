using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MazeAndBlue
{
    public class Button : Sprite
    {
        public bool selected { get; set; }
        public bool selectable { get; set; }
        public string path { get; set; }
        string text;
        Sprite hoverBg, selectBg;
        bool isStream;

        public Button(Point pos, int w, int h, string s, string p)
            : this(pos, w, h, s, p, false) { }

        public Button(Point pos, int w, int h, string s, string p, bool stream) : base(pos, w, h)
        {
            isStream = stream;
            text = s;
            path = p;
            hoverBg = new Sprite(new Point(pos.X - 10, pos.Y - 10), w + 20, h + 20);
            selectBg = new Sprite(new Point(pos.X - 5, pos.Y - 5), w + 10, h + 10);
            selected = false;
            selectable = true;
        }

        public void loadContent()
        {
            if (isStream)
                loadContent(File.OpenRead(path));
            else
                loadContent(path);
            hoverBg.loadContent("Buttons/black");
            selectBg.loadContent("Buttons/maize");
        }

        public override void draw(SpriteBatch spriteBatch, Color textureColor)
        {
            if (isOver() && selectable)
                hoverBg.draw(spriteBatch);
            if (selected)
                selectBg.draw(spriteBatch);
            base.draw(spriteBatch, textureColor);
            if (path == "Buttons/button")
            {
                Vector2 textSize = MazeAndBlue.font.MeasureString(text);
                int x = (int)(position.X + (width - textSize.X) / 2);
                int y = (int)(position.Y + (height - textSize.Y) / 2);
                Vector2 textPos = new Vector2(x, y);
                spriteBatch.DrawString(MazeAndBlue.font, text, textPos, Color.Black);
            }
        }

        public bool isSelected()
        {
            bool selected = false;

            if (Program.game.ms.newPointReady && contains(Program.game.ms.point))
            {
                Program.game.ms.newPointReady = false;
                selected = true;
            }
            else if (Program.game.vs.newWordReady && Program.game.vs.word == text.ToLower())
            {
                Program.game.vs.newWordReady = false;
                selected = true;
            }
            foreach (Player player in Program.game.players)
            {
                if ((player.overlaps(this) && player.selecting()) || player.buttonSelecting(this))
                    selected = true;
            }

            if (selected)
            {
                foreach (Player player in Program.game.players)
                    player.deselect();
                Program.game.soundEffectPlayer.playButton();
            }

            return selected;
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
