using System;

namespace CoffeeMachine
{
    enum Drink : byte // Drinks names with prices
    {
        Espresso = 12,
        Americano = 15,
        Irish = 16,
        Latte = 20,
        Capucino = 18
    }

    class CoffeeMachine
    {
        public string Model
        {
            get
            {
                return Model;
            }

            private set
            {
                if (value.Length == 0)
                    throw new ArgumentNullException();
                if (value.IndexOf(' ') != -1) // string has space char
                    throw new ArgumentException();
                Model = value;
            }
        }

        private ushort coffee_max, milk_max, water_max, sugar_max;

        public ushort Coffee
        {
            get
            {
                return Coffee;
            }

            private set
            {
                if (value > coffee_max || value < 0)
                    throw new ArgumentOutOfRangeException();
                Coffee = value;
            }
        }

        public ushort Milk
        {
            get
            {
                return Milk;
            }

            private set
            {
                if (value > milk_max || value < 0)
                    throw new ArgumentOutOfRangeException();
                Milk = value;
            }
        }

        public ushort Water
        {
            get
            {
                return Water;
            }

            private set
            {
                if (value > water_max || value < 0)
                    throw new ArgumentOutOfRangeException();
                Water = value;
            }
        }

        public ushort Sugar
        {
            get
            {
                return Sugar;
            }

            private set
            {
                if (value > sugar_max || value < 0)
                    throw new ArgumentOutOfRangeException();
                Sugar = value;
            }
        }

        public float Cashbox
        {
            get
            {
                return Cashbox;
            }

            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                Cashbox = value;
            }
        }

        
        CoffeeMachine(string model, ushort coffee_max, ushort milk_max, ushort water_max, ushort sugar_max)
        {
            Model = model;
            this.coffee_max = coffee_max;
            this.milk_max = milk_max;
            this.water_max = water_max;
            this.sugar_max = sugar_max;

            //Empty by default
            Coffee = 0;
            Milk = 0;
            Water = 0;
            Sugar = 0;
            Cashbox = 0;
        }

        public float PrepareDrink(Drink select, float payment) // Returns change
        {
            if (payment < (byte)select) // Payment less than price of selected drink
            {
                Console.WriteLine("Payment not paid.");
                return payment;
            }

            switch(select)
            {
                case Drink.Americano:
                    //TODO: Handle the Americano prepare: whether resources in the coffee machine enough and charge
                    break;
                case Drink.Capucino:
                    //TODO: Handle the Capucino prepare
                    break;
                case Drink.Espresso:
                    //TODO: Handle the Espresso prepare
                    break;
                case Drink.Irish:
                    //TODO: Handle the Irish prepare
                    break;
                case Drink.Latte:
                    //TODO: Handle the Latte prepare
                    break;
                default: // Invalid select of drink
                    throw new ArgumentOutOfRangeException();
            }

            return payment;
        }

        public float PrepareSomeDrinks(Drink[] select, float payment)
        {
            foreach ( Drink d in select )
            {
                payment = PrepareDrink(d, payment);
            }

            return payment;
        }

    }
    
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: Tests of use of the coffee machine
        }
    }
}
