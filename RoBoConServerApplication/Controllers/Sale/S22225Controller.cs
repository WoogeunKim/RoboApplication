using ModelsLibrary.Sale;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s22225")]
    public class S22225Controller : ApiController
    {

        //private string title = "GR번호 확정";

        #region MyRegion
        /*// GET api/<controller>
       public IEnumerable<string> Get()
       {
           return new string[] { "value1", "value2" };
       }

       // GET api/<controller>/5
       public string Get(int id)
       {
           return "value";
       }

       // POST api/<controller>
       public void Post([FromBody] string value)
       {
       }

       // PUT api/<controller>/5
       public void Put(int id, [FromBody] string value)
       {
       }

       // DELETE api/<controller>/5
       public void Delete(int id)
       {
       }*/ 
        #endregion

        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22225SelectMstList", vo));
            }
            catch(Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// GR번호 확정 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] List<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (SaleVo _item in voList)
                {
                    Properties.EntityMapper.Delete("S22225DeleteMst", _item);
                    Properties.EntityMapper.Insert("S22225InsertMst", _item);
                }
                Properties.EntityMapper.CommitTransaction();
                return Ok<int?>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }



        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22225SelectDtlList", vo));
        }

    }
}