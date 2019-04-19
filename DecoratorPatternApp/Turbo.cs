using System;

namespace DecoratorPatternApp
{
    public class Turbo : CarDecorator
    {
        private readonly string[] _features = { "Turbo" };

        public Turbo(ICar car) : base(car, 2) { }

        public override void Assemble()
        {
            base.Assemble();
            Console.WriteLine("Adding turbo boost");
        }

        public override void ListFeatures()
        {
            base.ListFeatures();
            Console.WriteLine($"{Indent}Turbo");
        }
    }
}