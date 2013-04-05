using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;

namespace MazeAndBlue
{
    public class Player : Sprite
    {
        int id;
        float yRange, xRangeMin, xRangeMax;
        Color color;
        Texture2D rh, lh;
        
        public bool rightHanded { get; private set; }

        public Player(float xmin, float xmax, Color c, int playerNum)
        {
            rightHanded = true;
            yRange = 0.5f;
            xRangeMin = xmin;
            xRangeMax = xmax;
            color = c;
            id = playerNum;
        }

        public void loadContent()
        {
            rh = Program.game.Content.Load<Texture2D>("righthand");
            lh = Program.game.Content.Load<Texture2D>("lefthand");

            if (rightHanded)
                texture = rh;
            else
                texture = lh;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            draw(spriteBatch, color);
        }

        public void update(Skeleton skeleton)
        {          
            if (skeleton != null)
                moveHand(getPosition(skeleton));
        }

        public void switchHand(bool righthand)
        {
            rightHanded = righthand;
            if (rightHanded)
                texture = rh;
            else
                texture = lh;
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

        private Point getPosition(Skeleton skeleton)
        {
            SkeletonPoint point;
            if (rightHanded)
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

            int x = (int)(Program.game.screenWidth * xPercent);
            int y = (int)(Program.game.screenHeight * (1 - yPercent));
            
            return new Point(x, y);
        }

        private void moveHand(Point pos)
        {
            pos.X -= width / 2;
            pos.Y -= height / 2;
            position = pos;
        }

    }
}
