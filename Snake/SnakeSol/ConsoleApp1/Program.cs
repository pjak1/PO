using System.Text.Json;

namespace Snake
{
    interface IRenderable
    {
        int CoordX { get; }
        int CoordY { get; }
        string Color { get; }
    }

    interface IGameRenderer
    {
        void DrawBorder(int width, int height);
        void DrawRenderables(IEnumerable<IRenderable> renderables);
        void DisplayGameOver(int width, int height, int score);
        void Clear();
    }

    interface IInputHandler
    {
        string GetDirection(string currentDirection);
    }

    class GameConfig
    {
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public string SnakeHeadColor { get; set; }
        public string SnakeBodyColor { get; set; }
        public string BerryColor { get; set; }

        public static GameConfig LoadConfig(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Konfigurační soubor {filePath} nenalezen.");
            }

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameConfig>(json);
        }
    }

    class Game
    {
        private readonly int ScreenWidth;
        private readonly int ScreenHeight;
        private readonly IGameRenderer Renderer;
        private readonly IInputHandler InputHandler;
        private Snake Snake;
        private Berry Berry;
        private int Score;
        private bool GameOver;

        public Game(int width, int height, IGameRenderer renderer, IInputHandler inputHandler, GameConfig config)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            Renderer = renderer;
            InputHandler = inputHandler;
            Snake = new Snake(ScreenWidth / 2, ScreenHeight / 2, config.SnakeHeadColor, config.SnakeBodyColor);
            Berry = new Berry(ScreenWidth, ScreenHeight, config.BerryColor);
            Score = 5;
            GameOver = false;
            Console.WindowHeight = ScreenHeight;
            Console.WindowWidth = ScreenWidth;
        }

        public void Start()
        {
            string direction = "RIGHT";

            while (!GameOver)
            {
                GameOver = Snake.CheckCollisionWithWalls(ScreenWidth, ScreenHeight) || Snake.CheckCollisionWithBody();
                if (GameOver) break;

                if (Berry.CheckIfEaten(Snake.Head))
                {
                    Score++;
                    Berry.Respawn(ScreenWidth, ScreenHeight);
                }

                Renderer.Clear();
                Renderer.DrawBorder(ScreenWidth, ScreenHeight);
                Renderer.DrawRenderables(Snake.GetRenderables());
                Renderer.DrawRenderables(Berry.GetRenderables());

                direction = InputHandler.GetDirection(direction);
                Snake.Move(direction, Score);
                Thread.Sleep(100);
            }

            Renderer.DisplayGameOver(ScreenWidth, ScreenHeight, Score);
        }
    }

    abstract class GameObject
    {
        public abstract IEnumerable<IRenderable> GetRenderables();
    }

    class Snake : GameObject
    {
        public IRenderable Head { get; private set; }
        private readonly List<RenderableObject> Body = new();
        private readonly string HeadColor;
        private readonly string BodyColor;

        public Snake(int startX, int startY, string headColor, string bodyColor)
        {
            HeadColor = headColor;
            BodyColor = bodyColor;
            Head = new RenderableObject(startX, startY, HeadColor);
        }

        public void Move(string direction, int score)
        {
            if (Body.Count > 0)
            {
                for (int i = Body.Count - 1; i > 0; i--)
                {
                    Body[i].SetPosition(Body[i - 1].CoordX, Body[i - 1].CoordY);
                }
                Body[0].SetPosition(Head.CoordX, Head.CoordY);
            }

            if (Body.Count < score)
            {
                Body.Insert(0, new RenderableObject(Head.CoordX, Head.CoordY, BodyColor));
            }

            int newX = Head.CoordX;
            int newY = Head.CoordY;

            switch (direction)
            {
                case "UP": newY--; break;
                case "DOWN": newY++; break;
                case "LEFT": newX--; break;
                case "RIGHT": newX++; break;
            }

            ((RenderableObject)Head).SetPosition(newX, newY);
        }

        public bool CheckCollisionWithWalls(int width, int height) =>
            Head.CoordX <= 0 || Head.CoordX >= width - 1 || Head.CoordY <= 0 || Head.CoordY >= height - 1;

        public bool CheckCollisionWithBody() =>
            Body.Exists(segment => segment.CoordX == Head.CoordX && segment.CoordY == Head.CoordY);

        public override IEnumerable<IRenderable> GetRenderables()
        {
            foreach (var segment in Body)
            {
                yield return segment;
            }
            yield return Head;
        }
    }

    class Berry : GameObject
    {
        private readonly Random random = new();
        private IRenderable berryObject;
        private readonly string Color;

        public Berry(int screenWidth, int screenHeight, string color)
        {
            Color = color;
            Respawn(screenWidth, screenHeight);
        }

        public void Respawn(int screenWidth, int screenHeight)
        {
            int newX = random.Next(2, screenWidth - 2);
            int newY = random.Next(2, screenHeight - 2);
            berryObject = new RenderableObject(newX, newY, Color);
        }

        public bool CheckIfEaten(IRenderable head) =>
            berryObject.CoordX == head.CoordX && berryObject.CoordY == head.CoordY;

        public override IEnumerable<IRenderable> GetRenderables()
        {
            yield return berryObject;
        }
    }

    class ConsoleInputHandler : IInputHandler
    {
        public string GetDirection(string currentDirection)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                return pressedKey.Key switch
                {
                    ConsoleKey.UpArrow when currentDirection != "DOWN" => "UP",
                    ConsoleKey.DownArrow when currentDirection != "UP" => "DOWN",
                    ConsoleKey.LeftArrow when currentDirection != "RIGHT" => "LEFT",
                    ConsoleKey.RightArrow when currentDirection != "LEFT" => "RIGHT",
                    _ => currentDirection
                };
            }
            return currentDirection;
        }
    }

    class ConsoleGameRenderer : IGameRenderer
    {
        private ConsoleColor GetConsoleColor(string colorName) => Enum.TryParse(colorName, out ConsoleColor color) ? color : ConsoleColor.White;

        public void DrawBorder(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, height - 1);
                Console.Write("■");
            }
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(width - 1, i);
                Console.Write("■");
            }
        }

        public void DrawRenderables(IEnumerable<IRenderable> renderables)
        {
            foreach (var renderable in renderables)
            {
                Console.SetCursorPosition(renderable.CoordX, renderable.CoordY);
                Console.ForegroundColor = GetConsoleColor(renderable.Color);
                Console.Write("■");
            }
            Console.ResetColor();
        }

        public void DisplayGameOver(int width, int height, int score)
        {
            Console.SetCursorPosition(width / 5, height / 2);
            Console.WriteLine("Game Over, Score: " + score);
        }

        public void Clear() => Console.Clear();
    }

    class RenderableObject : IRenderable
    {
        public int CoordX { get; private set; }
        public int CoordY { get; private set; }
        public string Color { get; }

        public RenderableObject(int coordX, int coordY, string color)
        {
            CoordX = coordX;
            CoordY = coordY;
            Color = color;
        }

        public void SetPosition(int x, int y)
        {
            CoordX = x;
            CoordY = y;
        }
    }
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
