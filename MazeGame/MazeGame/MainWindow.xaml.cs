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
using System.Windows.Threading;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace MazeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        KinectSensor sensor;
        Skeleton[] skeletonArray;
        Skeleton[] playerSkeleton;
        SkeletonPoint[] handPosition;
        Rectangle[] walls;
        bool[] isSelected;
        bool[] isMouseSelected;
        bool isOverLappedPrevCheck;
        bool[] isRightPrim;
        int gameTime;
        Point nextPosition;
        double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
        double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
        SpeechRecognitionEngine speechRec;
        

        public MainWindow()
        {
            InitializeComponent();
            this.Height = screenHeight;
            this.Width = screenWidth;

            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            sensor = (from s in KinectSensor.KinectSensors.ToArray() where s.Status == KinectStatus.Connected select s).FirstOrDefault();
            if (sensor != null)
            {
                initializeKinect(sensor);
            }


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

            isOverLappedPrevCheck = false;

            isSelected = new bool[2];
            isSelected[0] = false;
            isSelected[1] = false;

            isMouseSelected = new bool[2];
            isMouseSelected[0] = false;
            isMouseSelected[1] = false;

            isRightPrim = new bool[2];
            isRightPrim[0] = true;
            isRightPrim[1] = true;

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0,0,1);

            time.Text = "Time: " + gameTime + " sec";
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

            speechRec = CreateSpeechRecognizer();
        }

        void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame == null)
                {
                    return;
                }

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

                handPosition[0] = getHandPosition(0, true);
                handPosition[1] = getHandPosition(1, true);

                moveHand(p1hand, handPosition[0].X, handPosition[0].Y);
                moveHand(p2hand, handPosition[1].X, handPosition[1].Y);

                //                if (isHandSelect(0) && isObjOver(p1hand, p1ball))
                if (isObjOver(p1hand, p1ball))
                {
                    //                    isSelected[0] = !isSelected[0];
                    isSelected[0] = true;
                    if (isSelected[0])
                        p1hand.Visibility = Visibility.Collapsed;
                    else
                        p1hand.Visibility = Visibility.Visible;
                }
                //                if (isHandSelect(1) && isObjOver(p2hand, p2ball))
                if (isObjOver(p2hand, p2ball))
                {
                    //                    isSelected[1] = !isSelected[1];
                    isSelected[1] = true;
                    if (isSelected[1])
                        p2hand.Visibility = Visibility.Collapsed;
                    else
                        p2hand.Visibility = Visibility.Visible;
                }

                if (isSelected[0] || (isSelected[1]))
                    timer.Start();

                if (isSelected[0])
                    moveBall(p1ball, handPosition[0].X, handPosition[0].Y);
                if (isSelected[1])
                    moveBall(p2ball, handPosition[1].X, handPosition[1].Y);
            }
        }

        private void timerTick(object sender, EventArgs e)
        {
            gameTime++;
            time.Text = "Time: " + gameTime + " sec";
        }

        private SkeletonPoint getHandPosition(int playerId, bool isPrimHand)
        {
            /*
    One player using each hand to control one ball
    if (playerSkeleton[0] != null)
    {
        float yRange = 0.5f;
        float xRangeMinLeft = -0.5f;
        float xRangeMaxLeft = 0f;
        float xRangeMinRight = 0f;
        float xRangeMaxRight = 0.5f;
        float xPercentLeft = (playerSkeleton[0].Joints[JointType.HandLeft].Position.X - xRangeMinLeft) / (xRangeMaxLeft - xRangeMinLeft);
        if (xPercentLeft < 0) xPercentLeft = 0;
        if (xPercentLeft > 1) xPercentLeft = 1;
        float yPercentLeft = (playerSkeleton[0].Joints[JointType.HandLeft].Position.Y / yRange) + 0.5f;
        if (yPercentLeft < 0) yPercentLeft = 0;
        if (yPercentLeft > 1) yPercentLeft = 1;
        float xPercentRight = (playerSkeleton[0].Joints[JointType.HandRight].Position.X - xRangeMinRight) / (xRangeMaxRight - xRangeMinRight);
        if (xPercentRight < 0) xPercentRight = 0;
        if (xPercentRight > 1) xPercentRight = 1;
        float yPercentRight = (playerSkeleton[0].Joints[JointType.HandRight].Position.Y / yRange) + 0.5f;
        if (yPercentRight < 0) yPercentRight = 0;
        if (yPercentRight > 1) yPercentRight = 1;
        handPosition[0].X = (float)screenWidth * xPercentLeft;
        handPosition[0].Y = (float)screenHeight * (1 - yPercentLeft);
        handPosition[1].X = (float)screenWidth * xPercentRight;
        handPosition[1].Y = (float)screenHeight * (1 - yPercentRight);

    }
*/
            SkeletonPoint position = new SkeletonPoint();

            if (playerSkeleton[playerId] != null)
            {
                float yRange = 0.5f;
                float xRangeMin;
                float xRangeMax;
                if (playerId == 0)
                {
                    xRangeMin = -0.5f;
                    xRangeMax = 0f;
                }
                else
                {
                    xRangeMin = 0f;
                    xRangeMax = 0.5f;
                }

                float xPercent;
                if ((isPrimHand && isRightPrim[playerId]) || (!isPrimHand && !isRightPrim[playerId]))
                    xPercent = (playerSkeleton[playerId].Joints[JointType.HandRight].Position.X - xRangeMin) / (xRangeMax - xRangeMin);
                else
                    xPercent = (playerSkeleton[playerId].Joints[JointType.HandLeft].Position.X - xRangeMin) / (xRangeMax - xRangeMin);

                if (xPercent < 0)
                    xPercent = 0;
                if (xPercent > 1)
                    xPercent = 1;

                float yPercent;
                if ((isPrimHand && isRightPrim[playerId]) || (!isPrimHand && !isRightPrim[playerId]))
                    yPercent = (playerSkeleton[playerId].Joints[JointType.HandRight].Position.Y / yRange) + 0.5f;
                else
                    yPercent = (playerSkeleton[playerId].Joints[JointType.HandLeft].Position.Y / yRange) + 0.5f;

                if (yPercent < 0)
                    yPercent = 0;
                if (yPercent > 1)
                    yPercent = 1;

                position.X = (float)screenWidth * xPercent;
                position.Y = (float)screenHeight * (1 - yPercent);
            }

            return position;
        }

        private bool isObjOver(FrameworkElement obj1, FrameworkElement obj2)
        {
            return Canvas.GetTop(obj1) + obj1.Height > Canvas.GetTop(obj2) && Canvas.GetTop(obj1) < Canvas.GetTop(obj2) + obj2.Height
                && Canvas.GetLeft(obj1) + obj1.Width > Canvas.GetLeft(obj2) && Canvas.GetLeft(obj1) < Canvas.GetLeft(obj2) + obj2.Width;
        }

        private bool isHandSelect(int playerId)
        {
            //Requires pre-scaled hand locations
            //Output: if left hand is in a threshold*threshold box around right hand, return true for select

            int threshold = 20;
            SkeletonPoint npHandPosition = new SkeletonPoint();
            npHandPosition = getHandPosition(playerId, false);

            bool returnVal = false;
            isOverLappedPrevCheck = false;

            if ((npHandPosition.X <= (handPosition[playerId].X + threshold) && npHandPosition.X >= (handPosition[playerId].X - threshold)) &&
                (npHandPosition.Y <= (handPosition[playerId].Y + threshold) && npHandPosition.Y >= (handPosition[playerId].Y - threshold)))
            {
                if (!isOverLappedPrevCheck)
                    returnVal = true;

                isOverLappedPrevCheck = true;
            }

            return returnVal;
        }

        private void moveHand(Image hand, double x, double y)
        {
            nextPosition.Y = y - hand.Height / 2;
            nextPosition.X = x - hand.Width / 2;

            Canvas.SetTop(hand, nextPosition.Y);
            Canvas.SetLeft(hand, nextPosition.X);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (p1ball.IsMouseOver)
                isMouseSelected[0] = !isMouseSelected[0];
            if (p2ball.IsMouseOver)
                isMouseSelected[1] = !isMouseSelected[1];

            if (isMouseSelected[0] || (isMouseSelected[1]))
                timer.Start();

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
            {
                goal.Fill = new SolidColorBrush(Colors.Green);
                timer.Stop();
                ScoreWindow scoreWindow = new ScoreWindow(screenHeight,screenWidth,gameTime);
                this.Visibility = Visibility.Collapsed;
                scoreWindow.Show();
            }
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
            return isObjOver(ball, goal);
        }

        private void recognizeSpeech()
        {
            speechRec.SetInputToAudioStream(
                sensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            speechRec.RecognizeAsync(RecognizeMode.Multiple);
        }

        private SpeechRecognitionEngine CreateSpeechRecognizer()
        {
            RecognizerInfo ri = GetKinectRecognizer();

            SpeechRecognitionEngine speechRec;

            speechRec = new SpeechRecognitionEngine(ri.Id);
           
            var grammar = new Choices();
            grammar.Add("player one select");
            grammar.Add("player two select");
            grammar.Add("pause");

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(grammar);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            var g = new Grammar(gb);

            speechRec.LoadGrammar(g);
            speechRec.SpeechRecognized += phraseRecognized;

            return speechRec;
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        private void phraseRecognized (object sender, SpeechRecognizedEventArgs e)
        {
            //required confidence is relatively high, may want to reduce to the .30 range in quiet rooms
            if (e.Result.Confidence < 0.5)
            {
                updateSpeechInfo("Unsure what was said, please repeat");
                return;
            }

            switch (e.Result.Text.ToLower())
            {
                case "player one select":
                    isSelected[0] = !isSelected[0];
                    break;
                case "player two select":
                    isSelected[1] = !isSelected[1];
                    break;
                case "pause":
                    //pause the game
                    break;
                default:
                    //do we need to handle unrecognized words?
                    break;
            }

            updateSpeechInfo("Recognized: " + e.Result.Text + " " + e.Result.Confidence);
        }

        private void updateSpeechInfo (string instructions)
        {
            Dispatcher.BeginInvoke(new Action(() => { speechInfo.Text = instructions; }), DispatcherPriority.Normal);
        }
    }
}