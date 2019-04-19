namespace DecoratorPatternApp
{
    public static class CarFactory
    {
        public static ICar GetCar(CarType type)
        {
            switch (type)
            {
                case CarType.Sport:
                    return new SportBasicCar(new BasicCar(1));
                default:
                    return new BasicCar(1);
            }
        }

        public static ICar AddFeature(ICar car, Feature feature)
        {
            switch (feature)
            {
                case Feature.Turbo:
                    return new Turbo(car);
                case Feature.SoundSystemBasic:
                    return new SoundSystemBasic(car);
                default:
                    return car;
            }
        }

        public static void DemoCar(ICar car)
        {
            car.Assemble();
            car.ListFeatures();
        }

        public static void DemoCar(CarType type)
        {
            var car = GetCar(type);
            DemoCar(car);
        }
    }
}