namespace Snake
{
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

        public void Move(Direction direction, int score)
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
                case Direction.Up: newY--; break;
                case Direction.Down: newY++; break;
                case Direction.Left: newX--; break;
                case Direction.Right: newX++; break;
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
}
