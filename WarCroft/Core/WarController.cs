using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Constants;
using WarCroft.Entities.Characters;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Items;

namespace WarCroft.Core
{
    public class WarController
    {
        private readonly List<Character> party;
        private readonly Stack<Item> itemPool;

        public WarController()
        {
            this.party = new List<Character>();
            this.itemPool = new Stack<Item>();
        }
        public string JoinParty(string[] args)
        {
            string characterType = args[0];
            string name = args[1];

            Character character = null;


            if (characterType == "Warrior")
            {
                character = new Warrior(name);
                party.Add(character);
                return $"{name} joined the party!";

            }
            else if (characterType == "Priest")
            {
                character = new Priest(name);
                party.Add(character);
                return $"{name} joined the party!";
            }

            return $"Invalid character type \"{characterType}\"!";
        }

        public string AddItemToPool(string[] args)
        {
            string itemName = args[0];

            Item currItem = null;
            string addedMsg = $"{itemName} added to pool.";


            if (itemName == "FirePotion")
            {
                currItem = new FirePotion();
                itemPool.Push(currItem);
                return addedMsg;
            }
            else if (itemName == "HealthPotion")
            {
                currItem = new HealthPotion();
                itemPool.Push(currItem);
                return addedMsg;
            }
            else
            {
                throw new ArgumentException($"Invalid item \"{itemName}\"!");
            }

        }

        public string PickUpItem(string[] args)
        {
            string characterName = args[0];

            Character character = party.FirstOrDefault(c => c.Name == characterName);

            if (character == null)
            {
                throw new ArgumentException($"Character {characterName} not found!");
            }
            if (!itemPool.Any())
            {
                throw new InvalidOperationException("No items left in pool!");
            }

            Item currItem = itemPool.Pop();
            character.Bag.AddItem(currItem);

            return $"{characterName} picked up {currItem.GetType().Name}!";
        }

        public string UseItem(string[] args)
        {
            string characterName = args[0];
            string itemName = args[1];

            Character currCharacter = party.FirstOrDefault(n => n.Name == characterName);
            Item currItem = currCharacter.Bag.GetItem(itemName);

            if (currCharacter == null)
            {
                throw new ArgumentException($"Character {characterName} not found!");
            }
            //if (currItem == null)
            //{
            //    throw new ArgumentException($"No item with name {itemName} in bag!");
            //}
            //if (currCharacter.Bag.Items.Any == 0)
            //{
            //    throw new ArgumentException(ExceptionMessages.EmptyBag);
            //}
           // var item = currCharacter.Bag.GetItem(itemName);

            //currCharacter.Bag.GetItem(itemName);
            currCharacter.UseItem(currItem);

            return $"{currCharacter.Name} used {itemName}.";

        }

        public string GetStats()
        {
            StringBuilder sb = new StringBuilder();
            List<Character> characters = new List<Character>(party.OrderByDescending(x => x.IsAlive)
                .ThenByDescending(x => x.Health)
                .ToList());

            foreach (var character in characters)
            {
                sb.AppendLine($"{character.Name} - HP: {character.Health}/{character.BaseHealth}, AP: {character.Armor}/{character.BaseArmor}, Status: {(character.IsAlive ? "Alive" : "Dead")}");
            }

            return sb.ToString().TrimEnd();
        }

        public string Attack(string[] args)
        {
            string attackerName = args[0];
            string receiverName = args[1];


            Character attacker = party.FirstOrDefault(x => x.Name == attackerName);
            Character receiver = party.FirstOrDefault(x => x.Name == receiverName);

            if (attacker == null)
            {
                throw new ArgumentException($"Character {attackerName} not found!");
            }
            if (receiver == null)
            {
                throw new ArgumentException($"Character {receiverName} not found!");
            }

            if (!(attacker is IAttacker attackerChar))
            {
                throw new ArgumentException($"{attacker.Name} cannot attack!");
            }

            attackerChar.Attack(receiver);

            string msg = $"{attackerName} attacks {receiverName} for {attacker.AbilityPoints} hit points! {receiverName} has {receiver.Health}/{receiver.BaseHealth} HP and {receiver.Armor}/{receiver.BaseArmor} AP left!";

            if (!receiver.IsAlive)
            {
                msg += Environment.NewLine + $"{receiver.Name} is dead!";
            }

            return msg;
        }

        public string Heal(string[] args)
        {
            string healerName = args[0];
            string healingReceiverName = args[1];

            Character healer = party.FirstOrDefault(x => x.Name == healerName);
            Character receiver = party.FirstOrDefault(x => x.Name == healingReceiverName);

            if (healer == null)
            {
                throw new ArgumentException("Character {name} not found!");
            }
            if (receiver == null)
            {
                throw new ArgumentException("Character {name} not found!");
            }

            if (!(healer is IHealer healerCharacter))
            {
                throw new ArgumentException($"{healer.Name} cannot heal!");
            }

            healerCharacter.Heal(receiver);

            string msg = $"{healer.Name} heals {receiver.Name} for {healer.AbilityPoints}! {receiver.Name} has {receiver.Health} health now!";
            return msg;
        }
    }
}
