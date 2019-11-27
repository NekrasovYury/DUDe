using DUDe.Models;
using DUDe.Services;
using MQTTnet;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DUDe.App_Start
{
    public class MqttServer
    {
     //static  DeviceContext db = new DeviceContext();
        public static void ConfigureAndStart()
        {
            DeviceContext db = new DeviceContext();

            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(1884);

            var mqttServer = new MqttFactory().CreateMqttServer();

           // mqttServer.UseApplicationMessageReceivedHandler(new MqttMessageReceivedHandler());
            mqttServer.UseApplicationMessageReceivedHandler(e =>
            {
            
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                //Console.WriteLine(payload);
             
                Device device = new Device {Temperature = payload };
                db.devices.Add(device);
                db.SaveChanges();
                //db.devices.Add(new Device { Id = 2, Temperature = payload });
                //db.devices.Add(new Device {Id =4, Temperature= 3.ToString() });
              
            });

            mqttServer.StartAsync(optionsBuilder.Build()).GetAwaiter().GetResult();
        }
         
    }
}