using GraphQL.MicrosoftDI;
using GraphQL;
using GraphQL.Types;
using Sana_Todo.Services;
using Sana_Todo.GraphQL;
using GraphQL.Server.Ui.GraphiQL;
using Microsoft.AspNetCore.Builder;
using Sana_Todo.GraphQL.Types;
using Sana_Todo.GraphQL.Mutations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<XmlTaskService>();
builder.Services.AddScoped<IStorageFactory, StorageFactory>();
builder.Services.AddGraphQL(b => b
    .AddAutoSchema<TaskQuery>()  
    .AddSystemTextJson());

// GraphQL
builder.Services.AddScoped<TaskQuery>();
builder.Services.AddScoped<TaskType>();
builder.Services.AddScoped<CategoryType>();
builder.Services.AddScoped<ISchema, TodoSchema>();
builder.Services.AddScoped<TaskMutation>();
builder.Services.AddScoped<TaskInputType>();
builder.Services.AddScoped<TaskInputUpdate>();
builder.Services.AddHttpContextAccessor();



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowReactApp");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseGraphQL("/graphql");
app.UseGraphQLGraphiQL("/ui/graphiql", new GraphQL.Server.Ui.GraphiQL.GraphiQLOptions
{
    GraphQLEndPoint = "/graphql"
});

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=Index}/{id?}");

app.Run();
