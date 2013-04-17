using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MazeAndBlue
{
    public class MouseSelect
    {
        MouseState prev;
        public bool newPointReady = false;
        public Point point;

        public void grabInput()
        {
            MouseState cur = Mouse.GetState();
            if (cur.LeftButton == ButtonState.Released && prev.LeftButton == ButtonState.Pressed && Program.game.IsActive)
                newPointReady = true;
            point = new Point(cur.X, cur.Y);
            prev = cur;
        }
    }
}

