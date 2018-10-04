using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
    class CoffeeMachine
    {
        public ushort Coffee = 15;
        public ushort Milk = 10;
        public ushort Water = 10;
        public ushort Sugar = 55;

        public ushort MaxOfCoffee = 200;
        public ushort MaxOfMilk = 200;
        public ushort MaxOfWater = 200;
        public ushort MaxOfSugar = 600;

        public uint CashBox = 3250;

        public int PreparationDrink(string TypeOfCoffee, double Price)
        {
            return 0;
        }
        public int SeveralDrinks(int SumOfDrinks, double Payment)
        {
            return 0;
        }
        public int Restocking(string TypeOfProducts)
        {
        return 0;
        }
        public int CashBack()
        {
            return 0;
        }
        public string Message()
        {
            return "Message";
        }
    }

}
