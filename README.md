
# TeraFrontendMVC

This project was created with the goal of learning Razor. Through it, I explored various concepts and implementations, including partial views, dynamic page rendering, layout management, and data binding. Additionally, I focused on maintaining best practices throughout the development process.

The project focused on the 2024 elections held in Venezuela.



## Requirements

* .NET 8.0 SDK
* Docker
* Any compatible IDE (e.g., Visual Studio, Visual Studio Code)

## Setup

#### Clone the Repository

 _git clone https://github.com/ron2702/TeraFrontendMVC.git_ 

### Run Docker

Make sure that docker is running, then run this command in the folder where the docker-compose.yml is located 

_docker-compose up --build_

_Note: that command builds and starts all services defined in the Docker Compose file_

Once the container runs correctly you will be able to access the following: 

#### Dashboard

http://localhost:8081/

On the page you will find some selections as well as tables, in the selections you can search the results by state, municipality and parish.

One of the tables will show the detailed results by table, candidates, etc. You can change the size of the rows to be displayed.

The second table will show the total results per candidate.


