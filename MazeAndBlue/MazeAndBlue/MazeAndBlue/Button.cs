﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class Button : Sprite
    {
        string text;

        public Button(Vector2 pos, int w, int h, string s) : base(pos, w, h)
        {
            text = s;
        }

        public void loadContent(ContentManager content)
        {
            loadContent(content, "button");
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

    }
}
