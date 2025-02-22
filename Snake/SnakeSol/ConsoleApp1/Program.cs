using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    class Program
    {
        static int ScreenWidth = 50;
        static int ScreenHeight = 50;

        static void Main(string[] args)
        {
            Console.WindowHeight = ScreenHeight;
            Console.WindowWidth = ScreenWidth;

            Random random = new Random();
            int berryCoordX = random.Next(1, ScreenWidth - 2);
            int berryCoordY = random.Next(1, ScreenHeight - 2);
            int score = 5;
            bool gameOver = false;

            Pixel head = new Pixel { CoordX = ScreenWidth / 2, CoordY = ScreenHeight / 2, ColorOfHead = ConsoleColor.Red };
            string direction = "RIGHT";
            List<int> bodyCoordX = new List<int>();
            List<int> bodyCoordY = new List<int>();

            DateTime timeBeforeCycle = DateTime.Now;
            DateTime timeBeforeInteraction = DateTime.Now;
            bool isButtonPressed = false;

            while (!gameOver)
            {
                Console.Clear();
                DrawBorder();

                
                if (CheckCollisionWithWalls(head))
                {
                    gameOver = true;
                }

                
                if (berryCoordX == head.CoordX && berryCoordY == head.CoordY)
                {
                    score++;
                    berryCoordX = random.Next(1, ScreenWidth - 2);
                    berryCoordY = random.Next(1, ScreenHeight - 2);
                }

                
                if (CheckCollisionWithBody(bodyCoordX, bodyCoordY, head))
                {
                    gameOver = true;
                }

                
                DrawSnake(head, bodyCoordX, bodyCoordY, berryCoordX, berryCoordY);

                
                ProcessInput(ref direction, ref isButtonPressed);

                
                MoveSnake(ref head, ref bodyCoordX, ref bodyCoordY, direction, score);

                
                timeBeforeCycle = DateTime.Now;
                isButtonPressed = false;

                
                if (gameOver)
                {
                    Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
                    Console.WriteLine("Game Over, Score: " + score);
                }
            }
        }

        static void DrawBorder()
        {
            for (int i = 0; i < ScreenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
            }
            for (int i = 0; i < ScreenWidth; i++)
            {
                Console.SetCursorPosition(i, ScreenHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < ScreenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
            }
            for (int i = 0; i < ScreenHeight; i++)
            {
                Console.SetCursorPosition(ScreenWidth - 1, i);
                Console.Write("■");
            }
        }

        static bool CheckCollisionWithWalls(Pixel head)
        {
            return head.CoordX == ScreenWidth - 1 || head.CoordX == 0 || head.CoordY == ScreenHeight - 1 || head.CoordY == 0;
        }

        static bool CheckCollisionWithBody(List<int> bodyCoordX, List<int> bodyCoordY, Pixel head)
        {
            for (int i = 0; i < bodyCoordX.Count; i++)
            {
                if (bodyCoordX[i] == head.CoordX && bodyCoordY[i] == head.CoordY)
                {
                    return true;
                }
            }
            return false;
        }

        static void DrawSnake(Pixel head, List<int> bodyCoordX, List<int> bodyCoordY, int berryCoordX, int berryCoordY)
        {
            
            for (int i = 0; i < bodyCoordX.Count; i++)
            {
                Console.SetCursorPosition(bodyCoordX[i], bodyCoordY[i]);
                Console.Write("■");
            }

            
            Console.SetCursorPosition(head.CoordX, head.CoordY);
            Console.ForegroundColor = head.ColorOfHead;
            Console.Write("■");

            
            Console.SetCursorPosition(berryCoordX, berryCoordY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        static void ProcessInput(ref string direction, ref bool isButtonPressed)
        {
            DateTime timeBeforeInteraction = DateTime.Now;
            while (true)
            {
                if (DateTime.Now.Subtract(timeBeforeInteraction).TotalMilliseconds > 500) break;
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    if (!isButtonPressed)
                    {
                        if (pressedKey.Key == ConsoleKey.UpArrow && direction != "DOWN")
                        {
                            direction = "UP";
                            isButtonPressed = true;
                        }
                        else if (pressedKey.Key == ConsoleKey.DownArrow && direction != "UP")
                        {
                            direction = "DOWN";
                            isButtonPressed = true;
                        }
                        else if (pressedKey.Key == ConsoleKey.LeftArrow && direction != "RIGHT")
                        {
                            direction = "LEFT";
                            isButtonPressed = true;
                        }
                        else if (pressedKey.Key == ConsoleKey.RightArrow && direction != "LEFT")
                        {
                            direction = "RIGHT";
                            isButtonPressed = true;
                        }
                    }
                }
            }
        }

        static void MoveSnake(ref Pixel head, ref List<int> bodyCoordX, ref List<int> bodyCoordY, string direction, int score)
        {
            bodyCoordX.Add(head.CoordX);
            bodyCoordY.Add(head.CoordY);

            switch (direction)
            {
                case "UP": head.CoordY--; break;
                case "DOWN": head.CoordY++; break;
                case "LEFT": head.CoordX--; break;
                case "RIGHT": head.CoordX++; break;
            }

            if (bodyCoordX.Count > score)
            {
                bodyCoordX.RemoveAt(0);
                bodyCoordY.RemoveAt(0);
            }
        }
    }

    class Pixel
    {
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public ConsoleColor ColorOfHead { get; set; }
    }
}
