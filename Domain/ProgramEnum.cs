using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public enum DemoAppLauncherSelection
    {
        TutorialHelloWorld = 1,
        TutorialWorkQueue = 2,
        TutorialPubSub = 3,
        TutorialRouting = 4,
        TutorialTopics = 5,
        TutorialRpc = 6,
        FanoutExchange = 7,
        DirectExchange = 8,
        RoutingKeyFanoutExchange = 9,
        Exit = 0
    }

    public enum BrokerAppLauncherSelection
    {
        Publisher = 1,
        Consumer = 2,
        All = 3,
        Exit = 0
    }

    public enum BrokerAppType
    {
        Publisher = 1,
        Consumer = 2
    }
}
