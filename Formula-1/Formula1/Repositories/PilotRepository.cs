using Formula1.Models.Contracts;
using Formula1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formula1.Repositories
{
    internal class PilotRepository : IRepository<IPilot>
    {
        private List<IPilot> models;
        public PilotRepository()
        {
            models = new List<IPilot>();
        }
        public IReadOnlyCollection<IPilot> Models => this.models;

        public void Add(IPilot model)
        {
            models.Add(model);
        }

        public IPilot FindByName(string name)
        {
            IPilot pilot = models.FirstOrDefault(x => x.FullName == name);
            if (pilot == null)
            {
                return null;
            }
            else
            {
                return pilot;
            }
        }

        public bool Remove(IPilot model)
        {
            return models.Remove(model);
        }
    }
}
