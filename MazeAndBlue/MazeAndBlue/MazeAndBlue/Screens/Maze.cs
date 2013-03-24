using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        const int mazeWidth = 970, mazeHeight = 490;
        public static int width { get { return mazeWidth; } }
        public static int height { get { return mazeHeight; } }

        public Maze(string mazeFile)
        {
            goalColor = Color.Red;
            timer = new Timer();

            balls = new List<Ball>();
            walls = new List<Rectangle>();
            goal = new Rectangle();
            switchs = new List<DoorSwitch>();

            readFile(mazeFile);

            pauseButton = new Button(new Point(Program.game.screenWidth - 130, 30), 100, 40, "Pause");

/*          // 12 x 6 grid
            p1StartPos = new Point(52, 68);
            p2StartPos = new Point(52, 468);
            goal = new Rectangle(917, 53, 70, 70);
            walls.Add(new Rectangle(27, 43, 10, 490));  // left
            walls.Add(new Rectangle(107, 43, 10, 490));
            walls.Add(new Rectangle(187, 43, 10, 490));
            walls.Add(new Rectangle(267, 43, 10, 490));
            walls.Add(new Rectangle(347, 43, 10, 490));
            walls.Add(new Rectangle(427, 43, 10, 490));
            walls.Add(new Rectangle(507, 43, 10, 490));
            walls.Add(new Rectangle(587, 43, 10, 490));
            walls.Add(new Rectangle(667, 43, 10, 490));
            walls.Add(new Rectangle(747, 43, 10, 490));
            walls.Add(new Rectangle(827, 43, 10, 490));
            walls.Add(new Rectangle(907, 43, 10, 490));
            walls.Add(new Rectangle(987, 43, 10, 490)); // right
            walls.Add(new Rectangle(27, 43, 970, 10));  // top
            walls.Add(new Rectangle(27, 123, 970, 10));
            walls.Add(new Rectangle(27, 203, 970, 10));
            walls.Add(new Rectangle(27, 283, 970, 10));
            walls.Add(new Rectangle(27, 363, 970, 10));
            walls.Add(new Rectangle(27, 443, 970, 10));
            walls.Add(new Rectangle(27, 523, 970, 10)); // bottom
*/
        }

        public void loadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            foreach (Ball ball in balls)
                ball.loadContent(content);
            foreach (DoorSwitch dswitch in switchs)
                dswitch.loadContent(graphicsDevice);
            wallTexture = new Texture2D(graphicsDevice, 1, 1);
            wallTexture.SetData<Color>(new Color[] { Color.White });
            goalTexture = new Texture2D(graphicsDevice, 1, 1);
            goalTexture.SetData<Color>(new Color[] { Color.White });
            pauseButton.loadContent(content);
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
            foreach (Ball ball in balls)
                ball.draw(spriteBatch);
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

        private void detectWall(Sprite sprite, Rectangle wall, ref Point nextPosition)
        {
            int spriteTop = (int)sprite.position.Y;
            int spriteBottom = (int)sprite.position.Y + sprite.height;
            int spriteLeft = (int)sprite.position.X;
            int spriteRight = (int)sprite.position.X + sprite.width;

            int nextTop = (int)nextPosition.Y;
            int nextBottom = (int)nextPosition.Y + sprite.height;
            int nextLeft = (int)nextPosition.X;
            int nextRight = (int)nextPosition.X + sprite.width;

            if (spriteBottom > wall.Top && spriteTop < wall.Bottom)
            {
                if (spriteRight <= wall.Left && nextRight > wall.Left)
                    nextPosition.X = wall.Left - sprite.width;
                else if (spriteLeft >= wall.Right && nextLeft < wall.Right)
                    nextPosition.X = wall.Right;
            }
            if (spriteRight > wall.Left && spriteLeft < wall.Right)
            {
                if (spriteBottom <= wall.Top && nextBottom > wall.Top)
                    nextPosition.Y = wall.Top - sprite.height;
                else if (spriteTop >= wall.Bottom && nextTop < wall.Bottom)
                    nextPosition.Y = wall.Bottom;
            }
            if (spriteRight < wall.Left && spriteBottom < wall.Top && nextRight > wall.Left && nextBottom > wall.Top)
            {
                nextPosition.X = wall.Left - sprite.width;
                nextPosition.Y = wall.Top - sprite.height;
            }
            else if (spriteRight < wall.Left && spriteTop > wall.Bottom && nextRight > wall.Left && nextTop < wall.Bottom)
            {
                nextPosition.X = wall.Left - sprite.width;
                nextPosition.Y = wall.Bottom;
            }
            else if (spriteLeft > wall.Right && spriteBottom < wall.Top && nextLeft < wall.Right && nextBottom > wall.Top)
            {
                nextPosition.X = wall.Right;
                nextPosition.Y = wall.Top - sprite.height;
            }
            else if (spriteLeft > wall.Right && spriteTop > wall.Bottom && nextLeft < wall.Right && nextTop < wall.Bottom)
            {
                nextPosition.X = wall.Right;
                nextPosition.Y = wall.Bottom;
            }
        }

        public void detectWalls(Sprite sprite, ref Point nextPosition)
        {
            foreach (Rectangle wall in walls)
                detectWall(sprite, wall, ref nextPosition);
        }

        public void update()
        {
            foreach (DoorSwitch dswitch in switchs)
                dswitch.update(balls, ref walls);

            foreach (Ball ball in balls)
                ball.update(this);

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
                Program.game.startScoreScreen(timer.time);
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
                }
            }
            else
                return false;
            
            return true;
        }

        public void onLeftClick(Point point)
        {
            if (pauseButton.contains(point))
                onPauseButtonPress();
        }

        private void onPauseButtonPress()
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
}
