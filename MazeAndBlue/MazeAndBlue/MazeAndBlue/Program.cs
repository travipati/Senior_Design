using System;
using System.Runtime.InteropServices;

namespace MazeAndBlue
{
#if WINDOWS || XBOX
    static class Program
    {
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        static private MazeAndBlue _game;
        static public MazeAndBlue game { get { return _game; } }

        static void Main(string[] args)
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
            using (_game = new MazeAndBlue())
            {
                _game.Run();
            }
        }
    }
#endif
}

