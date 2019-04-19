using System;

namespace DecoratorPatternApp
{
    public class SportBasicCar : CarDecorator
    {
        private readonly string[] _features = { "Spoiler", "Hood Scoop" };

        public SportBasicCar(ICar car) : base(car, 2) { }

        public override void Assemble()
        {
            base.Assemble();
            Console.WriteLine("Adding sport features");
        }

        public override void ListFeatures()
        {
            base.ListFeatures();

            foreach (var feature in _features)
            {
                Console.WriteLine($"{Indent}{feature}");
            }
        }
    }
}