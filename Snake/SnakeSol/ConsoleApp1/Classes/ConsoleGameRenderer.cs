namespace Snake
{
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
}
