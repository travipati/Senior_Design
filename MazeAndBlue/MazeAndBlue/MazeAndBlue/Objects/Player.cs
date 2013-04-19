using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;

namespace MazeAndBlue
{
    public class Player : Sprite
    {
        int id;
        Color color;
        Texture2D rh, lh;
        bool hovering;
        Timer hoverTime;
        Button hoverBt;
        ProgressCircle pc;
        DateTime startTime;
        
        public bool rightHanded { get; private set; }
        public bool visible { get; set; }

        public Player(float xmin, float xmax, Color c, int playerNum)
        {
            rightHanded = true;
            visible = true;
            color = c;
            id = playerNum;
            hovering = false;
            hoverTime = new Timer();
            pc = new ProgressCircle();
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
            if (hovering)
                pc.draw(spriteBatch, hoverTime.time * 1000 + (DateTime.Now - startTime).Milliseconds, position);
        }

        public void update(Skeleton skeleton)
        {          
            if (skeleton != null)
                moveHand(getPosition(skeleton));

            if (hovering && !overlaps(hoverBt))
                deselect();
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

        public bool buttonSelecting(Button bt)
        {
            if (!hovering && overlaps(bt))
            {
                startTime = DateTime.Now;
                hoverTime.start();
                hovering = true;
                hoverBt = bt;
            }
            if (hoverTime.time == 3 && overlaps(bt))
                return true;

            return false;
        }

        public void deselect()
        {
            hoverTime.stop();
            hoverTime.time = 0;
            hovering = false;
        }

        public void setMovementRange(Skeleton skeleton)
        {
            SkeletonPoint center = skeleton.Joints[JointType.ShoulderCenter].Position;
            SkeletonPoint point;

            if (rightHanded)
            {
                point = skeleton.Joints[JointType.HandRight].Position;
                Program.game.movementRange[id] = (point.X - center.X);
                Program.game.yPreference[id] = point.Y - center.Y;
            }
            else
            {
                point = skeleton.Joints[JointType.HandLeft].Position;
                Program.game.movementRange[id] = (center.X - point.X);
                Program.game.yPreference[id] = point.Y - center.Y;
            }
            //Program.game.movementRange[id] = (Program.game.movementRange[id] * .95f);
        }

        private Point getPosition(Skeleton skeleton)
        {
            SkeletonPoint center = skeleton.Joints[JointType.ShoulderCenter].Position;
            SkeletonPoint point;

            float xPercent = 0;
            if (rightHanded)
            {
                point = skeleton.Joints[JointType.HandRight].Position;
                xPercent = (point.X - (center.X)) / (Program.game.movementRange[id] * .7f);
            }
            else
            {
                point = skeleton.Joints[JointType.HandLeft].Position;
                xPercent = (((center.X) - point.X) / (Program.game.movementRange[id] * .7f));
            }
             
            if (xPercent < 0)
                xPercent = 0;
            if (xPercent > 1)
                xPercent = 1;

            float yPercent = ((center.Y - Program.game.yPreference[id]) - point.Y) / (Program.game.movementRange[id] * .7f);
            if (yPercent < 0)
                yPercent = 0;
            if (yPercent > 1)
                yPercent = 1;

            int x = (int)(Program.game.screenWidth * xPercent);
            if (!rightHanded)
            {
                x = Program.game.screenWidth - x;
            }
            int y = (int)(Program.game.screenHeight * (yPercent));
            
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
