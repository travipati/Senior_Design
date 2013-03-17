using Microsoft.Xna.Framework.Input;

namespace MazeAndBlue
{
    class keyboardSelect
    {
        public selectStates states;
        KeyboardState prev;

        public keyboardSelect (ref selectStates sharedState)
        {
            states = sharedState;
        }

        public void grabInput (KeyboardState k)
        {
            if (k.IsKeyDown(Keys.D1) && prev.IsKeyUp(Keys.D1))
            {
                if (states.select[0])
                {
                    states.select[0] = false;
                }
                else
                {
                    states.selectStated[0] = true;
                }
                System.Diagnostics.Debug.WriteLine("write!");
            }
            if (k.IsKeyDown(Keys.D2) && prev.IsKeyUp(Keys.D2))
            {
                if (states.select[1])
                {
                    states.select[1] = false;
                }
                else
                {
                    states.selectStated[1] = true;
                }
            }
        }

    }
}

