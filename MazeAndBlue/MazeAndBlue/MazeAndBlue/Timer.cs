using System;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeAndBlue
{
    class Timer
    {
        DispatcherTimer timer;
        Vector2 timePos;
        int _time;
        public int time { get { return _time; } }

        public Timer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            _time = 0;
            timePos = new Vector2(0, 550);
        }

        private void timerTick(object sender, EventArgs e)
        {
            _time++;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(MazeAndBlue.font, "Time: " + _time.ToString() + " sec", timePos, Color.Black);
        }

        public void start()
        {
            timer.Start();
        }

        public void stop()
        {
            timer.Stop();
        }

    }
}
