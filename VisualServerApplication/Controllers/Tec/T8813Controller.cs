using ModelsLibrary.Sale;
using ModelsLibrary.Tec;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Tec
{
    [RoutePrefix("t8813")]
    public class T8813Controller : ApiController
    {
        #region MyRegion
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //} 
        #endregion

        /// <summary>
        /// 부자재 품질검사 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] TecVo vo)
        {
            return Ok<IEnumerable<TecVo>>(Properties.EntityMapper.QueryForList<TecVo>("T8813SelectMstList", vo));
        }


        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] TecVo vo)
        {
            return Ok<IEnumerable<TecVo>>(Properties.EntityMapper.QueryForList<TecVo>("T8813SelectDtlList", vo));
        }


        /// <summary>
        /// 부자재 품질검사 MST - 수정
        /// </summary>
        [Route("m")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] IEnumerable<TecVo> voList)
        {
            //try
            //{
            //    return Ok<int>(Properties.EntityMapper.Update("T8813UpdateMst", vo));
            //}
            //catch (System.Exception eLog)
            //{
            //    return Ok<string>(eLog.Message);
            //}

            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (TecVo item in voList)
                {
                    Properties.EntityMapper.QueryForObject<int?>("T8813UpdateMst", item);
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


    }
}