namespace DecoratorPatternApp
{
    public class CarDecorator : BasicCar
    {
        private readonly ICar _car;

        protected CarDecorator(ICar car, int decoratorLevel) : base(decoratorLevel)
        {
            _car = car;
        }

        public override void Assemble()
        {
            _car.Assemble();
        }

        public override void ListFeatures()
        {
            _car.ListFeatures();
        }
    }
}