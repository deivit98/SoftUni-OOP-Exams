using System;
using System.Collections.Generic;
using System.Text;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Inventory;

namespace WarCroft.Entities.Characters
{
    public class Warrior : Character, IAttacker
    {
        //100 Base Health, 50 Base Armor, 40 Ability Points, and a Satchel 

        const double WarriorHealth = 100;
        const double WarriorArmor = 50;
        const double WarriorAbilityPoints = 40;

        public Warrior(string name)
             : base(name, WarriorHealth, WarriorArmor, WarriorAbilityPoints, new Satchel())
        {
        }

        public void Attack(Character character)
        {
            this.EnsureAlive();

            if (this.Name == character.Name)
            {
                throw new InvalidOperationException("Cannot attack self!");
            }

            character.TakeDamage(this.AbilityPoints);
        }
    }
}
