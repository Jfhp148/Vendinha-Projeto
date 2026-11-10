# Vendinha

API em **.NET 8 (C#)** para controlar as dívidas ("pendura") dos clientes de uma vendinha.
Permite cadastrar clientes, cadastrar dívidas para cada cliente e marcar as dívidas como pagas.

## Tecnologias

- C# / .NET 8 (ASP.NET Core Web API)
- Entity Framework Core
- SQLite
- Swagger (para testar a API pelo navegador)

## Como executar

### Pelo Visual Studio

1. Abra o arquivo `Vendinha.sln`.
2. Aguarde o Visual Studio restaurar os pacotes NuGet.
3. Aperte **F5** (ou o botão verde de executar).
4. O navegador vai abrir o Swagger em `http://localhost:5080/swagger`.

### Pelo terminal

```bash
dotnet run --project Vendinha.Api
```

Depois acesse `http://localhost:5080/swagger`.

> Requisito: ter o **.NET 8 SDK** (ou mais novo) instalado.

## Banco de dados

- Usa **SQLite**. O arquivo `vendinha.db` é criado automaticamente na primeira execução.
- O script SQL com a criação das tabelas está em `database/script.sql`.

## Regras do sistema

- Nome, CPF e data de nascimento são obrigatórios.
- O CPF precisa ser válido e não pode repetir entre clientes.
- O e-mail é opcional, mas se for informado precisa ser válido.
- A idade é calculada a partir da data de nascimento.
- Cada cliente só pode ter **uma dívida em aberto** por vez.
- A listagem de clientes é ordenada de quem mais deve para quem menos deve, paginada de 10 em 10, com busca por nome.

## Endpoints

| Método | Rota | O que faz |
|--------|------|-----------|
| GET | `/api/clientes?busca=&pagina=1` | Lista clientes (10 por página, busca por nome) |
| GET | `/api/clientes/{id}` | Busca um cliente |
| POST | `/api/clientes` | Cadastra um cliente |
| PUT | `/api/clientes/{id}` | Atualiza um cliente |
| DELETE | `/api/clientes/{id}` | Exclui um cliente (e as dívidas dele) |
| GET | `/api/clientes/{id}/dividas` | Lista as dívidas de um cliente |
| POST | `/api/clientes/{id}/dividas` | Cadastra uma dívida para o cliente |
| PUT | `/api/dividas/{id}/pagar` | Marca a dívida como paga |

## Exemplos

Cadastrar cliente (`POST /api/clientes`):

```json
{
  "nome": "Maria da Silva",
  "cpf": "529.982.247-25",
  "dataNascimento": "1990-05-20",
  "email": "maria@email.com"
}
```

Cadastrar dívida (`POST /api/clientes/1/dividas`):

```json
{
  "valor": 45.90
}
```

Resposta da listagem (`GET /api/clientes`):

```json
{
  "pagina": 1,
  "tamanhoPagina": 10,
  "totalClientes": 1,
  "totalPaginas": 1,
  "somaTotalDividas": 45.90,
  "clientes": [
    {
      "id": 1,
      "nome": "Maria da Silva",
      "cpf": "52998224725",
      "dataNascimento": "1990-05-20T00:00:00",
      "idade": 36,
      "email": "maria@email.com",
      "totalDividas": 45.90
    }
  ]
}
```

## Estrutura do projeto

```
Vendinha
├── Vendinha.sln
├── database/script.sql
└── Vendinha.Api
    ├── Controllers   -> recebem as requisições
    ├── Services      -> regras de negócio
    ├── Models        -> classes que viram tabelas (Cliente, Divida)
    ├── Dtos          -> dados que entram e saem da API
    ├── Data          -> contexto do Entity Framework
    ├── Utils         -> validadores de CPF e e-mail
    ├── Excecoes      -> erros de regra de negócio
    ├── Middlewares   -> tratamento dos erros
    └── Program.cs
```
