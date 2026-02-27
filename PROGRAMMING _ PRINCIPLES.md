## 1. Single Responsibility Principle (SRP)
My project based on MVC pattern, and follows SRP in places like: 
+ [Models](<./Sana Todo/Models>): The Model's single responsibility is to represent data, and it's one reason to change when is **data structure changes**
+ [TaskService.cs](<./Sana Todo/Services/TaskService.cs>) and [XmlTaskService.cs](<./Sana Todo/Services/XmlTaskService.cs>): responsible for connectting to the DB and performing CRUD operation on Tasks 

## 2. Interface Segregation Principle (ISP)
[ITaskImplement](<./Sana Todo/Services/ITaskImplement.cs>) contains only shared CRUD operations that both [TaskService.cs](<./Sana Todo/Services/TaskService.cs>) and [XmlTaskService.cs](<./Sana Todo/Services/XmlTaskService.cs>) **implement**, so ISP is satisfied

## 3. Dependency Inversion Principle (DIP)
In [TaskController](<./Sana Todo/Controllers/TaskController.cs>) at [line 13](<./Sana Todo/Controllers/TaskController.cs#L13>) we follow DIP because we rely on the abstraction `IStorageFactory` rather than a concrete implementation, and with this approach we achieve **loose coupling**
