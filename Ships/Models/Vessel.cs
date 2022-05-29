using NavalVessels.Models.Contracts;
using NavalVessels.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace NavalVessels.Models
{
    public abstract class Vessel : IVessel
    {

        private string name;
        private ICaptain captain;
        private double armorThinkness;
        private double mainWeaponCaliber;
        private double speed;
        private readonly List<string> targets;

        public Vessel(string name, double mainWeaponCaliber, double speed, double armorThickness)
        {
            Name = name;
            MainWeaponCaliber = mainWeaponCaliber;
            Speed = speed;
            ArmorThickness = armorThickness;
            targets = new List<string>();
        }

        public string Name 
        { 
            get 
            {
                return name;
            }
            
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(ExceptionMessages.InvalidVesselName);
                }
                name = value;
            }
        }

        public ICaptain Captain 
        {
            get
            {
                return captain;
            }
            set 
            {
                if (value == null)
                {
                    throw new NullReferenceException(ExceptionMessages.InvalidCaptainToVessel);
                }
                captain = value;
            }
        }
        public double ArmorThickness 
        {
            get => armorThinkness;
            
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                armorThinkness = value;
            }
        }

        public double MainWeaponCaliber 
        {
            get => mainWeaponCaliber;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                mainWeaponCaliber = value;
            }
        }
    
        public double Speed 
        {
            get => speed;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                speed = value;
            }
        }

        public ICollection<string> Targets { get => targets; }

        public void Attack(IVessel target)
        {
            if (target == null)
            {
                throw new NullReferenceException(ExceptionMessages.InvalidTarget);
            }
            target.ArmorThickness -= MainWeaponCaliber;
            if (target.ArmorThickness < 0)
            {
                target.ArmorThickness = 0;
            }
            Targets.Add(target.Name);
        }

        public abstract void RepairVessel();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"- {Name}");
            sb.AppendLine($" *Type: { this.GetType().Name}");
            sb.AppendLine($" *Armor thickness: {this.ArmorThickness}");
            sb.AppendLine($" *Main weapon caliber: {this.MainWeaponCaliber}");
            sb.AppendLine($" *Speed: {this.Speed} knots");
            sb.Append(" *Targets: ");
            if (this.targets.Count > 0)
            {
                sb.Append(string.Join(", ", this.targets));
            }
            else
            {
                sb.AppendLine("None");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
