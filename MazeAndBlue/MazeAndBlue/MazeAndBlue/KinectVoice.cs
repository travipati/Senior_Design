using System;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace MazeAndBlue
{
    class voiceControl
    {
        public selectStates states;
        KinectSensor sensor;
        SpeechRecognitionEngine speechRec;

        public voiceControl()
        {
            states.select = new bool[] { false, false };
            states.selectStated = new bool[] { false, false };

            RecognizerInfo ri = GetKinectRecognizer();

            SpeechRecognitionEngine tempSpeechRec;

            tempSpeechRec = new SpeechRecognitionEngine(ri.Id);

            var grammar = new Choices();
            grammar.Add("select one", "SELECT ONE");
            grammar.Add("select two", "SELECT TWO");
            grammar.Add("pause", "PAUSE");
            grammar.Add("restart", "PAUSE");

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(grammar);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            var g = new Grammar(gb);

            tempSpeechRec.LoadGrammar(g);
            tempSpeechRec.SpeechRecognized += phraseRecognized;

            speechRec = tempSpeechRec;
        }

        public void recognizeSpeech(KinectSensor inputSensor)
        {
            if (inputSensor == null)
                return;
            sensor = inputSensor;
            try
            {
                speechRec.SetInputToAudioStream(
                    sensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechRec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                System.Windows.Forms.Application.Exit();
            }
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
            if (e.Result.Confidence < 0.5)
            {
                return;
            }

            switch (e.Result.Text.ToLower())
            {
                case "select one":
                    if (states.select[0])
                    {
                        states.select[0] = false;
                    }
                    else
                    {
                        states.selectStated[0] = true;
                    }
                    break;
                case "select two":
                    if (states.select[1])
                    {
                        states.select[1] = false;
                    }
                    else
                    {
                        states.selectStated[1] = true;
                    }
                    break;
                case "pause":
                    //pause the game
                    break;
                case "restart":
                    /*this.Visibility = Visibility.Collapsed;
                    MainWindow window = new MainWindow();
                    window.Show();*/
                    break;
                default:
                    break;
            }
        }
    }
}
