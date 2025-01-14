﻿using PostCommon.Events;

namespace Infrastructure.Handler;

public interface IEventHandler
{
    Task On(PostCreatedEvent @event);
    Task On(MessageUpdatedEvent @event);
    Task On(PostLikedEvent @event);
    Task On(CommentAddedEvent @event);
    Task On(CommentUpdatedEvent @event);
    Task On(CommentRemovedEvent @event);
    Task On(PostRemovedEvent @event);
}