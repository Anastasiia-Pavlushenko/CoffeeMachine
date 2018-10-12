using System;
using System.Collections.Generic;

namespace CoffeeMachine
{
    enum Drink : byte
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
        private float Cashbox
        {
            get
            {
                return cashbox;
            }

            set
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
            recipes = new Dictionary<Drink, Recipe>
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
            if (!ResourcesEnough(select))
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
                Console.WriteLine("Please fill the tank with sugar!");
        }

        public float MakeDrink(Drink select, float payment) // Returns change
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

            Cashbox += rec.Price;
            payment -= rec.Price;

            return payment;
        }

        public float MakeSomeDrinks(Drink[] select, float payment)
        {
            foreach (Drink d in select)
            {
                payment = MakeDrink(d, payment);
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
            UnitTest.TestCase_FilledTanks();

            UnitTest.TestCase_MakeAmericano();
            UnitTest.TestCase_MakeCappuccino();
            UnitTest.TestCase_MakeEspresso();
            UnitTest.TestCase_MakeIrish();
            UnitTest.TestCase_MakeLatte();

            UnitTest.TestCase_MakeThreeDrinks();
            UnitTest.TestCase_MakeDrinksNotEnoughMoney();

            UnitTest.TestCase_EmptyModelName();
            UnitTest.TestCase_ModelNameWithSpace();

            UnitTest.TestCase_NotEnoughResources();

            UnitTest.TestCase_WithdrawEmptyCashbox();
            UnitTest.TestCase_WithdrawCashbox();
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

        private static CoffeeMachine CreateFilledCoffeeMachine()
        {
            var cm = new CoffeeMachine("Nescafe", 10, 10, 10, 10);
            cm.FillCoffeeTank();
            cm.FillMilkTank();
            cm.FillWaterTank();
            cm.FillSugarTank();
            return cm;
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

        public static void TestCase_FilledTanks()
        {
            var cm = new CoffeeMachine("Nescafe", 10, 10, 10, 10);

            cm.FillCoffeeTank();
            Require.That(cm.Coffee == 10, "Coffee tank is not filled.");

            cm.FillMilkTank();
            Require.That(cm.Milk == 10, "Milk tank is not filled.");

            cm.FillWaterTank();
            Require.That(cm.Water == 10, "Water tank is not filled.");

            cm.FillSugarTank();
            Require.That(cm.Sugar == 10, "Sugar tank is not filled.");
        }

        public static void TestCase_MakeAmericano()
        {
            var cm = CreateFilledCoffeeMachine();
            float cash = 50.0F;

            cash = cm.MakeDrink(Drink.Americano, cash);

            Require.That(cash == 35, "Uncorrect payment.");
            Require.That(cm.Coffee == 9, "Uncorrect coffee using.");
            Require.That(cm.Milk == 10, "Uncorrect milk using.");
            Require.That(cm.Water == 7, "Uncorrect water using.");
            Require.That(cm.Sugar == 9, "Uncorrect sugar using.");
        }

        public static void TestCase_MakeCappuccino()
        {
            var cm = CreateFilledCoffeeMachine();
            float cash = 50.0F;

            cash = cm.MakeDrink(Drink.Cappuccino, cash);

            Require.That(cash == 32, "Uncorrect payment.");
            Require.That(cm.Coffee == 9, "Uncorrect coffee using.");
            Require.That(cm.Milk == 8, "Uncorrect milk using.");
            Require.That(cm.Water == 9, "Uncorrect water using.");
            Require.That(cm.Sugar == 8, "Uncorrect sugar using.");
        }

        public static void TestCase_MakeEspresso()
        {
            var cm = CreateFilledCoffeeMachine();
            float cash = 50.0F;

            cash = cm.MakeDrink(Drink.Espresso, cash);

            Require.That(cash == 38, "Uncorrect payment.");
            Require.That(cm.Coffee == 9, "Uncorrect coffee using.");
            Require.That(cm.Milk == 10, "Uncorrect milk using.");
            Require.That(cm.Water == 9, "Uncorrect water using.");
            Require.That(cm.Sugar == 9, "Uncorrect sugar using.");
        }

        public static void TestCase_MakeIrish()
        {
            var cm = CreateFilledCoffeeMachine();
            float cash = 50.0F;

            cash = cm.MakeDrink(Drink.Irish, cash);

            Require.That(cash == 34, "Uncorrect payment.");
            Require.That(cm.Coffee == 9, "Uncorrect coffee using.");
            Require.That(cm.Milk == 9, "Uncorrect milk using.");
            Require.That(cm.Water == 9, "Uncorrect water using.");
            Require.That(cm.Sugar == 8, "Uncorrect sugar using.");
        }

        public static void TestCase_MakeLatte()
        {
            var cm = CreateFilledCoffeeMachine();
            float cash = 50.0F;

            cash = cm.MakeDrink(Drink.Latte, cash);

            Require.That(cash == 30, "Uncorrect payment.");
            Require.That(cm.Coffee == 9, "Uncorrect coffee using.");
            Require.That(cm.Milk == 8, "Uncorrect milk using.");
            Require.That(cm.Water == 9, "Uncorrect water using.");
            Require.That(cm.Sugar == 8, "Uncorrect sugar using.");
        }

        public static void TestCase_MakeThreeDrinks()
        {
            var cm = CreateFilledCoffeeMachine();
            float cash = 50.0F;
            Drink[] order = { Drink.Espresso, Drink.Latte, Drink.Irish };

            cash = cm.MakeSomeDrinks(order, cash);

            Require.That(cash == 50 - 12 - 20 - 16, "Uncorrect payment.");
            Require.That(cm.Coffee == 7, "Uncorrect coffee using.");
            Require.That(cm.Milk == 7, "Uncorrect milk using.");
            Require.That(cm.Water == 7, "Uncorrect water using.");
            Require.That(cm.Sugar == 5, "Uncorrect sugar using.");
        }

        public static void TestCase_MakeDrinksNotEnoughMoney()
        {
            var cm = CreateFilledCoffeeMachine();
            float cash = 40.0F;
            Drink[] order = { Drink.Americano, Drink.Cappuccino, Drink.Espresso };

            cash = cm.MakeSomeDrinks(order, cash); // Makes only first two drinks

            Require.That(cash == 40 - 15 - 18, "Uncorrect payment.");
            Require.That(cm.Coffee == 8, "Uncorrect coffee using.");
            Require.That(cm.Milk == 8, "Uncorrect milk using.");
            Require.That(cm.Water == 6, "Uncorrect water using.");
            Require.That(cm.Sugar == 7, "Uncorrect sugar using.");
        }

        public static void TestCase_EmptyModelName()
        {
            bool catched = false;

            try
            {
                var cm = new CoffeeMachine("", 10, 10, 10, 10);
            }
            catch (Exception)
            {
                catched = true;
            }

            Require.That(catched);
        }

        public static void TestCase_ModelNameWithSpace()
        {
            bool catched = false;

            try
            {
                var cm = new CoffeeMachine("Mama Mia", 10, 10, 10, 10);
            }
            catch (Exception)
            {
                catched = true;
            }

            Require.That(catched);
        }

        public static void TestCase_NotEnoughResources()
        {
            var cm = new CoffeeMachine("Nescafe", 10, 10, 10, 10);
            float cash = 50.0F;

            cash = cm.MakeDrink(Drink.Americano, cash);

            Require.That(cash == 50);
        }

        public static void TestCase_WithdrawEmptyCashbox()
        {
            var cm = CreateFilledCoffeeMachine();

            float profit = cm.WithdrawCashbox();

            Require.That(profit == 0);
        }

        public static void TestCase_WithdrawCashbox()
        {
            var cm = CreateFilledCoffeeMachine();
            float cash = 50.0F;
            Drink[] order = { Drink.Espresso, Drink.Latte, Drink.Irish };

            cash = cm.MakeSomeDrinks(order, cash);

            float profit = cm.WithdrawCashbox();

            Require.That(cash == 2);
            Require.That(profit == 48);
        }
    }
}
