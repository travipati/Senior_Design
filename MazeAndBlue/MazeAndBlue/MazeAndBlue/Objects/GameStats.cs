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
        public int singleNextLevelToUnlock;
        public int coopNextLevelToUnlock;
        public int numCustomLevels;
        public LevelData[] levelData;

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
            if (Program.game.singlePlayer)
                level += 12;

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
            lines[0] = data.totalGameTime.ToString();
            lines[1] = data.totalScore.ToString();
            lines[2] = data.singleNextLevelToUnlock.ToString();
            lines[3] = data.coopNextLevelToUnlock.ToString();
            lines[4] = data.numCustomLevels.ToString();
            for (int i = 5; i < 29; i++)
                lines[i] = data.levelData[i].getLine();
            File.WriteAllLines(filename, lines);
        }

        public bool loadStats()
        {            
            if (!File.Exists(filename))
                return false;

            string[] lines = File.ReadAllLines(filename);

            if (lines.Length != 29)
                return false;

            data.totalGameTime = Convert.ToInt32(lines[0]);
            data.totalScore = Convert.ToInt32(lines[1]);
            data.singleNextLevelToUnlock = Convert.ToInt32(lines[2]);
            data.coopNextLevelToUnlock = Convert.ToInt32(lines[3]);
            data.numCustomLevels = Convert.ToInt32(lines[4]);

            for (int i = 5; i < 29; i++)
            {
                string[] values = lines[i].Split(' ');
                if (values.Length != 5)
                    return false;
                data.levelData[i].setValues(values);
            }

            return true;            
        }

    }
}
