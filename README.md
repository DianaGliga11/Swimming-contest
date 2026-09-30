# Swimming Contest

A full-stack **Swimming Competition Management System** designed to manage swimmers, swimming events, registrations, and competition-related data through a multi-client architecture.

The project was developed using **Java** and extended with a **Flutter/Dart client**, allowing the application to be accessed through different client implementations while communicating with the same backend infrastructure.

The system combines **client-server communication, REST services, WebSockets, multiple serialization protocols, database persistence, and layered architecture** to provide a complete competition management solution.

---

### IMPORTANT : in every branch there are screenshots with the progress.
## My Role

I worked on the development of the application across multiple layers, with a focus on backend, networking, persistence, and client-server communication.

My work included:

* Developing the application using **Java**
* Implementing the **JavaFX desktop client**
* Developing the **Flutter/Dart client**
* Designing and implementing the client-server communication layer
* Working with **REST services**
* Implementing **WebSocket communication for real-time notifications**
* Working with **JSON, binary serialization, and Google Protocol Buffers**
* Implementing the **Repository layer** for database access
* Working with the competition database
* Implementing services for managing participants, swimming events, and registrations
* Integrating multiple clients with the same server-side infrastructure
* Debugging communication, database, and multi-client issues
* Using **Gradle** for project configuration and build management

---

## About

The Swimming Contest application provides a centralized platform for managing a swimming competition.

The system allows competition data to be managed through a structured client-server architecture. Different clients communicate with the server through dedicated networking and service layers, while the application handles persistence and real-time updates.

The project includes:

* A **Java/JavaFX desktop client**
* A **Flutter/Dart client**
* A Java-based server-side architecture
* REST services
* WebSocket communication
* Multiple serialization mechanisms
* Repository-based database access
* A relational/local competition database

The architecture was designed to separate responsibilities between the user interface, business logic, networking, persistence, and database layers.

---

## Main Objectives

The main objectives of the project were:

* Manage swimmers participating in competitions
* Manage swimming events
* Register participants for events
* Store and retrieve competition data
* Provide multiple client applications
* Enable communication between clients and the server
* Support different data serialization protocols
* Provide real-time notifications
* Maintain a clear separation between application layers
* Demonstrate practical implementation of distributed application concepts

---

## System Architecture

The application follows a layered **client-server architecture**.

```text
                         Swimming Contest
                               │
                ┌──────────────┴──────────────┐
                │                             │
       ┌────────▼────────┐           ┌────────▼────────┐
       │   JavaFX Client │           │ Flutter Client  │
       │      Java       │           │   Dart/Flutter  │
       └────────┬────────┘           └────────┬────────┘
                │                             │
                └──────────────┬──────────────┘
                               │
                    ┌──────────▼──────────┐
                    │ Networking / REST   │
                    │ Communication Layer │
                    └──────────┬──────────┘
                               │
                    ┌──────────▼──────────┐
                    │       Server        │
                    │      Service        │
                    │   RestServices      │
                    └──────────┬──────────┘
                               │
             ┌─────────────────┼─────────────────┐
             │                 │                 │
      ┌──────▼──────┐   ┌──────▼──────┐   ┌──────▼──────┐
      │ Repository  │   │  WebSockets │   │   Protocol  │
      │    Layer    │   │ Notifications│   │   Buffers   │
      └──────┬──────┘   └─────────────┘   └─────────────┘
             │
      ┌──────▼──────┐
      │  Database   │
      └─────────────┘
```

The architecture separates the main responsibilities of the application:

1. **Clients** – provide user interaction.
2. **Networking** – handles communication between clients and the server.
3. **Services** – implement application and business operations.
4. **Repository** – manages database access.
5. **Database** – stores competition information.
6. **WebSockets** – provide real-time server-to-client notifications.
7. **Serialization** – allows data to be exchanged using different formats.

---

## Clients

### JavaFX Client

The project includes a desktop client developed in **JavaFX**.

The `ClientFX` module is responsible for providing a graphical user interface through which users can interact with the competition management system.

The client communicates with the server instead of accessing the database directly.

```text
JavaFX UI
    │
    ▼
Client Services
    │
    ▼
Networking
    │
    ▼
Server
```

---

### Flutter Client

The project was also extended with a **Flutter client written in Dart**.

The `flutter_client` module provides an additional client implementation that communicates with the existing application infrastructure.

This allowed the same competition management system to be accessed through a different technology stack and demonstrated the ability to integrate a cross-platform client with an existing backend.

```text
Flutter UI
    │
    ▼
Dart Client Logic
    │
    ▼
Networking / Services
    │
    ▼
Server
```

The Flutter implementation also integrates with the application's communication infrastructure, including real-time notification functionality.

---

## Backend Architecture

The server-side implementation is organized into several components:

```text
Server
  │
  ├── Service
  │     └── Business operations
  │
  ├── RestServices
  │     └── REST communication
  │
  ├── Networking
  │     └── Client-server communication
  │
  ├── Repository
  │     └── Data access
  │
  └── Model
        └── Domain entities
```

This structure separates the application into logical layers, making the system easier to maintain and extend.

---

## Core Functionality

### Participant Management

The application manages swimmers participating in the competition.

Typical operations include:

* Adding participants
* Retrieving participant information
* Updating participant information
* Removing participants
* Associating participants with competition events

---

### Swimming Events

The system manages the swimming events available in a competition.

Events can be stored and retrieved through the server-side services and associated with registered participants.

---

### Competition Registrations

Participants can be registered for swimming events.

The registration process connects swimmers with the events in which they participate and allows competition information to be maintained centrally.

---

### CRUD Operations

The application implements standard CRUD operations:

* **Create**
* **Read**
* **Update**
* **Delete**

These operations are handled through the appropriate service and repository layers rather than directly from the client interface.

---

## Networking

Networking is an important component of the application.

The `Networking` module provides the infrastructure required for communication between clients and the server.

The project separates communication concerns from the user interface and business logic, allowing different clients to use the same backend services.

```text
Client
   │
   │ Request
   ▼
Networking
   │
   ▼
Server
   │
   │ Response
   ▼
Client
```

This architecture also makes it possible to support multiple client implementations.

---

## REST Services

The `RestServices` module provides REST-based communication between the client applications and the server.

REST services are used for operations that require communication with the server for retrieving or modifying competition data.

The architecture therefore separates:

* Client UI
* REST communication
* Business logic
* Persistence

This provides a cleaner structure and makes individual components easier to develop and maintain.

---

## WebSocket Communication

The application also implements **WebSocket communication** for real-time notifications.

Unlike traditional request-response communication, WebSockets allow the server to send information to connected clients when an event occurs.

```text
                  Server
                    │
           ┌────────┼────────┐
           │        │        │
           ▼        ▼        ▼
        Client 1  Client 2  Client 3
           ▲        ▲        ▲
           └────────┴────────┘
              WebSockets
```

This functionality is particularly useful in a multi-client competition environment where changes need to be communicated to connected clients without requiring each client to continuously poll the server.

---

## Data Serialization

The project explores multiple approaches for serializing data exchanged between application components.

### JSON

JSON provides a human-readable format suitable for structured data exchange.

### Binary Serialization

Binary serialization provides a more compact representation of application data for communication between components.

### Google Protocol Buffers

The project also includes a **Protocol Buffers** configuration through the `Protoconfig` module.

Protocol Buffers provide a structured and efficient mechanism for serializing data exchanged between applications.

The project therefore demonstrates different approaches to data representation and communication.

---

## Database and Persistence

The application uses a dedicated **Repository layer** for database access.

```text
Service
   │
   ▼
Repository
   │
   ▼
Database
```

The repository abstraction keeps database operations separate from the business logic.

The repository layer is responsible for operations such as:

* Saving entities
* Retrieving entities
* Updating entities
* Deleting entities
* Querying competition data

The repository communicates with the competition database stored in:

```text
SwimingContest.db
```

---

## Project Structure

The repository is organized into several modules and components:

```text
Swimming-contest/
│
├── ClientFX/
│   └── JavaFX desktop client
│
├── Model/
│   └── Domain models and shared application data
│
├── Networking/
│   └── Client-server communication
│
├── Protoconfig/
│   └── Protocol Buffers configuration
│
├── Repository/
│   └── Database access layer
│
├── RestServices/
│   └── REST services
│
├── Server/
│   └── Server-side application
│
├── Service/
│   └── Business logic and application services
│
├── flutter_client/
│   └── Flutter/Dart client
│
├── SwimingContest.db
│   └── Competition database
│
├── build/
│   └── Build and generated reports
│
├── gradle/
│   └── Gradle wrapper configuration
│
├── gradlew
├── gradlew.bat
└── settings.gradle
```

---

## Technology Stack

### Programming Languages

* **Java**
* **Dart**

### Client Technologies

* **JavaFX**
* **Flutter**

### Backend & Services

* Java
* REST Services
* WebSockets
* Service-oriented layered architecture

### Communication & Serialization

* REST
* WebSockets
* JSON
* Binary serialization
* Google Protocol Buffers

### Persistence

* Repository pattern
* SQLite / `.db` database

### Build & Development

* Gradle
* Git
* GitHub

---

## Application Workflow

A typical interaction with the application follows this flow:

```text
1. User opens a client
        │
        ▼
2. Client connects to server
        │
        ▼
3. User performs an operation
        │
        ▼
4. Request is sent through networking/services
        │
        ▼
5. Server processes the request
        │
        ▼
6. Repository accesses the database
        │
        ▼
7. Server returns the result
        │
        ▼
8. Client updates the interface
        │
        ▼
9. Relevant clients can receive
   real-time WebSocket notifications
```

---

## Multi-Client Architecture

One of the main technical aspects of the project is the use of multiple clients communicating with the same backend infrastructure.

```text
                 ┌───────────────────┐
                 │      Server       │
                 └─────────┬─────────┘
                           │
             ┌─────────────┴─────────────┐
             │                           │
     ┌───────▼────────┐         ┌────────▼───────┐
     │ JavaFX Client  │         │ Flutter Client │
     │     Java       │         │  Dart/Flutter  │
     └────────────────┘         └────────────────┘
```

This approach demonstrates how different client technologies can interact with the same server-side application.

It also separates the presentation layer from the backend logic, allowing additional clients to be introduced without rewriting the entire system.

---

## Development Workflow

The project was developed incrementally, with different modules being integrated into the final application.

The development process involved:

1. Defining the domain model
2. Implementing repositories
3. Developing application services
4. Implementing networking
5. Adding REST services
6. Integrating the JavaFX client
7. Adding WebSocket notifications
8. Implementing Protocol Buffers support
9. Developing the Flutter/Dart client
10. Integrating the different components
11. Debugging database and multi-client communication
12. Managing the project using Git and Gradle

---

## Testing and Debugging

During development, testing and debugging focused on:

* Client-server communication
* Database access
* CRUD operations
* Multiple connected clients
* WebSocket notifications
* Serialization and deserialization
* REST service behavior
* Integration between Java and Flutter clients
* Configuration and build issues

The project also involved debugging situations related to database configuration and running multiple clients simultaneously.

---

## How to Run

### Prerequisites

Make sure the development environment includes:

* Java JDK
* Gradle / Gradle Wrapper
* JavaFX-compatible environment
* Flutter SDK
* Dart SDK
* A configured local database

### Java Version

Clone the repository:

```bash
git clone https://github.com/DianaGliga11/Swimming-contest.git
cd Swimming-contest
```

Build the project using the Gradle wrapper:

```bash
./gradlew build
```

On Windows:

```bash
gradlew.bat build
```

Run the server and then start the JavaFX client according to the project configuration.

### Flutter Client

Navigate to the Flutter client:

```bash
cd flutter_client
```

Install dependencies:

```bash
flutter pub get
```

Run the application:

```bash
flutter run
```

The Flutter client must be configured to communicate with the running server.

---

## Technical Concepts Demonstrated

This project combines several important software engineering concepts:

* Object-Oriented Programming
* Layered architecture
* Client-server architecture
* Multi-client systems
* Separation of concerns
* Repository pattern
* CRUD operations
* REST APIs
* WebSocket communication
* Real-time notifications
* Data serialization
* JSON
* Binary serialization
* Protocol Buffers
* Database persistence
* JavaFX application development
* Flutter/Dart application development
* Gradle build automation
* Git version control

---

## What I Learned

Through this project, I gained practical experience with:

* Designing multi-layered applications
* Developing client-server systems
* Building applications with multiple clients
* Working with JavaFX
* Developing cross-platform interfaces using Flutter and Dart
* Implementing REST-based communication
* Working with WebSockets and real-time notifications
* Designing repository and service layers
* Working with databases and persistence
* Comparing different serialization approaches
* Using Protocol Buffers for structured communication
* Debugging distributed application components
* Managing a larger project using Gradle and Git

---

## Future Improvements

Potential improvements for the project include:

* Adding authentication and authorization
* Improving the Flutter user interface
* Adding more advanced competition statistics
* Implementing richer participant and event filtering
* Improving error handling and validation
* Adding automated tests across all layers
* Containerizing the backend
* Improving deployment configuration
* Adding more real-time competition features
* Extending the system with additional client applications

---


Computer Science Graduate
Babeș-Bolyai University
