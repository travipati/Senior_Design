using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;

namespace MazeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor sensor;
        Skeleton[] skeletonArray;
        Skeleton[] playerSkeleton;
        SkeletonPoint[] handPosition;
        Rectangle[] walls;
        bool[] isSelected;
        bool[] isMouseSelected;
        Point nextPosition;
        double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
        double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

        public MainWindow()
        {
            InitializeComponent();
            this.Height = screenHeight;
            this.Width = screenWidth;

            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            sensor = (from s in KinectSensor.KinectSensors.ToArray() where s.Status == KinectStatus.Connected select s).FirstOrDefault();

            playerSkeleton = new Skeleton[2];
            handPosition = new SkeletonPoint[2];

            walls = new Rectangle[10];
            walls[0] = wall0;
            walls[1] = wall1;
            walls[2] = wall2;
            walls[3] = wall3;
            walls[4] = wall4;
            walls[5] = wall5;
            walls[6] = wall6;
            walls[7] = wall7;
            walls[8] = wall8;
            walls[9] = wall9;

            isSelected = new bool[2];
            isSelected[0] = true;
            isSelected[1] = true;

            isMouseSelected = new bool[2];
            isMouseSelected[0] = false;
            isMouseSelected[1] = false;
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (e.Status == KinectStatus.Connected)
            {
                if (sensor != null)
                {
                    sensor.Stop();
                }

                sensor = e.Sensor;
            }
            else if (e.Status == KinectStatus.Disconnected)
            {
                if (sensor != null)
                    sensor.Stop();
                sensor = null;
            }

            if (sensor != null)
            {
                initializeKinect(sensor);
            }
        }

        private void initializeKinect(KinectSensor inSensor)
        {
            inSensor.SkeletonStream.Enable(new TransformSmoothParameters()
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });

            inSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(sensor_SkeletonFrameReady);
            inSensor.Start();
        }

        void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                skeletonArray = new Skeleton[frame.SkeletonArrayLength];
                frame.CopySkeletonDataTo(skeletonArray);

                int i = 0;
                foreach (Skeleton s in skeletonArray)
                {
                    if (s.TrackingState == SkeletonTrackingState.Tracked && i < 2)
                    {
                        playerSkeleton[i] = s;
                        i++;
                    }
                }

                if (playerSkeleton[0] != null)
                    handPosition[0] = playerSkeleton[0].Joints[JointType.HandRight].ScaleTo((int)screenWidth, (int)screenHeight).Position;
                if (playerSkeleton[1] != null)
                    handPosition[1] = playerSkeleton[0].Joints[JointType.HandRight].ScaleTo((int)screenWidth, (int)screenHeight).Position;

                if (isSelected[0])
                    moveBall(p1ball, handPosition[0].X, handPosition[0].Y);
                else if (isSelected[1])
                    moveBall(p2ball, handPosition[1].X, handPosition[1].Y);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (p1ball.IsMouseOver)
                isMouseSelected[0] = !isMouseSelected[0];
            if (p2ball.IsMouseOver)
                isMouseSelected[1] = !isMouseSelected[1];

 	        base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
//            if (Canvas.GetTop(ball) - e.GetPosition(panel).Y > 20 || Canvas.GetLeft(ball) - e.GetPosition(panel).X > 20 ||
//                e.GetPosition(panel).Y - Canvas.GetTop(ball) > p1ball.Height + 20 || e.GetPosition(panel).X - Canvas.GetLeft(ball) > p1ball.Width + 20)
//                isMouseSelected = false;

            if (isMouseSelected[0])
                moveBall(p1ball, e.GetPosition(panel).X, e.GetPosition(panel).Y);
            else if (isMouseSelected[1])
                moveBall(p2ball, e.GetPosition(panel).X, e.GetPosition(panel).Y);

            base.OnMouseMove(e);
        }

        private void moveBall(Ellipse ball, double x, double y)
        {
            nextPosition.Y = y - ball.Height / 2;
            nextPosition.X = x - ball.Width / 2;

            foreach (Rectangle wall in walls)
                detectWall(ball, wall);

            Canvas.SetTop(ball, nextPosition.Y);
            Canvas.SetLeft(ball, nextPosition.X);

            if (isGoalReached(p1ball) && isGoalReached(p2ball))
                goal.Fill = new SolidColorBrush(Colors.Green);
            else
                goal.Fill = new SolidColorBrush(Colors.Red);
        }

        private void detectWall(Ellipse ball, Rectangle wall)
        {
            if (Canvas.GetTop(ball) + ball.Height > Canvas.GetTop(wall) && Canvas.GetTop(ball) < Canvas.GetTop(wall) + wall.Height)
            {
                if (Canvas.GetLeft(ball) + ball.Width <= Canvas.GetLeft(wall) && nextPosition.X + ball.Width > Canvas.GetLeft(wall))
                    nextPosition.X = Canvas.GetLeft(wall) - ball.Width;
                if (Canvas.GetLeft(ball) >= Canvas.GetLeft(wall) + wall.Width && nextPosition.X < Canvas.GetLeft(wall) + wall.Width)
                    nextPosition.X = Canvas.GetLeft(wall) + wall.Width;
            }
            if (Canvas.GetLeft(ball) + ball.Width > Canvas.GetLeft(wall) && Canvas.GetLeft(ball) < Canvas.GetLeft(wall) + wall.Width)
            {
                if (Canvas.GetTop(ball) + ball.Height <= Canvas.GetTop(wall) && nextPosition.Y + ball.Height > Canvas.GetTop(wall))
                    nextPosition.Y = Canvas.GetTop(wall) - ball.Height;
                if (Canvas.GetTop(ball) >= Canvas.GetTop(wall) + wall.Height && nextPosition.Y < Canvas.GetTop(wall) + wall.Height)
                    nextPosition.Y = Canvas.GetTop(wall) + wall.Height;
            }
        }

        private bool isGoalReached(Ellipse ball)
        {
            return Canvas.GetTop(ball) + ball.Height > Canvas.GetTop(goal) && Canvas.GetTop(ball) < Canvas.GetTop(goal) + goal.Height
                && Canvas.GetLeft(ball) + ball.Width > Canvas.GetLeft(goal) && Canvas.GetLeft(ball) < Canvas.GetLeft(goal) + goal.Width;
        }
    }
}
