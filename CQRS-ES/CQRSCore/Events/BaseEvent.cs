using CQRSCore.Messages;

namespace CQRSCore.Events;

public abstract class BaseEvent: Message
{
    protected BaseEvent(string type)
    {
        this.Type = type;
    }
    
    public int Version { get; set; }
    public string Type { get; set; }
}