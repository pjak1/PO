namespace Snake
{
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
}
