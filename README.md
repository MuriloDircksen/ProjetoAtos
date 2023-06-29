# Cervejaria

Este projeto foi desenvolvido para simular um sistema de controle de estoque e produção em uma cervejaria, tornando de fácil acesso a essas informações. 
Na aplicação é possível acompanhar as informações de forma simples e rápida, podendo inserir, alterar e deletar dados conforme a necessidade da empresa.
Para o desenvolvimento dessa aplicação, foram criados um sistema front-end através da IDE VScode usando Angular CLI, que consome uma web API criado através da IDE Visual Studio usando C# .Net, com entity framework.

## Ferramentas e requisitos necessárias para rodar o projeto
- Visual Studio 22
- Sql server express 19
- net 7.0
- Pacote AutoMapper 12.0.1
- Pacote Microsoft.AspNetCore.Mvc.NewtonsoftJson 7.0.5
- Pacote Microsoft.AspNetCore.OpenApi 7.0.5
- Pacote Microsoft.EntityFrameworkCore 7.0.5
- Pacote Microsoft.EntityFrameworkCore.Design 7.0.5
- Pacote Microsoft.EntityFrameworkCore.SqlServer 7.0.5
- Pacote Microsoft.EntityFrameworkCore.Tools 7.0.5
- Pacote Microsoft.AspNetCore.Authentication.JwtBearer 7.0.0
- VSCode
- Angular CLI 15.1.6 e suas dependências
- Bootstrap 5.3.0

### Como rodar a Aplicação

1. Iniciar suas IDE's para o front-end e back-end com seus respectivos códigos
2. Iniciar o sistema sql server com a base de dados do sistema
3. Conectar o banco de dados
4. Rodar a aplicação no visual studio 
5. Na pasta app no VS code, rodar o comando ng serve no prompt para carregar aplicação
6. Após carregar o projeto, acessar http://localhost:4200/login para começar a navegar


### Modelo JSON de entrada
```json
 "usuarios": [
    {
      "id": 1,
      "nome": "admin",
      "senha": "root1234",
      "nomeEmpresa": "admin",
      "cnpj": "11111111000111",
      "email": "admin@admin.com"
    }
]
"receitas": [
    {
      "id": 1,
      "nomeReceita": "Pilsen",
      "responsavelReceita": "Murilo Dircksen",
      "estilo": "American Lager",
      "ultima_atualizacao": "2023-06-16T14:02:10.261Z",
      "orcamento": 2985,
      "volumeReceita": 2000
      "Producao":{
                    "id": 1,
                    "quantidade": 2000,
                    "receitaId": 1,
                    "responsavel": "Murilo",
                    "dataProducao": "2023/03/2"
                 }
      "ReceitaIngredientes": {
                                "id": 3,
                                "receitaId": 1,
                                "ingredienteId": 1,
                                "quantidadeIngrediente": 700
                              }
    }
  ]
"estoques": [
    {
      "id": 1,
      "nome": "Malte "
      "ingredientes": {
                        "id": 1,
                        "nomeIngrediente": "Malte Pilsen",
                        "idEstoque": 1,
                        "quantidade": 9300,
                        "tipo": "Malte",
                        "valor_unidade": 2.5,
                        "valorTotal": 25000,
                        "unidade": "kg",
                        "fornecedor": "agraria",
                        "validade": "04/2024",
                        "data_entrada": "2023/04/20"
                        "ReceitaIngredientes": {
                                                  "id": 3,
                                                  "receitaId": 1,
                                                  "ingredienteId": 1,
                                                  "quantidadeIngrediente": 700
                                                }
                      }
    }
}


```
### Diagrama Relacional de Entidades
![alt text](https://github.com/MuriloDircksen/ProjetoAtos/blob/main/Cervejaria-front/src/assets/diagrama%20relacional.PNG)

### Documentação Web API

  Para melhor entendimento do funcionamento da API criado nesse projeto, foi criado um arquivo xml descrevendo cada end point.
[Diagrama xml](https://github.com/MuriloDircksen/ProjetoAtos/blob/main/Cervejaria/Cervejaria.xml)

### Tipos de dados

- id -> identificador único criado automaticamente
- nome -> String de preenchimento obrigatório com minimo de 10 e máximo de 200 caracteres
- senha -> String de preenchimento obrigatório com minimo de 8 caracteres
- NomeEmpresa -> String de preenchimento obrigatório
- Cnpj -> String de preenchimento obrigatório de 14 caracteres
- Email -> String de preenchimento obrigatório to tipo email
- NomeReceita -> String de preenchimento obrigatório com minimo de 10 e máximo de 200 caracteres
- Responsavel -> String de preenchimento obrigatório com minimo de 10 e máximo de 200 caracteres
- Estilo -> String de preenchimento obrigatório com minimo de 3 e máximo de 80 caracteres
- UltimaAtualizacao -> DateTime de preenchimento obrigatório gerado automaticamente pelo sistema
- Orcamento -> Number de preenchimento obrigatório gerado automaticamente pelo sistema
- VolumeReceita -> Number de preenchimento obrigatório
- ReceitaId -> Inteiro de preenchimento obrigatório
- VolumeApronte -> Number de preenchimento obrigatório
- DataProducao -> DateTime de preenchimento obrigatório gerado automaticamente pelo sistema
- IdReceita -> Inteiro de preenchimento obrigatório
- IdIngrediente -> Inteiro de preenchimento obrigatório
- QuantidadeDeIngrediente -> Number de preenchimento obrigatório
- NomeIngrediente -> String de preenchimento obrigatório com minimo de 5 e máximo de 200 caracteres
- IdEstoque -> Inteiro de preenchimento obrigatório
- Quantidade -> Number de preenchimento obrigatório
- ValorUnidade -> Number de preenchimento obrigatório
- ValorTotal -> Number de preenchimento obrigatório
- Unidade -> String de preenchimento obrigatório
- Tipo -> String de preenchimento obrigatório com minimo de 3 e máximo de 50 caracteres
- Fornecedor -> String de preenchimento obrigatório com minimo de 3 e máximo de 80 caracteres
- Validade -> DateTime de preenchimento obrigatório
- DataEntrada -> DateTime de preenchimento obrigatório gerado automaticamente pelo sistema
- NomeEstoque -> String de preenchimento obrigatório com minimo de 3 e máximo de 60 caracteres


### Autor

Murilo Dircksen
https://www.linkedin.com/in/murilodircksen/
