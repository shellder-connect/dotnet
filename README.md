# final-project-advanced-dotnet-api

## 📝 Resumo do Projeto

**Este projeto representa a entrega final da disciplina Advanced Business Development with .NET. Trata-se de uma API RESTful desenvolvida em .NET, com foco em boas práticas de arquitetura, organização de camadas, e implementação de testes automatizados (unitários e/ou de integração).**

A solução contempla os seguintes pontos principais:

📦 Camadas bem definidas (Controllers, Services, Repositories, DTOs e Models)

🔐 Padrões modernos de autenticação/autorização

🧪 Testes automatizados com xUnit

📊 Banco de dados relacional com Entity Framework Core

🛠️ Boas práticas de Clean Code e organização do código

🧩 Padrões de projeto aplicados (como Repository, Dependency Injection, etc.)

🔄 Endpoints RESTful completos para operações de CRUD

O projeto simula um cenário real de negócios, com ênfase na criação de soluções escaláveis, testáveis e alinhadas às exigências do mercado atual de desenvolvimento corporativo com .NET.


# Projeto Backend Challenge Odontoprev

## Dados dos alunos

1. Claudio Silva Bispo
```bash
    RM 553472
```

2. Patricia Naomi Yamagishi
```bash
    RM 552981
```

## Visão Geral

Este projeto é uma API para gerenciamento de  clientes, clínicas, consultas, feedback e outros recursos relacionados a um sistema odontológico. A API é construída usando ASP.NET Core e MongoDB para armazenamento de dados. Na segunda fase, iremos usar Oracle, pois nosso acesso está bloqueado.

## Problema que o projeto pretende resolver:

1. Ineficiência no gerenciamento de consultas e tratamentos preventivos: muitas clínicas têm dificuldades em organizar e automatizar os agendamentos. Além de perder a oportunidade de atender 100% da carteira de clientes da seguradora. Quando os clientes entram em contato, já é para utilizar o seguro/convênio em momentos de emergências, gerando alto custo/gasto. 

2. Falta de centralização dos dados do paciente: o projeto centraliza todas as informações relevantes sobre a saúde bucal do paciente, incluindo histórico familiar, condições físicas, custo das consultas, notas atribuidas as clinicas, especilistas e clientes. 

3. Dificuldade na comunicação entre a clínica, paciente e seguradora: a plataforma permite notificações automatizadas e mantém um fluxo de comunicação eficiente entre todas as partes envolvidas. 

## Nosso objetivo

Desenvolver uma aplicação móvel, gerenciada em Java, e uma aplicação web, gerenciada em ASP.NET / C#, com o objetivo de sugerir consultas para novos e antigos clientes utilizando inteligência artificial (IA). As sugestões de consultas serão baseadas na localidade preferida do cliente,no dia e turno de disponibilidade que ele cadastrar, nas avaliações de feedback das clínicas/especialistas e nos custos mais baixos. Com essa combinação, os clientes poderão realizar suas consultas de rotina de forma contínua, promovendo um ciclo de alta qualidade. Ao mesmo tempo, as clínicas e especialistas manterão um fluxo constante de clientes em suas carteiras. Para que possamos atender um dos pilares que é ter a informação (feedback e informações complementares dos clientes) vamos criar um programa de relacionamento, que visa engajar os clientes e especialistas a criarem conteúdos e informações para que possamos treinar o modelo, entregando valor e ao mesmo tempo, bonificando eles.

## Arquitetura Monolítica

Toda nossa aplicação será construída como um único sistema unificado. Isso significa que todas as funcionalidades da API (como autenticação, cadastro de usuários, login, cadastro das preferências, e busca das clínicas) estão dentro de um único código-base e banco de dados.

## ✅ Vantagens do Monolito:

***Simplicidade***
Desenvolvimento, manutenção e deploy são mais fáceis, pois tudo está em um só lugar.

***Menos complexidade***
Não há necessidade de gerenciar múltiplos serviços e comunicação entre eles.

***Performance*** 
Como tudo está no mesmo sistema, há menos sobrecarga de comunicação entre serviços.

***Facilidade no banco de dados***
Apenas uma base de dados central que será o MongoDb, evitando sincronização entre múltiplas instâncias.

## ❌ Desvantagens do Monolito

***Dificuldade de escalabilidade***
Se o sistema crescer muito, fica mais difícil escalar apenas uma parte do código sem afetar o todo.

***Manutenção complexa a longo prazo***
Com o tempo, o código pode se tornar um "grande emaranhado" difícil de modificar sem causar erros.

***Menos flexibilidade***
Se precisar mudar uma tecnologia específica, será necessário refatorar toda a aplicação.

# Link com vídeo do prótotipo da nossa aplicação

```bash
    https://youtu.be/4rk6KTjp8mM?si=o-7w2aOF_NlqJ5b-
``` 

1. Será interessante para você entender melhor nossa aplicação, iniciando pelo Mobile.
2. Logo iremos criar o prótotipo da nossa aplicação na web. Será enviada na segunda sprint.

## Documentação e rotas da API

👉 Acesse a rota abaixo e veja a documentação com as rotas e explicações necesárias para testar e entender nosso modelo e dados salvos no banco.
```bash
    http://localhost:3001/index.html
```

## Tecnologias Utilizadas
- ASP.NET Core
- MongoDB, não vamos usar o Oracle ainda, pois não é necessário.
- C#
- React para aplicação front-end que será onde os dados serão inseridos pelo cliente. Esta funcionalidade estará pronta na segunda sprint.

## Estrutura do Diretório

Nosso projeto será gerenciada com base na Clean Architecture, contendo interfaces dos repósitorios, mantendo a regra do clean code.

src/
├── Domain/                     -> Lógica de negócio e entidades
│   └── Entities/               -> Classes de domínio (models atuais)
│   └── Repositories/           -> Interfaces de repositórios
├── Application/                -> Casos de uso e lógica de aplicação
│   └── Services/               -> Casos de uso (services atuais)
│   └── DTOs/                   -> Objetos de transferência de dados
├── Infrastructure/             -> Implementação de repositórios 
├── Web/                        -> API e interface de usuário(Logo teremos o front)
│   └── Controllers/            -> Controladores da API (controllers atuais)
└── Tests/                      -> Testes unitários e de integração. Está etapa será inserida na segunda sprint.

## Configuração e Execução

### Pré-requisitos

- .NET SDK
- MongoDB: link de acesso ao banco: 

```bash
    mongodb+srv://csspclaudio:clnzEcsY8xmMVXMr@cluster0.kfgkjua.mongodb.net/
``` 

Demais configurações se for necessária:

## Tabelas que serão criadas no banco

```bash
    "ConfiguraracaoMongoDb": {
        "ConnectionString": "mongodb+srv://csspclaudio:clnzEcsY8xmMVXMr@cluster0.kfgkjua.mongodb.net/",
        "DatabaseName": "ProjetoChallenge",
        "CadastroCollectionName": "Cadastros",
        "ClienteCollectionName": "Clientes",
        "AgendaCollectionName": "Agendas",
        "ClinicaCollectionName": "Clinicas",
        "ConsultaCollectionName": "Consultas",
        "DentistaCollectionName": "Dentistas",
        "EstadoCivilCollectionName": "EstadosCivis",
        "FeedbackCollectionName": "Feedbacks",
        "FormularioDetalhadoCollectionName": "FormulariosDetalhados",
        "NotificacoesCollectionName": "Notificacoes",
        "TipoNotificacaoCollectionName": "TiposNotificacao",
        "SinistroCollectionName": "Sinistros"
    },
``` 

### Configuração

1. Clone o repositório:
```bash
    https://github.com/patinaomi/delfos-machine.git
``` 
   
## Execução

1. Restaure as dependências:
```bash
    dotnet restore
```
2. Compile e execute a aplicação:
```bash
    dotnet run
```
3. Acesse o Swagger UI para testar a API:
```bash
http://localhost:3001/swagger
```

## Escopo

O projeto inclui o desenvolvimento de uma plataforma com as seguintes funcionalidades principais:

**Cadastro e gerenciamento de pacientes:** com histórico odontológico, informações pessoais, e detalhes de saúde bucal.

**Agendamento de consultas:** com integração de agenda, preferências de horário e clínicas.

**Notificações automatizadas:** para lembretes de consulta, sinistros, e interações com a clínica.

**Gerenciamento de dentistas e clínicas:** permite o cadastro de dentistas e avaliação das clínicas com base em preço, avaliação e localização.

**Gestão de sinistros:** integração com seguradoras para acompanhamento de sinistros odontológicos, avaliação de processos, e análise de cobertura.

**Autenticação e segurança:** com funcionalidades de login seguro e armazenamento de logs.

## Requisitos Funcionais e Não Funcionais

### Requisitos Funcionais:

**Cadastro de Pacientes:** A aplicação deve permitir o cadastro de clientes com dados detalhados (condição física, histórico familiar, saúde bucal).

**Agendamento de Consultas:** O sistema deve sugerir horários de consultas com base na disponibilidade e preferência do cliente.
Notificações: Envio de notificações automáticas sobre consultas, sinistros e status de tratamento.

**Gerenciamento de Clínicas e Dentistas:** A aplicação deve armazenar e permitir a avaliação de clínicas e dentistas.

**Processamento de Sinistros:** A funcionalidade de sinistros deve permitir o envio e acompanhamento do processo por parte do paciente e seguradora.

**Autenticação de Usuários:** O sistema deve garantir segurança através de login e autenticação de usuários.

### Requisitos Não Funcionais:

**Desempenho:** O sistema deve ser capaz de gerenciar múltiplos agendamentos e dados de pacientes simultaneamente, sem perder performance.
Segurança: Criptografia de dados sensíveis, como informações de login e histórico médico.

**Escalabilidade:** A arquitetura deve permitir a adição de novas funcionalidades, como integração com mais APIs ou novos tipos de notificações.

**Disponibilidade:** O sistema deve estar disponível 99% do tempo para garantir que consultas e notificações sejam acessadas sem interrupções.

**Manutenibilidade:** O uso de uma arquitetura desacoplada deve facilitar a manutenção e atualização do código.


## Fluxo do Assistente Inteligente – OdontoPrev - Uso da IA no projeto

1️⃣ O assistente inicia automaticamente o chat quando identifica que há formulários pendentes, notificando o usuário sobre a necessidade de preenchimento.

Notificações automatizadas via chat ou e-mail lembram o usuário de concluir os formulários inacabados.
2️⃣ O assistente possui IA especializada exclusivamente em Odontologia e Seguro Odontológico, entendendo perguntas e fornecendo respostas relacionadas a:

Cobertura de planos odontológicos
Procedimentos cobertos
Agendamentos e reembolsos
Cuidados com a saúde bucal
(Nenhum outro assunto será permitido no chat)
3️⃣ O bot consulta a API para verificar os formulários disponíveis e os status de preenchimento.

4️⃣ O bot avalia os campos preenchidos e os que ainda precisam de resposta.

5️⃣ O assistente faz perguntas ao usuário para completar os campos pendentes e envia as respostas em tempo real para a API via INSERT/PATCH.

6️⃣ Quando o formulário estiver completo, o assistente confirma com o usuário e finaliza o processo.

7️⃣ Relatórios automáticos são gerados para acompanhar o status dos formulários preenchidos e pendentes, ajudando na gestão do atendimento odontológico.

8️⃣ Todas as interações no chat serão registradas no banco de dados, armazenando perguntas e respostas dos usuários.

Esses dados serão utilizados para treinar o modelo de IA, tornando o assistente mais preciso e eficiente nas respostas futuras.

9️⃣ Aprimoramento do NLP (Processamento de Linguagem Natural)

Melhorias contínuas no treinamento do assistente com base nas interações registradas.
Implementação de um sistema de feedback para o usuário avaliar se a resposta foi útil.

🔟 Automação do Agendamento Odontológico (Se aplicável à OdontoPrev)

O assistente pode oferecer sugestões de horários disponíveis e permitir que o usuário agende consultas diretamente pelo chat.

1️⃣1️⃣ Reconhecimento de Voz (Opcional, mas inovador)

Integração com reconhecimento de voz para permitir que usuários interajam falando em vez de digitando.

## 📌 O fluxo básico seria:

1️⃣ Após o login, o chat aparece no lado direito da tela.

2️⃣ O usuário pode enviar e receber mensagens.

3️⃣ O chat se conecta à API do assistente para obter os formulários pendentes.

4️⃣ O assistente faz perguntas, e o usuário responde.

5️⃣ As interações são armazenadas no banco de dados.

## Testes Unittário com xUnit

**Instalação**
```bash
    dotnet add package xunit
```

```bash
    dotnet add package xunit.runner.visualstudio
```

**SDK de testes da Microsoft**
```bash
    dotnet add package Microsoft.NET.Test.Sdk
```

**Componentes que dependem do ASP.NET Core (como controllers)**
```bash
    dotnet add package Microsoft.AspNetCore.Mvc.Testing
```

**Instalar Mocks**
```bash
    dotnet add package Moq
```

# Criar a pasta de teste na raiz do projeto

**Executar o comando dentro da pasta test para iniciar os testes**
```bash
    dotnet new xunit -n Project.Tests 
```

**Dentro da pasta teste/Project.Tests**
```bash
    dotnet add reference ../../Project.csproj
```

***O que isso faz?***
```bash
    Adiciona uma referência ao arquivo Project.csproj no .csproj do teste.

    Permite que você teste classes do projeto principal.
```

Nosso arquivo final do Project.Tests.csproj

```bash
    <Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>net9.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <IsPackable>false</IsPackable>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="coverlet.collector" Version="6.0.2" />
        <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
        <PackageReference Include="xunit" Version="2.9.2" />
        <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
    </ItemGroup>

    <ItemGroup>
        <Using Include="Xunit" />
    </ItemGroup>

    <ItemGroup>
        <ProjectReference Include="..\..\Project.csproj" />
    </ItemGroup>

    </Project>
```

## Os testes serão feitos nos Services, Repositories e por último nos Controllers

**1. Testando o UsuarioService (Camada de Aplicação)**
Estratégia: Vamos usar mocks para isolar o serviço das dependências (como IUsuarioRepository).

