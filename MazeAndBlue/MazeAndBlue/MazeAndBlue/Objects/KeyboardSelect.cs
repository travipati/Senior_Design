using Microsoft.Xna.Framework.Input;

namespace MazeAndBlue
{
    public class KeyboardSelect
    {
        KeyboardState prev;
        public bool newKeyReady = false;
        public string key;

        public void grabInput()
        {
            KeyboardState cur = Keyboard.GetState();
            if (cur.IsKeyDown(Keys.D1) && prev.IsKeyUp(Keys.D1))
            {
                newKeyReady = true;
                key = "1";
            }
            else if (cur.IsKeyDown(Keys.D2) && prev.IsKeyUp(Keys.D2))
            {
                newKeyReady = true;
                key = "2";
            }
            else if (cur.IsKeyDown(Keys.Escape) && prev.IsKeyUp(Keys.Escape))
            {
                newKeyReady = true;
                key = "Esc";
            }
            else if (cur.IsKeyDown(Keys.Space) && prev.IsKeyUp(Keys.Space))
            {
                newKeyReady = true;
                key = "Space";
                //Program.game.startCalibrateScreen();
            }
            prev = cur;
        }
    }
}

