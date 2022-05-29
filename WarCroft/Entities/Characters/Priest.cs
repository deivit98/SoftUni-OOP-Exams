using System;
using System.Collections.Generic;
using System.Text;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Inventory;

namespace WarCroft.Entities.Characters
{
    public class Priest : Character, IHealer
    {
        //has 50 Base Health, 25 Base Armor, 40 Ability Points, and a Backpack as a bag.

        private const double PriestHealth = 50;
        private const double PriestArmor = 25;
        private const double PriestAbilityPoints = 40;

        public Priest(string name)
            : base(name, PriestHealth, PriestArmor, PriestAbilityPoints, new Backpack())
        {
        }

        public void Heal(Character character)
        {
            this.EnsureAlive();

            if (!character.IsAlive)
            {
                throw new InvalidOperationException("Must be alive to perform this action!");
            }

            character.Health = Math.Min(character.BaseHealth, character.Health + this.AbilityPoints);
        }
    }
}
