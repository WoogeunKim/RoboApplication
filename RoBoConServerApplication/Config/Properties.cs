using IBatisNet.DataMapper;
using IBatisNet.DataMapper.SessionStore;
using System;

namespace VisualServerApplication.Config
{
    class Properties
    {
        public static ISqlMapper EntityMapper
        {
            get
            {
                try
                {
                    ISqlMapper mapper = Mapper.Instance();
                    mapper.SessionStore = new HybridWebThreadSessionStore(mapper.Id);
                    return mapper;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}