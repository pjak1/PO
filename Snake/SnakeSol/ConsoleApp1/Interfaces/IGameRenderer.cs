namespace Snake
{
    interface IGameRenderer
    {
        void DrawBorder(int width, int height);
        void DrawRenderables(IEnumerable<IRenderable> renderables);
        void DisplayGameOver(int width, int height, int score);
        void Clear();
    }
}
