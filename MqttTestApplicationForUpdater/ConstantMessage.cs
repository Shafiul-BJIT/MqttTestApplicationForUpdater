using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttTestApplicationForUpdater
{
    public class ConstantMessage
    {
        public static bool MqttSubscription = false;

        // MQTT Default Constants
        public const string DefaultBrokerHost = "localhost";
        public const int DefaultBrokerPort = 8883;
        public const string DefaultUsername = "sdkmeldcx";
        public const string DefaultPassword = "SDKmeldCX";
    }
}
