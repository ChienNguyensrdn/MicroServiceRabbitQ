using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostService.Data;
using PostService.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PostService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            ListenForIntegrationEvents();
            host.Run();
        }

        private static void ListenForIntegrationEvents()
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var contextOptions = new DbContextOptionsBuilder<PostServiceContext>()
                    .UseSqlServer (@"Data Source=DESKTOP-42MI8U9\SQLEXPRESS;Initial Catalog=PostService;Integrated Security=True")
                    .Options;
                var dbContext = new PostServiceContext(contextOptions);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                var data = JObject.Parse(message);
                var type = ea.RoutingKey;
                if (type == "user.add")
                {
                    dbContext.User.Add(new User()
                    {
                       //ID = data["id"].Value<int>(),
                        Name = data["name"].Value<string>()
                    });
                    dbContext.SaveChanges();
                }
                else if (type == "user.update")
                {
                    var user = dbContext.User.First(a => a.ID == data["id"].Value<int>());
                    user.Name = data["newname"].Value<string>();
                    dbContext.SaveChanges();
                }
            };
            channel.BasicConsume(queue: "user.postservice",
                                     autoAck: true,
                                     consumer: consumer);
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<PostService.Data.PostServiceContext>();
                    context.Database.EnsureCreated();
                    // DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
