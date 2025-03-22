namespace Snake
{
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
}
