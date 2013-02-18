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
        Skeleton playerSkeleton;
        SkeletonPoint handPosition;
        Rectangle[] walls;
        Point nextPosition;
        double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
        double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
        bool isSelected = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Height = screenHeight;
            this.Width = screenWidth;

            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            sensor = (from s in KinectSensor.KinectSensors.ToArray() where s.Status == KinectStatus.Connected select s).FirstOrDefault();
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

                playerSkeleton = (from s in skeletonArray where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();
                if (playerSkeleton != null)
                    handPosition = playerSkeleton.Joints[JointType.HandRight].ScaleTo((int)screenWidth, (int)screenHeight).Position;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (p1ball.IsMouseOver)
                isSelected = true;

 	        base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isSelected)
            {
                nextPosition.Y = e.GetPosition(panel).Y - p1ball.Height / 2;
                nextPosition.X = e.GetPosition(panel).X - p1ball.Width / 2;

                foreach (Rectangle wall in walls)
                    detectWall(wall);

                Canvas.SetTop(p1ball, nextPosition.Y);
                Canvas.SetLeft(p1ball, nextPosition.X);

                if (isGoalReached())
                    goal.Fill = new SolidColorBrush(Colors.Green);
            }

            base.OnMouseMove(e);
        }

        private void detectWall(Rectangle wall)
        {
            if (nextPosition.Y + p1ball.Height > Canvas.GetTop(wall) && nextPosition.Y < Canvas.GetTop(wall) + wall.Height)
            {
                if (Canvas.GetLeft(p1ball) + p1ball.Width <= Canvas.GetLeft(wall) && nextPosition.X + p1ball.Width > Canvas.GetLeft(wall))
                    nextPosition.X = Canvas.GetLeft(wall) - p1ball.Width;
                if (Canvas.GetLeft(p1ball) >= Canvas.GetLeft(wall) + wall.Width && nextPosition.X < Canvas.GetLeft(wall) + wall.Width)
                    nextPosition.X = Canvas.GetLeft(wall) + wall.Width;
            }
            if (nextPosition.X + p1ball.Width > Canvas.GetLeft(wall) && nextPosition.X < Canvas.GetLeft(wall) + wall.Width)
            {
                if (Canvas.GetTop(p1ball) + p1ball.Height <= Canvas.GetTop(wall) && nextPosition.Y + p1ball.Height > Canvas.GetTop(wall))
                    nextPosition.Y = Canvas.GetTop(wall) - p1ball.Height;
                if (Canvas.GetTop(p1ball) >= Canvas.GetTop(wall) + wall.Height && nextPosition.Y < Canvas.GetTop(wall) + wall.Height)
                    nextPosition.Y = Canvas.GetTop(wall) + wall.Height;
            }
        }

        private bool isGoalReached()
        {
            return Canvas.GetTop(p1ball) + p1ball.Height > Canvas.GetTop(goal) && Canvas.GetTop(p1ball) < Canvas.GetTop(goal) + goal.Height
                && Canvas.GetLeft(p1ball) + p1ball.Width > Canvas.GetLeft(goal) && Canvas.GetLeft(p1ball) < Canvas.GetLeft(goal) + goal.Width;
        }
    }
}
