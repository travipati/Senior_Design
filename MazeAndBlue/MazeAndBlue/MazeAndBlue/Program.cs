namespace MazeAndBlue
{
#if WINDOWS || XBOX
    static class Program
    {
        static private MazeAndBlue _game;
        static public MazeAndBlue game { get { return _game; } }

        static void Main(string[] args)
        {
            using (_game = new MazeAndBlue())
            {
                _game.Run();
            }
        }
    }
#endif
}

