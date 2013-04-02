using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

            if (level == 0)
                data.levelData0 = newLevel;
            else if (level == 1)
                data.levelData1 = newLevel;
            else if (level == 2)
                data.levelData2 = newLevel;
            else if (level == 3)
                data.levelData3 = newLevel;
            else if (level == 4)
                data.levelData4 = newLevel;
            else if (level == 5)
                data.levelData5 = newLevel;
            else
                System.Windows.Forms.MessageBox.Show("error updating level stats", "Error @ GameStats");
            Console.Out.WriteLine(data.nextLevelToUnlock);
            Console.Out.WriteLine(data.totalGameTime);
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
