namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services to the container.

            var app = builder.Build();

            // Configure Http request Pipeline

            app.Run();
        }
    }
}
