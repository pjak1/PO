namespace Snake
{
    abstract class GameObject
    {
        public abstract IEnumerable<IRenderable> GetRenderables();
    }
}
