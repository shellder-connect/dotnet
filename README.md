# 🆘 Shellder Connect

> *Conectando vidas, oferecendo esperança em momentos de crise*

Uma API robusta e compassiva desenvolvida para ser a ponte entre pessoas em situação de calamidade pública e os recursos de apoio disponíveis. O Socorro Solidário é mais que uma aplicação - é uma rede de solidariedade digital que salva vidas.

---

## 🌟 Sobre o Projeto

Em momentos de desastres naturais, emergências ou crises humanitárias, cada segundo conta. O **Socorro Solidário** foi criado para conectar rapidamente pessoas em necessidade com:

- 🏠 **Abrigos próximos** para proteção e acolhimento
- 🍲 **Doações de alimentos** para suprir necessidades básicas
- 👕 **Roupas e agasalhos** para proteção e dignidade
- 💊 **Medicamentos essenciais** para cuidados de saúde
- 🩺 **Profissionais de saúde** para atendimento pós-trauma
- 🧠 **Orientações psicológicas** para lidar com traumas e ansiedade

### 🎯 Missão

Democratizar o acesso à ajuda humanitária através da tecnologia, garantindo que ninguém enfrente uma crise sozinho.

---

## 🛠️ Tecnologias Utilizadas

### Core Framework
- **ASP.NET Core 8.0** - Framework principal para Web API
- **C# 12** - Linguagem de programação
- **Entity Framework Core** - ORM para persistência de dados

### Arquitetura e Design
- **Minimal APIs** - APIs leves e performáticas
- **RESTful Design** - Padrões REST para comunicação

### Banco de Dados
- **MongoDb** - Banco de dados principal

### 🤖 Machine Learning Features

- **ML.NET** - Machine Learning para:
    
    **Predição de Necessidades por Região**
    -  Tecnologia: Python (Scikit-learn/PyTorch)
    -  Inputs: Dados de dos registros de eventos que são solicitados pelos usuários
    -  Output: Previsão de recursos necessários (alimentos, medicamentos, cobertores) por região

    **Análise de sentimentos em mensagens de feedback e das mensagens no Registro do Evento**
    -   Tecnologia: ML.NET (C#)
    -   Aplicação: Mensagens de feedback dos usuários
    -   Registros textuais de eventos críticos
    -   Modelo: Classificação binária (Positivo/Negativo)
    -   Saída: Dashboard com indicadores de satisfação
    
    **Otimização de rotas para distribuição de recursos**
    -   Tecnologia: Python
    -   Parâmetros: Localização dos abrigos e Suporte
    -   Disponibilidade de itens
    -   Prioridade de entregas
    -   Resultado: Rotas otimizadas em tempo real

### Testes e Qualidade
- **xUnit** - Framework de testes unitários
- **Moq** - Framework para mocking

### Documentação e Observabilidade
- **Swagger/OpenAPI** - Documentação interativa da API com os endpoints e descrições de cada método.

### DevOps e Infraestrutura
- **Docker** - Containerização
- **Azure** - Hospedagem em nuvem via o projeto de Devops

## 🚀 Como Executar o Projeto

### 1. Clone o Repositório
```bash
git clone https://github.com/seu-usuario/socorro-solidario-api.git
cd socorro-solidario-api
```

### 2. Configuração do Ambiente

#### Configuração de conexão no Mongo
```bash
    "mongodb://localhost:27017"
```

**Salvar a variavel de ambiente**
```bash
    $env:MONGODB_CONNECTION_STRING = "mongodb://localhost:27017"
```

### 3. Instalação das Dependências
```bash
dotnet restore
```

### 4. Executar a Aplicação
```bash
    dotnet run
```

### 5. Acesso à Aplicação
- **API**: https://localhost:3001
- **Swagger UI**: http://localhost:3001/swagger/index.html

---

## 📋 Documentação dos Endpoints

Aqui teremos apenas uma breve explicação, o detalhamento da documentação com endpoints completos, estão disponíveis no Swagger.

### 🏠 Abrigos (`/api/abrigos`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `POST` | `/api/Abrigo/CadastrarAbrigo`        | Cadastra novo abrigo                      | ❌ |
| `GET` | `/api/Abrigo/ConsultarTodosAbrigo`    | Lista todos os abrigos disponíveis        | ❌ |
| `GET` | `/api/Abrigo/ConsultarAbrigoId/{id}`  | Busca abrigo específico por ID            | ❌ |
| `PUT` | `/api/Abrigo/AtualizarAbrigo/{id}`    | Atualiza todas as informações do abrigo   | ✅ |
| `PATCH` | `/api/Abrigo/AtualizarParcial/{id}` | Atualiza parcialmente dados do abrigo     | ✅ |
| `DELETE` | `/api/Abrigo/ExcluirAbrigo/{id}`   | Remove abrigo permanentemente             | ✅ |

**Exemplo de Request (POST/PUT)**
```json
{
  "descricao": "Abrigo Central",
  "capacidadeTotal": 100,
  "ocupacaoAtual": 25
}
```

**Exemplo de Response:**
```json
{
    "id": "6659fbbd3fae4c001fcf6d93",
    "descricao": "Abrigo Central",
    "capacidadeTotal": 100,
    "ocupacaoAtual": 25
}
```

### 👤 Usuário (/api/Usuario)
| Método   | Endpoint                                    | Descrição                                | Auth |
| -------- | ------------------------------------------- | ---------------------------------------- | ---- |
| `POST`   | `/api/Usuario/CadastrarUsuario`             | Cadastra novo usuário                    | ❌    |
| `GET`    | `/api/Usuario/ConsultarTodosUsuarios`       | Lista todos os usuários                  | ❌    |
| `GET`    | `/api/Usuario/ConsultarUsuarioId/{id}`      | Consulta usuário específico por ID       | ❌    |
| `PUT`    | `/api/Usuario/AtualizarUsuario/{id}`        | Atualiza todas as informações do usuário | ✅    |
| `PATCH`  | `/api/Usuario/AtualizarParcialUsuario/{id}` | Atualiza parcialmente dados do usuário   | ✅    |
| `DELETE` | `/api/Usuario/ExcluirUsuario/{id}`          | Remove usuário permanentemente           | ✅    |

## 🔑 TipoUsuario (/api/TipoUsuario)

| Método   | Endpoint                                       | Descrição                                           | Auth |
| -------- | ---------------------------------------------- | --------------------------------------------------- | ---- |
| `POST`   | `/api/TipoUsuario/CadastrarTipoUsuario`        | Cadastra um novo tipo de usuário                    | ❌    |
| `GET`    | `/api/TipoUsuario/ConsultarTodosTiposUsuario`  | Consulta todos os tipos de usuários                 | ❌    |
| `GET`    | `/api/TipoUsuario/ConsultarTipoUsuarioId/{id}` | Consulta um tipo de usuário específico por ID       | ❌    |
| `PUT`    | `/api/TipoUsuario/AtualizarTipoUsuario/{id}`   | Atualiza todas as informações de um tipo de usuário | ✅    |
| `PATCH`  | `/api/TipoUsuario/AtualizarParcial/{id}`       | Atualiza parcialmente os dados do tipo de usuário   | ✅    |
| `DELETE` | `/api/TipoUsuario/ExcluirTipoUsuario/{id}`     | Remove um tipo de usuário permanentemente           | ✅    |


## 🏷️ Categoria (/api/Categoria)

| Método   | Endpoint                                   | Descrição                                   | Auth |
| -------- | ------------------------------------------ | ------------------------------------------- | ---- |
| `POST`   | `/api/Categoria/CadastrarCategoria`        | Cadastra nova categoria                     | ❌    |
| `GET`    | `/api/Categoria/ConsultarTodasCategorias`  | Lista todas as categorias                   | ❌    |
| `GET`    | `/api/Categoria/ConsultarCategoriaId/{id}` | Consulta categoria específica por ID        | ❌    |
| `PUT`    | `/api/Categoria/AtualizarCategoria/{id}`   | Atualiza todas as informações da categoria  | ✅    |
| `PATCH`  | `/api/Categoria/AtualizarParcial/{id}`     | Atualiza parcialmente os dados da categoria | ✅    |
| `DELETE` | `/api/Categoria/ExcluirCategoria/{id}`     | Remove categoria permanentemente            | ✅    |


## 📦 Distribuição (/api/Distribuicao)

| Método   | Endpoint                                         | Descrição                                     | Auth |
| -------- | ------------------------------------------------ | --------------------------------------------- | ---- |
| `POST`   | `/api/Distribuicao/CadastrarDistribuicao`        | Cadastra nova distribuição                    | ❌    |
| `GET`    | `/api/Distribuicao/ConsultarTodosDistribuicao`   | Lista todas as distribuições                  | ❌    |
| `GET`    | `/api/Distribuicao/ConsultarDistribuicaoId/{id}` | Consulta distribuição específica por ID       | ❌    |
| `PUT`    | `/api/Distribuicao/AtualizarDistribuicao/{id}`   | Atualiza todas as informações da distribuição | ✅    |
| `PATCH`  | `/api/Distribuicao/AtualizarParcial/{id}`        | Atualiza parcialmente dados da distribuição   | ✅    |
| `DELETE` | `/api/Distribuicao/ExcluirDistribuicao/{id}`     | Remove uma distribuição permanentemente       | ✅    |

## 📦 Doação (/api/Doacao)

| Método   | Endpoint                             | Descrição                               | Auth |
| -------- | ------------------------------------ | --------------------------------------- | ---- |
| `POST`   | `/api/Doacao/CadastrarDoacao`        | Cadastra nova doação                    | ❌    |
| `GET`    | `/api/Doacao/ConsultarTodosDoacao`   | Lista todas as doações                  | ❌    |
| `GET`    | `/api/Doacao/ConsultarDoacaoId/{id}` | Consulta doação específica por ID       | ❌    |
| `PUT`    | `/api/Doacao/AtualizarDoacao/{id}`   | Atualiza todas as informações da doação | ✅    |
| `PATCH`  | `/api/Doacao/AtualizarParcial/{id}`  | Atualiza parcialmente dados da doação   | ✅    |
| `DELETE` | `/api/Doacao/ExcluirDoacao/{id}`     | Remove uma doação permanentemente       | ✅    |

## 📍 Endereço (/api/Endereco)

| Método   | Endpoint                                 | Descrição                                 | Auth |
| -------- | ---------------------------------------- | ----------------------------------------- | ---- |
| `POST`   | `/api/Endereco/CadastrarEndereco`        | Cadastra novo endereço                    | ❌    |
| `GET`    | `/api/Endereco/ConsultarTodosEndereco`   | Lista todos os endereços                  | ❌    |
| `GET`    | `/api/Endereco/ConsultarEnderecoId/{id}` | Consulta endereço específico por ID       | ❌    |
| `PUT`    | `/api/Endereco/AtualizarEndereco/{id}`   | Atualiza todas as informações do endereço | ✅    |
| `PATCH`  | `/api/Endereco/AtualizarParcial/{id}`    | Atualiza parcialmente dados do endereço   | ✅    |
| `DELETE` | `/api/Endereco/ExcluirEndereco/{id}`     | Remove um endereço permanentemente        | ✅    |

## ⭐ Feedback (/api/Feedback)

| Método   | Endpoint                                 | Descrição                                  | Auth |
| -------- | ---------------------------------------- | ------------------------------------------ | ---- |
| `POST`   | `/api/Feedback/CadastrarFeedback`        | Cadastra um novo feedback                  | ❌    |
| `GET`    | `/api/Feedback/ConsultarTodosFeedbacks`  | Lista todos os feedbacks                   | ❌    |
| `GET`    | `/api/Feedback/ConsultarFeedbackId/{id}` | Consulta um feedback específico por ID     | ❌    |
| `PUT`    | `/api/Feedback/AtualizarFeedback/{id}`   | Atualiza todas as informações do feedback  | ✅    |
| `PATCH`  | `/api/Feedback/AtualizarParcial/{id}`    | Atualiza parcialmente os dados do feedback | ✅    |
| `DELETE` | `/api/Feedback/ExcluirFeedback/{id}`     | Remove um feedback permanentemente         | ✅    |

## 📑 Documentação dos Endpoints — RegistroEvento

| Método   | Endpoint                                             | Descrição                                            | Auth |
| -------- | ---------------------------------------------------- | ---------------------------------------------------- | ---- |
| `POST`   | `/api/RegistroEvento/CadastrarRegistroEvento`        | Cadastra um novo registro de evento                  | ❌    |
| `GET`    | `/api/RegistroEvento/ConsultarTodosRegistroEvento`   | Lista todos os registros de evento                   | ❌    |
| `GET`    | `/api/RegistroEvento/ConsultarRegistroEventoId/{id}` | Consulta um registro de evento específico por ID     | ❌    |
| `PUT`    | `/api/RegistroEvento/AtualizarRegistroEvento/{id}`   | Atualiza todas as informações do registro de evento  | ✅    |
| `PATCH`  | `/api/RegistroEvento/AtualizarParcial/{id}`          | Atualiza parcialmente os dados do registro de evento | ✅    |
| `DELETE` | `/api/RegistroEvento/ExcluirRegistroEvento/{id}`     | Remove um registro de evento permanentemente         | ✅    |


## 🧪 Instruções de Testes

### Executando Todos os Testes
```bash
    dotnet test
```

### Estrutura de Testes

```
📁 tests/Project.Tests
├── 📁 IntegrationTests.Repositories/
│   ├── 📁 Abrigo
│   ├── 📁 Categoria/
│   ├── 📁 Distribuicao/
│   └── 📁 Doacao/
│   └── 📁 Endereco/
│   └── 📁 TipoUsuario/
│   └── 📁 Usuario/
├── 📁 UnitTests.Services/
│   ├── 📁 Abrigo
│   ├── 📁 Categoria/
│   ├── 📁 Distribuicao/
│   └── 📁 Doacao/
│   └── 📁 Endereco/
│   └── 📁 TipoUsuario/
│   └── 📁 Usuario/
└── 📁 WebTests.Controllers/
│   ├── 📁 Abrigo
│   ├── 📁 Categoria/
│   ├── 📁 Distribuicao/
│   └── 📁 Doacao/
│   └── 📁 Endereco/
│   └── 📁 TipoUsuario/
│   └── 📁 Usuario/
```

### Cobertura de Testes
O projeto mantém uma cobertura mínima de **100%** para garantir qualidade e confiabilidade.

---

### Padrões de Commit
```
feat: adiciona nova funcionalidade
fix: corrige bug
docs: atualiza documentação
test: adiciona ou modifica testes
refactor: refatora código sem alterar funcionalidade
```

---

## 📞 Contato

**Equipe Socorro Solidário**
- 📧 Email: contato@socorrosolidario.org
- 🐛 Issues: [GitHub Issues](https://github.com/seu-usuario/socorro-solidario-api/issues)
- 💬 Discussões: [GitHub Discussions](https://github.com/seu-usuario/socorro-solidario-api/discussions)

---

<div align="center">

**Feito com ❤️ para salvar vidas e conectar corações**

*"Na hora da tempestade, somos todos uma família"*

[![GitHub Claudio](https://github.com/Claudio-Silva-Bispo)
[![GitHub Patricia](https://github.com/patinaomi)

</div>