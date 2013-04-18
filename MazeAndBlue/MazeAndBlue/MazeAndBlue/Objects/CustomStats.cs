using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MazeAndBlue
{
    public class CustomSaveGameData
    {
        //public string PlayerName;
        public int totalGameTime;
        public int totalScore;
        public int numCustomLevels;
        public int[] customLevelIDs;
        public Dictionary<int, LevelData> customData;

        public CustomSaveGameData()
        {
            customLevelIDs = new int[numCustomLevels];
            customData = new Dictionary<int, LevelData>();
        }
    };

    public class CustomStats
    {
        public CustomSaveGameData data;
        const string customFileName = "customStats.sav";


        public CustomStats()
        {
            data = new CustomSaveGameData();
            if(!loadStats())
                data = new CustomSaveGameData();
        }

        public void resetData()
        {
            data = new CustomSaveGameData();
        }

        public void updateLevelStats(int numSeconds, int numHitWall, int score, int stars)
        {
            int level = Program.game.level;
            LevelData newLevel = new LevelData();
            if (!data.customData.ContainsKey(level))
            {
                newLevel.level = level;
                newLevel.time = numSeconds;
                newLevel.hits = numHitWall;
                newLevel.score = score;
                newLevel.numStars = stars;
                data.totalScore += score;
            }
            else
            {
                newLevel.level = level;

                if (numSeconds < data.customData[level].time)
                    newLevel.time = numSeconds;
                else
                    newLevel.time = data.customData[level].time;

                if(numHitWall < data.customData[level].hits)
                    newLevel.hits = numHitWall;
                else
                    newLevel.hits = data.customData[level].hits;

                if (score > data.customData[level].score)
                {
                    data.totalScore += score;
                    newLevel.score = score;
                }
                else
                    newLevel.score = data.customData[level].score;

                if (stars > data.customData[level].numStars)
                    newLevel.numStars = stars;
                else
                    newLevel.numStars = data.customData[level].numStars;
            }

            data.totalGameTime+= numSeconds;
            data.customData[level] = newLevel;
            saveStats();
        }

        public void saveStats()
        {
            string[] lines = new string[3+2*data.numCustomLevels];
            lines[0] = data.totalGameTime.ToString();
            lines[1] = data.totalScore.ToString();
            lines[2] = data.numCustomLevels.ToString();
            for (int i = 0; i < data.numCustomLevels; i++)
            {
                lines[3 + 2 * i] = data.customLevelIDs[i].ToString();
                lines[4 + 2 * i] = data.customData[data.customLevelIDs[i]].getLine();
            }
            File.WriteAllLines(customFileName, lines);
        }

        public bool loadStats()
        {
            if (!File.Exists(customFileName))
                return false;

            string[] lines = File.ReadAllLines(customFileName);

            if (lines.Length < 3)
                return false;

            data.totalGameTime = Convert.ToInt32(lines[0]);
            data.totalScore = Convert.ToInt32(lines[1]);
            data.numCustomLevels = Convert.ToInt32(lines[2]);

            for (int i = 0; i < data.numCustomLevels; i++)
            {
                data.customLevelIDs[i] = Convert.ToInt32(lines[3 + 2 * i]);
                string[] values = lines[4 + 2 * i].Split(' ');
                if (values.Length != 5)
                    return false;
                LevelData newLeveldata= new LevelData();
                newLeveldata.setValues(values);
                data.customData.Add(data.customLevelIDs[i],newLeveldata);
            }


            return true;            
        }


    }
}
