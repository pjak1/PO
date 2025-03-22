using System.Threading;

namespace Snake
{
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
            Direction direction = Direction.Right;

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
}
