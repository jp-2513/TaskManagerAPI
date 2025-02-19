TaskManager API 🚀

Uma API para gerenciamento de tarefas com autenticação JWT, arquitetura modular e banco de dados PostgreSQL.

📋 Sumário

📦 Pré-requisitos

⚙️ Configuração do Banco de Dados

🔐 Autenticação e Autorização

🏗️ Arquitetura da Aplicação

💻 Executando o Projeto Localmente

🚀 Rodando com IIS Express

📂 Subindo a API a partir do Repositório

📄 Documentação da API

📦 Pré-requisitos

Antes de iniciar, você precisará ter instalado:

.NET 8 SDK

PostgreSQL

Git

Visual Studio 2022

IIS Express

⚙️ Configuração do Banco de Dados

A API usa PostgreSQL como banco de dados principal. Para configurar:

1️⃣ Crie um banco de dados no PostgreSQL

CREATE DATABASE taskmanager_db;

2️⃣ **Configure a string de conexão no **appsettings.json

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=taskmanager_db;Username=seu_usuario;Password=sua_senha"
}

3️⃣ Execute as migrations para criar as tabelas

dotnet ef database update

Se precisar criar uma nova migration:

dotnet ef migrations add NomeDaMigration
dotnet ef database update

🔐 Autenticação e Autorização

A API usa JWT (JSON Web Token) para autenticação. Para acessar as rotas protegidas:

1️⃣ Crie um usuário via endpoint de registro (/api/auth/register)

{
  "name": "John Doe",
  "email": "johndoe@email.com",
  "password": "123456"
}

2️⃣ Faça login para obter o token JWT (/api/auth/login)

{
  "email": "johndoe@email.com",
  "password": "123456"
}

Resposta esperada:

{
  "token": "seu-token-aqui"
}

3️⃣ Use o token nas requisições

No Swagger, clique em Authorize e insira Bearer SEU_TOKEN.

Ou no Postman, adicione Authorization: Bearer SEU_TOKEN no Header.

🏗️ Arquitetura da Aplicação

A API segue uma arquitetura modular baseada em Camadas (Layers):

📂 TaskManagerAPI
 ├── 📂 Controllers        # Controladores da API
 ├── 📂 Services           # Regras de negócio (camada de serviço)
 ├── 📂 Repositories       # Camada de acesso ao banco de dados
 ├── 📂 Models             # Modelos das entidades
 ├── 📂 DTOs               # Objetos de transferência de dados
 ├── 📂 Validators         # Validações com FluentValidation
 ├── 📂 Tests              # Testes unitários (XUnit + FluentAssertions + Moq)
 ├── 📄 Program.cs         # Ponto de entrada da aplicação
 ├── 📄 appsettings.json   # Configurações da API

Controllers: Apenas expõem os endpoints e chamam os Services.

Services: Contêm a lógica de negócios e validações.

Repositories: Abstraem o acesso ao banco de dados.

DTOs: Evitam expor diretamente os modelos da base de dados.

Validators: Usam FluentValidation para validar dados de entrada.

💻 Executando o Projeto Localmente

1️⃣ Clone o repositório

git clone https://github.com/seu-usuario/taskmanager-api.git
cd taskmanager-api

2️⃣ Instale as dependências

dotnet restore

3️⃣ Configure o banco de dados

dotnet ef database update

4️⃣ Execute a API

dotnet run

A API estará disponível em https://localhost:5001 ou http://localhost:5000.

🚀 Rodando com IIS Express

Caso queira rodar via IIS Express no Visual Studio:

1️⃣ Abra a solução no Visual Studio
2️⃣ Clique no menu "Debug" > "Iniciar Sem Depuração" (Ctrl + F5)
3️⃣ O Visual Studio abrirá o navegador com o Swagger carregado

Se precisar configurar a porta:

No Properties > launchSettings.json, edite:

"applicationUrl": "https://localhost:44309"

📂 Subindo a API a partir do Repositório

Para contribuir ou rodar o projeto em outro ambiente:

1️⃣ Clone o repositório

git clone https://github.com/seu-usuario/taskmanager-api.git
cd taskmanager-api

2️⃣ Crie um novo branch

git checkout -b minha-feature

3️⃣ Faça suas alterações e commit

git add .
git commit -m "Minha feature implementada"
git push origin minha-feature

4️⃣ Crie um Pull Request (PR) no GitHub e aguarde a revisão.

📄 Documentação da API

A API está documentada com Swagger.

Rodando localmente: Acesse https://localhost:5001/swagger

Endpoints disponíveis:

POST /api/auth/register → Criar um usuário

POST /api/auth/login → Obter um token JWT

GET /api/tasks → Listar todas as tarefas (requer autenticação)

POST /api/tasks → Criar uma nova tarefa (usuário autenticado)

PUT /api/tasks/{id} → Atualizar uma tarefa (somente criador)

DELETE /api/tasks/{id} → Excluir uma tarefa (somente criador)
