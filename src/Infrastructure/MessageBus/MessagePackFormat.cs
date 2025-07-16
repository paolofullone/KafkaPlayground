using MessagePack;
using MessagePack.Resolvers;

namespace Infrastructure.MessageBus;
public static class MessagePackFormat
{
    private static readonly MessagePackSerializerOptions Options = MessagePackSerializerOptions
        .Standard.WithResolver(CompositeResolver.Create(
            NativeDateTimeResolver.Instance,
            ContractlessStandardResolver.Instance));

    public static T Deserialize<T>(byte[] bytes)
    {
        return MessagePackSerializer.Deserialize<T>(bytes, Options);
    }

    public static byte[] Serialize<T>(T obj)
    {
        return MessagePackSerializer.Serialize(obj, Options);
    }
}
