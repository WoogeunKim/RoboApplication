using ModelsLibrary.Pur;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p44011")]
    public class P44011Controller : ApiController
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
        /// 수주 마감 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P44011SelectMstList", vo));
        }


        /// <summary>
        /// 수주 마감 MST - Ok
        /// </summary>
        [Route("mst/ok")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstOk([FromBody] IEnumerable<PurVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //
                foreach (PurVo item in voList)
                {
                    item.CLZ_FLG = "Y";
                    item.CRE_USR_ID = item.UPD_USR_ID;
                    item.UPD_USR_ID = item.UPD_USR_ID;
                    if (Properties.EntityMapper.QueryForObject<PurVo>("P44011SelectMstChk", item) == null)
                    {
                        Properties.EntityMapper.Insert("P44011InsertMst", item);
                    }
                    else
                    {
                        Properties.EntityMapper.Update("P44011UpdateMst", item);
                    }
                }
                //
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 수주 마감 MST - Cancel
        /// </summary>
        [Route("mst/cancel")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstCancel([FromBody] IEnumerable<PurVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                //
                foreach (PurVo item in voList)
                {
                    item.CLZ_FLG = "N";
                    item.CRE_USR_ID = item.UPD_USR_ID;
                    item.UPD_USR_ID = item.UPD_USR_ID;
                    if (Properties.EntityMapper.QueryForObject<PurVo>("P44011SelectMstChk", item) == null)
                    {
                        Properties.EntityMapper.Insert("P44011InsertMst", item);
                    }
                    else
                    {
                        Properties.EntityMapper.Update("P44011UpdateMst", item);
                    }
                }
                //
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }






    }
}