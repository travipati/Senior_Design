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
        Color goalColor, wallColor;
        Rectangle goal;
        List<Ball> balls;
        List<Rectangle> walls;
        List<DoorSwitch> switches;
        Texture2D wallTexture, goalTexture;
        Timer timer;
        int level;
        bool singlePlayer;
        bool[] prevHit;
        const int mazeWidth = 970, mazeHeight = 490;
        public static int width { get { return mazeWidth; } }
        public static int height { get { return mazeHeight; } }
        public int wallHits { get; set; }

        public Maze(int _level, bool _singlePlayer)
        {
            goalColor = Color.Red;
            wallColor = Color.Black;
            timer = new Timer();
            wallHits = 0;
            prevHit = new bool[2] { false, false };
            level = _level;
            singlePlayer = _singlePlayer;

            balls = new List<Ball>();
            walls = new List<Rectangle>();
            goal = new Rectangle();
            switches = new List<DoorSwitch>();

            string mazeFile;
            if (singlePlayer)
                mazeFile = "Mazes/" + (level + 12) + ".maze";
            else
                mazeFile = "Mazes/" + level + ".maze";
            readFile(mazeFile);

            pauseButton = new Button(new Point(Program.game.screenWidth - 170, 30), 136, 72, "Pause", "Buttons/pause");
        }

        public void loadContent()
        {
            foreach (Ball ball in balls)
                ball.loadContent();
            foreach (DoorSwitch dswitch in switches)
                dswitch.loadContent();
            wallTexture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            wallTexture.SetData<Color>(new Color[] { Color.White });
            goalTexture = new Texture2D(Program.game.GraphicsDevice, 1, 1);
            goalTexture.SetData<Color>(new Color[] { Color.White });
            pauseButton.loadContent();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            pauseButton.draw(spriteBatch);
            foreach (Rectangle rect in walls)
                spriteBatch.Draw(wallTexture, rect, wallColor);
            foreach (DoorSwitch dswitch in switches)
                dswitch.draw(spriteBatch);
            spriteBatch.Draw(goalTexture, goal, goalColor);
            timer.draw(spriteBatch);
            if (!singlePlayer)
                balls[1].draw(spriteBatch);
            balls[0].draw(spriteBatch);
            foreach (Player player in Program.game.players)
            {
                if (player.visible)
                    player.draw(spriteBatch);
            }
        }

        public void update()
        {
            pauseButton.selectable = true;

            foreach (DoorSwitch dswitch in switches)
                dswitch.update(balls, ref walls);

            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].select();

                Point position = balls[i].position;
                position.X += balls[i].width / 2;
                position.Y += balls[i].height / 2;

                if (balls[i].mouseSelected)
                    position = Program.game.ms.point;
                else if (balls[i].playerId >= 0)
                    position = Program.game.players[balls[i].playerId].position;

                detectWalls(i, ref position);
                balls[i].moveBall(position);
            }

            if (balls[0].playerId >= 0 || (!singlePlayer && balls[1].playerId >= 0)
                || balls[0].mouseSelected || (!singlePlayer && balls[1].mouseSelected))
                timer.start();

            if (pauseButton.isSelected())
            {
                timer.stop();
                foreach (Ball ball in balls)
                {
                    ball.playerId = -1;
                    ball.mouseSelected = false;
                }
                foreach (Player player in Program.game.players)
                    player.visible = true;
                pauseButton.selectable = false;
                Program.game.startPauseSelectionScreen();
            }

            if (balls[0].overlaps(goal) && (singlePlayer || balls[1].overlaps(goal)))
            {
                goalColor = Color.Green;
                timer.stop();
                foreach (Ball ball in balls)
                {
                    ball.playerId = -1;
                    ball.mouseSelected = false;
                }
                foreach (Player player in Program.game.players)
                    player.visible = true;
                Program.game.soundEffectPlayer.playGoal();
                pauseButton.selectable = false;
                Program.game.startScoreScreen(timer.time, wallHits);
            }
        }

        public void detectWalls(int ballNum, ref Point nextPosition)
        {
            Point desPosition = nextPosition;
            bool sliding = false, corner = false, hit = false;

            foreach (Rectangle wall in walls)
            {
                if (detectWall(ballNum, wall, ref nextPosition, desPosition, ref sliding, ref corner))
                    hit = true;
            }

            if (hit && !prevHit[ballNum])
            {
                wallHits++;
                prevHit[ballNum] = true;
                Program.game.soundEffectPlayer.playWall();
                wallColor = Color.DarkRed;
            }
            else if (!hit && prevHit[ballNum])
            {
                wallColor = Color.Black;
                prevHit[ballNum] = false;
            }
        }

        private bool detectWall(int ballNum, Rectangle wall, ref Point nextPosition, Point oldPosition, ref bool sliding, ref bool corner)
        {
            Ball ball = balls[ballNum];

            int spriteTop = (int)ball.position.Y;
            int spriteBottom = (int)ball.position.Y + ball.height;
            int spriteLeft = (int)ball.position.X;
            int spriteRight = (int)ball.position.X + ball.width;

            int nextTop = (int)nextPosition.Y - ball.height / 2;
            int nextBottom = (int)nextPosition.Y + ball.height / 2;
            int nextLeft = (int)nextPosition.X - ball.width / 2;
            int nextRight = (int)nextPosition.X + ball.width / 2;

            bool hit = false;

            if (spriteBottom > wall.Top && spriteTop < wall.Bottom)
            {
                if (spriteRight <= wall.Left && (nextRight > wall.Left ||
                    (!sliding && corner && nextRight >= wall.Left)))
                {
                    nextPosition.X = wall.Left - ball.width / 2;
                    if (!sliding && corner)
                        nextPosition.Y = oldPosition.Y;
                    sliding = true;
                    hit = true;
                }
                if (spriteLeft >= wall.Right && (nextLeft < wall.Right ||
                    (!sliding && corner && nextLeft <= wall.Right)))
                {
                    nextPosition.X = wall.Right + ball.width / 2;
                    if (!sliding && corner)
                        nextPosition.Y = oldPosition.Y;
                    sliding = true;
                    hit = true;
                }
            }
            if (spriteRight > wall.Left && spriteLeft < wall.Right)
            {
                if (spriteBottom <= wall.Top && (nextBottom > wall.Top ||
                    (!sliding && corner && nextBottom >= wall.Top)))
                {
                    nextPosition.Y = wall.Top - ball.height / 2;
                    if (!sliding && corner)
                        nextPosition.X = oldPosition.X;
                    sliding = true;
                    hit = true;
                }
                if (spriteTop >= wall.Bottom && (nextTop < wall.Bottom ||
                    (!sliding && corner && nextTop <= wall.Bottom)))
                {
                    nextPosition.Y = wall.Bottom + ball.width / 2;
                    if (!sliding && corner)
                        nextPosition.X = oldPosition.X;
                    sliding = true;
                    hit = true;
                }
            }

            if (spriteRight <= wall.Left && spriteBottom <= wall.Top && nextRight > wall.Left && nextBottom > wall.Top)
            {
                nextPosition.X = wall.Left - ball.width / 2;
                nextPosition.Y = wall.Top - ball.height / 2;
                /*if (oldPosition.X - spriteRight > oldPosition.Y - spriteBottom)
                    nextPosition.X++;
                else
                    nextPosition.Y++;*/
                hit = true;
                corner = true;
            }
            if (spriteRight <= wall.Left && spriteTop >= wall.Bottom && nextRight > wall.Left && nextTop < wall.Bottom)
            {
                nextPosition.X = wall.Left - ball.width / 2;
                nextPosition.Y = wall.Bottom + ball.height / 2;
                /*if (oldPosition.X - spriteRight >= spriteTop - oldPosition.Y)
                    nextPosition.X++;
                else
                    nextPosition.Y++;*/
                hit = true;
                corner = true;
            }
            if (spriteLeft >= wall.Right && spriteBottom <= wall.Top && nextLeft < wall.Right && nextBottom > wall.Top)
            {
                nextPosition.X = wall.Right + ball.width / 2;
                nextPosition.Y = wall.Top - ball.height / 2;
                /*if (spriteLeft - oldPosition.X > oldPosition.Y - spriteBottom)
                    nextPosition.X++;
                else
                    nextPosition.Y++;*/
                hit = true;
                corner = true;
            }
            if (spriteLeft >= wall.Right && spriteTop >= wall.Bottom && nextLeft < wall.Right && nextTop < wall.Bottom)
            {
                nextPosition.X = wall.Right + ball.width / 2;
                nextPosition.Y = wall.Bottom + ball.height / 2;
                /*if (spriteLeft - oldPosition.X >= spriteTop - oldPosition.Y)
                    nextPosition.X++;
                else
                    nextPosition.Y++;*/
                hit = true;
                corner = true;
            }
            
            return hit;
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
            Sprite sprite;

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
            else if (words[0] == "goal" || words[0] == "wall" || words[0] == "door")
            {
                if (words.Length != 5)
                    return false;
                rect = new Rectangle(Program.game.sx(Convert.ToInt32(words[1])), Program.game.sy(Convert.ToInt32(words[2])),
                                        Convert.ToInt32(words[3]), Convert.ToInt32(words[4]));

                if (words[0] == "goal")
                    goal = rect;
                else if (words[0] == "wall")
                    walls.Add(rect);
                else if (words[0] == "door")
                {
                    if (switches.Count == 0)
                        return false;
                    switches[switches.Count - 1].addDoor(rect);
                    walls.Add(rect);
                }
            }
            else if (words[0] == "door_switch")
            {
                if (words.Length != 3)
                    return false;

                switches.Add(new DoorSwitch(Convert.ToBoolean(words[1]), Convert.ToInt32(words[2])));
            }
            else if (words[0] == "switch")
            {
                if (words.Length != 5 || switches.Count == 0)
                    return false;
                sprite = new Sprite(new Point(Program.game.sx(Convert.ToInt32(words[1])), Program.game.sy(Convert.ToInt32(words[2]))),
                    Convert.ToInt32(words[3]), Convert.ToInt32(words[4]));

                switches[switches.Count - 1].addSwitch(sprite);
            }
            else
                return false;

            return true;
        }
    }
}