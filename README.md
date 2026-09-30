# Swimming Contest

A full-stack **Swimming Competition Management System** implemented in **two technology stacks: Java and C#/.NET**. The application was designed as a multi-client competition management platform for handling swimmers, competition events, registrations, and real-time communication between clients and the server.

The project focuses on backend development, client-server architecture, database persistence, network communication, data serialization, and real-time synchronization.

Two implementations of the application were developed during the project:

* a **Java-based implementation**, focused on object-oriented programming, client-server communication, persistence, and application architecture;
* a **C# / ASP.NET Core implementation**, extending the system with modern backend technologies, Entity Framework Core, multiple serialization protocols, and WebSocket communication.

The project was developed as a university project at **Babeș-Bolyai University**.

## My Role

Contributed as **Backend Developer**, responsible for implementing and integrating the main functionality of the competition management system.

My work included:

* Designing and implementing the application domain model
* Developing the **Java version** of the Swimming Contest application
* Developing the **C# / ASP.NET Core version** of the application
* Implementing CRUD operations for competition entities
* Designing and working with the persistence layer
* Implementing client-server communication
* Working with relational databases
* Implementing different data serialization approaches
* Working with **JSON, Binary serialization, and Google Protocol Buffers**
* Implementing **WebSocket communication** for real-time updates
* Testing communication between multiple clients and the server
* Debugging backend, database, and network-related issues
* Building and maintaining the projects using the appropriate build systems

## About

The Swimming Contest application provides a centralized system for managing swimming competitions.

The system allows competition organizers to manage information about:

* swimmers / participants;
* swimming events;
* registrations;
* competition data;
* client-server communication;
* real-time updates.

The project was developed in two different implementations in order to explore and apply similar application requirements using different programming languages and technology ecosystems.

### Java Implementation

The Java implementation focused on applying core software engineering and object-oriented programming concepts.

The application was structured around a client-server architecture, with communication between the application components and persistent storage for competition-related data.

This version provided practical experience with:

* Java;
* object-oriented programming;
* client-server communication;
* data persistence;
* CRUD operations;
* application architecture;
* database interaction.

### C# / ASP.NET Core Implementation

The C# implementation introduced a modern .NET-based backend using **ASP.NET Core**.

This version expanded the communication layer by supporting multiple serialization formats and real-time communication through WebSockets.

The implementation uses:

* C#;
* ASP.NET Core;
* Entity Framework Core;
* JSON serialization;
* Binary serialization;
* Google Protocol Buffers;
* WebSockets;
* MSBuild.

## Main Objectives

The main objectives of the project were:

* Develop a complete swimming competition management system
* Implement the same application domain using **Java and C#/.NET**
* Apply object-oriented programming principles
* Build a client-server architecture
* Implement CRUD operations
* Persist competition data in a relational database
* Implement multiple serialization formats
* Explore different approaches to network communication
* Implement real-time communication using WebSockets
* Support multiple clients connected to the same backend
* Gain practical experience with different backend ecosystems

## System Architecture

The application follows a **multi-client client-server architecture**.

```text
                       ┌─────────────────┐
                       │    Client 1     │
                       └────────┬────────┘
                                │
                       ┌────────▼────────┐
                       │                 │
                       │      Server     │
                       │                 │
                       │ Business Logic  │
                       │ CRUD Operations │
                       │ Communication   │
                       │                 │
                       └────────┬────────┘
                                │
               ┌────────────────┼────────────────┐
               │                │                │
               ▼                ▼                ▼
          Serialization      WebSockets      Database
               │                                 │
       ┌───────┼───────┐                         │
       │       │       │                         │
      JSON   Binary  Protobuf                    │
               │                                 │
               └─────────────────────────────────┘
                               
                       ┌─────────────────┐
                       │    Client 2     │
                       └─────────────────┘
```

The architecture allows multiple clients to interact with the same central server.

The server is responsible for:

* processing client requests;
* executing business logic;
* managing competition data;
* communicating with the database;
* serializing and deserializing data;
* sending responses;
* notifying connected clients about relevant changes.

## Java Architecture

The Java implementation follows the same general client-server concept.

```text
┌──────────────┐
│ Java Client  │
└──────┬───────┘
       │
       │ Client-Server Communication
       ▼
┌──────────────┐
│ Java Server  │
│              │
│ Business     │
│ Logic        │
│ CRUD         │
└──────┬───────┘
       │
       ▼
┌──────────────┐
│   Database   │
└──────────────┘
```

This implementation provided the foundation for understanding the domain model and the communication between application components.

## C# / ASP.NET Core Architecture

The .NET version uses ASP.NET Core as the backend framework.

```text
┌──────────────────┐
│      Client      │
└────────┬─────────┘
         │
         ▼
┌──────────────────────────┐
│      ASP.NET Core        │
│          Server          │
│                          │
│ Controllers / Services   │
│ CRUD Operations          │
│ WebSocket Communication  │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│    Entity Framework      │
│           Core           │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│        Database          │
└──────────────────────────┘
```

## Core Functionality

### 👥 Participant Management

The application provides functionality for managing swimming participants.

Operations include:

* creating participants;
* retrieving participant information;
* updating participant information;
* deleting participants;
* registering participants for swimming events.

### 🏊 Competition Events

Competition events represent the different swimming races available in the system.

The backend provides functionality for:

* creating events;
* retrieving events;
* modifying event information;
* deleting events;
* associating participants with events.

### 📝 Registrations

Participants can register for available competition events.

The server validates and processes registration operations before storing the resulting information in the database.

### 🔄 CRUD Operations

The main entities are managed using standard CRUD operations:

```text
Create
  ↓
Read
  ↓
Update
  ↓
Delete
```

These operations provide the foundation of the competition management system.

## Data Serialization

A major component of the C# implementation is the support for multiple serialization formats.

### JSON

JSON provides a human-readable representation of application data.

```json
{
    "id": 1,
    "name": "100m Freestyle",
    "distance": 100,
    "style": "Freestyle"
}
```

JSON is particularly useful for interoperability and debugging because the serialized data can easily be inspected.

### Binary Serialization

The application also supports binary serialization.

```text
Object
   ↓
Serialization
   ↓
Binary representation
   ↓
Network
   ↓
Deserialization
   ↓
Object
```

This approach demonstrates how application objects can be transferred in a compact binary representation rather than as human-readable text.

### Google Protocol Buffers

The project also uses **Google Protocol Buffers (Protobuf)** for structured serialization.

```text
Application Object
       ↓
Protobuf Message
       ↓
Serialization
       ↓
Network
       ↓
Deserialization
       ↓
Application Object
```

Protobuf uses a predefined schema for representing application data and provides a structured approach to communication between components.

## WebSocket Communication

The C# / ASP.NET Core implementation uses **WebSockets** to support persistent, real-time communication between the server and clients.

Instead of requiring clients to continuously request updated information, the server can send notifications when relevant data changes.

```text
Client A
   │
   │ WebSocket
   ▼
Server
   │
   │ Notification
   ├──────────────► Client B
   │
   └──────────────► Client C
```

This mechanism is useful in a competition environment where multiple clients may need to observe changes to shared competition data.

## Database and Persistence

The application uses a relational database to persist competition information.

The C# implementation uses **Entity Framework Core** as the Object-Relational Mapping layer.

```text
C# Entity
    ↓
Entity Framework Core
    ↓
Relational Database
```

EF Core maps C# entities to database records and provides the functionality required to create, read, update, and delete persistent data.

The Java implementation also works with persistent competition data and provides practical experience with database interaction from a Java application.

## Java vs C# Implementation

One of the main aspects of this project was implementing the application using two different technology ecosystems.

| Aspect                  | Java Implementation                | C# Implementation                               |
| ----------------------- | ---------------------------------- | ----------------------------------------------- |
| Language                | Java                               | C#                                              |
| Backend                 | Java-based server                  | ASP.NET Core                                    |
| Architecture            | Client-Server                      | Multi-client Client-Server                      |
| Database                | Relational database                | Relational database                             |
| Persistence             | Database layer                     | Entity Framework Core                           |
| Communication           | Client-Server communication        | ASP.NET Core / WebSockets                       |
| Serialization           | Application-specific communication | JSON / Binary / Protobuf                        |
| Real-time communication | Server communication               | WebSockets                                      |
| Build                   | Java build tooling                 | MSBuild                                         |
| Main focus              | OOP, architecture, persistence     | Backend, serialization, real-time communication |

Developing both versions helped demonstrate how the same application domain can be implemented using different programming languages, frameworks, and communication technologies.

## Project Structure

A simplified representation of the project is:

```text
SwimmingContest/
│
├── Java/
│   ├── Client/
│   ├── Server/
│   ├── Model/
│   ├── Services/
│   └── ...
│
├── CSharp/
│   ├── Server/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── Data/
│   │   ├── WebSockets/
│   │   └── Program.cs
│   │
│   ├── Client/
│   └── ...
│
├── Protobuf/
│   └── *.proto
│
├── Database/
│
└── README.md
```

The exact structure depends on the project configuration, but the implementations separate domain models, communication, persistence, and client/server functionality.

## Tech Stack

### Java Version

* **Language:** Java
* **Architecture:** Client-Server
* **Database:** Relational Database
* **Concepts:** OOP, CRUD, persistence, networking
* **Build:** Java build tooling

### C# Version

* **Language:** C#
* **Framework:** ASP.NET Core
* **ORM:** Entity Framework Core
* **Communication:** WebSockets
* **Serialization:** JSON
* **Serialization:** Binary
* **Serialization:** Google Protocol Buffers
* **Build:** MSBuild
* **Architecture:** Multi-client Client-Server

### Common Technologies

* Git
* GitHub
* Relational Databases
* Client-Server Architecture
* CRUD
* Object-Oriented Programming

## Development Workflow

The project was developed progressively:

```text
Domain Analysis
      ↓
Entity & Model Design
      ↓
Java Implementation
      ↓
Database Integration
      ↓
CRUD Functionality
      ↓
C# / ASP.NET Core Implementation
      ↓
Entity Framework Core
      ↓
Serialization
      ↓
WebSockets
      ↓
Multi-Client Communication
      ↓
Testing & Debugging
```

The two implementations provided an opportunity to apply the same functional requirements in different technological environments.

## Testing and Debugging

Testing focused on verifying both the application functionality and communication between components.

The main areas tested included:

* participant CRUD operations;
* event CRUD operations;
* participant registrations;
* database persistence;
* Java client-server communication;
* C# client-server communication;
* JSON serialization and deserialization;
* Binary serialization and deserialization;
* Protobuf serialization and deserialization;
* WebSocket connections;
* communication between multiple clients;
* synchronization of shared competition data.

Debugging involved tracing operations through the different layers of the application, from the client to the server, database, and communication components.

## How to Run

### Prerequisites

For the Java implementation:

* JDK
* Java-compatible IDE
* Required database configuration
* Git

For the C# implementation:

* .NET SDK
* Visual Studio or another C# IDE
* Git
* Required database configuration

### Clone the Repository

```bash
git clone <YOUR_REPOSITORY_URL>
cd SwimmingContest
```

### Run the Java Version

Open the Java project in the preferred IDE and:

1. Configure the database connection.
2. Build the project.
3. Start the Java server.
4. Start one or more Java clients.
5. Test competition management and client-server communication.

### Run the C# Version

Build the project using:

```bash
dotnet build
```

Run the ASP.NET Core server:

```bash
dotnet run
```

Then start one or more configured clients.

Multiple clients can be launched simultaneously to test the multi-client and real-time communication functionality.

## Example Application Workflow

```text
Start Server
     ↓
Connect Clients
     ↓
Load Competition Data
     ↓
Create Swimming Events
     ↓
Register Participants
     ↓
Persist Data
     ↓
Notify Connected Clients
     ↓
Update Client Data
```

The server acts as the central point responsible for processing operations and maintaining consistent competition information.

## Technical Concepts Demonstrated

This project combines several important software engineering concepts.

### Object-Oriented Programming

Both implementations use object-oriented programming to model competition entities and application behavior.

### Client-Server Architecture

Clients communicate with a centralized backend responsible for business logic and persistence.

### Database Management

Competition entities are persisted in a relational database and accessed through application-specific persistence layers.

### Serialization

The project explores several ways of representing data for network communication:

* JSON;
* Binary;
* Protocol Buffers.

### Real-Time Communication

WebSockets provide persistent connections between clients and the server, allowing information to be propagated without requiring continuous polling.

### Multi-Client Systems

The architecture allows multiple clients to communicate with the same backend and work with shared competition data.

## What I Learned

The Swimming Contest project provided practical experience with both **Java and C#/.NET backend development**.

Through this project, I gained experience with:

* Java application development;
* C# development;
* ASP.NET Core;
* object-oriented programming;
* client-server architectures;
* multi-client systems;
* relational databases;
* Entity Framework Core;
* CRUD operations;
* JSON serialization;
* Binary serialization;
* Google Protocol Buffers;
* WebSockets;
* real-time communication;
* backend architecture;
* database persistence;
* network communication;
* debugging distributed application components;
* Git and collaborative development.

Implementing the same application in two technology stacks also provided a practical comparison between the Java and .NET ecosystems and helped reinforce the importance of separating business logic, persistence, communication, and presentation concerns.

## Future Development

Potential future improvements include:

* authentication and authorization;
* administrator and participant roles;
* competition scheduling;
* automatic race result calculation;
* live race timing;
* participant statistics;
* competition dashboards;
* advanced real-time synchronization;
* automated unit and integration testing;
* Docker-based deployment;
* cloud deployment;
* improved error handling and validation;
* automated CI/CD pipelines.

## Academic Context

**Swimming Contest — Competition Management System** was developed as a university project at **Babeș-Bolyai University**.

The project combines concepts from:

* backend development;
* object-oriented programming;
* distributed systems;
* database management;
* network communication;
* serialization;
* real-time applications;
* multi-client architectures.

A key aspect of the project was the implementation of the same competition-management domain using **both Java and C#/.NET**, providing hands-on experience with different programming languages, backend frameworks, persistence technologies, and communication mechanisms.
