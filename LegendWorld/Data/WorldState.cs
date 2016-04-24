using Data.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Network
{
    public abstract class WorldState
    {
        protected byte swingDmg = 24;
        protected byte swingEnergy = 34;
        protected byte healAmount = 11;

        protected TimeSpan baseRegenTick = new TimeSpan(0, 0, 1);
        //protected TimeSpan baseEnergyTick = new TimeSpan(0, 0, 1);
        private long nextRegendTick = 0;

        protected Dictionary<int, Character> characters = new Dictionary<int, Character>();
        public virtual Character GetCharacter(int id)
        {
            if (characters.ContainsKey(id))
            {
                return characters[id];
            }

            return null;
        }

        public virtual void AddCharacter(Character newCharacter)
        {
            if (characters.ContainsKey(newCharacter.Id))
                return;

            newCharacter.MoveToMapPointValidating += NewCharacter_MoveToMapPointValidating;
            characters.Add(newCharacter.Id, newCharacter);
        }

        private void NewCharacter_MoveToMapPointValidating(object sender, Character.MoveToMapPointValidatingEventArgs e)
        {
            WorldMap map = this.GetCharactersMap((Character)sender);
            e.IsValid = map.Bounds.Contains(e.MoveToMapPoint);
        }

        public virtual void Update(GameTime gameTime)
        {
            var idList = characters.Keys;

            bool isRegenTick = gameTime.TotalGameTime.Ticks >= nextRegendTick;

            foreach (int characterId in idList)
            {
                characters[characterId].UpdateMapPosition(gameTime);

                if (isRegenTick)
                {
                    if (characters[characterId].Health < characters[characterId].MaxHealth)
                        characters[characterId].Health += 1;

                    if (characters[characterId].Energy < characters[characterId].MaxEnergy)
                        characters[characterId].Energy += 1;

                    nextRegendTick = (gameTime.TotalGameTime + baseRegenTick).Ticks;
                }                
            }
        }

        public virtual void PerformSwing(Character character)
        {
            if (character.Energy < swingEnergy)
                return;

            double coneAngle = this.VectorToAngle(character.AimToPosition.ToVector2() - character.Position.ToVector2());
            Rectangle areaFilter = new Rectangle(character.Position.X - 100, character.Position.Y - 100, 200, 200);
            foreach (int characterId in characters.Keys)
            {
                if (character.Id == characterId)
                    continue;

                if (areaFilter.Contains(characters[characterId].Position))
                {
                    if (IsPointWithinCone(characters[characterId].Position.ToVector2(), character.Position.ToVector2(), coneAngle, 20)) //(affectedArea.Contains(characters[characterId].MapPoint))
                    {
                        characters[characterId].Health -= swingDmg;
                        characters[characterId].Energy -= swingEnergy;
                    }
                }
            }
        }
        public virtual void PerformHeal(Character character)
        {
            double coneAngle = this.VectorToAngle(character.AimToPosition.ToVector2() - character.Position.ToVector2());
            Rectangle areaFilter = new Rectangle(character.Position.X - 100, character.Position.Y - 100, 200, 200);
            foreach (int characterId in characters.Keys)
            {
                if (character.Id == characterId)
                    continue;

                if (areaFilter.Contains(characters[characterId].Position))
                {
                    if (IsPointWithinCone(characters[characterId].Position.ToVector2(), character.Position.ToVector2(), coneAngle, 20)) //(affectedArea.Contains(characters[characterId].MapPoint))
                    {
                        characters[characterId].Health -= swingDmg;
                    }
                }
            }
        }

        protected abstract WorldMap GetCharactersMap(Character character);

        public double CanonizeAngle(double angle)
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

        public double VectorToAngle(Vector2 vector)
        {
            Vector2 direction = Vector2.Normalize(vector);
            return Math.Atan2(direction.Y, direction.X);
        }

        public bool IsPointWithinCone(Vector2 point, Vector2 conePosition, double coneAngle, double coneSize)
        {
            double toPoint = VectorToAngle(point - conePosition);
            double angleDifference = CanonizeAngle(coneAngle - toPoint);
            double halfConeSize = coneSize * 0.5f;

            return angleDifference >= -halfConeSize && angleDifference <= halfConeSize;
        }

    }
}