using AutoMapper;
using GreenPipes;
using MassTransit.Poc.Application;
using MassTransit.Poc.Consumers;
using MassTransit.Poc.Domain.Application;
using MassTransit.Poc.Domain.Events;
using MassTransit.Poc.Domain.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MassTransit.Poc.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            services.AddSingleton(config.CreateMapper());
            #endregion

            #region Repositorios e Servicos
            services.AddScoped<IVehicleApplication, MessageApplication>();
            #endregion

            #region Swagger
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MassTransit.Poc.Api", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            #endregion

            #region MassTransit
            services.AddMassTransit(bus =>
            {
                bus.SetKebabCaseEndpointNameFormatter();

                bus.UsingRabbitMq((ctx, busConfigurator) =>
                {
                    busConfigurator.Host(Configuration.GetConnectionString("RabbitMq"));
                    busConfigurator.UseDelayedMessageScheduler();

                    busConfigurator.Publish<IOrchestratorDirectType>(a => a.ExchangeType = ExchangeType.Direct);
                    busConfigurator.Publish<IOrchestratorTopicType>(a => a.ExchangeType = ExchangeType.Topic);

                    busConfigurator.ReceiveEndpoint("message-simple", e =>
                    {
                        e.Consumer<MessageSimpleValidateConsumer>();
                    });
                    busConfigurator.ReceiveEndpoint("message-simple-validate", e =>
                    {
                        e.Consumer<MessageSimpleValidateConsumer>();
                    });
                    busConfigurator.ReceiveEndpoint("message-simple-erro", e =>
                    {
                        e.Consumer<MessageSimpleErrorConsumer>();
                    });
                    busConfigurator.ReceiveEndpoint("message-retry", e =>
                    {
                        e.Consumer(() => new MessageRetryConsumer(), c =>
                        {
                            c.UseMessageRetry(r => r.Immediate(3));
                        });
                    });
                    busConfigurator.ReceiveEndpoint("message-redelivery", e =>
                    {
                        e.Consumer(() => new MessageRedeliveryConsumer(), c =>
                        {
                            c.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15)));
                            c.UseMessageRetry(r => r.Immediate(1));
                            c.UseInMemoryOutbox();
                        });
                    });
                    busConfigurator.ReceiveEndpoint("message-error", e =>
                    {
                        e.Consumer<MessageErrorConsumer>();
                    });
                    busConfigurator.ReceiveEndpoint("log-fault-message-error", e =>
                    {
                        e.Consumer<LogFaultMessageErrorConsumer>();
                    });

                    busConfigurator.ReceiveEndpoint("message-direct-one-consumer", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Consumer<MessageDirectOneConsumer>();
                        e.Bind<IOrchestratorDirectType>(s =>
                        {
                            s.RoutingKey = "directone";
                            s.ExchangeType = ExchangeType.Direct;
                        });
                    });
                    busConfigurator.ReceiveEndpoint("message-direct-two-consumer", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Consumer<MessageDirectTwoConsumer>();
                        e.Bind<IOrchestratorDirectType>(s =>
                        {
                            s.RoutingKey = "directtwo";
                            s.ExchangeType = ExchangeType.Direct;
                        });
                    });
                    busConfigurator.ReceiveEndpoint("vehicle-economic-consumer", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Consumer<VehicleEconomicConsumer>();
                        e.Bind<IOrchestratorTopicType>(s =>
                        {
                            s.RoutingKey = "*.*.eco";
                            s.ExchangeType = ExchangeType.Topic;
                        });
                    });
                    busConfigurator.ReceiveEndpoint("vehicle-uno-consumer", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Consumer<VehicleUnoConsumer>();
                        e.Bind<IOrchestratorTopicType>(s =>
                        {
                            s.RoutingKey = "Uno.#";
                            s.ExchangeType = ExchangeType.Topic;
                        });
                    });
                });
            });
            services.AddMassTransitHostedService();
            #endregion
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MassTransit.Poc.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
