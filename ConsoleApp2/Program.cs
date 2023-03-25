namespace Farm
{
    public class Farm
    {
        public delegate void BarnHandler(string message, Priority priority);
        private BarnHandler notify;
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

        public void Load(int weight)
        {
            if (CurrentLoad + weight <= 100)
            {
                CurrentLoad += weight;
                notify?.Invoke($"Додано продукцiї до амбару: {weight}", Priority.Low);
            }
            else
            {
                notify?.Invoke($"Амбар переповнений. Не вдалося додати: {weight}", Priority.High);
            }
        }

        public void Unload(int weight)
        {
            if (CurrentLoad - weight >= 0)
            {
                CurrentLoad -= weight;
                notify?.Invoke($"Вiдвантажено продукцiї з амбару: {weight}", Priority.Low);
            }
            else
            {
                notify?.Invoke($"На амбарi недостатньо продукцiї для вiдвантаження: {weight}", Priority.High);
            }
        }

        private int currentLoad;
        public int CurrentLoad
        {
            get { return currentLoad; }
            private set
            {
                currentLoad = value;
                notify?.Invoke($"Змiнено заповненicть амбару: {currentLoad}", Priority.Low);
            }
        }
    }

    public enum Priority
    {
        Low,
        High
    }

    public class Subscriber
    {
        private Priority priority;

        public Subscriber(Priority priority)
        {
            this.priority = priority;
        }

        public void Subscribe(Farm farm)
        {
            farm.Notify += ReceiveMessage;
        }

        public void Unsubscribe(Farm farm)
        {
            farm.Notify -= ReceiveMessage;
        }

        private void ReceiveMessage(string message, Priority priority)
        {
            if (priority == this.priority)
            {
                Console.WriteLine(message);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Farm farm = new Farm();
            Subscriber highPrioritySubscriber = new Subscriber(Priority.High);
            Subscriber lowPrioritySubscriber = new Subscriber(Priority.Low);
            highPrioritySubscriber.Subscribe(farm);
            lowPrioritySubscriber.Subscribe(farm);

            farm.Load(50);
            farm.Load(2);
            farm.Load(7);
            farm.Load(50);
            farm.Unload(10);
            farm.Unload(20);
            farm.Unload(50);
            farm.Unload(35);

            highPrioritySubscriber.Unsubscribe(farm);
            lowPrioritySubscriber.Unsubscribe(farm);

            Console.ReadKey();
        }
    }
}

