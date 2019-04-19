using System;

namespace DecoratorPatternApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ICar car;

            Console.WriteLine("Basic Car");
            CarFactory.DemoCar(CarType.Basic);

            Console.ReadLine();

            Console.WriteLine("Sport Car");
            CarFactory.DemoCar(CarType.Sport);

            Console.ReadLine();

            Console.WriteLine("Sport Car");
            car = CarFactory.GetCar(CarType.Sport);
            car = CarFactory.AddFeature(car, Feature.SoundSystemBasic);
            CarFactory.DemoCar(car);

            Console.ReadLine();

            Console.WriteLine("Basic Car");
            car = CarFactory.GetCar(CarType.Basic);
            car = CarFactory.AddFeature(car, Feature.Turbo);
            CarFactory.DemoCar(car);

            Console.ReadLine();

            Console.WriteLine("Sport Extreme Car");
            car = CarFactory.GetCar(CarType.Sport);
            car = CarFactory.AddFeature(car, Feature.Turbo);
            car = CarFactory.AddFeature(car, Feature.SoundSystemBasic);
            CarFactory.DemoCar(car);

            Console.ReadLine();
        }
    }
}
