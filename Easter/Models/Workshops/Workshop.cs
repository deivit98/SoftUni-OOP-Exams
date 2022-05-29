using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes;
using Easter.Models.Dyes.Contracts;
using Easter.Models.Eggs.Contracts;
using Easter.Models.Workshops.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easter.Models.Workshops
{
    public class Workshop : IWorkshop
    {
        public Workshop()
        {

        }

        public void Color(IEgg egg, IBunny bunny)
        {
            if (bunny.Energy > 0 && bunny.Dyes.Any(x => !x.IsFinished()))
            {
                IDye usedDye = bunny.Dyes.FirstOrDefault(x => !x.IsFinished());
                while (true)
                {
                    if (egg.IsDone())
                    {
                        break;
                    }

                    if (bunny.Energy == 0 || bunny.Dyes.All(x => x.IsFinished()))
                    {
                        break;
                    }
                    
                    egg.GetColored();
                    bunny.Work();
                    usedDye.Use();

                    if (usedDye.IsFinished() && bunny.Dyes.Any(x => !x.IsFinished()))
                    {
                        usedDye = bunny.Dyes.FirstOrDefault(x => !x.IsFinished());
                    }
                }
            }
        }
    }
}
