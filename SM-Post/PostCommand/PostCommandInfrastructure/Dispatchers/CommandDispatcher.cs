using CQRSCore.Commands;
using CQRSCore.Infrastructure;

namespace Infrastructure.Dispatchers;

public class CommandDispatche: ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers =
        new Dictionary<Type, Func<BaseCommand, Task>>();
    
    public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if(_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException($"Handler for '{typeof(T).Name}' command is already registered.");
        }
        
        _handlers.Add(typeof(T), command => handler((T)command));
    }

    public async Task SendAsync(BaseCommand command)
    {
        if(_handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task> handler))
        {
            await handler(command);
        }else
        {
            throw new ArgumentNullException(nameof(command), "No command handler was registered.");
        }
    }
}