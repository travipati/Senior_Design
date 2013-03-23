using System;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace MazeAndBlue
{
    public class VoiceControl
    {
        KinectSensor sensor;
        SpeechRecognitionEngine speechRec;
        double precision;
        public bool newWordReady;
        public string word;

        public VoiceControl()
        {
            precision = .5;
            newWordReady = false;

            RecognizerInfo ri = GetKinectRecognizer();

            SpeechRecognitionEngine tempSpeechRec;

            tempSpeechRec = new SpeechRecognitionEngine(ri.Id);

            var grammar = new Choices();
            grammar.Add("select one", "SELECT ONE");
            grammar.Add("select two", "SELECT TWO");
            grammar.Add("exit", "EXIT");
            grammar.Add("pause", "PAUSE");
            grammar.Add("restart", "RESTART");
            grammar.Add("co op mode", "CO-OP MODE");

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

        private void phraseRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < precision)
                return;

            word = e.Result.Text.ToLower();
            newWordReady = true;
        }
    }
}
