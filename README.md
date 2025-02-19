# TaskManager API 🚀

Uma API para gerenciamento de tarefas com autenticação JWT, arquitetura modular e banco de dados PostgreSQL.

## 📋 Sumário

- [📦 Pré-requisitos](#pré-requisitos)
- [⚙️ Configuração do Banco de Dados](#configuração-do-banco-de-dados)
- [🔐 Autenticação e Autorização](#autenticação-e-autorização)
- [🏗️ Arquitetura da Aplicação](#arquitetura-da-aplicação)
- [🧪 Testes Unitários](#testes-unitários)
- [💻 Executando o Projeto Localmente](#executando-o-projeto-localmente)
- [🚀 Rodando com IIS Express](#rodando-com-iis-express)
- [📂 Subindo a API a partir do Repositório](#subindo-a-api-a-partir-do-repositório)
- [📄 Documentação da API](#documentação-da-api)

---

## 📦 Pré-requisitos

Antes de iniciar, você precisará ter instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/downloads/)
- [IIS Express](https://learn.microsoft.com/en-us/iis/extensions/introduction-to-iis-express/iis-express-overview)

---

## ⚙️ Configuração do Banco de Dados

A API usa **PostgreSQL** como banco de dados principal. Para configurar:

1️⃣ **Crie um banco de dados no PostgreSQL**

```sql
CREATE DATABASE taskmanager_db;
```

2️⃣ **Configure a string de conexão no `appsettings.json`**

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=taskmanager_db;Username=seu_usuario;Password=sua_senha"
}
```

3️⃣ **Execute as migrations para criar as tabelas**

```sh
dotnet ef database update
```

Se precisar criar uma nova migration:

```sh
dotnet ef migrations add NomeDaMigration
dotnet ef database update
```

---

## 🔐 Autenticação e Autorização

A API usa **JWT (JSON Web Token)** para autenticação. Para acessar as rotas protegidas:

1️⃣ **Crie um usuário via endpoint de registro (`/api/auth/register`)**

```json
{
  "name": "John Doe",
  "email": "johndoe@email.com",
  "password": "123456"
}
```

2️⃣ **Faça login para obter o token JWT (`/api/auth/login`)**

```json
{
  "email": "johndoe@email.com",
  "password": "123456"
}
```

Resposta esperada:

```json
{
  "token": "seu-token-aqui"
}
```

3️⃣ **Use o token nas requisições**

- No **Swagger**, clique em **Authorize** e insira `Bearer SEU_TOKEN`.
- Ou no **Postman**, adicione `Authorization: Bearer SEU_TOKEN` no Header.

---

## 🏗️ Arquitetura da Aplicação

A API segue uma arquitetura modular baseada em **Camadas (Layers)**:

```
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
```

- **Controllers**: Apenas expõem os endpoints e chamam os **Services**.
- **Services**: Contêm a lógica de negócios e validações.
- **Repositories**: Abstraem o acesso ao banco de dados.
- **DTOs**: Evitam expor diretamente os modelos da base de dados.
- **Validators**: Usam **FluentValidation** para validar dados de entrada.

---

## 🧪 Testes Unitários

O projeto de testes está dentro da aplicação, na pasta **TaskManagerAPI.Tests**.
Para adicioná-lo à solução principal:

1️⃣ **Abra o Visual Studio**
2️⃣ **Clique com o botão direito na solução > Adicionar > Projeto existente**
3️⃣ **Selecione `TaskManagerAPI.Tests.csproj`**
4️⃣ **No projeto principal, adicione uma referência ao projeto de testes**

Depois disso pode excluir a pasta TaskManagerAPI.Tests na Raiz do projeto

Para rodar os testes:

```sh
dotnet test
```

---

## 💻 Executando o Projeto Localmente

1️⃣ **Clone o repositório**

```sh
git clone https://github.com/seu-usuario/taskmanager-api.git
cd taskmanager-api
```

2️⃣ **Instale as dependências**

```sh
dotnet restore
```

3️⃣ **Configure o banco de dados**

```sh
dotnet ef database update
```

4️⃣ **Execute a API**

```sh
dotnet run
```

A API estará disponível em `https://localhost:5001` ou `http://localhost:5000`.

---

## 🚀 Rodando com IIS Express

Caso queira rodar via **IIS Express** no Visual Studio:

1️⃣ **Abra a solução no Visual Studio**
2️⃣ **Clique no menu "Debug" > "Iniciar Sem Depuração" (Ctrl + F5)**
3️⃣ **O Visual Studio abrirá o navegador com o Swagger carregado**

Se precisar configurar a porta:

- No **Properties > launchSettings.json**, edite:

```json
"applicationUrl": "https://localhost:44309"
```

---

## 📂 Subindo a API a partir do Repositório

Para contribuir ou rodar o projeto em outro ambiente:

1️⃣ **Clone o repositório**

```sh
git clone https://github.com/seu-usuario/taskmanager-api.git
cd taskmanager-api
```

2️⃣ **Crie um novo branch**

```sh
git checkout -b minha-feature
```

3️⃣ **Faça suas alterações e commit**

```sh
git add .
git commit -m "Minha feature implementada"
git push origin minha-feature
```

4️⃣ **Crie um Pull Request (PR) no GitHub** e aguarde a revisão.

---

## 📄 Documentação da API

A API está documentada com **Swagger**.

- **Rodando localmente**: Acesse [`https://localhost:5001/swagger`](https://localhost:5001/swagger)

---

## 🎯 Conclusão

Agora você tem a **TaskManager API** rodando localmente! 🎉

Se tiver dúvidas ou sugestões, **abra uma issue** no repositório. 🚀

