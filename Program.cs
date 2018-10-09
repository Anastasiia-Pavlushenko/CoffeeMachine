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

            private float price;
            public float Price
            {
                get
                {
                    return price;
                }

                private set
                {
                    price = value;
                }
            }

            private ushort coffee;
            public ushort Coffee
            {
                get
                {
                    return coffee;
                }

                private set
                {
                    coffee = value;
                }
            }

            private ushort milk;
            public ushort Milk
            {
                get
                {
                    return milk;
                }

                private set
                {
                    milk = value;
                }
            }

            private ushort water;
            public ushort Water
            {
                get
                {
                    return water;
                }

                private set
                {
                    water = value;
                }
            }

            private ushort sugar;
            public ushort Sugar
            {
                get
                {
                    return sugar;
                }

                private set
                {
                    sugar = value;
                }
            }
        }

        private Dictionary<Drink, Recipe> recipes;

        private string model;
        public string Model
        {
            get
            {
                return model;
            }

            private set
            {
                if (value.Length == 0)
                    throw new ArgumentNullException("Empty model name");
                if (value.IndexOf(' ') != -1) // string has space char
                    throw new ArgumentException("Model name must have no spaces");
                model = value;
            }
        }

        private ushort coffee, coffee_max;
        public ushort Coffee
        {
            get
            {
                return coffee;
            }

            private set
            {
                if (value > coffee_max || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid value for coffee portions amount");
                coffee = value;
            }
        }

        private ushort milk, milk_max;
        public ushort Milk
        {
            get
            {
                return milk;
            }

            private set
            {
                if (value > milk_max || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid value for milk portions amount");
                milk = value;
            }
        }

        private ushort water, water_max;
        public ushort Water
        {
            get
            {
                return water;
            }

            private set
            {
                if (value > water_max || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid value for water portions amount");
                water = value;
            }
        }

        private ushort sugar, sugar_max;
        public ushort Sugar
        {
            get
            {
                return sugar;
            }

            private set
            {
                if (value > sugar_max || value < 0)
                    throw new ArgumentOutOfRangeException("Invalid value for sugar portions amount");
                sugar = value;
            }
        }

        private float cashbox;
        public float Cashbox
        {
            get
            {
                return cashbox;
            }

            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Cashbox cannot hold a negative amount of money");
                cashbox = value;
            }
        }


        public CoffeeMachine(string model, ushort coffee_max, ushort milk_max, ushort water_max, ushort sugar_max)
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
            var recipes = new Dictionary<Drink, Recipe>
            {
                { Drink.Americano, new Recipe(15, 1, 0, 3, 1) },
                { Drink.Cappuccino, new Recipe(18, 1, 2, 1, 2) },
                { Drink.Espresso, new Recipe(12, 1, 0, 1, 1) },
                { Drink.Irish, new Recipe(16, 1, 1, 1, 2) },
                { Drink.Latte, new Recipe(20, 1, 2, 1, 2) }
            };
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
            foreach (Drink d in select)
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

        public float WithdrawCashbox()
        {
            float rv = Cashbox;
            Cashbox = 0;
            return rv;
        }
    }

    class Program
    {   
        static void Main(string[] args)
        {
            UnitTest.TestCase_EmptyTanks();
        }
    }

    public static class UnitTest
    {
        private static class Require
        {
            public static void That(bool expression, string exceptionMessage = "")
            {
                if (!expression) throw new InvalidProgramException(exceptionMessage);
            }
        }

        public static void TestCase_EmptyTanks()
        {
            // Preparation:
            var cm = new CoffeeMachine("Nescafe", 10, 10, 10, 10);

            // Expectation:
            Require.That(cm.Coffee == 0, "Coffee tank is not empty.");
            Require.That(cm.Milk == 0, "Milk tank is not empty.");
            Require.That(cm.Water == 0, "Water tank is not empty.");
            Require.That(cm.Sugar == 0, "Sugar tank is not empty.");
        }

        //TODO: Add more test cases using Requre.That()
    }

    
}
