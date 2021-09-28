using System;

namespace SilverbackIntegrationTest {
    public class MyMessageHandler
    {

        private readonly MessageStore _store;
        public MyMessageHandler(MessageStore store) {
            _store = store;
        }

        public void OnMessageReceived(MyMessageType message) {
            Console.WriteLine($"Received {message.LastMessage}");
            _store.Store(message.LastMessage);
        }
    }
}
