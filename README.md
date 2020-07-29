# Instruções

## Preparando ambiente

Sistema Operacional: Windows 10

IDE:

- Visual Studio (Community, Pro ou Enterprise) 2019 - versão 16.6.3 (Instalar o módulo "ASP .NET and Web Development)
- Visual Studio Code - versão 1.47.3
  - Extensões que usei:
    - ESLint 2.1.8
    - Auto Import - ES6, TS, JSX, TSX 1.4.3
    - Visual Studio IntelliCode 1.2.9
    - Simple React Snippets 1.2.3

SDK:

- .NET Core - versão 3.1.301

Instalar:

- Node.js - versão 12.18.3
- npm - versão 6.14.6

Banco de Dados:

- Criar uma instância do PostgreSQL (eu usei a versão 11, mas pode usar também a 12)

## Realizar a build dos Projetos e Executar

1. Clonar o repositório https://github.com/miguelkmarques/Programmingchallange2020.git.

2. O Projeto tem 4 Pastas:

   - Api com o Projeto da API Rest em Asp .NET Core.
   - Database com o Projeto do banco de dados com os Models, DbContext e Migrations usando o Entity Framework.
   - SeedMovieLens com o Projeto de um App Console para aplicar as Migrations no banco de dados e carregar os dados dos arquivos .csv.
   - web-client com o react-app, o client web escrito em React para consumir a API Rest.

3. Copiar os arquivos .csv e colar na raiz da pasta "SeedMovieLens\ml-25m".

4. Nos Projetos SeedMovieLens e Api, editar o arquivo "appsettings.json" para alterar a ConnectionString, caso algum parâmetro na sua instância do PostgreSQL seja diferente (ex: nome do Server, o nome do Database caso já exista um banco com o nome "movielens", porta e usuário e senha do banco).

5. Abrir o arquivo "Programmingchallange2020.sln" no Visual Studio 2019.

6. Apertar o botão F6 ou a opção Build Solution na Aba Build.

7. Clicar com o botão direito no Projeto SeedMovieLens e clicar em "Set as Startup Project".

8. Apertar F5 para iniciar no modo de Debugging ou Ctrl+F5 para iniciar sem o Debugging e aguardar o programa finalizar em carregar todos os dados no banco de dados (Pode demorar uns 30 minutos).

9. Após aparecer no log ou no console do Programa a mensagem "end", pode fechar.

10. Abrir a Pasta "web-client" no Visual Studio Code.

11. Abrir o Terminal com Ctrl+` ou na opção na aba View.

12. Executar o comando `npm i` no terminal para instalar todas as dependências do web-client.

13. Executar o comando `npm start` no terminal para iniciar a aplicação Web, ela deve rodar na porta 3000. Caso a porta 3000 já esteja em uso por outra aplicação, favor finalizar, pois o Projeto da API usa essa porta como proxy em ambiente de desenvolvimento.

14. Após isso o seu navegador deve abrir uma aba na página http://localhost:3000/, porém pode fechar esta aba pois não usaremos o Client nesta porta.

15. No Visual Studio 2019, clicar com o botão direito no Projeto Api e em "Set as Startup Project".

16. Apertar F5 para iniciar no modo de Debugging ou Ctrl+F5 para iniciar sem o Debugging.

17. Após isso o seu navegador deve abrir uma aba na página https://localhost:44316/.

18. O Client é bem simples, tem um formulário com os campos de Filtro e uma tabela onde lista os Filmes.

19. Inserir um ano para filtrar por Ano, selecionar um Gênero para filtrar por Gênero e inserir um valor númerico para filtrar os Top K Filmes baseado na avaliação média dos usuários. Após definir o filtro, apertar no botão "Apply filter".

20. Todos os filtros podem ser aplicados simultaneamente e em qualquer combinação de filtros.

# Tecnologias e Decisões no meio do Desafio

- **Asp .Net Core**: É o Framework Web que estou mais familiarizado e que tenho mais expêriencia. Tem uma documentação muito rica e é um framework robusto e com boa performance, que pode ser aplicado de diversas maneiras diferentes de acordo com a necessidade.

- **Entity Framework Core**: É um Framework ORM muito bom e que facilita no desenvolvimento e no uso de um banco de dados, independente de qual provider seja usado (MSSQL, Postgre, MySQL, Oracle), onde é possível fazer uso do LINQ para gerar automaticamente scripts SQL, e também permite manter um histórico de alterações feitas no banco de dados. Além disso tem mecanismos de proteção contra SQL Injection.

- **OData**: OData facilita bastante para criar APIs Rest ricas com diversas opções, e que podem traduzir direto para uma script SQL junto com o Entity Framework. Onde é aplicado uma série de melhores práticas para consumir uma API com segurança.

- **React.js**: É uma biblioteca para criação de aplicações Web e também para aplicativos mobile. Onde é possível fazer um SPA ou PWA para melhor performance e que tem diversas ferramentas para criar desde projetos simples, quanto projetos complexos. O bom do React é a criação dos Components que são reutilizáveis, assim evita a repetição de código e agiliza bastante o desenvolvimento de novas telas e funcionalidades, além de deixar o código mais limpo, legível e aplicar manuntenção mais facilmente. React.js é usado nos sites do Facebook, Instagram, Twitter e muitos outros.

- **joi-browser**: É uma biblioteca JavaScript para aplicar validação automática de objetos de acordo com um Schema e que usei para o formulário de filtro para impedir que o usuário informe valores inválidos.

- **bootstrap**: Agiliza bastante para aplicar estilo na aplicação web. Uso bastante pois não tenho muita experiência em design web para fazer algo mais customizado.

- **PostgreSQL**: Eu inicialmente tinha começado usando o mssqllocaldb, porém estava com performance bem ruim para inserir no banco de dados, estava demorando muito para inserir por exemplo a tabela Ratings. Aí resolvi mudar para usar Postgre e inseriu os dados 5 vezes mais rápido. É um DBMS muito bom e de graça, que tenho usado bastante em projetos pessoais. Escolhi usar um banco relacional pois não tenho muita experiência com banco NoSQL.

- **Dificuldades**:

  - A maior dificuldade que tive no projeto foi carregar o arquivo .csv, principalmente os que tinham milhões de linhas. De início eu tinha feito um código que armazenava tudo em memória para aí sim inserir no banco de dados, mas isso estava ocupando muita memória e processamento. Até que fiz um código onde ele não enumera todas as linhas para a memória e ele insere no banco de dados de 1000 em 1000 linhas em vez de inserir linha por linha, para ser mais rápido. Além disso no DbContext do EF eu mudo o State das Entries do Context que foram inseridas, para parar o Tracking e não ficar acumulando na memória após realizar a inserção das 1000 linhas. E para melhorar ainda mais a performance, para os arquivos com o volume de dados muito grande eu fiz um algoritmo para dividir a tarefa em N Threads, cada Thread fica responsável de carregar determinada parte do arquivo .csv, e cada Thread trabalha independente do outro, assim carregando com mais eficácia os arquivos pro banco de dados, sem extrapolar o uso da memória do computador.

  - A outra dificuldade que tive foi na parte de como aplicar o filtro por gênero, pois do jeito que modelei o banco de dados, um filme tem N gêneros, onde é armazenado em outra tabela, em uma relação de 1 para N. E usando o OData com o Entity Framework não foi possível que o Entity Framework conseguisse gerar automaticamente um script SQL para poder aplicar o filtro. A solução mais fácil seria então enumerar em memória a lista de filmes e aí sim o LINQ conseguiria aplicar o filtro corretamente, porém a desvantagem era, que isso tava ocupando muita memória pois teria que carregar todos os filmes primeiro para uma lista, antes de aplicar o filtro, e pelos testes que realizei estava demorando em média 400ms para a API me retornar com o resultado. A solução que achei, que roda o Script SQL direto no banco de dados e sem ocupar memória, foi criar um parâmetro separado do OData e rodar um Script SQL que escrevi para poder filtrar por gênero, após os testes a API me respondia em 10ms, performance muito melhor.

# Considerações

No final, acredito que consegui obter o resultado que queria alcançar, queria uma aplicação que não apenas funcionasse, mas que tivesse boa performance e controlasse o uso da memória. São pontos muito importantes, fazer bom uso dos recursos do computador, seja do servidor que esteja rodando a API ou o usuário usando o Client da aplicação. Me desafiou no quesito do volume de dados dos arquivos csv, que ainda não tinha trabalhado com um arquivo grande assim, porém já trabalhei com tabelas grandes, porém fazendo Interfaces de banco para banco. Foi um ótimo desafio e obrigado pela oportunidade!

# Programming Challenge

Congratulations on being selected to participate in our technical test. It consists of a programming challenge and it will address different skills. Read the instructions carefully and we wish you the best of luck.

## Before You Start

Fork this repository and once you have finished your challenge, grant access to the Github user "kavlac". Upload all your deliverables to your forked repository. We will use it to evaluate your test.

## Introduction

We want you to develop a project that makes uses of the [MovieLens](https://grouplens.org/datasets/movielens/) dataset. It consists of three goals and the details on each one of them is given below.

## Preparing the Data

The first goal of this challenge is to obtain and prepare the data you will work with.

In order to do so, you must download a [publicly available dataset](http://files.grouplens.org/datasets/movielens/ml-25m.zip). You can find the details about what data is stored and how it is structured in the [instructions](http://files.grouplens.org/datasets/movielens/ml-25m-README.html).

Then, you are asked to write a program to read the input files for the dataset and create a database out of it. You can choose to use the database in memory, in files, or in a database management system, as long as you process and consume this data in the upcoming parts.

## Making the Data Available

The second goal of this challenge is to make the processed data available for consumption.

To do such, you must implement a REST API and it should provide the following methods:

- List movies by year: given a year, we want to know what movies match the given year;
- List movies by year and genre: given a year and a genre, we want to know what movies match the given year and are of the given genre;
- List top K rated movies: given a number K, we want to know the best K rated movies in descending order.

## Consuming the Data

The third goal of this challenge is to consume the methods of the REST API.

Thus, you are asked to implement a client application that accesses such an API. It must have a graphical interface to interact with users to consume the three methods above. It is up to you how to design the user interface, as long as it is usable.

## Deliverables

You must provide the following artifacts:

- The source-code of the programs that you implemented;
- A set of instructions on how to prepare the environment, build the programs, run each part of the challenge, and how to use your project;
- Comments on what technologies and patterns you used and the reasons to do so, as well as the decisions you made throughout the challenge;
- Any other artifact you find relevant for your overall evaluation.

## Tips

- Make sure your instructions are easy to follow and that each step works as expected;
- Our main environment is Windows, so please make sure that your solution works on it;
- If you want, you can show us how you can set up your project using Docker;
- We suggest you implement the challenge using the following languages (you can use more than one of them if you want): C#, Java, and/or JavaScript;
- Testing is more than welcome;
- Show us everything you know about best practices in Git;
- Think carefully about your code quality, in terms of maintainability, readability, and simplicity;
- Do not overengineer your solution.
