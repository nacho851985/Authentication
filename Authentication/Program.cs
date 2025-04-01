using Authentication;

var builder = WebApplication.CreateBuilder(args);
// Adaptación para Lambda
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
AppBuilder.Configure(builder);

var app = builder.Build();
AppBuilder.ConfigureApp(app);

if (!app.Environment.IsEnvironment("Lambda"))
{
    app.Run();
}