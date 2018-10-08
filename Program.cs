using System;
using System.Collections.Generic;

namespace CoffeeMachine
{
    enum Drink : byte // Drinks names with prices
    {
        Espresso,
        Americano,
        Irish,
        Latte,
        Cappuccino
    }

    class CoffeeMachine
    {
        private class Recipe
        {
            public Recipe(float price, ushort coffee, ushort milk, ushort water, ushort sugar)
            {
                Price = price;
                Coffee = coffee;
                Milk = milk;
                Water = water;
                Sugar = sugar;
            }

            public float Price
            {
                get
                {
                    return Price;
                }

                private set
                {
                    Price = value;
                }
            }

            public ushort Coffee
            {
                get
                {
                    return Coffee;
                }

                private set
                {
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
                    Sugar = value;
                }
            }
        }

        private Dictionary<Drink, Recipe> recipes; //TODO: Use initialization

        public string Model
        {
            get
            {
                return Model;
            }

            private set
            {
                if (value.Length == 0)
                    throw new ArgumentNullException("Empty model name");
                if (value.IndexOf(' ') != -1) // string has space char
                    throw new ArgumentException("Model name must have no spaces");
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
                    throw new ArgumentOutOfRangeException("Invalid value for coffee portions amount");
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
                    throw new ArgumentOutOfRangeException("Invalid value for milk portions amount");
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
                    throw new ArgumentOutOfRangeException("Invalid value for water portions amount");
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
                    throw new ArgumentOutOfRangeException("Invalid value for sugar portions amount");
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
                    throw new ArgumentOutOfRangeException("Cashbox cannot hold a negative amount of money");
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

            //Empty containers by default
            Coffee = 0;
            Milk = 0;
            Water = 0;
            Sugar = 0;
            Cashbox = 0;

            //Filling recipes
            recipes.Add(Drink.Americano, new Recipe(15, 1, 0, 3, 1));
            recipes.Add(Drink.Cappuccino, new Recipe(18, 1, 2, 1, 2));
            recipes.Add(Drink.Espresso, new Recipe(12, 1, 0, 1, 1));
            recipes.Add(Drink.Irish, new Recipe(16, 1, 1, 1, 2));
            recipes.Add(Drink.Latte, new Recipe(20, 1, 2, 1, 2));
        }

        private bool ResourcesEnough(Drink select)
        {
            Recipe rec = recipes[select];
            return (rec.Coffee <= Coffee &&
                rec.Milk <= Milk &&
                rec.Water <= Water &&
                rec.Sugar <= Sugar);
        }

        private void UseResources(Drink select)
        {
            if (ResourcesEnough(select))
                throw new ArgumentException("Not enough resources to use");
            Recipe rec = recipes[select];
            Coffee -= rec.Coffee;
            Milk -= rec.Milk;
            Water -= rec.Water;
            Sugar -= rec.Sugar;
        }

        private void TanksFillingNotification(Drink select)
        {
            Recipe rec = recipes[select];
            if (rec.Coffee > Coffee)
                Console.WriteLine("Please fill the tank with coffee!");
            if (rec.Milk > Milk)
                Console.WriteLine("Please fill the tank with milk!");
            if (rec.Water > Water)
                Console.WriteLine("Please fill the tank with water!");
            if (rec.Sugar > Sugar)
                Console.WriteLine("Please fill the tank with coffee!");
        }

        public float PrepareDrink(Drink select, float payment) // Returns change
        {
            Recipe rec = recipes[select];
            if (payment < rec.Price)
            {
                Console.WriteLine($"[select] not paid.");
                return payment;
            }

            if (!ResourcesEnough(select))
            {
                Console.WriteLine($"Sorry! [Model] have not enough resouces for your coffee.");
                TanksFillingNotification(select);
                return payment;
            }

            UseResources(select);

            payment -= rec.Price;

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

        public void FillCoffeeTank()
        {
            Coffee = coffee_max;
        }

        public void FillMilkTank()
        {
            Milk = milk_max;
        }

        public void FillWaterTank()
        {
            Water = water_max;
        }

        public void FillSugarTank()
        {
            Sugar = sugar_max;
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
