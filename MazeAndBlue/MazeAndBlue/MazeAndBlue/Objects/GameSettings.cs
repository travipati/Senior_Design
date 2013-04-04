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
    public class GameSettings
    {
        public struct SettingData
        {
            //right =1; left =0;
            public int p1PrimaryHand;
            public int p2PrimaryHand;
            //quiet=0; avg=1; noisy=2
            public int volume;
            //on=1; off=0
            public int gameSound;
        };

        public SettingData data;
        
        public GameSettings()
        {
            data = new SettingData();
            loadStats();
        }

        public void resetData()
        {
            data = new SettingData();
        }

        public void updateP1PrimaryHand( int hand)
        {
            data.p1PrimaryHand = hand;
        }

        public void updateP2PrimaryHand(int hand)
        {
            data.p2PrimaryHand = hand;
        }

        public void updateVolume(int vol)
        {
            data.volume = vol;
        }

        public void updateSound(int sound)
        {
            data.gameSound = sound;
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

            string filename = "gameSettings.sav";

            // Check to see whether the save exists.
            if (container.FileExists(filename))
                // Delete it so that we can create one fresh.
                container.DeleteFile(filename);

            // Create the file.
            Stream stream = container.CreateFile(filename);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer = new XmlSerializer(typeof(SettingData));

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

            string filename = "gameSettings.sav";

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

            XmlSerializer serializer = new XmlSerializer(typeof(SettingData));

            data = (SettingData)serializer.Deserialize(stream);

            // Close the file.
            stream.Close();

            // Dispose the container.
            container.Dispose();
        }
    }
}
