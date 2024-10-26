# APS-NET-Minimals
Neste projeto, desenvolvemos uma API minimalista para gerenciar o cadastro de veículos, utilizando a abordagem de Minimal APIs no .NET. A API inclui funcionalidades de autenticação com JWT, organização de código seguindo a Arquitetura em Camadas (ou Arquitetura de Cebola) e persistência de dados em um banco de dados SQL Server através do Entity Framework Core. Aqui estão os pontos principais abordados durante o desenvolvimento:

## 1. Organização do Projeto
Estruturamos o projeto em pastas, separando as responsabilidades em camadas, como Domínio (definição de entidades e interfaces), Infraestrutura (repositórios e contexto de banco de dados), Serviços (autenticação e regras de negócio) e DTOs (objetos de transferência de dados). Essa separação facilita a manutenção e ampliação do projeto, tornando-o mais organizado e seguindo as melhores práticas de arquitetura.
## 2. Autenticação com JWT
Implementamos autenticação utilizando JSON Web Token (JWT) para assegurar que apenas usuários autorizados possam acessar os endpoints de CRUD. Criamos uma rota de login (/login) que valida as credenciais e, se válidas, gera um token JWT que é retornado ao usuário. Esse token é usado em chamadas subsequentes para autenticar e autorizar o acesso aos recursos protegidos.
A configuração do JWT foi realizada utilizando o IConfiguration para ler as informações do arquivo appsettings.json, como a chave secreta e parâmetros do token (Issuer e Audience), e foram configuradas políticas de autenticação e autorização no Program.cs.
## 3. CRUD para Veículos
Implementamos operações CRUD (Create, Read, Update, Delete) para a entidade Veículo, com endpoints protegidos que exigem autenticação JWT. Cada endpoint mapeia uma operação de repositório correspondente, permitindo a manipulação dos dados de veículos de forma simplificada e segura.
A camada de repositórios foi criada para interagir com o banco de dados, delegando as operações de persistência ao Entity Framework Core. Essa abordagem desacopla a lógica de acesso ao banco de dados da lógica de negócios, facilitando a manutenção.
## 4. Uso do Entity Framework Core e SQL Server
O Entity Framework Core foi utilizado para mapear a entidade Veículo e realizar as operações de persistência no SQL Server. Configuramos o contexto do banco (DbContexto) com uma DbSet para a entidade Veiculo e configuramos a string de conexão no appsettings.json.
Além disso, aproveitamos o poder das migrations do Entity Framework para garantir que o banco de dados estivesse atualizado com as últimas mudanças no modelo de dados.
## 5. Configuração e Teste com Swagger
Adicionamos o Swagger para gerar automaticamente a documentação da API, permitindo que as rotas e operações CRUD sejam facilmente testadas e visualizadas. O Swagger simplifica o processo de teste e validação da API, além de servir como documentação para outros desenvolvedores que irão consumir o serviço.
## 6. Tratamento de Nullability e Boa Prática de Código
Durante o desenvolvimento, foi necessário lidar com verificações de nulidade, especialmente nas configurações do JWT. Adotamos o operador ?? para definir valores padrão e prevenir possíveis exceções de referência nula. Esse cuidado aumenta a robustez do código e previne erros comuns que poderiam ocorrer em produção.
Conclusão
O projeto de API minimalista foi desenvolvido com uma arquitetura modular e organizada, seguindo boas práticas e padrões recomendados. A implementação de autenticação JWT e proteção dos endpoints de CRUD garante segurança, enquanto o uso do Entity Framework Core e SQL Server oferece persistência de dados de maneira eficiente. O uso do Swagger facilitou o teste e validação, tornando o projeto uma base sólida para uma aplicação de cadastro de veículos escalável e segura.
