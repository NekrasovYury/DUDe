using DUDe.Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DUDe.Services
{
    public class MqttMessageReceivedHandler : IMqttApplicationMessageReceivedHandler
    {
        DeviceContext db = new DeviceContext();

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {

            var mqttClient = new MqttFactory().CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                   .WithClientId("arduinoClient2")
                   .WithTcpServer("localhost", 1884)
                   .WithKeepAlivePeriod(new TimeSpan(1, 0, 0))
                   .Build();

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("device/FH677-L8EHP-86ZAB-CWV8D-2RGXC/temperature").Build());
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                string topic = e.ApplicationMessage.Topic;
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                db.devices.Add(new Device { Id = 1, Temperature= payload }) ;
            }
           );
        }
    }
 }
