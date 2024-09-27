using CQRSCore.Events;

namespace PostCommon.Events;

public class PostRemovedEvent: BaseEvent
{
    public PostRemovedEvent() : base(nameof(PostRemovedEvent))
    {
    }

}