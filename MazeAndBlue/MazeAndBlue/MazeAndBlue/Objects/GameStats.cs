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
        public int nextLevelToUnlock;

        public SaveGameData()
        {
            levelData = new LevelData[12];
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

        public void updateLevelStats(int level, int numSeconds, int numHitWall, int score, int stars)
        {
            LevelData newLevel = new LevelData();
            newLevel.level = level;
            newLevel.time = numSeconds;
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
            if (data.nextLevelToUnlock == level)
                data.nextLevelToUnlock = level + 1;

            if (level >= 12)
                System.Windows.Forms.MessageBox.Show("error updating level stats", "Error @ GameStats");
            else
                data.levelData[level] = newLevel;
            
            Console.Out.WriteLine(numHitWall);
            //System.Windows.Forms.MessageBox.Show(numHitWall.ToString(), "");
            saveStats();
        }

        public void saveStats()
        {
            string[] lines = new string[15];
            for (int i = 0; i < 12; i++)
                lines[i] = data.levelData[i].getLine();
            lines[12] = data.totalGameTime.ToString();
            lines[13] = data.totalScore.ToString();
            lines[14] = data.nextLevelToUnlock.ToString();
            File.WriteAllLines(filename, lines);
        }

        public bool loadStats()
        {            
            if (!File.Exists(filename))
                return false;

            string[] lines = File.ReadAllLines(filename);

            if (lines.Length != 15)
                return false;

            for (int i = 0; i < 12; i++)
            {
                string[] values = lines[i].Split(' ');
                if (values.Length != 5)
                    return false;
                data.levelData[i].setValues(values);
            }

            data.totalGameTime = Convert.ToInt32(lines[12]);
            data.totalScore = Convert.ToInt32(lines[13]);
            data.nextLevelToUnlock = Convert.ToInt32(lines[14]);

            return true;            
        }

    }
}
