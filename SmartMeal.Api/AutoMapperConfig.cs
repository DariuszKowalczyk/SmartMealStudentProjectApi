using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMeal.Service
{
    public static class AutoMapperConfig
    {
        private static object _thislock = new object();
        private static bool _initialized = false;

        public static void Initialize()
        {
            lock (_thislock)
            {
                if (!_initialized)
                {
                    AutoMapper.Mapper.Initialize(cfg => { cfg.AddProfile<MapperProfile>(); });
                    _initialized = true;
                }
            }
        }


    }
}
