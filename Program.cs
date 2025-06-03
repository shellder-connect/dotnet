
using Microsoft.AspNetCore.Authentication.Cookies;
using Project.Infrastructure.Interfaces;
using Project.Application.Services;
using Project.Repositories;
using Project.Domain;
using MongoDB.Driver;
using Project.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Varivavel de ambiente para o MongoDB - Configuração do MongoDB

/*
    Instalar dotnet add package DotNetEnv
*/

// Carregar variáveis de ambiente do arquivo .env.local
DotNetEnv.Env.Load(".env.local");

// Registrar as configurações no DI
builder.Configuration.AddEnvironmentVariables();

var mongoDbConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ??
    throw new Exception("A variável de ambiente 'MONGODB_CONNECTION_STRING' não está definida.");


// Configurar o objeto ConfigMongoDb com as demais infos do appsettings
builder.Services.Configure<ConfigMongoDb>(options =>
{
    options.ConnectionString = mongoDbConnectionString;
    options.DatabaseName = builder.Configuration["ConfigMongoDb:DatabaseName"] ?? throw new Exception("DatabaseName is not configured in ConfigMongoDb.");
    options.DatabaseTestsName = builder.Configuration["ConfigMongoDb:DatabaseTestsName"] ?? throw new Exception("DatabaseTestsName is not configured in ConfigMongoDb.");
    options.UsuarioCollectionName = builder.Configuration["ConfigMongoDb:UsuarioCollectionName"] ?? throw new Exception("UsuarioCollectionName is not configured in ConfigMongoDb.");
    options.LoginCollectionName = builder.Configuration["ConfigMongoDb:LoginCollectionName"] ?? throw new Exception("LoginCollectionName is not configured in ConfigMongoDb.");
    options.EnderecoCollectionName = builder.Configuration["ConfigMongoDb:EnderecoCollectionName"] ?? throw new Exception("EnderecoCollectionName is not configured in ConfigMongoDb.");
    options.FeedbackCollectionName = builder.Configuration["ConfigMongoDb:FeedbackCollectionName"] ?? throw new Exception("FeedbackCollectionName is not configured in ConfigMongoDb.");
    options.TipoUsuarioCollectionName = builder.Configuration["ConfigMongoDb:TipoUsuarioCollectionName"] ?? throw new Exception("TipoUsuarioCollectionName is not configured in ConfigMongoDb.");
    options.CategoriaCollectionName = builder.Configuration["ConfigMongoDb:CategoriaCollectionName"] ?? throw new Exception("CategoriaCollectionName is not configured in ConfigMongoDb.");
    options.AbrigoCollectionName = builder.Configuration["ConfigMongoDb:AbrigoCollectionName"] ?? throw new Exception("AbrigoCollectionName is not configured in ConfigMongoDb.");
    options.DoacaoCollectionName = builder.Configuration["ConfigMongoDb:DoacaoCollectionName"] ?? throw new Exception("DoacaoCollectionName is not configured in ConfigMongoDb.");
    options.DistribuicaoCollectionName = builder.Configuration["ConfigMongoDb:DistribuicaoCollectionName"] ?? throw new Exception("DistribuicaoCollectionName is not configured in ConfigMongoDb.");
    options.RegistroEventoCollectionName = builder.Configuration["ConfigMongoDb:RegistroEventoCollectionName"] ?? throw new Exception("RegistroEventoCollectionName is not configured in ConfigMongoDb.");
    options.MuralCollectionName = builder.Configuration["ConfigMongoDb:MuralCollectionName"] ?? throw new Exception("MuralCollectionName is not configured in ConfigMongoDb.");
    options.EnderecoAbrigoCollectionName = builder.Configuration["ConfigMongoDb:EnderecoAbrigoCollectionName"] ?? throw new Exception("EnderecoAbrigoCollectionName is not configured in ConfigMongoDb.");
    
    
});

builder.Services.AddTransient<IMongoClient>(_ =>
{
    return new MongoClient(mongoDbConnectionString);
});

// Registrar os serviços necessários

//Usuario -- Cadastro
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

//Tipo de Usuario
builder.Services.AddTransient<ITipoUsuarioService, TipoUsuarioService>();
builder.Services.AddTransient<ITipoUsuarioRepository, TipoUsuarioRepository>();

// Login
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<ILoginRepository, LoginRepository>();

// Consultar o CEP
builder.Services.AddTransient<ICepService, CepService>();

// Endereco
builder.Services.AddTransient<IEnderecoService, EnderecoService>();
builder.Services.AddTransient<IEnderecoRepository, EnderecoRepository>();

// Feedbacks
builder.Services.AddTransient<IFeedbackService, FeedbackService>();
builder.Services.AddTransient<IFeedbackRepository, FeedbackRepository>();

// Dados cadastrais consolidado
builder.Services.AddTransient<IDadosCadastraisService, DadosCadastraisService>();
builder.Services.AddTransient<IDadosCadastraisRepository, DadosCadastraisRepository>();

// Categoria
builder.Services.AddTransient<ICategoriaService, CategoriaService>();
builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();

// Abrigo
builder.Services.AddTransient<IAbrigoService, AbrigoService>();
builder.Services.AddTransient<IAbrigoRepository, AbrigoRepository>();

// Doação
builder.Services.AddTransient<IDoacaoService, DoacaoService>();
builder.Services.AddTransient<IDoacaoRepository, DoacaoRepository>();

// Distribuição
builder.Services.AddTransient<IDistribuicaoService, DistribuicaoService>();
builder.Services.AddTransient<IDistribuicaoRepository, DistribuicaoRepository>();

// Registro de Evento
builder.Services.AddTransient<IRegistroEventoService, RegistroEventoService>();
builder.Services.AddTransient<IRegistroEventoRepository, RegistroEventoRepository>();

// Análise de sentimento
builder.Services.AddTransient<ISentimentAnalysisService, SentimentAnalysisService>();

// Para materia Database
builder.Services.AddHttpClient<IDatabaseService, DatabaseService>();
builder.Services.AddTransient<IDatabaseService, DatabaseService>();

// Materio de IOT
builder.Services.AddTransient<IMLRepository, MLRepository>();
builder.Services.AddTransient<IMLService, MLService>();

// Mural
builder.Services.AddTransient<IMuralRepository, MuralRepository>();
builder.Services.AddTransient<IMuralService, MuralService>();

// Endereço Abrigo
builder.Services.AddTransient<IEnderecoAbrigoRepository, EnderecoAbrigoRepository>();
builder.Services.AddTransient<IEnderecoAbrigoService, EnderecoAbrigoService>();

// Configurar autenticação com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Logar";
    });

// Adicionando suporte a sessões
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});

builder.Services.AddHttpClient();

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Shellder Connection", 
        Version = "v3",
        Description = @"Esta API foi desenvolvida para o projeto *Global Solution*.  

    Para mais detalhes, assista ao vídeo da aplicação (https://link-do-video.com).  

    Essa abordagem visa proporcionar a melhor experiência ao usuário, garantindo conveniência, qualidade e suporte.",
        Contact = new OpenApiContact
        {
            Name = "Claudio Silva Bispo e Patricia Naomi",
            Email = "rm553472@fiap.com.br"
        },
        License = new OpenApiLicense
        {
            Name = "Shellder Connection Group - Global Solution",
            Url = new Uri("https://github.com/patinaomi/delfos-machine-2-sem.git")
        }
        
    });
    var xmlFile = $"{AppDomain.CurrentDomain.FriendlyName}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.WebHost.ConfigureKestrel(options =>
{
    //options.ListenAnyIP(3001);
    //options.ListenLocalhost(3001);
    options.ListenAnyIP(3001);

});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilita o Swagger
app.UseSwagger();

// Habilita o Swagger UI
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shellder Connection API");
    c.RoutePrefix = "swagger"; 
});

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Inicio}/{id?}");

app.Run();

