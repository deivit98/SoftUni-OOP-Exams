using NavalVessels.Models;
using NavalVessels.Models.Contracts;
using NavalVessels.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavalVessels.Core.Contracts
{
    public class Controller : IController
    {
        private VesselRepository vessels;
        private List<ICaptain> captains;

        public Controller()
        {
            vessels = new VesselRepository();
            captains = new List<ICaptain>();
        }
        public string AssignCaptain(string selectedCaptainName, string selectedVesselName)
        {
            ICaptain neededCaptain = captains.FirstOrDefault(x => x.FullName == selectedCaptainName);
            IVessel neededVessel = vessels.FindByName(selectedVesselName);
            if (neededCaptain == null)
            {
                return $"Captain {selectedCaptainName} could not be found.";
            }
            if (neededVessel == null)
            {
                return "Vessel {selectedVesselName} could not be found.";
            }
            if (neededVessel.Captain != null)
            {
                return $"Vessel {selectedVesselName} is already occupied.";
            }
            neededCaptain.Vessels.Add(neededVessel);
            neededVessel.Captain = neededCaptain;
            return $"Captain {selectedCaptainName} command vessel {selectedVesselName}.";
        }

        public string AttackVessels(string attackingVesselName, string defendingVesselName)
        {
            IVessel attackingVessel = vessels.FindByName(attackingVesselName);
            IVessel defendingVessel = vessels.FindByName(defendingVesselName);
            if (attackingVessel == null)
            {
                return $"Vessel {attackingVesselName} could not be found.";
            }
            if (defendingVessel == null)
            {
                return $"Vessel {defendingVesselName} could not be found.";
            }
            if (attackingVessel.ArmorThickness == 0)
            {
                return $"Unarmored vessel {attackingVesselName} cannot attack or be attacked.";
            }
            if (defendingVessel.ArmorThickness == 0)
            {
                return $"Unarmored vessel {defendingVesselName} cannot attack or be attacked.";
            }
            attackingVessel.Attack(defendingVessel);
            attackingVessel.Captain.IncreaseCombatExperience();
            defendingVessel.Captain.IncreaseCombatExperience();
            return $"Vessel {defendingVesselName} was attacked by vessel {attackingVesselName} - current armor thickness: {defendingVessel.ArmorThickness}.";
        }

        public string CaptainReport(string captainFullName)
        {
            ICaptain neededCaptain = captains.FirstOrDefault(x => x.FullName == captainFullName);
            return neededCaptain.Report();
        }

        public string HireCaptain(string fullName)
        {
            ICaptain myCaptain = new Captain(fullName);
            if (!captains.Contains(myCaptain))
            {
                captains.Add(myCaptain);
                return $"Captain {fullName} is hired.";
            }
            return "Captain {fullName} is already hired.";
        }

        public string ProduceVessel(string name, string vesselType, double mainWeaponCaliber, double speed)
        {
            IVessel myVessel;
            if (vesselType == "Battleship")
            {
                myVessel = new Battleship(name, mainWeaponCaliber, speed);
            }
            else if (vesselType == "Submarine")
            {
                myVessel = new Submarine(name, mainWeaponCaliber, speed);
            }
            else
            {
                return "Invalid vessel type.";
            }

            IVessel neededVessel = vessels.FindByName(name);
            if (neededVessel != null)
            {
                return $"{neededVessel.GetType().Name} vessel {name} is already manufactured.";
            }
            vessels.Add(myVessel);
            return $"{myVessel.GetType().Name} {name} is manufactured with the main weapon caliber of {mainWeaponCaliber} inches and a maximum speed of {speed} knots.";
        }

        public string ServiceVessel(string vesselName)
        {
            IVessel neededVessel = vessels.FindByName(vesselName);
            if (neededVessel != null)
            {
                neededVessel.RepairVessel();
                return $"Vessel {vesselName} was repaired.";
            }
            return $"Vessel {vesselName} could not be found.";
        }

        public string ToggleSpecialMode(string vesselName)
        {
            IVessel neededVessel = vessels.FindByName(vesselName);
            if (neededVessel.GetType().Name == "Battleship")
            {
                Battleship myVessel1 = neededVessel as Battleship;
                myVessel1.ToggleSonarMode();
                return $"Battleship {vesselName} toggled sonar mode.";
            }
            if (neededVessel.GetType().Name == "Submarine" )
            {
                Submarine myVessel2 = neededVessel as Submarine;
                myVessel2.ToggleSubmergeMode();
                return $"Submarine {vesselName} toggled submerge mode.";
            }
            return $"Vessel {vesselName} could not be found.";
        }

        public string VesselReport(string vesselName)
        {
            IVessel neededVessel = vessels.FindByName(vesselName);
            return neededVessel.ToString();
        }
    }
}
