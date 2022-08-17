using AutoMapper;

namespace CleaningManagement.Service.Infrastructure
{
    // On larger project with more service AutoMapper would be injected,
    // but for this example this is good enough.
    public static class AutoMapperUtility
    {
        private static object syncLock = new object();
        private static IMapper mapper;

        public static IMapper GetAutomapper()
        {
            lock (syncLock)
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<CleaningPlanProfile>();
                });

                mapper = mapperConfiguration.CreateMapper();
            }

            return mapper;
        }
    }
}
