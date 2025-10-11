namespace Sana_Todo.Services
{
    public interface IStorageFactory
    {
        ITaskImplement CreateTaskImplement(string storageOption);
    }
}
