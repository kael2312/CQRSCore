﻿using CQRSCore.Events;

namespace PostCommon.Events;

public class PostLikedEvent: BaseEvent
{
    public PostLikedEvent() : base(nameof(PostLikedEvent))
    {
    }
}