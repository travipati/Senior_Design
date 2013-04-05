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
            public int hits;
            public int score; 
            public int numStars; 
        };

        public struct SaveGameData
        {
            public string PlayerName;
            public int totalGameTime;
            public int totalScore;
            public LevelData[] levelData;
            public int nextLevelToUnlock;
        };

        public SaveGameData data;

        public GameStats()
        {
            data = new SaveGameData();
            data.levelData = new LevelData[12];
            loadStats();
        }

        public void resetData()
        {
            data = new SaveGameData();
            data.levelData = new LevelData[12];
        }

        public void updateLevelStats(int level, int numSeconds, int numHitWall, int score, int stars)
        {
            LevelData newLevel= new LevelData();
            newLevel.level = level;
            newLevel.time = numSeconds;
            newLevel.hits = numHitWall;
            if (score > newLevel.score)
            {
                newLevel.score = score;
                data.totalScore += score;
            }
            newLevel.numStars = stars;

            data.totalGameTime+= numSeconds;
            if (data.nextLevelToUnlock == level)
                data.nextLevelToUnlock = level + 1;

            if (level >= 12)
                System.Windows.Forms.MessageBox.Show("error updating level stats", "Error @ GameStats");
            else
                data.levelData[level] = newLevel;
            
            //Console.Out.WriteLine(data.nextLevelToUnlock);
            Console.Out.WriteLine(numHitWall);
            System.Windows.Forms.MessageBox.Show(numHitWall.ToString(), "");
            saveStats();
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
