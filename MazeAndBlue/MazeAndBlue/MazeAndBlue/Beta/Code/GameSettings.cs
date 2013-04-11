using System;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;

namespace MazeAndBlue
{
    class SettingData
    {
        public bool p1RightHanded;
        public bool p2RightHanded;
        public int volume; //quiet=0; avg=1; noisy=2
        public bool soundsOn;

        public SettingData()
        {
            p1RightHanded = true;
            p2RightHanded = true;
            volume = 1;
            soundsOn = true;
        }
    };

    public class GameSettings
    {
        SettingData data;
        const string filename = "gameSettings.sav";
        
        public GameSettings()
        {
            data = new SettingData();
            if (!loadSettings())
                resetData();
            applySettings();
        }

        public void resetData()
        {
            data = new SettingData();
        }

        public void updateP1PrimaryHand(bool rightHand)
        {
            data.p1RightHanded = rightHand;
        }

        public bool getP1RightHanded()
        {
            return data.p1RightHanded;
        }

        public void updateP2PrimaryHand(bool rightHand)
        {
            data.p2RightHanded = rightHand;
        }

        public bool getP2RightHanded()
        {
            return data.p2RightHanded;
        }

        public void updateVolume(int vol)
        {
            data.volume = vol;
        }

        public int getVolume()
        {
            return data.volume;
        }

        public void updateSound(bool sound)
        {
            data.soundsOn = sound;
        }

        public bool getSoundsOn()
        {
            return data.soundsOn;
        }

        public void saveSettings()
        {
            string[] lines = new string[4];
            lines[0] = data.p1RightHanded.ToString();
            lines[1] = data.p2RightHanded.ToString();
            lines[2] = data.volume.ToString();
            lines[3] = data.soundsOn.ToString();

            File.WriteAllLines(filename, lines);
        }

        public bool loadSettings()
        {
            if (!File.Exists(filename))
                return false;

            string[] lines = File.ReadAllLines(filename);

            if (lines.Length != 4)
                return false;

            if (lines[0] == "True")
                data.p1RightHanded = true;
            else if (lines[0] == "False")
                data.p1RightHanded = false;
            else
                return false;

            if (lines[1] == "True")
                data.p2RightHanded = true;
            else if (lines[1] == "False")
                data.p2RightHanded = false;
            else
                return false;

            if (lines[2] == "0")
                data.volume = 0;
            else if (lines[2] == "1")
                data.volume = 1;
            else if (lines[2] == "2")
                data.volume = 2;
            else
                return false;

            if (lines[3] == "True")
                data.soundsOn = true;
            else if (lines[3] == "False")
                data.soundsOn = false;
            else
                return false;

            return true;
        }

        public void applySettings()
        {
            Program.game.players[0].switchHand(data.p1RightHanded);
            Program.game.players[1].switchHand(data.p2RightHanded);

            switch (data.volume)
            {
                case 0:
                    Program.game.vs.precision = 0.6;
                    break;
                case 1:
                    Program.game.vs.precision = 0.5;
                    break;
                case 2:
                    Program.game.vs.precision = 0.4;
                    break;
            }

            Program.game.soundEffectPlayer.soundsOn = data.soundsOn;
        }

    }
}
