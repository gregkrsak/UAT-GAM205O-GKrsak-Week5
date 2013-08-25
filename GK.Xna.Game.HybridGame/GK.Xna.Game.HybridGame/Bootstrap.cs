#if WINDOWS || XBOX
    static class Boostrap
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            GK.Xna.Game.HybridGame _game = new GK.Xna.Game.HybridGame();
            _game.Run();
        }
    }
#endif