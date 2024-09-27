using CQRSCore.Events;

namespace CQRSCore.Domain;

public abstract class AggregateRoot
{
    protected Guid _id { get; set; }
    private readonly List<BaseEvent> _changes = new();

    public Guid Id
    {
        get { return _id; }
    }

    public int Version { get; set; } = -1;

    public IEnumerable<BaseEvent> GetUncommittedChanges()
    {
        return _changes;
    }
    
    public void MarkChangesAsCommitted()
    {
        _changes.Clear();
    }
    
    public void ReplayEvents(IEnumerable<BaseEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyChange(@event, false);
        }
    }
    
    protected void RaiseEvent(BaseEvent @event)
    {
        ApplyChange(@event, true);
    }
    
    private void ApplyChange(BaseEvent @event, bool isNew)
    {
        var method = this.GetType()
            .GetMethod("Apply", new Type[] { @event.GetType() });
        if (method == null)
            throw new ArgumentNullException(nameof(method), $"Method not found in the aggregate for event {@event.GetType().Name}");
        
        method.Invoke(this, new object[] { @event });
        
        if(isNew)
            _changes.Add(@event);
    }
    
    
    
}