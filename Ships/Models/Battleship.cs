using NavalVessels.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace NavalVessels.Models
{
    public class Battleship : Vessel, IBattleship
    {
        public Battleship(string name, double mainWeaponCaliber, double speed) 
            : base(name, mainWeaponCaliber, speed, 300)
        {
            SonarMode = false;
        }
        public bool SonarMode { get; private set; }

        public void ToggleSonarMode()
        {
            if (SonarMode == false)
            {
                SonarMode = true;
                MainWeaponCaliber += 40;
                Speed -= 5;
            }
            else
            {
                SonarMode = false;
                MainWeaponCaliber -= 40;
                Speed += 5;
            }
        }

        private string SonarModeOnOrOff()
        {
            if (SonarMode)
            {
                return "ON";
            }
            else
            {
                return "OFF";
            }
        }

        public override void RepairVessel()
        {
            if (ArmorThickness < 300)
            {
                ArmorThickness = 300;
            }
        }
        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + $" *Sonar mode: {SonarModeOnOrOff()}";
        }
    }
}
