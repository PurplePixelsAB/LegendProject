using Data.World;
using LegendWorld.Data;
using LegendWorld.Data.Abilities;
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
        public Dictionary<int, Character>.KeyCollection Characters {  get { return characters.Keys; } }
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
        public virtual void RemoveCharacter(Character charToRemove)
        {
            if (!characters.ContainsKey(charToRemove.Id))
                return;

            characters.Remove(charToRemove.Id);
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
                    if (characters[characterId].Health < characters[characterId].MaxHealth - 10 && characters[characterId].Health >= 10)
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

            //double coneAngle = this.VectorToAngle(character.AimToPosition.ToVector2() - character.Position.ToVector2());
            //Rectangle areaFilter = new Rectangle(character.Position.X - 100, character.Position.Y - 100, 200, 200);
            //foreach (int characterId in characters.Keys)
            //{
            //    if (character.Id == characterId)
            //        continue;

            //    if (areaFilter.Contains(characters[characterId].Position))
            //    {
            //        if (IsPointWithinCone(characters[characterId].Position.ToVector2(), character.Position.ToVector2(), coneAngle, 20)) //(affectedArea.Contains(characters[characterId].MapPoint))
            //        {
            //            characters[characterId].Health -= swingDmg;
            //        }
            //    }
            //}
            
            var swingAbility = new SwingAbilityEffect();
            swingAbility.Perform(this, character);
            

            //foreach (var affectedChar in affectedCharacters)
            //{
            //    affectedChar.Health -= swingDmg;
            //}

            //character.Energy -= swingEnergy;
        }
        public virtual void PerformHeal(Character character)
        {
            if (character.Energy < swingEnergy)
                return;

            //double coneAngle = this.VectorToAngle(character.AimToPosition.ToVector2() - character.Position.ToVector2());
            //Rectangle areaFilter = new Rectangle(character.Position.X - 100, character.Position.Y - 100, 200, 200);
            //foreach (int characterId in characters.Keys)
            //{
            //    if (character.Id == characterId)
            //        continue;

            //    if (areaFilter.Contains(characters[characterId].Position))
            //    {
            //        if (IsPointWithinCone(characters[characterId].Position.ToVector2(), character.Position.ToVector2(), coneAngle, 20)) //(affectedArea.Contains(characters[characterId].MapPoint))
            //        {
            //            characters[characterId].Health -= swingDmg;
            //        }
            //    }
            //}

            var collitionArea = new ConeCollitionArea();
            collitionArea.Range = 20;
            var affectedCharacters = collitionArea.GetAffected(this, character);
        }

        protected abstract WorldMap GetCharactersMap(Character character);


        public double VectorToRadian(Vector2 direction)
        {
            direction.Normalize();
            return Math.Atan2(direction.X, -direction.Y) + MathHelper.Pi;
        }
        //public double VectorToAngle(Vector2 vector)
        //{
        //    Vector2 direction = Vector2.Normalize(vector);
        //    return Math.Atan2(direction.Y, direction.X);
        //}

    }
}