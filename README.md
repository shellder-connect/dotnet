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
- **API**: https://localhost:3001
- **Swagger UI**: http://localhost:3001/swagger/index.html

---

## 📋 Documentação dos Endpoints

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