using Gym.Models.Athletes;
using Gym.Models.Athletes.Contracts;
using Gym.Models.Equipment;
using Gym.Models.Equipment.Contracts;
using Gym.Models.Gyms;
using Gym.Models.Gyms.Contracts;
using Gym.Repositories;
using Gym.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gym.Core.Contracts
{
    public class Controller : IController
    {
        private EquipmentRepository equipments;
        private List<IGym> gyms;

        public Controller()
        {
            equipments = new EquipmentRepository();
            gyms = new List<IGym>();
        }


        public string AddAthlete(string gymName, string athleteType, string athleteName, string motivation, int numberOfMedals)
        {
            IGym gym = gyms.Find(x => x.Name == gymName);
            IAthlete athlete;

            if(athleteType == "Boxer")
            {
                athlete = new Boxer(athleteName, motivation, numberOfMedals);
            }
            else if(athleteType == "Weightlifter")
            {
                athlete = new Weightlifter(athleteName, motivation, numberOfMedals);
            }
            else
            {
                throw new InvalidCastException(ExceptionMessages.InvalidAthleteType);
            }

            if(athlete.GetType().Name == "Boxer" && gym.GetType().Name == "BoxingGym")
            {
                gym.AddAthlete(athlete);
            }
            else if(athlete.GetType().Name == "Weightlifter" && gym.GetType().Name == "WeightliftingGym")
            {
                gym.AddAthlete(athlete);
            }
            else
            {
                return string.Format(OutputMessages.InappropriateGym);
            }
            return string.Format(OutputMessages.EntityAddedToGym, athleteType, gymName);
        }

        public string AddEquipment(string equipmentType)
        {
            IEquipment equipment;
            if (equipmentType == "Kettlebell")
            {
                equipment = new Kettlebell();
            }
            else if(equipmentType == "BoxingGloves")
            {
                equipment = new BoxingGloves();
            }
            else
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidEquipmentType);
            }
            equipments.Add(equipment);
            return string.Format(OutputMessages.SuccessfullyAdded, equipmentType);
        }

        public string AddGym(string gymType, string gymName)
        {
            IGym gym;
            if(gymType == "BoxingGym")
            {
                gym = new BoxingGym(gymName);
            }
            else if(gymType == "WeightliftingGym")
            {
                gym = new WeightliftingGym(gymName);
            }
            else
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidGymType);
            }
            gyms.Add(gym);
            return string.Format(OutputMessages.SuccessfullyAdded, gymType);
        }

        public string EquipmentWeight(string gymName)
        {
            IGym gym = gyms.Find(x => x.Name == gymName);
            decimal sum = (decimal)gym.EquipmentWeight;
            return string.Format(OutputMessages.EquipmentTotalWeight, gymName, sum );
        }

        public string InsertEquipment(string gymName, string equipmentType)
        {
            IGym gym = gyms.Find(x => x.Name == gymName);
            IEquipment equipment = equipments.Models.FirstOrDefault(x => x.GetType().Name == equipmentType);
            if(equipment == null)
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidEquipmentType);
            }

            gym.AddEquipment(equipment);
            return string.Format(OutputMessages.EntityAddedToGym, equipmentType, gymName);


            
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var gym in gyms)
            {
                sb.AppendLine(gym.GymInfo());
            }
            return sb.ToString().TrimEnd();
        }

        public string TrainAthletes(string gymName)
        {
            IGym gym = gyms.Find(x => x.Name == gymName);
            int count = 0;
            foreach(Athlete athlete in gym.Athletes)
            {
                athlete.Exercise();
                count++;
            }
            return string.Format(OutputMessages.AthleteExercise, count);
        }
    }
}
