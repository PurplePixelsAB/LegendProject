using Data.World;
using Microsoft.Xna.Framework;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendWorld.Data.Modifiers
{
    public class ModifiersCollection : List<CharacterModifier>
    {        
        Queue<CharacterModifier> durationRunOutModifiers = new Queue<CharacterModifier>(10);
        public virtual void Update(GameTime gameTime, Character character)
        {
            for (int i = 0; i < this.Capacity; i++)//foreach (var modifier in this)
            {
                var modifier = this[i];
                modifier.Update(gameTime, character);
                if (modifier.Duration.HasValue)
                {
                    modifier.Duration -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (modifier.Duration < 0)
                    {
                        durationRunOutModifiers.Enqueue(modifier);
                    }
                }
                else if (modifier.IsUsed)
                {
                    durationRunOutModifiers.Enqueue(modifier);
                }
            }

            while (durationRunOutModifiers.Count > 0)
                this.Remove(durationRunOutModifiers.Dequeue());
        }
    }
}
