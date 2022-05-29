﻿using Formula1.Models.Contracts;
using Formula1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formula1.Repositories
{
    internal class RaceRepository : IRepository<IRace>
    {
        private List<IRace> models;
        public RaceRepository()
        {
            models = new List<IRace>();
        }
        public IReadOnlyCollection<IRace> Models => this.models;

        public void Add(IRace model)
        {
            models.Add(model);
        }

        public IRace FindByName(string name)
        {
            IRace race = models.FirstOrDefault(x => x.RaceName == name);
            if (race == null)
            {
                return null;
            }
            else
            {
                return race;
            }
        }

        public bool Remove(IRace model)
        {
            return models.Remove(model);
        }
    }
}
