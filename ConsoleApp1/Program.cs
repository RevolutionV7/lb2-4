using System;
namespace Farm
{
    class Farm
    {
        public delegate void BarnHandler(string message);
        private BarnHandler notify;
        private int currentLoad;

        public event BarnHandler Notify
        {
            add
            {
                notify += value;
                Console.WriteLine($"{value.Method.Name} додано");
            }
            remove
            {
                notify -= value;
                Console.WriteLine($"{value.Method.Name} видалено");
            }
        }

        public int CurrentLoad
        {
            get { return currentLoad; }
            private set
            {
                currentLoad = value;
                notify?.Invoke($"Змiнено заповненicть амбару: {currentLoad}");
            }
        }

        public void Load(int weight)
        {
            if (CurrentLoad + weight <= 100)
            {
                CurrentLoad += weight;
                notify?.Invoke($"Додано продукцiї до амбару: {weight}");
            }
            else
            {
                notify?.Invoke($"Амбар переповнений. Не вдалося додати: {weight}");
            }
        }

        public void Unload(int weight)
        {
            if (CurrentLoad - weight >= 0)
            {
                CurrentLoad -= weight;
                notify?.Invoke($"Вiдвантажено продукцiї з амбару: {weight}");
            }
            else
            {
                notify?.Invoke($"На амбарi недостатньо продукцiї для вiдвантаження: {weight}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Farm farm = new Farm();
            farm.Notify += ShowMessage;
            farm.Load(50);
            farm.Load(2);
            farm.Load(7);
            farm.Load(50);
            farm.Unload(10);
            farm.Unload(20);
            farm.Unload(50);
            farm.Unload(35);

            Console.ReadKey();
        }

        static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
