using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Kinect;

namespace MazeAndBlue
{
    class Player
    {
        Sprite ball, hand;
        float yRange, xRangeMin, xRangeMax;
        
        public bool selected { get; set; }
        public bool mouseSelected { get; set; }
        public bool righthanded { get; set; }
        public Color color { get; set; }
        int ID;

        public Player(Vector2 ballPos, float xmin, float xmax, Color c, int playerNum)
        {
            ball = new Sprite(ballPos);
            hand = new Sprite(new Vector2(0, 0));
            selected = false;
            mouseSelected = false;
            righthanded = true;
            yRange = 0.5f;
            xRangeMin = xmin;
            xRangeMax = xmax;
            color = c;
            ID = playerNum;
        }

        public void loadContent(ContentManager content)
        {
            ball.loadContent(content, "ball");
            hand.loadContent(content, "hand");
        }

        public void drawBall(SpriteBatch spriteBatch)
        {
            ball.draw(spriteBatch, color);
        }

        public void drawHand(SpriteBatch spriteBatch)
        {
            if (!selected)
                hand.draw(spriteBatch, color);
        }

        public void update(Skeleton skeleton, Maze maze, voiceControl VC)
        {
            Vector2 position;
            if (mouseSelected)
            {
                MouseState ms = Mouse.GetState();
                position = new Vector2(ms.X, ms.Y);
            }
            else
            {
                if (skeleton == null)
                    return;
                position = getPosition(skeleton);
            }

            if (selected || mouseSelected)
                moveBall(position, maze);
            else
                moveHand(position);

            if (VC != null)
            {
                if (hand.overlaps(new Rectangle((int)ball.position.X, (int)ball.position.Y, ball.width, ball.height)) &&
                    VC.states.selectStated[ID])
                {
                    VC.states.select[ID] = true;
                }
                selected = VC.states.select[ID];
                VC.states.selectStated[ID] = false;
            }
        }

        private Vector2 getPosition(Skeleton skeleton)
        {
            SkeletonPoint point;
            if (righthanded)
                point = skeleton.Joints[JointType.HandRight].Position;
            else
                point = skeleton.Joints[JointType.HandLeft].Position;
            
            float xPercent = (point.X - xRangeMin) / (xRangeMax - xRangeMin);
            if (xPercent < 0)
                xPercent = 0;
            if (xPercent > 1)
                xPercent = 1;

            float yPercent = (point.Y / yRange) + 0.5f;
            if (yPercent < 0)
                yPercent = 0;
            if (yPercent > 1)
                yPercent = 1;

            float x = (float)Program.game.screenWidth * xPercent;
            float y = (float)Program.game.screenHeight * (1 - yPercent);
            return new Vector2(x, y);
        }

        private void moveHand(Vector2 pos)
        {
            pos.X -= hand.width / 2;
            pos.Y -= hand.height / 2;
            hand.position = pos;
        }

        private void moveBall(Vector2 pos, Maze maze)
        {
            pos.X -= ball.width / 2;
            pos.Y -= ball.height / 2;
            maze.detectWalls(ball, ref pos);
            ball.position = pos;
        }

        public bool overlaps(Rectangle rect)
        {
            return ball.overlaps(rect);
        }

        public void onLeftClick(Point point)
        {
            if (ball.contains(point))
                mouseSelected = !mouseSelected;
        }

    }
}
