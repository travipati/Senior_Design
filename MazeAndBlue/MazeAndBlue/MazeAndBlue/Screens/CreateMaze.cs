using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeAndBlue
{
    public class CreateMaze
    {
        enum CreateState { WALLS, GOAL, P1, P2 };
        Button wallButton, goalButton, p1Button, p2Button;
        CreateState state;
        Color tempColor, hlColor, wallColor, goalColor;
        Texture2D texture;
        List<Rectangle> border, walls, twalls, tgoals, tplayers;
        Rectangle hlrect, goal;
        Ball p1, p2;
        bool hl, prevDown, goalSet, p1set, p2set;

        public CreateMaze()
        {
            hl = prevDown = goalSet = p1set = p2set = false;

            state = CreateState.WALLS;

            wallButton = new Button(new Point(30, 30), 136, 72, "Walls", "Buttons/button");
            goalButton = new Button(new Point(200, 30), 136, 72, "Goal", "Buttons/button");
            p1Button = new Button(new Point(370, 30), 136, 72, "P1", "Buttons/button");
            p2Button = new Button(new Point(540, 30), 136, 72, "P2", "Buttons/button");

            hlColor = Color.LightYellow;
            tempColor = new Color(0, 0, 0, 25);
            wallColor = Color.Black;
            goalColor = Color.Red;

            border = new List<Rectangle>();
            walls = new List<Rectangle>();
            twalls = new List<Rectangle>();
            tgoals = new List<Rectangle>();
            tplayers = new List<Rectangle>();
            p1 = new Ball(new Point(0, 0), Color.Blue);
            p2 = new Ball(new Point(0, 0), Color.Yellow);

            border.Add(new Rectangle(Program.game.sx(0), Program.game.sy(0), 10, 490));
            border.Add(new Rectangle(Program.game.sx(960), Program.game.sy(0), 10, 490));
            border.Add(new Rectangle(Program.game.sx(0), Program.game.sy(0), 970, 10));
            border.Add(new Rectangle(Program.game.sx(0), Program.game.sy(480), 970, 10));
            
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (j > 0)
                        twalls.Add(new Rectangle(Program.game.sx(i * 160), Program.game.sy(j * 160), 170, 10));
                    if (i > 0)
                        twalls.Add(new Rectangle(Program.game.sx(i * 160), Program.game.sy(j * 160), 10, 170));
                    tgoals.Add(new Rectangle(Program.game.sx(10 + i * 160), Program.game.sy(10 + j * 160), 150, 150));
                    tplayers.Add(new Rectangle(Program.game.sx(i * 160 + 65), Program.game.sy(j * 160 + 65), 40, 40));
                }
            }
        }

        public void loadContent()
        {
            texture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            wallButton.loadContent();
            goalButton.loadContent();
            p1Button.loadContent();
            p2Button.loadContent();
            p1.loadContent();
            p2.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            wallButton.draw(spriteBatch);
            goalButton.draw(spriteBatch);
            p1Button.draw(spriteBatch);
            p2Button.draw(spriteBatch);

            switch (state)
            {
                case CreateState.WALLS:
                    foreach (Rectangle rect in twalls)
                        spriteBatch.Draw(texture, rect, tempColor);
                    break;
                case CreateState.GOAL:
                    foreach (Rectangle rect in tgoals)
                        spriteBatch.Draw(texture, rect, tempColor);
                    break;
                case CreateState.P1:
                case CreateState.P2:
                    foreach (Rectangle rect in tplayers)
                        spriteBatch.Draw(texture, rect, tempColor);
                    break;
            }

            foreach (Rectangle rect in walls)
                spriteBatch.Draw(texture, rect, wallColor);
            foreach (Rectangle rect in border)
                spriteBatch.Draw(texture, rect, wallColor);
            if(goalSet)
                spriteBatch.Draw(texture, goal, goalColor);
            if (p1set)
                p1.draw(spriteBatch);
            if (p2set)
                p2.draw(spriteBatch);
            if (hl)
                spriteBatch.Draw(texture, hlrect, hlColor);
        }

        public void update()
        {
            MouseState mouse = Mouse.GetState();
            bool down = mouse.LeftButton == ButtonState.Pressed;
            hl = false;
            switch (state)
            {
                case CreateState.WALLS:
                    foreach (Rectangle rect in twalls)
                    {
                        if (rect.Contains(mouse.X, mouse.Y))
                        {
                            if (prevDown && !down)
                            {
                                if (walls.Contains(rect))
                                    walls.Remove(rect);
                                else
                                    walls.Add(rect);
                            }
                            else
                            {
                                hl = true;
                                hlrect = rect;
                            }
                        }
                    }
                    break;
                case CreateState.GOAL:
                    foreach (Rectangle rect in tgoals)
                    {
                        if (rect.Contains(mouse.X, mouse.Y))
                        {
                            if (prevDown && !down)
                            {
                                if (goalSet && goal == rect)
                                    goalSet = false;
                                else
                                {
                                    goal = rect;
                                    goalSet = true;
                                }
                            }
                            else
                            {
                                hl = true;
                                hlrect = rect;
                            }
                        }
                    }
                    break;
                case CreateState.P1:
                    foreach (Rectangle rect in tplayers)
                    {
                        if (rect.Contains(mouse.X, mouse.Y))
                        {
                            if (prevDown && !down)
                            {
                                if (p1set && p1.position == new Point(rect.Left, rect.Top))
                                    p1set = false;
                                else
                                {
                                    p1.moveBall(new Point(rect.Left + p1.width / 2, rect.Top + p1.height / 2));
                                    p1set = true;
                                }
                            }
                            else
                            {
                                hl = true;
                                hlrect = rect;
                            }
                        }
                    }
                    break;
                case CreateState.P2:
                    foreach (Rectangle rect in tplayers)
                    {
                        if (rect.Contains(mouse.X, mouse.Y))
                        {
                            if (prevDown && !down)
                            {
                                if (p2set && p2.position == new Point(rect.Left, rect.Top))
                                    p2set = false;
                                else
                                {
                                    p2.moveBall(new Point(rect.Left + p2.width / 2, rect.Top + p2.height / 2));
                                    p2set = true;
                                }
                            }
                            else
                            {
                                hl = true;
                                hlrect = rect;
                            }
                        }
                    }
                    break;
            }
            prevDown = down;

            if (wallButton.isSelected())
                state = CreateState.WALLS;
            else if (goalButton.isSelected())
                state = CreateState.GOAL;
            else if (p1Button.isSelected())
                state = CreateState.P1;
            else if (p2Button.isSelected())
                state = CreateState.P2;
        }

    }
}