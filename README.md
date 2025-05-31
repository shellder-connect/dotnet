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

### Banco de Dados e Mensageria
- **MongoDb** - Banco de dados principal

### Inteligência Artificial
- **ML.NET** - Machine Learning para:
  - Predição de necessidades por região
  - Análise de sentimentos em mensagens de feedback e das mensagens no Registro do Evento. Este processo será feito via Python.
  - Otimização de rotas para distribuição de recursos -- Processo será realizado via Python

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
- **API**: https://localhost:7001
- **Swagger UI**: https://localhost:7001/swagger

---

## 📋 Documentação dos Endpoints

### 🏠 Abrigos (`/api/abrigos`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/abrigos` | Lista todos os abrigos disponíveis | ❌ |
| `GET` | `/api/abrigos/{id}` | Busca abrigo específico | ❌ |
| `GET` | `/api/abrigos/proximos` | Abrigos próximos (por geolocalização) | ❌ |
| `POST` | `/api/abrigos` | Cadastra novo abrigo | ✅ |
| `PUT` | `/api/abrigos/{id}` | Atualiza informações do abrigo | ✅ |
| `DELETE` | `/api/abrigos/{id}` | Remove abrigo | ✅ |

**Exemplo de Response:**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "nome": "Abrigo Municipal Centro",
  "endereco": {
    "rua": "Rua das Flores, 123",
    "cidade": "São Paulo",
    "cep": "01234-567",
    "coordenadas": {
      "latitude": -23.5505,
      "longitude": -46.6333
    }
  },
  "capacidade": {
    "total": 200,
    "ocupada": 45,
    "disponivel": 155
  },
  "recursos": ["alimentacao", "roupas", "medicamentos", "psicologico"],
  "contato": "(11) 98765-4321",
  "status": "ativo",
  "_links": {
    "self": "/api/abrigos/123e4567-e89b-12d3-a456-426614174000",
    "edit": "/api/abrigos/123e4567-e89b-12d3-a456-426614174000",
    "recursos": "/api/abrigos/123e4567-e89b-12d3-a456-426614174000/recursos"
  }
}
```

### 🩺 Profissionais de Saúde (`/api/profissionais`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/profissionais` | Lista profissionais disponíveis | ❌ |
| `GET` | `/api/profissionais/{id}` | Busca profissional específico | ❌ |
| `GET` | `/api/profissionais/especialidade/{tipo}` | Filtra por especialidade | ❌ |
| `POST` | `/api/profissionais` | Cadastra novo profissional | ✅ |
| `PUT` | `/api/profissionais/{id}` | Atualiza dados do profissional | ✅ |
| `POST` | `/api/profissionais/{id}/atendimentos` | Agenda atendimento | ✅ |

### 🆘 Solicitações de Socorro (`/api/socorro`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `POST` | `/api/socorro` | Cria nova solicitação de socorro | ❌ |
| `GET` | `/api/socorro/{id}` | Acompanha status da solicitação | ❌ |
| `PUT` | `/api/socorro/{id}/status` | Atualiza status (profissionais) | ✅ |
| `GET` | `/api/socorro/urgentes` | Lista casos urgentes | ✅ |

### 🤖 Inteligência Artificial (`/api/ia`)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `POST` | `/api/ia/analisar-necessidade` | Analisa texto e prediz necessidades | ❌ |
| `POST` | `/api/ia/otimizar-rota` | Otimiza rota de distribuição | ✅ |
| `GET` | `/api/ia/predicoes/regiao/{id}` | Predições para uma região | ✅ |

---

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


## 🤖 Machine Learning Features

### Modelos Implementados

#### 1. Predição de Demanda por Recursos
- **Algoritmo**: Regressão Linear
- **Input**: Histórico de solicitações, dados climáticos, população
- **Output**: Previsão de demanda por tipo de recurso

#### 2. Análise de Sentimentos
- **Algoritmo**: Classificação Binária
- **Input**: Mensagens de socorro
- **Output**: Urgência (Alta/Média/Baixa)

#### 3. Otimização de Rotas
- **Algoritmo**: Clustering K-Means
- **Input**: Localização de recursos e necessidades
- **Output**: Rotas otimizadas para distribuição

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