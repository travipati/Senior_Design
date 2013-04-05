using System;
using System.Linq;
using Microsoft.Kinect;

namespace MazeAndBlue
{
    class Kinect
    {
        KinectSensor sensor;
        public Skeleton[] playerSkeleton;

        public Kinect()
        {
            playerSkeleton = new Skeleton[2];
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            sensor = (from s in KinectSensor.KinectSensors.ToArray() where s.Status == KinectStatus.Connected select s).FirstOrDefault();
            if (sensor != null)
                initializeKinect();
        }

        ~Kinect()
        {
            sensor.Stop();
        }

        public KinectSensor getSensorReference()
        {
            return sensor;
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (e.Status == KinectStatus.Connected)
            {
                if (sensor != null)
                    sensor.Stop();
                sensor = e.Sensor;
            }
            else if (e.Status == KinectStatus.Disconnected)
            {
                if (sensor != null)
                    sensor.Stop();
                sensor = null;
            }

            if (sensor != null)
                initializeKinect();
        }

        private void initializeKinect()
        {
            sensor.SkeletonStream.Enable(new TransformSmoothParameters()
            {
                Smoothing = 0.6f,//0.5f,
                Correction = 0.4f,//0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });

            sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(sensor_SkeletonFrameReady);
            sensor.Start();
        }

        void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame == null)
                    return;

                Skeleton[] skeletonArray = new Skeleton[frame.SkeletonArrayLength];
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

                if (i == 2 && playerSkeleton[0].Joints[JointType.Head].Position.X > playerSkeleton[1].Joints[JointType.Head].Position.X)
                {
                    Skeleton temp = playerSkeleton[0];
                    playerSkeleton[0] = playerSkeleton[1];
                    playerSkeleton[1] = temp;
                }

            }
        }

    }
}
