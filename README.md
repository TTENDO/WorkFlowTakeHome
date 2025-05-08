# WorkFlowTakeHome

## Dynamic Survey Workflow API (ASP.NET Core)

### Overview

This API is designed to solve the challenge of **standardizing and staging diverse JSON data** from multiple survey tools (e.g., KoboToolbox, ODK, SurveyCTO). These tools often submit data in different formats, making it difficult to process uniformly.

The solution provides a **flexible workflow staging layer**, which:
- Accepts dynamic payloads
- Validates essential fields
- Transforms data into a common model
- Sets workflow status for review and further processing

---

### Problem Statement

- Survey tools have varying data formats (schemas).
- Data must be validated for required fields (e.g., `respondent`).
- Data needs transformation and normalization into a unified structure.
- The solution must be extensible for new tools, without tight coupling.
- Performance and memory management are key for scalability.

---

### Technologies

- **ASP.NET Core Web API (.NET 8)**
- **Dependency Injection**
- **JSON Parsing with `System.Text.Json`**
- **Validation in Service Layer**
- **Unit Testing with xUnit**

---

### Architecture Overview

- `/Services/IGenericDataParser` enables pluggable logic per survey tool.
- `/Services/WorkflowStagingService` handles validation, transformation, and staging logic.
- `/Models/WorkflowItem` is the normalized model added to the workflow.

---

### Validation Logic

- Checks for required fields (e.g., `"respondent"`)
- Ensures values are not null or empty
- Invalid submissions are rejected with detailed messages

---

### How to Run the API (2 ways)

#### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022

##### 1. in the terminal

```bash
git clone https://github.com/TTENDO/WorkFlowTakeHome.git

### Navigate to the API project directory e.g
cd survey-workflow-api

### Build and Run
dotnet build
dotnet run

### Sample Request
JSON Payload
{
  "respondent": "Jane Doe",
  "question1": "Yes",
  "submittedAt": "2025-05-08T12:00:00Z"
}

```

#### 2. Visual studio UI
- After running git clone succesfully
- Open the solution in VS 2022
- Run the build command (Build -> Buld Solution or press CTRL + SHIFT + B)
- Run the project (CTRL + F5)


## Contributors
[Namaganda Rebecca](https://github.com/TTENDO)







