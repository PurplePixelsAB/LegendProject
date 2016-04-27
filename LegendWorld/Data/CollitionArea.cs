using Data.World;
using Microsoft.Xna.Framework;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data
{
    public abstract class CollitionArea
    {
        public abstract List<Character> GetAffected(WorldState world, Character performer);

    }

    public class ConeCollitionArea : CollitionArea
    {
        public int Range { get; set; }
        public override List<Character> GetAffected(WorldState world, Character character)
        {
            List<Character> returnList = new List<Character>();
            double coneAngle = world.VectorToRadian(character.AimToPosition.ToVector2() - character.Position.ToVector2());
            Rectangle areaFilter = new Rectangle(character.Position.X - 100, character.Position.Y - 100, 200, 200);
            foreach (int characterId in world.Characters)
            {
                if (character.Id == characterId)
                    continue;

                Character checkCollitionVersus = world.GetCharacter(characterId);
                if (areaFilter.Contains(checkCollitionVersus.Position))
                {
                    double angle = world.VectorToRadian(checkCollitionVersus.CollitionArea.Position.ToVector2() - character.Position.ToVector2());
                    if (Contains(checkCollitionVersus.CollitionArea, coneAngle, angle))
                        returnList.Add(checkCollitionVersus);                    
                }
            }

            return returnList;
        }

        public bool Contains(CircleCollitionArea collitionArea, double coneAngle, double angle)
        {
            return CompareAngles(angle, coneAngle, this.Range);
        }

        public bool CompareAngles(double otherAngle, double coneAngle, double coneSize)
        {
            double angleDifference = CanonizeAngle(coneAngle - otherAngle);
            double halfConeSize = coneSize * 0.5f;

            return angleDifference >= -halfConeSize && angleDifference <= halfConeSize;
        }
        private double CanonizeAngle(double angle)
        {
            if (angle > Math.PI)
            {
                do
                {
                    angle -= MathHelper.TwoPi;
                }
                while (angle > Math.PI);
            }
            else if (angle < -Math.PI)
            {
                do
                {
                    angle += MathHelper.TwoPi;
                } while (angle < -Math.PI);
            }

            return angle;
        }

    }
    public class RectangleCollitionArea : CollitionArea
    {
        public Rectangle RectangleArea { get; set; }
        public override List<Character> GetAffected(WorldState world, Character performer)
        {
            List<Character> returnList = new List<Character>();
            if (RectangleArea == null || RectangleArea == Rectangle.Empty)
                return returnList;

            foreach (int characterId in world.Characters)
            {
                if (performer.Id == characterId)
                    continue;

                Character checkCollitionVersus = world.GetCharacter(characterId);
                if (RectangleArea.Contains(checkCollitionVersus.Position))
                {
                    returnList.Add(checkCollitionVersus);
                }
            }

            return returnList;
        }

        public bool Contains(CircleCollitionArea collitionArea)
        {
            return collitionArea.Contains(this);
        }
    }

    public class CircleCollitionArea : CollitionArea
    {
        public Point Position { get; set; }
        public int R { get; set; }

        public override List<Character> GetAffected(WorldState world, Character performer)
        {
            List<Character> returnList = new List<Character>();
            if (Position == null || Position == Point.Zero)
                return returnList;
            if (R == 0)
                return returnList;

            foreach (int characterId in world.Characters)
            {
                if (performer.Id == characterId)
                    continue;

                Character checkCollitionVersus = world.GetCharacter(characterId);
                if (Contains(checkCollitionVersus.CollitionArea))
                {
                    returnList.Add(checkCollitionVersus);
                }
            }

            return returnList;
        }

        public bool Contains(RectangleCollitionArea collitionArea)
        {
            Point circleDistance = new Point(Math.Abs(this.Position.X - collitionArea.RectangleArea.X), Math.Abs(this.Position.Y - collitionArea.RectangleArea.Y));

            if (circleDistance.X > (collitionArea.RectangleArea.Width / 2 + this.R)) { return false; }
            if (circleDistance.Y > (collitionArea.RectangleArea.Height / 2 + this.R)) { return false; }

            if (circleDistance.X <= (collitionArea.RectangleArea.Width / 2)) { return true; }
            if (circleDistance.Y <= (collitionArea.RectangleArea.Height / 2)) { return true; }

            var cornerDistance_sq = (circleDistance.X - collitionArea.RectangleArea.Width / 2) ^ 2 +
                                 (circleDistance.Y - collitionArea.RectangleArea.Height / 2) ^ 2;

            return (cornerDistance_sq <= (this.R ^ 2));
        }
        public bool Contains(CircleCollitionArea collitionArea)
        {
            var radius = this.R + collitionArea.R;
            var deltaX = this.Position.X - collitionArea.Position.X;
            var deltaY = this.Position.Y - collitionArea.Position.Y;
            return deltaX * deltaX + deltaY * deltaY <= radius * radius;
        }
    }
    
}
