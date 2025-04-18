
using Microsoft.AspNetCore.Authentication.Cookies;
using Project.Infrastructure.Interfaces;
using Project.Application.Services;
using Project.Repositories;
using Project.Domain;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
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

var mongoDbConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
if (string.IsNullOrEmpty(mongoDbConnectionString))
{
    throw new Exception("A variável de ambiente 'MONGODB_CONNECTION_STRING' não está definida.");
}

// Configurar o objeto ConfigMongoDb com as demais infos do appsettings
builder.Services.Configure<ConfigMongoDb>(options =>
{
    options.ConnectionString = mongoDbConnectionString;
    options.DatabaseName = builder.Configuration["ConfigMongoDb:DatabaseName"] ?? throw new Exception("DatabaseName is not configured in ConfigMongoDb.");
    options.UsuarioCollectionName = builder.Configuration["ConfigMongoDb:UsuarioCollectionName"] ?? throw new Exception("UsuarioCollectionName is not configured in ConfigMongoDb.");
    options.LoginCollectionName = builder.Configuration["ConfigMongoDb:LoginCollectionName"] ?? throw new Exception("LoginCollectionName is not configured in ConfigMongoDb.");
    options.EnderecoCollectionName = builder.Configuration["ConfigMongoDb:EnderecoCollectionName"] ?? throw new Exception("EnderecoCollectionName is not configured in ConfigMongoDb.");
    options.DiasPreferenciaCollectionName = builder.Configuration["ConfigMongoDb:DiasPreferenciaCollectionName"] ?? throw new Exception("DiasPreferenciaCollectionName is not configured in ConfigMongoDb.");
    options.TurnoCollectionName = builder.Configuration["ConfigMongoDb:TurnoCollectionName"] ?? throw new Exception("TurnoCollectionName is not configured in ConfigMongoDb.");
    options.HorariosCollectionName = builder.Configuration["ConfigMongoDb:HorariosCollectionName"] ?? throw new Exception("HorariosCollectionName is not configured in ConfigMongoDb.");
    options.ClinicaCollectionName = builder.Configuration["ConfigMongoDb:ClinicaCollectionName"] ?? throw new Exception("ClinicaCollectionName is not configured in ConfigMongoDb.");
    options.MedicoCollectionName = builder.Configuration["ConfigMongoDb:MedicoCollectionName"] ?? throw new Exception("MedicoCollectionName is not configured in ConfigMongoDb.");
    options.SugestaoConsultaClinicaCollectionName = builder.Configuration["ConfigMongoDb:SugestaoConsultaClinicaCollectionName"] ?? throw new Exception("SugestaoConsultaClinicaCollectionName is not configured in ConfigMongoDb.");
    options.SugestaoConsultaClienteCollectionName = builder.Configuration["ConfigMongoDb:SugestaoConsultaClienteCollectionName"] ?? throw new Exception("SugestaoConsultaClienteCollectionName is not configured in ConfigMongoDb.");
    options.MotivoRecusaCollectionName = builder.Configuration["ConfigMongoDb:MotivoRecusaCollectionName"] ?? throw new Exception("MotivoRecusaCollectionName is not configured in ConfigMongoDb.");
    options.ServicosAgendadosCollectionName = builder.Configuration["ConfigMongoDb:ServicosAgendadosCollectionName"] ?? throw new Exception("ServicosAgendadosCollectionName is not configured in ConfigMongoDb.");
    options.ConsultaCollectionName = builder.Configuration["ConfigMongoDb:ConsultaCollectionName"] ?? throw new Exception("ConsultaCollectionName is not configured in ConfigMongoDb.");
    options.FeedbackCollectionName = builder.Configuration["ConfigMongoDb:FeedbackCollectionName"] ?? throw new Exception("FeedbackCollectionName is not configured in ConfigMongoDb.");
    options.CampanhaCollectionName = builder.Configuration["ConfigMongoDb:CampanhaCollectionName"] ?? throw new Exception("CampanhaCollectionName is not configured in ConfigMongoDb.");
    options.ChatCollectionName = builder.Configuration["ConfigMongoDb:ChatCollectionName"] ?? throw new Exception("ChatCollectionName is not configured in ConfigMongoDb.");
});




builder.Services.AddTransient<IMongoClient>(_ =>
{
    return new MongoClient(mongoDbConnectionString);
});



// // Configuração do MongoDB - Se precisar, apaga o de cima e usa este
// builder.Services.Configure<ConfigMongoDb>(builder.Configuration.GetSection("ConfigMongoDb"));

// // Registra o cliente MongoDB como Transient, não Signton como o professor ensinou.
// builder.Services.AddTransient<IMongoClient>(sp =>   
// {
//     var settings = sp.GetRequiredService<IOptions<ConfigMongoDb>>().Value;
//     return new MongoClient(settings.ConnectionString);
// });


// Registrar os serviços necessários

//Usuario -- Cadastro
builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

// Login
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<ILoginRepository, LoginRepository>();

// Endereco
builder.Services.AddTransient<IEnderecoService, EnderecoService>();
builder.Services.AddTransient<IEnderecoRepository, EnderecoRepository>();

// Dias Preferencia
builder.Services.AddTransient<IDiasPreferenciaService, DiasPreferenciaService>();
builder.Services.AddTransient<IDiasPreferenciaRepository, DiasPreferenciaRepository>();

// Turno de preferencia
builder.Services.AddTransient<ITurnoService, TurnoService>();
builder.Services.AddTransient<ITurnoRepository, TurnoRepository>();

// Horários de preferencia
builder.Services.AddTransient<IHorariosService, HorariosService>();
builder.Services.AddTransient<IHorariosRepository, HorariosRepository>();

// Clinicas
builder.Services.AddTransient<IClinicaService, ClinicaService>();
builder.Services.AddTransient<IClinicaRepository, ClinicaRepository>();

// Médicos
builder.Services.AddTransient<IMedicoService, MedicoService>();
builder.Services.AddTransient<IMedicoRepository, MedicoRepository>();

// Sugestão de consultas para clínicas
builder.Services.AddTransient<ISugestaoConsultaClinicaService, SugestaoConsultaClinicaService>();
builder.Services.AddTransient<ISugestaoConsultaClinicaRepository, SugestaoConsultaClinicaRepository>();

// Sugestão de consultas para clientes
builder.Services.AddTransient<ISugestaoConsultaClienteService, SugestaoConsultaClienteService>();
builder.Services.AddTransient<ISugestaoConsultaClienteRepository, SugestaoConsultaClienteRepository>();

// Motivos de recusas
builder.Services.AddTransient<IMotivoRecusaService, MotivoRecusaService>();
builder.Services.AddTransient<IMotivoRecusaRepository, MotivoRecusaRepository>();

// Consultas que foram realizadas
builder.Services.AddTransient<IConsultaService, ConsultaService>();
builder.Services.AddTransient<IConsultaRepository, ConsultaRepository>();

// Feedbacks
builder.Services.AddTransient<IFeedbackService, FeedbackService>();
builder.Services.AddTransient<IFeedbackRepository, FeedbackRepository>();

// Atividades da campanha de incentivo
builder.Services.AddTransient<ICampanhaService, CampanhaService>();
builder.Services.AddTransient<ICampanhaRepository, CampanhaRepository>();

// Serviços agendados que ainda não ocorreram
builder.Services.AddTransient<IServicosAgendadosService, ServicosAgendadosService>();
builder.Services.AddTransient<IServicosAgendadosRepository, ServicosAgendadosRepository>();

// Chat
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();

// Dados cadastrais consolidado
builder.Services.AddTransient<IDadosCadastraisService, DadosCadastraisService>();
builder.Services.AddTransient<IDadosCadastraisRepository, DadosCadastraisRepository>();


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
        Title = "Delfos Machine", 
        Version = "v3",
        Description = @"Esta API foi desenvolvida para o projeto *Challenge OdontoPrev*.  

    Para mais detalhes, assista ao vídeo da aplicação (https://link-do-video.com).  

    Sobre o Projeto
    O sistema sugere consultas para os clientes com base em três pilares principais:  
    1️⃣ Local, Data e Turno de preferência do cliente 📍  
    2️⃣ Qualidade do atendimento (avaliada por pesquisas de satisfação) ⭐  
    3️⃣ Baixo custo 💰  
    4️⃣ Com ciclos de renovações que deixam a saúde do cliente em ciclos preventivos e sem perda de tempo e gastos desnecessários
    5️⃣ Não é necessário acessar as agendas dos médicos. Antes de sugerir uma consulta ao cliente, o modelo de IA analisa as clínicas que atendem às preferências do paciente. Em seguida, a clínica recebe a oportunidade de aceitar ou recusar a solicitação, com base na disponibilidade de horários dentro de sua rotina de atendimentos. 

    Essa abordagem visa proporcionar a melhor experiência ao usuário, garantindo conveniência, qualidade e economia. Diferente dos modelos tradicionais disponíveis no mercado, onde o próprio cliente precisa buscar clínicas e especialidades — muitas vezes em situações de urgência —, nosso sistema automatiza esse processo. A inteligência artificial identifica as opções mais adequadas com base nas preferências do cliente, otimizando o agendamento e reduzindo o tempo de espera. Isso não apenas melhora a experiência do paciente, mas também contribui para uma gestão mais eficiente das clínicas, permitindo um melhor aproveitamento da agenda e aumentando a taxa de ocupação dos profissionais de saúde.",
        Contact = new OpenApiContact
        {
            Name = "Claudio Silva Bispo e Patricia Naomi",
            Email = "rm553472@fiap.com.br"
        },
        License = new OpenApiLicense
        {
            Name = "Delfos Machine Group - Segundo semestre",
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
    options.ListenLocalhost(3001);

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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delfos Machine API v3");
    c.RoutePrefix = "swagger"; 
});

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Inicio}/{id?}");

app.Run();

