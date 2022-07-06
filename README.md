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
| SlideImgUrl         | string   | Imagens de apresentação do jogo  | Obrigatório      |
| ReleaseDate         | DateTime | Data de Lançamento do Jogo       | Obrigatório      |
| CreationDate        | DateTime | Data que o título foi cadastrado | Opcional         |
---
Json de geração:
~~~ json
{
  "id": 0,
  "name": "string",
  "description": "string",
  "categories": [
    "string"
  ],
  "coverImgUrl": "string",
  "slideImgUrl": [
    "string"
  ],
  "releaseDate": "2021-12-13T01:31:43.851Z",
  "creationDate": "2021-12-13T01:31:43.851Z"
}
~~~
---
Json de atualização:
~~~json
{
  "id": 1,
  "name": "string2",
  "description": "string2",
  "categories": [
    "string2"
  ],
  "coverImgUrl": "string2",
  "slideImgUrl": [
    "string2"
  ],
  "releaseDate": "2021-12-14T01:31:43.851Z",
  "creationDate": "2021-12-14T01:31:43.851Z"
}
~~~
---
### Rotas
> https://localhost:5001/api/Game -- GET

> https://localhost:5001/api/Game/GetById/?id=578732 -- GET

> https://localhost:5001/api/Game -- POST

> https://localhost:5001/api/Game/Update -- PATCH

> https://localhost:5001/api/Game/Delete?id=578732 -- DELETE
---
### User (Usuário)
Propriedades:
| Nome da Propriedade | Tipo   | Descrição                            |  Necessidade  |
|---------------------|--------|--------------------------------------|---------------|
| Id                  |  int     | Identificador único                |   Opcional    |
| Name                |  string  | Nome do usuário                    |  Obrigatório  |
| Email               |  string  | E-mail de identificação do usuário |  Obrigatório  |
| Password            |  string  | Senha de acesso do usuário         |  Obrigatório  |
| CreationDate        | DateTime | Data de Cadastro do usuário        |   Opcional    |
---
Json de geração:
~~~json
{
  "id": 0,
  "name": "string",
  "email": "user@example.com",
  "password": "string",
  "creationDate": "2021-12-13T01:32:30.371Z"
}
~~~
---
Jsom de atualização
~~~json
{
  "id": 1,
  "name": "string2",
  "email": "user1@example.com",
  "password": "string2",
  "creationDate": "2022-12-13T01:32:30.371Z"
}
~~~
---
### Rotas
> https://localhost:5001/api/User/Login -- POST

> https://localhost:5001/api/User -- POST

> https://localhost:5001/api/User/GetByEmail?email=user@email.com -- GET

> https://localhost:5001/api/User/Update -- PATCH
---

# Funcionamento
O funcionamento da API é simples e direto. Há uma necessidade de se estar cadastrado no sistema para poder se utilizar dos métodos **POST, PATCH e DELETE**.
> Nota: Para fazer o cadastro de um usuário, deve-se utilizar o **Json de Geração** acima na rota *api/User* via método **POST**.

## Geração de Token
Para gerar um Token de acesso, você deve realizar o Login na rota *api/User/Login* via método **POST**. Se o login for realizado com sucesso, será retornado o token de acesso.
> Nota: Os método GET são todos públicos, em suma, não há necessidade de se estar logado para poder visualizar um título ou todos os títulos.

## Create, Update e Delete
Com o Token de acesso, você poderá criar, deletar e atualizar os jogos persistidos no sistema.

