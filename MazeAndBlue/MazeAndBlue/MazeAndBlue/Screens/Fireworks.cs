using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace MazeAndBlue
{
    class Fireworks
    {
        Texture2D texture;
        List<Texture2D> textures;
        List<FireworkGenerator> fireworks;
        List<double> xDirections;
        Random random;

        GameTime gameTime;
        Rectangle window;

        int screenWidth;
        int screenHeight;

        List<bool> exploded;
        double heightMod;
        int iter;

        public Fireworks()
        {
            random = new Random();

            fireworks = new List<FireworkGenerator>();
            exploded = new List<bool>();
            xDirections = new List<double>();
            screenWidth = Program.game.screenWidth;
            screenHeight = Program.game.screenHeight;

            window = new Rectangle(screenWidth / 8, screenHeight / 8, 3 * screenWidth / 4, 3 * screenHeight / 4);

            xDirections.Add((double)(random.Next(-10, 10) / 1000.0));

            heightMod = (random.NextDouble() * .4 + .2);

            iter = 0;
        }

        private void addFirework()
        {
            xDirections.Add((double)(random.Next(-10, 10) / 10000.0));

            heightMod = (random.NextDouble() * .4 + .2);

            FireworkGenerator fireworkGen = new FireworkGenerator(textures, new Vector2(400, 240));
            fireworkGen.EmitterLocation = new Vector2((float)(random.NextDouble() * screenWidth), (float)screenHeight + 5);
            fireworks.Add(fireworkGen);
            exploded.Add(false);
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });

            textures = new List<Texture2D>();
            textures.Add(Program.game.Content.Load<Texture2D>("circle"));
            textures.Add(Program.game.Content.Load<Texture2D>("star"));
            textures.Add(Program.game.Content.Load<Texture2D>("diamond"));

            FireworkGenerator fireworkGen = new FireworkGenerator(textures, new Vector2(400, 240));
            fireworkGen.EmitterLocation = new Vector2((float)(random.NextDouble() * screenWidth), (float)screenHeight + 5);
            fireworks.Add(fireworkGen);
            exploded.Add(false);
        }

        public void update()
        {
            iter++;

            for (int i = 0; i < fireworks.Count; i++)
            {
                float x = fireworks[i].EmitterLocation.X;
                float y = fireworks[i].EmitterLocation.Y;

                if (fireworks[i].EmitterLocation.Y <= (float)(screenHeight * heightMod) && !exploded[i])
                {
                    exploded[i] = true;
                    Program.game.soundEffectPlayer.playFirework();
                }
                if (!exploded[i])
                {
                    x += (float)(xDirections[i] * screenWidth);
                    y -= (float)(.005 * screenHeight);
                }

                fireworks[i].EmitterLocation = new Vector2(x, y);
                fireworks[i].Update();

                if (fireworks[i].completed())
                {
                    exploded.RemoveAt(i);
                    fireworks.RemoveAt(i);
                    xDirections.RemoveAt(i);
                    i--;
                }
            }

            if (iter % 55 == 0)
            {
                this.addFirework();
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, window, new Color(0, 0, 0, 0));

            for (int i = 0; i < fireworks.Count; i++)
            {
                fireworks[i].Draw(spriteBatch, exploded[i]);
            }
        }
    }
}
