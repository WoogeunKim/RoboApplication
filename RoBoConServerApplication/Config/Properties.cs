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
                    ///mapper.DataSource.ConnectionString = "Data Source=210.217.42.142;Initial Catalog=SFND;Persist Security Info=True;User ID=sa; Password=cpc_1234; Timeout=800";
                    //mapper.DataSource.ConnectionString = "User Id=daegil;Password=daegil;Data Source=(DESCRIPTION=(ADDRESS_LIST= (ADDRESS=(PROTOCOL=TCP) (HOST=210.217.42.132) (PORT=1521))) (CONNECT_DATA = (SERVICE_NAME = daegil)))";
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