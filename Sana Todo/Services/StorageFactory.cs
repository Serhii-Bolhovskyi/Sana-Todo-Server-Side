namespace Sana_Todo.Services
{
    public class StorageFactory : IStorageFactory
    {
        private readonly TaskService _taskService;
        private readonly XmlTaskService _xmlTaskService;

        public StorageFactory(TaskService taskService, XmlTaskService xmlTaskService)
        {
            _taskService = taskService;
            _xmlTaskService = xmlTaskService;
        }

        public ITaskImplement CreateTaskImplement(string storageOption)
        {
            return storageOption == "db" ? _taskService : _xmlTaskService;
        }
    }
}
