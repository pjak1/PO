using System.IO;
using System.Text.Json;

namespace Snake
{
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
                throw new FileNotFoundException($"Konfiguraèní soubor {filePath} nenalezen.");
            }

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameConfig>(json);
        }
    }
}