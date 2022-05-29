namespace Bakery.Models.BakedFoods
{
    class Cake : BakedFood
    {
        private const int InitialBreadPortion = 245;
        public Cake(string name, decimal price) : base(name, InitialBreadPortion, price)
        {
        }
    }
}
