﻿using System;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace MazeAndBlue
{
    public class VoiceSelect
    {
        KinectSensor sensor;
        SpeechRecognitionEngine speechRec;
        public double precision;
        public bool newWordReady;
        public string word;

        public VoiceSelect()
        {
            precision = .7;
            newWordReady = false;

            RecognizerInfo ri = GetKinectRecognizer();

            SpeechRecognitionEngine tempSpeechRec;

            tempSpeechRec = new SpeechRecognitionEngine(ri.Id);

            var grammar = new Choices();
            grammar.Add("select one", "SELECT ONE", "Select One", "player one select", "PLAYER ONE SELECT");
            grammar.Add("select two", "SELECT TWO", "Select Two", "player two select", "PLAYER TWO SELECT");
            grammar.Add("exit", "EXIT");
            grammar.Add("single mode", "Single MODE");
            grammar.Add("co op mode", "CO OP MODE");
            grammar.Add("settings", "SETTINGS");
            grammar.Add("instructions", "INSTRUCTIONS");
            grammar.Add("statistics", "STATISTICS");
            grammar.Add("Main Menu", "MAIN MENU");
            grammar.Add("Easy", "EASY");
            grammar.Add("Hard", "HARD");
            grammar.Add("level one");
            grammar.Add("level two");
            grammar.Add("level three");
            grammar.Add("level four");
            grammar.Add("level five");
            grammar.Add("level six");
            grammar.Add("player one left");
            grammar.Add("player one right");
            grammar.Add("player two left");
            grammar.Add("player two right");
            grammar.Add("room low");
            grammar.Add("room medium");
            grammar.Add("room high");
            grammar.Add("sounds on");
            grammar.Add("sounds off");
            grammar.Add("reset stats");
            grammar.Add("pause", "PAUSE");
            grammar.Add("resume", "RESUME");
            grammar.Add("restart level", "RESTART LEVEL");
            grammar.Add("next", "NEXT");

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(grammar);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            var g = new Grammar(gb);

            tempSpeechRec.LoadGrammar(g);
            tempSpeechRec.SpeechRecognized += phraseRecognized;
            tempSpeechRec.SpeechHypothesized += phraseHyphothesized;
            tempSpeechRec.SpeechRecognitionRejected += phraseRejected;

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
            //System.Windows.Forms.MessageBox.Show("Recognized: \"" + e.Result.Text + "\" " + e.Result.Confidence);
            if (e.Result.Confidence < precision)
                return;

            word = e.Result.Text.ToLower();
            newWordReady = true;
        }
        private void phraseHyphothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Hypothesized: " + e.Result.Text + " " + e.Result.Confidence);
        }
        private void phraseRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Rejected: " + (e.Result == null ? string.Empty : e.Result.Text + " " + e.Result.Confidence));
        }
    }
}
