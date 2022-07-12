# GameCatalogV2
API desenvolvida em ASP.NET e C#, melhorando aspectos da [GameCatalogV1](https://github.com/lothar-matthaus/gamecatalog) com as mais básicas operações CRUD (Creation, Read, Update, Delete), incluindo autenticação BEARER Token JWT e Design Patterns como Unit Of Work e Repository Pattern.

## Restrições
Esta API foi desenvolvida pra a disciplina **Desenvolvimento de Software Para Web**. Por mais que a mesma ainda possua validações básicas, pode ocorrer inconsistência dos dados caso ocorra algum problema decorrido pela falta de validações.
- Impossibilidade de criar novo jogo com o mesmo ID.
- Imposibilidade de criar um usuário com o mesmo endereço de e-mail.

## Features
- A API possui as funcionalidades básicas de **criação, leitura, atualização e remoção** de dados (**CRUD**).
- Autenticação por **BearerToken** (JWT).
- **Persistência** de dados de forma relacional com SQLite.
- CORS com políticas abertas para qualquer **Origem, Header ou Credencial**.
- Geração de bearer token por Login e Senha.

# Documentação

## Entidades

### Game (Jogo)
Propriedades:
| Nome da Propriedade | Tipo     | Descrição                        |Necessidade       |
|---------------------|----------|----------------------------------|----------------- |
| Id                  | int      | Identificador único              | Opcional         |
| Name                | string   | Título do Jogo                   | Obrigatório      |
| Description         | string   | Descrição do título              | Obrigatório      |
| CoverImgUrl         | string   | Imagem de capa do jogo           | Obrigatório      |
| Genre               | int      | Gêneros do Jogo                  | Obrigatório      |
| SlideImgUrl         | string   | Imagens de apresentação do jogo  | Obrigatório      |
| ReleaseDate         | DateTime | Data de Lançamento do Jogo       | Obrigatório      |
| CreationDate        | DateTime | Data que o título foi cadastrado | Opcional         |
---
Json de geração:
~~~ json
{
  "title": "string",
  "description": "string",
  "coverUrl": "string",
  "releaseDate": "2022-07-12T21:03:51.688Z",
  "genres": [
    0
  ]
}
~~~
---
Json de atualização:
~~~json
{
  "id": 0,
  "title": "string",
  "description": "string",
  "coverUrl": "string",
  "releaseDate": "2022-07-12T21:03:23.995Z",
  "genres": [
    0
  ]
}
~~~
### Genre (Gênero)
Propriedades:
| Nome da Propriedade | Tipo     | Descrição                        |Necessidade       |
|---------------------|----------|----------------------------------|----------------- |
| Id                  | int      | Identificador único              | Opcional         |
| Name                | string   | Nome do Gênero                   | Obrigatório      |
| CreationDate        | DateTime | Data que o gênero foi cadastrado | Opcional         |
---
Json de geração:
~~~json
{
  "name": "string"
}
~~~
~~~json
[
  {
    "name": "string"
  }
]
~~~
---
Json de atualização
~~~json
{
  "name": "string",
  "id": 0
}
~~~
---
Json de Buscar uma lista de gêneros
~~~json
[
  1, 5, 6
]
~~~
---
### Rotas
> https://localhost:5001/api/Genre -- GET/PATCH/POST

> https://localhost:5001/api/Genre/12 -- GET/DELETE

> https://localhost:5001/api/Genre/New/List -- POST
---
### User (Usuário)
Propriedades:
| Nome da Propriedade | Tipo   | Descrição                            |  Necessidade  |
|---------------------|--------|--------------------------------------|---------------|
| Id                  |  int     | Identificador único                |   Opcional    |
| Name                |  string  | Nome do usuário                    |  Obrigatório  |
| UserRole            |  int     | Privilégio do Usuário              |  Obrigatório  |
| Email               |  string  | E-mail de identificação do usuário |  Obrigatório  |
| Password            |  string  | Senha de acesso do usuário         |  Obrigatório  |
| CreationDate        | DateTime | Data de Cadastro do usuário        |   Opcional    |
---
Json de geração:
~~~json
{
  "fullName": "string",
  "email": "user@example.com",
  "userRole": 0,
  "password": "string"
}
~~~
---
Jsom de atualização
~~~json
{
  "id": 0,
  "fullName": "string",
  "email": "user@example.com",
  "userRole": 0,
  "password": "string"
}
~~~
---
### Rotas
> https://localhost:5001/api/User/SignIn -- POST

> https://localhost:5001/api/User -- POST/PATCH
---

# Funcionamento
O funcionamento da API é simples e direto. Há uma necessidade de se estar cadastrado no sistema para poder se utilizar dos métodos **POST, PATCH e DELETE**.
> Nota: Para fazer o cadastro de um usuário, deve-se utilizar o **Json de Geração** acima na rota *api/User* via método **POST**.

## Geração de Token
Para gerar um Token de acesso, você deve realizar o Login na rota *api/User/Login* via método **POST**. Se o login for realizado com sucesso, será retornado o token de acesso.
> Nota: Os método GET são todos públicos, em suma, não há necessidade de se estar logado para poder visualizar um título ou todos os títulos.

## Create, Update e Delete
Com o Token de acesso, você poderá criar, deletar e atualizar os jogos persistidos no sistema.

