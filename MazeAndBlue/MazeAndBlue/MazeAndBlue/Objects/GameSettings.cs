using System;
using System.Collections.Generic;
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
        public bool unlockOn;
        public List<float> movementRange;
        public List<float> yPreference;

        public SettingData()
        {
            movementRange = new List<float>();
            yPreference = new List<float>();

            p1RightHanded = true;
            p2RightHanded = true;
            volume = 1;
            soundsOn = true;
            unlockOn = true;
            movementRange.Add(0.75f);
            movementRange.Add(0.75f);
            yPreference.Add(-.1f);
            yPreference.Add(-.1f);
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

        public void updateUnlock(bool unlock)
        {
            data.unlockOn = unlock;
        }

        public bool getUnlockOn()
        {
            return data.unlockOn;
        }

        public void setMovmentRange (int i, float val)
        {
            data.movementRange[i] = val;
        }

        public float getMovmentRange (int i)
        {
            return data.movementRange[i];
        }

        public void setYpreference (int i, float val)
        {
            data.yPreference[i] = val;
        }

        public float getYpreference (int i)
        {
            return data.yPreference[i];
        }

        public void saveSettings()
        {
            string[] lines = new string[9];
            lines[0] = data.p1RightHanded.ToString();
            lines[1] = data.p2RightHanded.ToString();
            lines[2] = data.volume.ToString();
            lines[3] = data.soundsOn.ToString();
            lines[4] = data.unlockOn.ToString();
            lines[5] = data.movementRange[0].ToString();
            lines[6] = data.movementRange[1].ToString();
            lines[7] = data.yPreference[0].ToString();
            lines[8] = data.yPreference[1].ToString();

            File.WriteAllLines(filename, lines);
        }

        public bool loadSettings()
        {
            if (!File.Exists(filename))
                return false;

            string[] lines = File.ReadAllLines(filename);

            if (lines.Length != 9)
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

            if (lines[4] == "True")
                data.unlockOn = true;
            else if (lines[4] == "False")
                data.unlockOn = false;
            else
                return false;

            data.movementRange[0] = float.Parse(lines[5]);
            data.movementRange[1] = float.Parse(lines[6]);

            data.yPreference[0] = float.Parse(lines[7]);
            data.yPreference[1] = float.Parse(lines[8]);

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
            Program.game.unlockOn = data.unlockOn;

            Program.game.movementRange = data.movementRange;
            Program.game.yPreference = data.yPreference;
        }

    }
}
