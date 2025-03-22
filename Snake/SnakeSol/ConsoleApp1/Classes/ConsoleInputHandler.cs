namespace Snake
{
    class ConsoleInputHandler : IInputHandler
    {
        public Direction GetDirection(Direction currentDirection)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                return pressedKey.Key switch
                {
                    ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                    ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                    ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                    ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                    _ => currentDirection
                };
            }
            return currentDirection;
        }
    }
}
