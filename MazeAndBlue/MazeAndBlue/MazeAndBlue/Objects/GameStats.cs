using System;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;

namespace MazeAndBlue
{

    public struct LevelData 
    { 
        public int level;  
        public int time;
        public int hits;
        public int score; 
        public int numStars;

        public string getLine()
        {
            string line =   level.ToString() + ' ' + 
                            time.ToString() + ' ' +
                            hits.ToString() + ' ' +
                            score.ToString() + ' ' + 
                            numStars.ToString();
            return line;
        }

        public void setValues(string[] values)
        {
            level = Convert.ToInt32(values[0]);
            time = Convert.ToInt32(values[1]);
            hits = Convert.ToInt32(values[2]);
            score = Convert.ToInt32(values[3]);
            numStars = Convert.ToInt32(values[4]);
        }

    };

    public class SaveGameData
    {
        public string PlayerName;
        public int totalGameTime;
        public int totalScore;
        public LevelData[] levelData;
        public int singleNextLevelToUnlock;
        public int coopNextLevelToUnlock;
        public int numCustomLevels;

        public SaveGameData()
        {
            levelData = new LevelData[24];
        }
    };

    public class GameStats
    {
        public SaveGameData data;
        const string filename = "gameStats.sav";

        public GameStats()
        {
            data = new SaveGameData();
            if(!loadStats())
                data = new SaveGameData();
        }

        public void resetData()
        {
            data = new SaveGameData();
        }

        public void updateLevelStats(int numSeconds, int numHitWall, int score, int stars)
        {
            int level = Program.game.level;
            LevelData newLevel = new LevelData();
            newLevel.level = level;
            newLevel.time = numSeconds;
            if(numHitWall< data.levelData[level].hits)
                newLevel.hits = numHitWall;
            if (score > data.levelData[level].score)
            {
                newLevel.score = score;
                data.totalScore += score;
            }
            else
                newLevel.score = data.levelData[level].score;
            newLevel.numStars = stars;

            data.totalGameTime+= numSeconds;

            if (Program.game.singlePlayer && data.singleNextLevelToUnlock <= level)
                data.singleNextLevelToUnlock = level + 1;
            if (!Program.game.singlePlayer && data.coopNextLevelToUnlock <= level)
                data.coopNextLevelToUnlock = level + 1;

            if (level >= 24)
                System.Windows.Forms.MessageBox.Show("error updating level stats", "Error @ GameStats");
            else
                data.levelData[level] = newLevel;
            
            saveStats();
        }

        public void saveStats()
        {
            string[] lines = new string[29];
            for (int i = 0; i < 24; i++)
                lines[i] = data.levelData[i].getLine();
            lines[24] = data.totalGameTime.ToString();
            lines[25] = data.totalScore.ToString();
            lines[26] = data.singleNextLevelToUnlock.ToString();
            lines[27] = data.coopNextLevelToUnlock.ToString();
            lines[28] = data.numCustomLevels.ToString();
            File.WriteAllLines(filename, lines);
        }

        public bool loadStats()
        {            
            if (!File.Exists(filename))
                return false;

            string[] lines = File.ReadAllLines(filename);

            if (lines.Length != 29)
                return false;

            for (int i = 0; i < 24; i++)
            {
                string[] values = lines[i].Split(' ');
                if (values.Length != 5)
                    return false;
                data.levelData[i].setValues(values);
            }

            data.totalGameTime = Convert.ToInt32(lines[24]);
            data.totalScore = Convert.ToInt32(lines[25]);
            data.singleNextLevelToUnlock = Convert.ToInt32(lines[26]);
            data.coopNextLevelToUnlock = Convert.ToInt32(lines[27]);
            data.numCustomLevels = Convert.ToInt32(lines[28]);

            return true;            
        }

    }
}
