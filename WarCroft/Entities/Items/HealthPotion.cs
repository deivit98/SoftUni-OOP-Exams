using System;
using System.Collections.Generic;
using System.Text;
using WarCroft.Entities.Characters.Contracts;

namespace WarCroft.Entities.Items
{
    public class HealthPotion : Item
    {
        private const int HitPointsRestored = 20;
        private const int potionWeight = 5;
        public HealthPotion()
            : base(potionWeight)
        {
        }


        public override void AffectCharacter(Character character)
        {
            base.AffectCharacter(character);
            character.Health += HitPointsRestored;
            if (character.Health > 100)
            {
                character.Health = 100;
            }
        }
    }
}
