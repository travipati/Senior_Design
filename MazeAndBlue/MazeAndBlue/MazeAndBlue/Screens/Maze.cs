using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    public class Maze
    {
        Button pauseButton;
        Color goalColor;
        Rectangle goal;
        List<Ball> balls;
        List<Rectangle> walls;
        List<DoorSwitch> switchs;
        Texture2D wallTexture, goalTexture;
        Timer timer;
        int prevTickCount, wallHits;
        const int mazeWidth = 970, mazeHeight = 490;
        public static int width { get { return mazeWidth; } }
        public static int height { get { return mazeHeight; } }

        public Maze(string mazeFile)
        {
            goalColor = Color.Red;
            timer = new Timer();
            prevTickCount = -1;
            wallHits = 0;

            balls = new List<Ball>();
            walls = new List<Rectangle>();
            goal = new Rectangle();
            switchs = new List<DoorSwitch>();

            readFile(mazeFile);

            pauseButton = new Button(new Point(Program.game.screenWidth - 130, 30), 100, 40, "Pause", "Buttons/button");
        }

        public void loadContent()
        {
            foreach (Ball ball in balls)
                ball.loadContent();
            foreach (DoorSwitch dswitch in switchs)
                dswitch.loadContent();
            wallTexture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            wallTexture.SetData<Color>(new Color[] { Color.White });
            goalTexture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            goalTexture.SetData<Color>(new Color[] { Color.White });
            pauseButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch, Color color)
        {
            pauseButton.draw(spriteBatch);
            foreach(Rectangle rect in walls)
                spriteBatch.Draw(wallTexture, rect, color);
            foreach (DoorSwitch dswitch in switchs)
                dswitch.draw(spriteBatch);
            spriteBatch.Draw(goalTexture, goal, goalColor);
            timer.draw(spriteBatch);
            balls[1].draw(spriteBatch);
            balls[0].draw(spriteBatch);
            for (int i = Program.game.players.Count - 1; i >= 0; i--)
            {
                if (balls[0].playerId != i && balls[1].playerId != i)
                    Program.game.players[i].draw(spriteBatch);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            draw(spriteBatch, Color.Black);
        }

        public void update()
        {
            foreach (DoorSwitch dswitch in switchs)
                dswitch.update(balls, ref walls);

            foreach (Ball ball in balls)
            {
                ball.select();

                if (ball.playerId >= 0 || ball.mouseSelected)
                {
                    Point position;
                    position = Program.game.ms.point;
                    if (ball.playerId >= 0)
                        position = Program.game.players[ball.playerId].position;
                    detectWalls(ball, ref position);
                    ball.moveBall(position);
                }
            }

            if (balls[0].playerId > 0 || balls[1].playerId > 0 || balls[0].mouseSelected || balls[1].mouseSelected)
                timer.start();

            if (balls[0].overlaps(goal) && balls[1].overlaps(goal))
            {
                goalColor = Color.Green;
                timer.stop();
                foreach (Ball ball in balls)
                {
                    ball.playerId = -1;
                    ball.mouseSelected = false;
                }
                Program.game.soundEffectPlayer.playGoal();
                Program.game.startScoreScreen(timer.time);
            }

            if (pauseButton.isSelected())
            {
                timer.stop();
                foreach (Ball ball in balls)
                {
                    ball.playerId = -1;
                    ball.mouseSelected = false;
                }
                Program.game.startPauseSelectionScreen();
            }
        }

        public void detectWalls(Ball ball, ref Point nextPosition)
        {
            foreach (Rectangle wall in walls)
                detectWall(ball, wall, ref nextPosition);
        }

        private void detectWall(Ball ball, Rectangle wall, ref Point nextPosition)
        {
            int spriteTop = (int)ball.position.Y;
            int spriteBottom = (int)ball.position.Y + ball.height;
            int spriteLeft = (int)ball.position.X;
            int spriteRight = (int)ball.position.X + ball.width;

            int nextTop = (int)nextPosition.Y;
            int nextBottom = (int)nextPosition.Y + ball.height;
            int nextLeft = (int)nextPosition.X;
            int nextRight = (int)nextPosition.X + ball.width;

            bool hit = false;

            if (spriteBottom > wall.Top && spriteTop < wall.Bottom)
            {
                if (spriteRight <= wall.Left && nextRight > wall.Left)
                {
                    nextPosition.X = wall.Left - ball.width;
                    hit = true;
                }
                else if (spriteLeft >= wall.Right && nextLeft < wall.Right)
                {
                    nextPosition.X = wall.Right;
                    hit = true;
                }
            }
            if (spriteRight > wall.Left && spriteLeft < wall.Right)
            {
                if (spriteBottom <= wall.Top && nextBottom > wall.Top)
                {
                    nextPosition.Y = wall.Top - ball.height;
                    hit = true;
                }
                else if (spriteTop >= wall.Bottom && nextTop < wall.Bottom)
                {
                    nextPosition.Y = wall.Bottom;
                    hit = true;
                }
            }
            if (spriteRight < wall.Left && spriteBottom < wall.Top && nextRight > wall.Left && nextBottom > wall.Top)
            {
                nextPosition.X = wall.Left - ball.width;
                nextPosition.Y = wall.Top - ball.height;
                hit = true;
            }
            if (spriteRight < wall.Left && spriteTop > wall.Bottom && nextRight > wall.Left && nextTop < wall.Bottom)
            {
                nextPosition.X = wall.Left - ball.width;
                nextPosition.Y = wall.Bottom;
                hit = true;
            }
            if (spriteLeft > wall.Right && spriteBottom < wall.Top && nextLeft < wall.Right && nextBottom > wall.Top)
            {
                nextPosition.X = wall.Right;
                nextPosition.Y = wall.Top - ball.height;
                hit = true;
            }
            if (spriteLeft > wall.Right && spriteTop > wall.Bottom && nextLeft < wall.Right && nextTop < wall.Bottom)
            {
                nextPosition.X = wall.Right;
                nextPosition.Y = wall.Bottom;
                hit = true;
            }

            if (hit)
            {
                wallHits++;
                if (System.Environment.TickCount-500 > prevTickCount)
                {
                    prevTickCount = System.Environment.TickCount;
                    Program.game.soundEffectPlayer.playWall();
                }
            }
        }

        private bool readFile(string filename)
        {
            if (!File.Exists(filename))
                return false;

            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                if (!parseLine(line))
                    return false;
            }

            return true;
        }

        private bool parseLine(string line)
        {
            if (line == null)
                return false;

            string[] words = line.Split(new char[] { ' ' });

            if (words.Length == 0)
                return false;

            Point vec;
            Rectangle rect;

            if (words[0] == "1" || words[0] == "2")
            {
                if (words.Length != 3)
                    return false;
                vec = new Point(Program.game.sx((int)Convert.ToSingle(words[1])), Program.game.sy((int)(Convert.ToSingle(words[2]))));

                if (words[0] == "1")
                    balls.Add(new Ball(vec, Color.Blue));
                else if (words[0] == "2")
                    balls.Add(new Ball(vec, Color.Yellow));
            }
            else if (words[0] == "goal" || words[0] == "wall" || words[0] == "switch" || words[0] == "door")
            {
                if (words.Length != 5)
                    return false;
                rect = new Rectangle(Program.game.sx(Convert.ToInt32(words[1])), Program.game.sy(Convert.ToInt32(words[2])),
                                        Convert.ToInt32(words[3]), Convert.ToInt32(words[4]));

                if (words[0] == "goal")
                    goal = rect;
                else if (words[0] == "wall")
                    walls.Add(rect);
                else if (words[0] == "switch")
                    switchs.Add(new DoorSwitch(rect));
                else if (words[0] == "door")
                {
                    if (switchs.Count == 0)
                        return false;
                    switchs[switchs.Count - 1].addDoor(rect);
                    walls.Add(rect);
                }
            }
            else
                return false;
            
            return true;
        }
    }
}
