using Gym.Models.Athletes.Contracts;
using Gym.Models.Equipment.Contracts;
using Gym.Models.Gyms.Contracts;
using Gym.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gym.Models.Gyms
{
    public abstract class Gym : IGym
    {
        private string name;
       
        private ICollection<IEquipment> equipment;
        private ICollection<IAthlete> athletes;
      
        public Gym(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
           equipment = new List<IEquipment>();
           athletes = new List<IAthlete>();
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(ExceptionMessages.InvalidGymName);
                }
                this.name = value;
            }
        }

        public int Capacity { get;private set; }

        public double EquipmentWeight 
        {
            get { return this.equipment.Select(x=>x.Weight).Sum(); }
        }


        public ICollection<IEquipment> Equipment => equipment;

        public ICollection<IAthlete> Athletes => athletes;
        public void AddAthlete(IAthlete athlete)
        {
           if(Capacity < athletes.Count)
            {
                throw new InvalidOperationException(ExceptionMessages.NotEnoughSize);
            }
            athletes.Add(athlete);
        }

        public void AddEquipment(IEquipment equipment)
        {
            this.equipment.Add(equipment);
        }

        public void Exercise()
        {
           foreach(var athlete in athletes)
            {
                athlete.Exercise();
            }
        }

        public string GymInfo()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            sb.AppendLine($"{this.Name} is a {this.GetType().Name}");
            if(athletes.Count <= 0)
            {
                sb.AppendLine("Athletes: No athletes");
            }
            else
            {
                string atl = String.Join(", ", athletes.Select(x => x.FullName));
                sb.AppendLine($"Athletes: {atl}");                
            }
            sb.AppendLine($"Equipment total count: {Equipment.Count}");
            sb.AppendLine($"Equipemnt total weight: {this.EquipmentWeight:f2} grams");

            return sb.ToString().TrimEnd();
        }

        public bool RemoveAthlete(IAthlete athlete)
        {
            return athletes.Remove(athlete);
        }
    }
}
