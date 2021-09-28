using Confluent.Kafka;
using Silverback.Messaging.Configuration;

namespace SilverbackIntegrationTest {


    public class EndpointsConfigurator : IEndpointsConfigurator {


        public void Configure(IEndpointsConfigurationBuilder builder) {
            builder.AddKafkaEndpoints(
                endpoints =>
                    endpoints.Configure(
                        config => {
                            config.BootstrapServers = "PLAINTEXT://localhost:9093";
                        }
                    ).AddInbound(endpoint =>
                        endpoint.ConsumeFrom("my-topic")
                            .Configure(
                                config => {
                                    config.GroupId = "silverback-test";
                                    config.AutoOffsetReset = AutoOffsetReset.Earliest;
                                })
                            .DeserializeJson(serializer => serializer.UseFixedType<MyMessageType>())
                    )
            );
        }

    }
}