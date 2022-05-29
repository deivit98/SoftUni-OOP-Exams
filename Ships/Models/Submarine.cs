using NavalVessels.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace NavalVessels.Models
{

    public class Submarine : Vessel, ISubmarine
    {
        
        public Submarine(string name, double mainWeaponCaliber, double speed) 
            : base(name, mainWeaponCaliber, speed, 200)
        {
            SubmergeMode = false;
        }

        public bool SubmergeMode { get; private set; }

        public void ToggleSubmergeMode()
        {
            if (SubmergeMode == false)
            {
                SubmergeMode = true;
                MainWeaponCaliber += 40;
                Speed -= 4;
            }
            else
            {
                SubmergeMode = false;
                MainWeaponCaliber -= 40;
                Speed += 4;
            }
        }

        public override void RepairVessel()
        {
            if (ArmorThickness < 200)
            {
                ArmorThickness = 200;
            }
        }

        private string SubmergeModeOnOrOff()
        {
            if (SubmergeMode)
            {
                return "ON";
            }
            else
            {
                return "OFF";
            }
        }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + $" *Submerge mode: {SubmergeModeOnOrOff()}";
        }
    }
}
