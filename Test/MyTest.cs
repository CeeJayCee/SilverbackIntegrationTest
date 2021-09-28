using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Silverback.Messaging.Broker;
using Silverback.Messaging.Configuration.Kafka;
using Silverback.Testing;
using SilverbackIntegrationTest;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Test {

    public static class BrokerExtensions {
        public static IProducer GetTestProducer(this IBroker broker, string topicName) {
            var builder = new KafkaProducerEndpointBuilder();
            builder
                .ProduceTo(topicName)
                .Configure(config => config.BootstrapServers = "PLAINTEXT://localhost:9093");
            var endpoint = builder.Build();

            return broker.GetProducer(endpoint);
        }
    }
    public class MyTest {
        [Test]
        public async Task Test1() {
            using WebApplicationFactory<Startup> factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(
                builder => {
                    builder.ConfigureTestServices(
                        services => {
                            services.ConfigureSilverback()
                                .UseMockedKafka()
                                .AddIntegrationSpy();
                        }
                    );
                }
            );

            HttpClient client = factory.CreateClient();

            HttpResponseMessage response_1 = await client.GetAsync("/");
            string response_message_1 = await response_1.Content.ReadAsStringAsync();

            IKafkaTestingHelper helper = factory.Services.GetRequiredService<IKafkaTestingHelper>();

            await helper.WaitUntilConnectedAsync();

            var producer = helper.Broker.GetTestProducer("my-topic");
            await producer.ProduceAsync(new MyMessageType() { LastMessage = 6 });

            await helper.WaitUntilAllMessagesAreConsumedAsync();

            HttpResponseMessage response_2 = await client.GetAsync("/");
            string response_message_2 = await response_2.Content.ReadAsStringAsync();

            Assert.AreNotEqual(response_message_1, response_message_2);
        }
    }
}
