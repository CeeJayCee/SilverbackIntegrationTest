namespace SilverbackIntegrationTest {
    public class MessageStore {

        private int _last_message = 0;

        public void Store(int message) {
            _last_message = message;
        }

        public int Get() {
            return _last_message;
        }
    }
}
