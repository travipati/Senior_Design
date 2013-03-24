using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Kinect;

namespace MazeAndBlue
{
    public class Player
    {
        Sprite hand;
        float yRange, xRangeMin, xRangeMax;
        
        public bool righthanded { get; set; }
        Color color;
        int id;

        public Player(float xmin, float xmax, Color c, int playerNum)
        {
            hand = new Sprite();
            righthanded = true;
            yRange = 0.5f;
            xRangeMin = xmin;
            xRangeMax = xmax;
            color = c;
            id = playerNum;
        }

        public void loadContent(ContentManager content)
        {
            hand.loadContent(content, "hand");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            hand.draw(spriteBatch, color);
        }

        public void update(Skeleton skeleton)
        {          
            if (skeleton != null)
                moveHand(getPosition(skeleton));
        }

        public bool selecting()
        {
            string sid = string.Empty;
            switch (id)
            {
                case 0:
                    sid = "one";
                    break;
                case 1:
                    sid = "two";
                    break;
            }

            if (Program.game.vs.newWordReady && Program.game.vs.word == "select " + sid)
            {
                Program.game.vs.newWordReady = false;
                return true;
            }
            else if (Program.game.ks.newKeyReady && Program.game.ks.key == (id + 1).ToString())
            {
                Program.game.ks.newKeyReady = false;
                return true;
            }
            return false;
        }

        public Point getPosition()
        {
            return hand.position;
        }

        private Point getPosition(Skeleton skeleton)
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

            int x = Program.game.screenWidth * (int)xPercent;
            int y = Program.game.screenHeight * (int)(1 - yPercent);
            return new Point(x, y);
        }

        private void moveHand(Point pos)
        {
            pos.X -= hand.width / 2;
            pos.Y -= hand.height / 2;
            hand.position = pos;
        }

        public bool overlaps(Sprite sprite)
        {
            return hand.overlaps(sprite);
        }

        public bool overlaps(Rectangle rect)
        {
            return hand.overlaps(rect);
        }
    }
}
