using Easter.Core.Contracts;
using Easter.Models.Bunnies;
using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes;
using Easter.Models.Dyes.Contracts;
using Easter.Models.Eggs;
using Easter.Models.Eggs.Contracts;
using Easter.Models.Workshops;
using Easter.Models.Workshops.Contracts;
using Easter.Repositories;
using Easter.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easter.Core
{
    public class Controller : IController
    {
        private BunnyRepository bunnies;
        private EggRepository eggs;
        private IWorkshop workshop;
        private int colouredEggsCount;

        public Controller()
        {
            bunnies = new BunnyRepository();
            eggs = new EggRepository();
            workshop = new Workshop();
        }

        public string AddBunny(string bunnyType, string bunnyName)
        {
            if (bunnyType != "HappyBunny" && bunnyType != "SleepyBunny")
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidBunnyType);
            }
            
            IBunny myBunny = null;
            if (bunnyType == "HappyBunny")
            {
                myBunny = new HappyBunny(bunnyName);
            }
            else if (bunnyType == "SleepyBunny")
            {
                myBunny = new SleepyBunny(bunnyName);
            }
            bunnies.Add(myBunny);
            string result = string.Format(OutputMessages.BunnyAdded, bunnyType, bunnyName);
            return result;
        }

        public string AddDyeToBunny(string bunnyName, int power)
        {
            IDye bunnysDye = new Dye(power);
            IBunny myBunny = bunnies.FindByName(bunnyName);
            if (myBunny == null)
            {
                throw new InvalidOperationException(ExceptionMessages.InexistentBunny);
            }
            myBunny.AddDye(bunnysDye);
            string result = string.Format(OutputMessages.DyeAdded, power, bunnyName);
            return result;
        }

        public string AddEgg(string eggName, int energyRequired)
        {
            IEgg myEgg = new Egg(eggName, energyRequired);
            eggs.Add(myEgg);
            string result = string.Format(OutputMessages.EggAdded, eggName);
            return result;
        }

        public string ColorEgg(string eggName)
        {
            IEgg selectedEgg = eggs.FindByName(eggName);
            var selectedBunnies = bunnies.Models.Where(x => x.Energy >= 50).ToList();
            selectedBunnies = selectedBunnies.OrderByDescending(x => x.Energy).ToList();
            if (!selectedBunnies.Any())
            {
                throw new InvalidOperationException(ExceptionMessages.BunniesNotReady);
            }
            foreach (var bunny in selectedBunnies)
            {
                workshop.Color(selectedEgg, bunny);
                if (bunny.Energy == 0)
                {
                    bunnies.Remove(bunny);
                }
            }
            string result = string.Empty;
            if (selectedEgg.IsDone())
            {
                colouredEggsCount++;
                result = string.Format(OutputMessages.EggIsDone, eggName);
            }
            else
            {
                result = string.Format(OutputMessages.EggIsNotDone, eggName);
            }
            return result;
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{colouredEggsCount} eggs are done!");
            sb.AppendLine("Bunnies info:");
            foreach (var bunny in bunnies.Models)
            {
                sb.AppendLine(bunny.ToString());
            }
            return sb.ToString().TrimEnd();
        }
    }
}
