using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class FireworkGenerator
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private Vector2 gravity;
        private List<Particle> particles;
        private List<Texture2D> textures;
        private int iter;
        private bool exploded;

        public FireworkGenerator(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            iter = 0;
            gravity = new Vector2((float)0, (float).09);
        }

        public void Update()
        {
            int total = 10;
            iter++;

            if (!exploded)
            {
                iter = 0;
            }
            if (exploded)
            {
                total = 0;
                if (iter < 7)
                {
                    total = (int)(random.NextDouble() * 15);
                }
            }

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();
                if (particles[i].lifespan <= 0)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }

        public bool completed()
        {
            return (particles.Count == 0 && iter > 250);
        }

        private Particle GenerateNewParticle()
        {
            Vector2 position = EmitterLocation;
            Texture2D texture;
            Vector2 velocity;
            Color color;
            float size;
            int lifespan;

            if (!exploded)
            {
                //Particle settings for trail
                texture = textures[0];
                velocity = new Vector2(
                                        1f * (float)(random.NextDouble() * .5 - .5),
                                        1f * (float)(random.NextDouble() * .5 - .5));
                color = new Color((float)1.0, (float)(random.NextDouble() * .21), (float).0);
                lifespan = 20 + random.Next(30);
                size = (float).25;
            }
            else
            {
                //particle settings for blast
                texture = textures[random.Next(textures.Count)];
                int n = random.Next(2, 5);
                velocity = new Vector2(
                                        1f * (float)(random.NextDouble() * 2 * n - n),
                                        1f * (float)(random.NextDouble() * .75 * n - 1.5 * n));
                color = new Color(
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble());
                lifespan = 60 + random.Next(40);
                size = (float)random.NextDouble();
            }

            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);


            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifespan);
        }

        public void Draw(SpriteBatch spriteBatch, bool isStage2)
        {
            exploded = isStage2;
            for (int i = 0; i < particles.Count; i++)
            {
                if (exploded && particles[i].Color.R != 1.0)
                {
                    particles[i].Velocity += gravity;
                }
                particles[i].Draw(spriteBatch);
            }
        }
    }
}
