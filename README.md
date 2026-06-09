# 🌱 Eclipse Protocol — API de Monitoramento Inteligente de Plantações

> **Global Solution 2026 — FIAP**  
> API REST desenvolvida em **ASP.NET Core 8** com persistência em **Oracle** via Entity Framework Core 8 e autenticação **JWT**.

---

## 📑 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Diagrama de Entidades](#diagrama-de-entidades)
- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Como Executar](#como-executar)
- [Migrations](#migrations)
- [Endpoints da API](#endpoints-da-api)
- [Exemplos de Testes (curl)](#exemplos-de-testes-curl)
- [Exemplos de Testes (Swagger)](#exemplos-de-testes-swagger)
- [Autenticação JWT](#autenticação-jwt)

---

## Sobre o Projeto

O **Eclipse Protocol** é uma solução de monitoramento inteligente para propriedades rurais. Sensores IoT instalados em plantações coletam dados em tempo real (temperatura, umidade, precipitação e índice NDVI) e a API processa essas leituras, disparando alertas automáticos quando os valores ultrapassam limites críticos — permitindo que o produtor rural tome decisões rápidas para proteger sua safra.

---

## Diagrama de Entidades

```
┌──────────────┐        ┌─────────────────┐        ┌──────────────────┐
│  TB_USUARIO  │  1:N   │ TB_PROPRIEDADE  │  1:N   │  TB_PLANTACAO    │
│──────────────│───────▶│─────────────────│───────▶│──────────────────│
│ ID_USUARIO   │        │ ID_PROPRIEDADE  │        │ ID_PLANTACAO     │
│ NM_USUARIO   │        │ NM_PROPRIEDADE  │        │ NM_PLANTACAO     │
│ DS_EMAIL     │        │ NR_AREA_TOTAL   │        │ DS_CULTURA       │
│ DS_SENHA     │        │ TP_SOLO         │        │ NR_AREA_HECTARES │
│ ST_ATIVO     │        │ ID_USUARIO (FK) │        │ DS_STATUS        │
│ DT_CRIACAO   │        │ ID_LOCALIZACAO  │        │ ID_PROPRIEDADE   │
└──────────────┘        └─────────────────┘        └──────────────────┘
                                 │                          │
                                 ▼                          ▼
                        ┌──────────────────┐      ┌──────────────────┐
                        │  TB_LOCALIZACAO  │  1:N │   TB_SENSOR      │
                        │──────────────────│      │──────────────────│
                        │ ID_LOCALIZACAO   │      │ ID_SENSOR        │
                        │ NM_CIDADE        │      │ NM_SENSOR        │
                        │ SG_ESTADO        │      │ TP_SENSOR        │
                        │ NM_PAIS          │      │ ST_ATIVO         │
                        │ NR_LATITUDE      │      │ DT_INSTALACAO    │
                        │ NR_LONGITUDE     │      │ ID_PLANTACAO (FK)│
                        │ NR_CEP           │      └──────────────────┘
                        └──────────────────┘               │
                                                           ▼
                        ┌──────────────────────────────────────────────┐
                        │                TB_LEITURA                    │
                        │──────────────────────────────────────────────│
                        │ ID_LEITURA  │ NR_TEMPERATURA │ NR_UMIDADE    │
                        │ NR_PRECIPITACAO │ NR_NDVI │ DT_LEITURA      │
                        │ ID_SENSOR (FK)                               │
                        └──────────────────────────────────────────────┘
                                           │
                                           ▼
                        ┌──────────────────────────────────────────────┐
                        │                 TB_ALERTA                    │
                        │──────────────────────────────────────────────│
                        │ ID_ALERTA │ TP_ALERTA │ DS_SEVERIDADE        │
                        │ DS_MENSAGEM │ DS_STATUS │ DT_CRIACAO         │
                        │ ID_LEITURA (FK) │ ID_PLANTACAO (FK)         │
                        └──────────────────────────────────────────────┘
```

### Relacionamentos

| Relação | Tipo | Descrição |
|---------|------|-----------|
| Usuario → Propriedade | **1:N** | Um usuário possui várias propriedades rurais |
| Localizacao → Propriedade | **1:N** | Uma localização abrange várias propriedades |
| Propriedade → Plantacao | **1:N** | Uma propriedade possui várias plantações |
| Plantacao → Sensor | **1:N** | Uma plantação possui vários sensores IoT |
| Sensor → Leitura | **1:N** | Um sensor gera várias leituras ao longo do tempo |
| Plantacao → Alerta | **1:N** | Uma plantação pode gerar vários alertas |
| Leitura → Alerta | **1:N** | Uma leitura pode originar vários alertas |

---

## Arquitetura

```
GlobalSolution/
├── Controller/        # Controllers REST (rotas + mapeamento DTO ↔ Model)
├── Data/              # AppDbContext (EF Core + configurações de mapeamento)
├── Dto/               # DTOs de entrada (Create/Update) e saída (Response)
├── Migrations/        # Histórico de migrações do banco de dados
├── Models/            # Entidades do domínio (anotações EF + DataAnnotations)
├── Service/           # Regras de negócio e acesso ao banco via EF Core
├── appsettings.json   # Configurações (ConnectionString + JWT)
└── Program.cs         # Bootstrap: DI, JWT, Swagger, pipeline HTTP
```

**Padrão:** `Controller → Service → DbContext (Repository implícito via EF Core)`

---

## Tecnologias

| Tecnologia | Versão |
|------------|--------|
| .NET / ASP.NET Core | **8.0** |
| Entity Framework Core | 8.0.3 |
| Oracle.EntityFrameworkCore | 8.23.40 |
| Swashbuckle (Swagger) | 6.6.2 |
| JWT Bearer Authentication | 8.0.0 |
| Oracle Database | FIAP — oracle.fiap.com.br |

---

## Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Acesso à rede da FIAP (VPN se necessário para o Oracle)
- [dotnet-ef CLI](https://learn.microsoft.com/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

### 1. Clonar o repositório

```bash
git clone https://github.com/<seu-usuario>/eclipse-protocol-dotnet.git
cd eclipse-protocol-dotnet/GlobalSolution
```

### 2. Aplicar as Migrations (criar tabelas no banco)

```bash
dotnet ef database update
```

### 3. Executar a API

```bash
dotnet run
```

A API estará disponível em:
- **HTTP:** `http://localhost:5041`
- **Swagger UI:** `http://localhost:5041` (raiz)

---

## Migrations

O projeto utiliza **EF Core Migrations** para controle versionado do esquema do banco Oracle.

```bash
# Criar uma nova migration após alterar os Models
dotnet ef migrations add NomeDaMigration --output-dir Migrations

# Aplicar todas as migrations pendentes ao banco
dotnet ef database update

# Reverter para uma migration específica
dotnet ef database update NomeDaMigration

# Remover a última migration (antes de aplicar ao banco)
dotnet ef migrations remove
```

---

## Endpoints da API

### 👤 Usuário — `/api/usuario`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/usuario` | Listar todos os usuários |
| GET | `/api/usuario/{id}` | Buscar usuário por ID |
| POST | `/api/usuario` | Criar novo usuário |
| PUT | `/api/usuario/{id}` | Atualizar usuário |
| DELETE | `/api/usuario/{id}` | Remover usuário |

### 📍 Localização — `/api/localizacao`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/localizacao` | Listar todas as localizações |
| GET | `/api/localizacao/{id}` | Buscar por ID |
| POST | `/api/localizacao` | Criar localização |
| PUT | `/api/localizacao/{id}` | Atualizar localização |
| DELETE | `/api/localizacao/{id}` | Remover localização |

### 🏡 Propriedade — `/api/propriedade`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/propriedade` | Listar todas as propriedades |
| GET | `/api/propriedade/{id}` | Buscar por ID |
| POST | `/api/propriedade` | Criar propriedade |
| PUT | `/api/propriedade/{id}` | Atualizar propriedade |
| DELETE | `/api/propriedade/{id}` | Remover propriedade |

### 🌾 Plantação — `/api/plantacao`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/plantacao` | Listar todas as plantações |
| GET | `/api/plantacao/{id}` | Buscar por ID |
| POST | `/api/plantacao` | Criar plantação |
| PUT | `/api/plantacao/{id}` | Atualizar plantação |
| DELETE | `/api/plantacao/{id}` | Remover plantação |

### 📡 Sensor — `/api/sensor`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/sensor` | Listar todos os sensores |
| GET | `/api/sensor/{id}` | Buscar por ID |
| POST | `/api/sensor` | Criar sensor |
| PUT | `/api/sensor/{id}` | Atualizar sensor |
| DELETE | `/api/sensor/{id}` | Remover sensor |

### 📊 Leitura — `/api/leitura`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/leitura` | Listar todas as leituras |
| GET | `/api/leitura/{id}` | Buscar por ID |
| POST | `/api/leitura` | Registrar leitura de sensor |
| PUT | `/api/leitura/{id}` | Atualizar leitura |
| DELETE | `/api/leitura/{id}` | Remover leitura |

### 🚨 Alerta — `/api/alerta`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/alerta` | Listar todos os alertas |
| GET | `/api/alerta/{id}` | Buscar por ID |
| POST | `/api/alerta` | Criar alerta |
| PUT | `/api/alerta/{id}` | Atualizar alerta |
| DELETE | `/api/alerta/{id}` | Remover alerta |

---

## Exemplos de Testes (curl)

> Base URL: `http://localhost:5041`

### 1. Criar Usuário

```bash
curl -X POST http://localhost:5041/api/usuario \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João da Silva",
    "email": "joao@email.com",
    "senha": "Senha@123",
    "ativo": true
  }'
```

**Resposta esperada (201 Created):**
```json
{
  "idUsuario": 1,
  "nome": "João da Silva",
  "email": "joao@email.com",
  "ativo": true,
  "dataCriacao": "2025-06-08T10:00:00"
}
```

---

### 2. Criar Localização

```bash
curl -X POST http://localhost:5041/api/localizacao \
  -H "Content-Type: application/json" \
  -d '{
    "cidade": "Ribeirão Preto",
    "estado": "SP",
    "pais": "Brasil",
    "latitude": -21.1775,
    "longitude": -47.8103,
    "cep": "14010-000"
  }'
```

---

### 3. Criar Propriedade

```bash
curl -X POST http://localhost:5041/api/propriedade \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Fazenda Santa Clara",
    "areaTotal": 500.0,
    "tipoSolo": "Latossolo Vermelho",
    "idUsuario": 1,
    "idLocalizacao": 1
  }'
```

---

### 4. Criar Plantação

```bash
curl -X POST http://localhost:5041/api/plantacao \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Talhão A1",
    "cultura": "Soja",
    "areaHectares": 120.5,
    "status": "ATIVA",
    "idPropriedade": 1
  }'
```

---

### 5. Criar Sensor

```bash
curl -X POST http://localhost:5041/api/sensor \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Sensor Temp-01",
    "tipo": "TEMPERATURA",
    "ativo": true,
    "idPlantacao": 1
  }'
```

---

### 6. Registrar Leitura

```bash
curl -X POST http://localhost:5041/api/leitura \
  -H "Content-Type: application/json" \
  -d '{
    "temperatura": 38.5,
    "umidade": 25.0,
    "precipitacao": 0.0,
    "ndvi": 0.42,
    "idSensor": 1
  }'
```

**Resposta esperada (201 Created):**
```json
{
  "idLeitura": 1,
  "temperatura": 38.5,
  "umidade": 25.0,
  "precipitacao": 0.0,
  "ndvi": 0.42,
  "dataLeitura": "2025-06-08T10:05:00",
  "idSensor": 1
}
```

---

### 7. Criar Alerta

```bash
curl -X POST http://localhost:5041/api/alerta \
  -H "Content-Type: application/json" \
  -d '{
    "tipoAlerta": "TEMPERATURA_ALTA",
    "severidade": "CRITICO",
    "mensagem": "Temperatura acima de 38°C detectada no Talhão A1.",
    "status": "ABERTO",
    "idLeitura": 1,
    "idPlantacao": 1
  }'
```

---

### 8. Listar todos os Alertas

```bash
curl http://localhost:5041/api/alerta
```

---

### 9. Atualizar status do Alerta (resolver)

```bash
curl -X PUT http://localhost:5041/api/alerta/1 \
  -H "Content-Type: application/json" \
  -d '{
    "tipoAlerta": "TEMPERATURA_ALTA",
    "severidade": "CRITICO",
    "mensagem": "Temperatura acima de 38°C detectada no Talhão A1.",
    "status": "RESOLVIDO",
    "idLeitura": 1,
    "idPlantacao": 1
  }'
```

---

### 10. Deletar Usuário

```bash
curl -X DELETE http://localhost:5041/api/usuario/1
```

**Resposta esperada:** `204 No Content`  
*(Erro 400 se o usuário possuir propriedades vinculadas)*

---

## Exemplos de Testes (Swagger)

1. Acesse `http://localhost:5041` no navegador
2. A interface Swagger UI exibe todos os endpoints documentados
3. Clique em qualquer endpoint → **"Try it out"** → preencha o body → **"Execute"**
4. O Swagger exibe o `curl` equivalente, o corpo da requisição e a resposta

### Fluxo de teste sugerido no Swagger:

```
POST /api/localizacao  →  POST /api/usuario
        ↓
POST /api/propriedade (usa idUsuario + idLocalizacao)
        ↓
POST /api/plantacao (usa idPropriedade)
        ↓
POST /api/sensor (usa idPlantacao)
        ↓
POST /api/leitura (usa idSensor)
        ↓
POST /api/alerta (usa idLeitura + idPlantacao)
```

---

## Autenticação JWT

A API possui suporte a **JWT Bearer Token**. O pacote `Microsoft.AspNetCore.Authentication.JwtBearer` está configurado em `Program.cs`.

**Configuração no `appsettings.json`:**
```json
"Jwt": {
  "Key": "GlobalSolutionEclipseProtocolSecretKey2026!@#",
  "Issuer": "GlobalSolution.API",
  "Audience": "GlobalSolution.Client",
  "ExpiresInHours": 8
}
```

Para usar o Swagger com token:
1. Gere o token JWT com as credenciais do usuário
2. No Swagger UI, clique em **"Authorize"** (🔒)
3. Informe: `Bearer <seu_token>`
4. Clique em **"Authorize"**

---

## 👥 Equipe

| Nome                    | RM       |
|-------------------------|----------|
| *Gustavo Gomes Martins* | 555999 |
| *Pedro dos Anjos* | 561716 |
| *Matheus de Mattos Vecchi* | 561716 |
| *Nicholas Albuquerque Buzo* | 561082 |
| *Nicholas Camillo Canadas de Paula* | 561262 |


---

## 📄 Licença

Projeto acadêmico — FIAP Global Solution 2026. Todos os direitos reservados.
