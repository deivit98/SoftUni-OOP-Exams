using System;

using WarCroft.Constants;
using WarCroft.Entities.Inventory;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Characters.Contracts
{
    public abstract class Character
    {
        // TODO: Implement the rest of the class.
        private string name { get; set; }
        private double baseHealth { get; set; }
        private double health { get; set; }
        private double baseArmor { get; set; }
        private double armor { get; set; }
        private double abilityPoints { get; set; }

        public Character(string name, double health, double armor, double abilityPoints, Bag bag)
        {
            this.Name = name;
            this.BaseHealth = health;
            this.Health = health;
            this.BaseArmor = armor;
            this.Armor = armor;
            this.AbilityPoints = abilityPoints;
            this.Bag = bag;

        }
        protected void EnsureAlive()
        {
            if (!this.IsAlive)
            {
                throw new InvalidOperationException(ExceptionMessages.AffectedCharacterDead);
            }
        }

        public Bag Bag { get; }

        public bool IsAlive { get; set; } = true;
        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.CharacterNameInvalid);
                }
                this.name = value;
            }
        }

        public double BaseHealth
        {
            get
            {
                return this.baseHealth;
            }
            private set
            {
                this.baseHealth = value;
            }
        }

        public double Health
        {
            get
            {
                return this.health;
            }
            set
            {
                this.health = Math.Min(value, this.BaseHealth);
            }
        }

        public double BaseArmor
        {
            get
            {
                return this.baseArmor;
            }
            private set
            {
                this.baseArmor = value;
            }
        }

        public double Armor
        {
            get
            {
                return this.armor;
            }
            set
            {
                if (this.baseArmor > value && value >= 0)
                {
                    this.armor = value;
                }
                this.armor = baseArmor;
            }
        }
        public double AbilityPoints
        {
            get
            {
                return abilityPoints;
            }
            private set
            {
                this.abilityPoints = value;
            }
        }

        public void TakeDamage(double hitPoints)
        {

            this.EnsureAlive();

            this.armor -= hitPoints;
            if (this.armor < 0)
            {
                double left = Math.Abs(this.armor);
                this.health -= left;
                this.armor = 0;
            }

            if (this.health <= 0)
            {
                this.IsAlive = false;
                this.health = 0;
            }

        }

        public void UseItem(Item item)
        {
            if (this.IsAlive)
            {
                item.AffectCharacter(this);
            }
        }


    }
}