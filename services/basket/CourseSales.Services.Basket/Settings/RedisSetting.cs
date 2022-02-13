namespace CourseSales.Services.Basket.Settings
{
    public interface IRedisSetting
    {
        string Host { get; init; }
        string Port { get; init; }
    }

    public record RedisSetting : IRedisSetting
    {
        public string Host { get; init; }
        public string Port { get; init; }
        public string ConnectionString => string.Concat(Host, ":", Port);
    }
}
