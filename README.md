# Course Sales Site Microservices Project

This is a microservices project simulating a course sales site. It consists of 6 microservices: cart, catalog, discount, order, payment, and file/photo services. These microservices are the main services that perform the actual work.

In addition, I used Ocelot Gateway as a gateway. All microservices are protected with JWT tokens. I created a separate project for user operations. In this project, I used Identity and IdentityServer4. Users perform user operations in this project and obtain tokens to access other microservices.

I also designed a web app interface that will communicate with all these systems.

In the project, I used many technologies such as MongoDB, PostgreSQL, Docker, MSSQL, Ocelot, Identity, Domain Driven Design.

## Technologies Used

- .NET Core
- Docker
- Ocelot
- IdentityServer4
- MongoDB
- PostgreSQL
- MSSQL

## Microservices

- Cart Service
- Catalog Service
- Discount Service
- Order Service
- Payment Service
- File/Photo Service

## Architecture

This project follows the principles of Domain Driven Design (DDD) and uses the following architecture:

- API Gateway
- Identity Server
- Microservices

## Getting Started

To get started with the project, follow these steps:

1. Clone the repository.
2. Run `docker-compose up` command to start all microservices.
3. Start the User Management Service.
4. Start the web app.

## Conclusion

This project is designed to simulate a course sales site using microservices. It uses technologies such as .NET Core, Docker, and Ocelot to provide a scalable and reliable system.


#### Contact Addresses
##### Linkedin: [Send a message on Linkedin](https://www.linkedin.com/in/cihatsolak/)
##### Twitter: [Go to @cihattsolak](https://twitter.com/cihattsolak)
##### Medium: [Read from medium](https://cihatsolak.medium.com/)
