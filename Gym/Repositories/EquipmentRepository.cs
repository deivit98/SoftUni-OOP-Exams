using Gym.Models.Equipment.Contracts;
using Gym.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gym.Repositories
{
    public class EquipmentRepository : IRepository<IEquipment>
    {
        private  List<IEquipment> equipment;
       

        public EquipmentRepository()
        {
            equipment = new List<IEquipment>();
        }
        public IReadOnlyCollection<IEquipment> Models => equipment;
        public void Add(IEquipment model)
        {
            equipment.Add(model);
        }

        public IEquipment FindByType(string type)
        {
            return equipment.FirstOrDefault(x => x.GetType().Name == type);
        }

        public bool Remove(IEquipment model)
        {
           return equipment.Remove(model);
        }
    }
}
