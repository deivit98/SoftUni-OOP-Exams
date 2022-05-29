using Formula1.Core.Contracts;
using Formula1.Models;
using Formula1.Models.Contracts;
using Formula1.Repositories;
using Formula1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formula1.Core
{
    internal class Controller : IController
    {
        private PilotRepository pilot;
        private RaceRepository race;
        private FormulaOneCarRepository formulaOneCar;
        public Controller()
        {
            pilot = new PilotRepository();
            race = new RaceRepository();
            formulaOneCar = new FormulaOneCarRepository();
        }
        public string AddCarToPilot(string pilotName, string carModel)
        {
            IFormulaOneCar car = this.formulaOneCar.Models.FirstOrDefault(x => x.Model == carModel);
            IPilot pilot = this.pilot.Models.FirstOrDefault(x => x.FullName == pilotName);
            if (pilot == null || pilot.CanRace)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.PilotDoesNotExistOrHasCarErrorMessage,pilotName));
            }
            if (car == null)
            {
                throw new NullReferenceException(String.Format(ExceptionMessages.CarDoesNotExistErrorMessage, carModel));
            }
            pilot.AddCar(car);
            formulaOneCar.Remove(car);
            return $"Pilot {pilotName} will drive a {car.GetType().Name} {carModel} car.";
        }

        public string AddPilotToRace(string raceName, string pilotFullName)
        {
            IRace race = this.race.Models.FirstOrDefault(x => x.RaceName == raceName);
            IPilot pilot = this.pilot.Models.FirstOrDefault(x => x.FullName == pilotFullName);
            if (race == null)
            {
                throw new NullReferenceException(String.Format(ExceptionMessages.RaceDoesNotExistErrorMessage, raceName));
            }
            if (pilot == null || !pilot.CanRace || race.Pilots.Contains(pilot))
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.PilotDoesNotExistErrorMessage, pilotFullName));
            }
            race.AddPilot(pilot);
            return $"Pilot {pilotFullName} is added to the {raceName} race.";
        }

        public string CreateCar(string type, string model, int horsepower, double engineDisplacement)
        {
            IFormulaOneCar carToAdd = null;
            IFormulaOneCar car = this.formulaOneCar.Models.FirstOrDefault(x => x.Model == model);
            if (car != null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.CarExistErrorMessage, model));
            }
            else
            {
                if (type == "Ferrari")
                {
                    carToAdd = new Ferrari(model, horsepower, engineDisplacement);
                }
                else if (type == "Williams")
                {
                    carToAdd = new Williams(model, horsepower, engineDisplacement);
                }
                else
                {
                    throw new InvalidOperationException(String.Format(ExceptionMessages.InvalidTypeCar, type));
                }
            }
            formulaOneCar.Add(carToAdd);
            return $"Car {type}, model {model} is created.";
        }

        public string CreatePilot(string fullName)
        {
            IPilot pilot = this.pilot.Models.FirstOrDefault(x => x.FullName == fullName);
            if (pilot != null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.PilotExistErrorMessage, fullName));
            }

            IPilot pilotToAdd = new Pilot(fullName);
            this.pilot.Add(pilotToAdd);
            return $"Pilot {fullName} is created.";

        }

        public string CreateRace(string raceName, int numberOfLaps)
        {
            IRace race = this.race.Models.FirstOrDefault(x => x.RaceName == raceName);
            if (race != null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.RaceExistErrorMessage, raceName));
            }

            IRace raceToAdd = new Race(raceName, numberOfLaps);
            this.race.Add(raceToAdd);
            return $"Race {raceName} is created.";

        }

        public string PilotReport()
        {
            StringBuilder sb = new StringBuilder();
            var sortedPilots = this.pilot.Models.OrderByDescending(x => x.NumberOfWins);
            foreach (IPilot pilot in sortedPilots)
            {
                sb.AppendLine(pilot.ToString());
            }
            return sb.ToString().TrimEnd();
        }

        public string RaceReport()
        {
            StringBuilder sb = new StringBuilder();
            foreach (IRace race in this.race.Models)
            {
                if (race.TookPlace)
                {
                    sb.AppendLine(race.RaceInfo());
                }
            }
            return sb.ToString().TrimEnd();
        }

        public string StartRace(string raceName)
        {
            IRace race = this.race.Models.FirstOrDefault(x => x.RaceName == raceName);
            if (race == null)
            {
                throw new NullReferenceException(String.Format(ExceptionMessages.RaceDoesNotExistErrorMessage, raceName));
            }
            if (race.Pilots.Count < 3)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.InvalidRaceParticipants, raceName));
            }
            if (race.TookPlace)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.RaceTookPlaceErrorMessage, raceName));
            }
            var winners = race.Pilots.OrderByDescending(x => x.Car.RaceScoreCalculator(race.NumberOfLaps)).ToList();
            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (var win in winners)
            {
                if (count == 3)
                {
                    break;
                }
                else if (count == 0)
                {
                    sb.AppendLine($"Pilot {win.FullName} wins the {race.RaceName} race.");
                }
                else if (count == 1)
                {
                    sb.AppendLine($"Pilot {win.FullName} is second the {race.RaceName} race.");
                }
                else if (count == 2)
                {
                    sb.AppendLine($"Pilot {win.FullName} is third the {race.RaceName} race.");
                }
                count++;
            }
            winners[0].WinRace();
            race.TookPlace = true;
            return sb.ToString().TrimEnd();
        }
    }
}
