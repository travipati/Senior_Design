using System;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;

namespace MazeAndBlue
{
    public class GameStats
    {
        public struct LevelData 
        { 
            public int level;  
            public int time; 
            public int score; 
            public int numStars; 
        };

        public struct SaveGameData
        {
            public string PlayerName;
            public int totalGameTime;
            public LevelData levelData0;
            public LevelData levelData1;
            public LevelData levelData2;
            public LevelData levelData3;
            public LevelData levelData4;
            public LevelData levelData5;
            public LevelData levelData6;
            public LevelData levelData7;
            public LevelData levelData8;
            public LevelData levelData9;
            public LevelData levelData10;
            public LevelData levelData11;
            public int nextLevelToUnlock;
        };

        public SaveGameData data;

        public GameStats()
        {
            data = new SaveGameData();
            loadStats();
        }

        public void resetData()
        {
            data = new SaveGameData();
        }

        public void updateLevelStats(int level, int numSeconds, int numHitWall)
        {
            LevelData newLevel= new LevelData();
            newLevel.level = level;
            newLevel.time = numSeconds;
            newLevel.score = calcScore(numSeconds, numHitWall);
            newLevel.numStars = calcNumStars(newLevel.score);

            data.totalGameTime+= numSeconds;
            if (data.nextLevelToUnlock == level)
                data.nextLevelToUnlock=level+1;

            switch (level)
            {
                case 0:
                    data.levelData0 = newLevel;
                    break;
                case 1:
                    data.levelData1 = newLevel;
                    break;
                case 2:
                    data.levelData2 = newLevel;
                    break;
                case 3:
                    data.levelData3 = newLevel;
                    break;
                case 4:
                    data.levelData4 = newLevel;
                    break;
                case 5:
                    data.levelData5 = newLevel;
                    break;
                case 6:
                    data.levelData6 = newLevel;
                    break;
                case 7:
                    data.levelData7 = newLevel;
                    break;
                case 8:
                    data.levelData8 = newLevel;
                    break;
                case 9:
                    data.levelData9 = newLevel;
                    break;
                case 10:
                    data.levelData10 = newLevel;
                    break;
                case 11:
                    data.levelData11 = newLevel;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("error updating level stats", "Error @ GameStats");
                    break;
            }
            //Console.Out.WriteLine(data.nextLevelToUnlock);
            Console.Out.WriteLine(numHitWall);
            System.Windows.Forms.MessageBox.Show(numHitWall.ToString(), "");
            saveStats();
        }

        public int calcNumStars(int score)
        {
            if (score >= 100)
                return 3;
            else if (score >= 50)
                return 2;
            else if (score > 0)
                return 1;
            else
                return 0;
        }

        public int calcScore(int numSeconds, int numHitWall)
        {
            int baseScore = 100 * 20 / numSeconds;
            
            return baseScore;
        }



        public void saveStats()
        {
            // Open a storage container.
            IAsyncResult result =
                StorageDevice.BeginShowSelector( null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageDevice device = StorageDevice.EndShowSelector(result);

            result = device.BeginOpenContainer("MaizeAndBlueStorage", null, null);

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string filename = "gameStats.sav";

            // Check to see whether the save exists.
            if (container.FileExists(filename))
                // Delete it so that we can create one fresh.
                container.DeleteFile(filename);

            // Create the file.
            Stream stream = container.CreateFile(filename);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));

            serializer.Serialize(stream, data);

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }

        public void loadStats()
        {
            // Open a storage container.
            IAsyncResult result =
                StorageDevice.BeginShowSelector(null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageDevice device = StorageDevice.EndShowSelector(result);

            result = device.BeginOpenContainer("MaizeAndBlueStorage", null, null);

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string filename = "gameStats.sav";

            // Check to see whether the save exists.
            if (!container.FileExists(filename))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                Console.Out.WriteLine("Error open file loading");
                return;
            }

            // Open the file.
            Stream stream = container.OpenFile(filename, FileMode.Open);

            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));

            data = (SaveGameData)serializer.Deserialize(stream);

            // Close the file.
            stream.Close();

            // Dispose the container.
            container.Dispose();
        }
    }
}
