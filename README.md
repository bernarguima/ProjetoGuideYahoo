Projeto Guide Yahoo Financial - Service

Criação Banco de dados SQL Server:

Dentro da pasta SQL estão todos os scripts que devem ser executados no banco.

- Passo 1: Execute o script no arquivo 01- Script Create DataBase.txt

- Passo 2: Execute o script no arquivo 02- Script Create Table.txt

- Passo 3: Execute o script no arquivo 03- Script Create Procedures.txt

Alteração Instância da conexão com o banco de dados:

Abra o arquivo appSettings.json na raiz do projeto API, o valor localhost\\SQLEXPRESS02, pela instância do seu banco SQL Server

Execução do Projeto:

Ao executar o projeto, o swagger apresentará os métodos da API:

Dados via Serviço:
- Passo 1: Execute o serviço api/GetQuoteYahoo, responsável por conectar na API do Yahoo Financial, buscar as cotações e gravar no banco de dados.
- Passo 2: Execute o serviço api/GetQuoteService, responsável por recuperar as informações do banco de dados com as informações de cotações e variações.

Dados via Download CSV:
- Passo 1: Execute o serviço api/GetQuoteCsvYahoo, reponsável por fazer o download do arquivo Csv com as cotações, e gravar os dados no banco de dados.
- Passo 2: Execute o serviço api/GetQuoteServiceCsv, responsável por recuperar as informações do banco de dados com as informações de cotações e variações, que foram gravas anterioemente pela leitura do arquivo CSV.


