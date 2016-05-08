using Microsoft.Xna.Framework;

namespace Data
{
    public interface IHasPosition : IEntity
    {
        int CurrentMapId { get; set; }
        Point Position { get; }
    }
}