﻿using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeAndBlue
{
    public class CreateMaze
    {
        enum CreateState { WALLS, GOAL, P1, P2 };
        Button wallButton, goalButton, p1Button, p2Button, mainMenuButton, saveButton;
        CreateState state;
        Color tempColor, hlColor, wallColor, goalColor;
        Texture2D texture;
        List<Rectangle> border, walls, twalls, tgoals, tplayers;
        Rectangle hlrect, goal;
        Ball p1, p2;
        bool hl, prevDown, goalSet, p1set, p2set, singleplayer;

        public CreateMaze(bool hard, bool _singleplayer)
        {
            singleplayer = _singleplayer;
            hl = prevDown = goalSet = p1set = p2set = false;

            state = CreateState.WALLS;

            wallButton = new Button(new Point(30, 30), 136, 72, "Walls", "Buttons/button");
            goalButton = new Button(new Point(200, 30), 136, 72, "Goal", "Buttons/button");
            p1Button = new Button(new Point(370, 30), 136, 72, "P1", "Buttons/button");
            p2Button = new Button(new Point(540, 30), 136, 72, "P2", "Buttons/button");
            mainMenuButton = new Button(new Point(Program.game.screenWidth - 332, 30), 136, 72, "Back", "Buttons/button");
            saveButton = new Button(new Point(Program.game.screenWidth-166, 30), 136, 72, "Save", "Buttons/button");

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

            int cols = 6, rows = 3, size = 160;
            if (hard)
            {
                cols *= 2;
                rows *= 2;
                size /= 2;
            }

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (j > 0)
                        twalls.Add(new Rectangle(Program.game.sx(i * size), Program.game.sy(j * size), size + 10, 10));
                    if (i > 0)
                        twalls.Add(new Rectangle(Program.game.sx(i * size), Program.game.sy(j * size), 10, size + 10));
                    tgoals.Add(new Rectangle(Program.game.sx(10 + i * size), Program.game.sy(10 + j * size), size - 10, size - 10));
                    tplayers.Add(new Rectangle(Program.game.sx(i * size + size / 2 - 15), Program.game.sy(j * size + size / 2 - 15), 40, 40));
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
            mainMenuButton.loadContent();
            saveButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            wallButton.draw(spriteBatch);
            goalButton.draw(spriteBatch);
            p1Button.draw(spriteBatch);
            if(!singleplayer)
                p2Button.draw(spriteBatch);
            mainMenuButton.draw(spriteBatch);
            saveButton.draw(spriteBatch);

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
            Point point = new Point(mouse.X, mouse.Y);
            bool down = mouse.LeftButton == ButtonState.Pressed;
            hl = false;
            switch (state)
            {
                case CreateState.WALLS:
                    wallsUpdate(point, down);
                    break;
                case CreateState.GOAL:
                    goalUpdate(point, down);
                    break;
                case CreateState.P1:
                    p1Update(point, down);
                    break;
                case CreateState.P2:
                    p2Update(point, down);
                    break;
            }
            prevDown = down;

            if (wallButton.isSelected())
                state = CreateState.WALLS;
            else if (goalButton.isSelected())
                state = CreateState.GOAL;
            else if (p1Button.isSelected())
                state = CreateState.P1;
            else if (p2Button.isSelected() && !singleplayer)
                state = CreateState.P2;
            else if (mainMenuButton.isSelected())
                Program.game.startMainMenu();
            else if (saveButton.isSelected())
                saveMaze();
        }

        void wallsUpdate(Point point, bool down)
        {
            foreach (Rectangle rect in twalls)
            {
                if (rect.Contains(point))
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
        }

        void goalUpdate(Point point, bool down)
        {
            foreach (Rectangle rect in tgoals)
            {
                if (rect.Contains(point))
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
        }

        void p1Update(Point point, bool down)
        {
            foreach (Rectangle rect in tplayers)
            {
                if (rect.Contains(point))
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
        }

        void p2Update(Point point, bool down)
        {
            foreach (Rectangle rect in tplayers)
            {
                if (rect.Contains(point))
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
        }

        void saveMaze()
        {
            if(!(goalSet && p1set && (singleplayer || p2set)))
                return;

            int size = walls.Count + 6; // 4 border walls, goal, p1
            if(!singleplayer)
                size++;
            
            string[] lines = new string[size];

            int index = 0;
            lines[index++] = "1 " + scaleX(p1.position.X) + ' ' + scaleY(p1.position.Y);
            if (!singleplayer)
                lines[index++] = "2 " + scaleX(p2.position.X) + ' ' + scaleY(p2.position.Y);
            lines[index++] = "goal " + scaleX(goal.Left) + ' ' + scaleY(goal.Top) + ' ' + goal.Width + ' ' + goal.Height;
            for(int i = 0; i<border.Count; i++)
                lines[index++] = "wall " + scaleX(border[i].Left) + ' ' + scaleY(border[i].Top) + ' ' + border[i].Width + ' ' + border[i].Height;
            for (int i = 0; i < walls.Count; i++)
                lines[index++] = "wall " + scaleX(walls[i].Left) + ' ' + scaleY(walls[i].Top) + ' ' + walls[i].Width + ' ' + walls[i].Height;

            string filename = "Mazes/temp.maze";
            File.WriteAllLines(filename, lines);

            //Hard coded coordinates
            //automatically save the thumbnail.
            System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(0, 0, 1280, 768);//define size of image output
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(bounds.Width, bounds.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.CopyFromScreen(new System.Drawing.Point(300, 200), System.Drawing.Point.Empty, bounds.Size);
            bitmap.Save("test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            Program.game.startMainMenu();
        }

        int scaleX(int x)
        {
            return x - (Program.game.screenWidth - Maze.width) / 2;
        }

        int scaleY(int y)
        {
            return y - (Program.game.screenHeight - Maze.height) / 2;
        }

    }
}