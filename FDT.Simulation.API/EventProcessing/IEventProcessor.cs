using FDT.Simulation.API.DTOs;
using Newtonsoft.Json;

namespace FDT.Simulation.API.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }

    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.DataReceived:

                    break;
                case EventType.Undetermined:
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var dataReceivedDto = JsonConvert.DeserializeObject<DataReceivedDto>(notificationMessage);

            var eventType = dataReceivedDto.Event;

            switch (eventType)
            {
                case "DataReceived":
                    Console.WriteLine("Data Detected");
                    return EventType.DataReceived;
                default:
                    Console.WriteLine("Could not determine event type");
                    return EventType.Undetermined;
            }
        }
    }

    enum EventType
    {
        DataReceived,
        Undetermined
    }
}
