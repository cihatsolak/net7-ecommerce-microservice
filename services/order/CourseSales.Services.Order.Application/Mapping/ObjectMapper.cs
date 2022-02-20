namespace CourseSales.Services.Order.Application.Mapping
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> lazyMapper = new(() =>
        {
            MapperConfiguration config = new(cfg =>
            {
                cfg.AddProfile<CustomMapping>();
            });

            return config.CreateMapper();
        });

        public static IMapper Mapper => lazyMapper.Value;
    }
}
