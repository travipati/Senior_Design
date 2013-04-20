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
        public List<float> xOffset;
        public string goalFile;

        public SettingData()
        {
            movementRange = new List<float>();
            yPreference = new List<float>();
            xOffset = new List<float>();

            p1RightHanded = true;
            p2RightHanded = true;
            volume = 1;
            soundsOn = true;
            unlockOn = true;
            movementRange.Add(0.75f);
            movementRange.Add(0.75f);
            yPreference.Add(.35f);
            yPreference.Add(.35f);
            xOffset.Add(.10f);
            xOffset.Add(.10f);
            goalFile = "null";
        }
    };

    public class GameSettings
    {
        SettingData data;
        const string filename = "gameSettings.sav";
        const int numLines = 12;
        
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
            applySettings();    
            saveSettings();
        }

        public void updateP2PrimaryHand(bool rightHand)
        {
            data.p2RightHanded = rightHand;
            applySettings();    
            saveSettings();
        }

        public void updateVolume(int vol)
        {
            data.volume = vol;
            applySettings();    
            saveSettings();
        }

        public int getVolume()
        {
            return data.volume;
        }

        public void updateSound(bool sound)
        {
            data.soundsOn = sound;
            applySettings();    
            saveSettings();
        }

        public void updateUnlock(bool unlock)
        {
            data.unlockOn = unlock;
            applySettings();    
            saveSettings();
        }

        public void updateMovmentRange (int i, float val)
        {
            data.movementRange[i] = val;
            applySettings();    
            saveSettings();
        }

        public void updateYpreference (int i, float val)
        {
            data.yPreference[i] = val;
            applySettings();    
            saveSettings();
        }

        public void updateXoffset(int i, float val)
        {
            data.xOffset[i] = val;
            applySettings();
            saveSettings();
        }

        public void updateGoalFile(string file)
        {
            data.goalFile = file;
            applySettings();
            saveSettings();
        }

        public void saveSettings()
        {
            string[] lines = new string[numLines];
            lines[0] = data.p1RightHanded.ToString();
            lines[1] = data.p2RightHanded.ToString();
            lines[2] = data.volume.ToString();
            lines[3] = data.soundsOn.ToString();
            lines[4] = data.unlockOn.ToString();
            lines[5] = data.movementRange[0].ToString();
            lines[6] = data.movementRange[1].ToString();
            lines[7] = data.yPreference[0].ToString();
            lines[8] = data.yPreference[1].ToString();
            lines[9] = data.xOffset[0].ToString();
            lines[10] = data.xOffset[1].ToString();
            lines[11] = data.goalFile;

            File.WriteAllLines(filename, lines);
        }

        public bool loadSettings()
        {
            if (!File.Exists(filename))
                return false;

            string[] lines = File.ReadAllLines(filename);

            if (lines.Length != numLines)
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

            data.xOffset[0] = float.Parse(lines[9]);
            data.xOffset[1] = float.Parse(lines[10]);
          
            data.goalFile = lines[11];

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
            Program.game.xOffset = data.xOffset;

            Program.game.goalImage = data.goalFile;
        }

    }
}
