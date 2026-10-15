# Habit Tracker Console Application

A console-based habit tracking application built in **C#** using **SQLite** for data persistence and **NUnit** for unit testing.

---

## What the App Does

This application allows users to track multiple habits and record daily progress (such as glasses of water drunk, minutes run, or pages read). It features a secure relational database, input validation, data seeding, and a clean, interactive console menu system.

---

## How It Works

* **Multi-Habit Support:** Users can create custom habits (specifying name and unit of measurement) and switch between them.
* **Record Management:** For any active habit, users can View, Insert, Update, and Delete progress records.
* **Smart Input Handling:** Validates dates (`dd-MM-yyyy`) and numerical input safely, with quick shortcuts (e.g., type `1` for today's date or `0` to go back).
* **Data Seeding:** Automatically generates random historical test data for development and testing convenience.

---

## Architectural Choices

* **Separation of Concerns:**
* `Program.cs`: Manages console UI and application flow loops.
* `Records.cs`: Contains the business logic and SQLite database commands.
* `InputHandler.cs`: Handles user inputs and pure validation logic (isolated to allow safe unit testing without blocking console IO).


* **Relational Database Design:** Two normalized tables (`habits` and `records`) linked via a Foreign Key (`HabitId`) to support unlimited distinct habits without table duplication.
* **Security:** Uses **Parameterized Queries** across all SQLite commands to fully protect against SQL injection vulnerabilities.
* **Testing:** Uses **NUnit** to unit test validation logic independently of the database or console interface.

---

## Setup and Run Instructions

### Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/) installed on your machine.

### Running the Application

1. Clone or download the repository to your local machine.
2. Open your terminal or command prompt in the project root directory.
3. Run the application using the following command:
```bash
dotnet run

```


4. The SQLite database (`Habit-Tracker.db`) will be automatically created in your directory upon the first launch.

### Running Unit Tests

1. Navigate to the test project directory.
2. Execute the tests with:
```bash
dotnet test

```



---

## Development Reflection

> Learning how to use SQLite with C# and mastering SQL parameterization was tough at first, but totally manageable once I got the hang of it. 
> Refactoring the app into a structured, relational architecture and separating validation logic for unit testing significantly improved my coding skills.
