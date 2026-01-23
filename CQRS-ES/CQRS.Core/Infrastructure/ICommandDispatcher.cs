using CQRS.Core.Commands;

namespace CQRS.Core.Infrastructure;

public interface ICommandDispatcher
{
    void RegisterHandler<T>(Func<T, Task> handle) where T : BaseCommand;
    Task SendAsync(BaseCommand command);
}