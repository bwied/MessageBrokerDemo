using System;
using System.Text;

namespace DecoratorPatternApp
{
    public class BasicCar : ICar
    {
        private readonly string[] _features = { "Engine", "Wheels", "Headlights" };
        private readonly int _decoratorLevel;
        protected string Indent { get; private set; }

        public BasicCar(int decoratorLevel)
        {
            _decoratorLevel = decoratorLevel;
            _decoratorLevel = 2;
            BuildIndent();
        }

        public virtual void Assemble()
        {
            Console.WriteLine("Assembling base vehicle");
        }

        public virtual void ListFeatures()
        {
            Console.WriteLine("Feature List:");

            foreach (var feature in _features)
            {
                Console.WriteLine($"{Indent}{feature}");
            }
        }

        protected void BuildIndent()
        {
            var strBldr = new StringBuilder();

            for (int i = 1; i < _decoratorLevel; i++)
            {
                strBldr.Append("    ");
            }

            Indent = strBldr.ToString();
        }
    }
}