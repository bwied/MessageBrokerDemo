using System;

namespace DecoratorPatternApp
{
    public class SoundSystemBasic : CarDecorator
    {
        public SoundSystemBasic(ICar car) : base(car, 2) { }

        public override void Assemble()
        {
            base.Assemble();
            Console.WriteLine("Adding sound system");
        }

        public override void ListFeatures()
        {
            base.ListFeatures();
            Console.WriteLine($"{Indent}Basic Sound System");
        }
    }
}