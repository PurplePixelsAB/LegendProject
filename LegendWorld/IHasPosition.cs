using Microsoft.Xna.Framework;

namespace Data
{
    public interface IHasPosition : IEntity
    {
        ushort CurrentMapId { get; set; }
        Point Position { get; }
    }
}