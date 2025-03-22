namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            string dirName = AppDomain.CurrentDomain.BaseDirectory;
            FileInfo fileInfo = new FileInfo(dirName);
            DirectoryInfo parentDir = fileInfo.Directory.Parent.Parent.Parent;
            string path = parentDir.FullName;
#else
            string path = AppDomain.CurrentDomain.BaseDirectory;
#endif
            var config = GameConfig.LoadConfig(Path.Combine(path, "config.json"));

            IGameRenderer renderer = new ConsoleGameRenderer();
            IInputHandler inputHandler = new ConsoleInputHandler();

            Game game = new Game(config.ScreenWidth, config.ScreenHeight, renderer, inputHandler, config);
            game.Start();
        }
    }
}
