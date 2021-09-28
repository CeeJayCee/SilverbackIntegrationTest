using Silverback.Messaging.Messages;

namespace SilverbackIntegrationTest {
    public class MyMessageType : IIntegrationEvent {
        public int LastMessage { get; set; }
    }
}
