using System.Text.Json;
using System.Text.Json.Serialization;
using CQRSCore.Events;
using PostCommon.Events;

namespace Infrastructure.Converters;

public class EventJsonConverter: JsonConverter<BaseEvent>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
    }

    public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(!JsonDocument.TryParseValue(ref reader, out var document))
        {
            throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
        }
        
        if(!document.RootElement.TryGetProperty("Type", out var typeProperty))
        {
            throw new JsonException($"Could not detect the Type discriminator property");
        }
        
        var typeDiscriminator = typeProperty.GetString();
        var json = document.RootElement.GetRawText();

        return typeDiscriminator switch
        {
            nameof(PostCreatedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
            nameof(MessageUpdatedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
            nameof(PostLikedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
            nameof(CommentAddedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
            nameof(CommentUpdatedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
            nameof(CommentRemovedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
            nameof(PostRemovedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
            _ => throw new JsonException($"Unknown event type {typeDiscriminator}")
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}