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
        public int time { get; set; }

        public Timer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            time = 0;
            timePos = new Vector2(Program.game.sx(10), Program.game.sy(495));
        }

        private void timerTick(object sender, EventArgs e)
        {
            time++;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(MazeAndBlue.font, "Time: " + time.ToString() + " sec", timePos, Color.Black);
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
