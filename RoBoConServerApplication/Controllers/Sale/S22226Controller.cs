using ModelsLibrary.Sale;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s22226")]
    public class S22226Controller : ApiController
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

        /// <summary>
        /// 생산최적화 MST - 수정
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22226SelectMstList", vo));
            }
            catch(Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 생산최적화 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] List<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (SaleVo _item in voList)
                {
                    Properties.EntityMapper.Update("S22226UpdateMst", _item);
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

        /// <summary>
        /// 생산최적화 POPUP (MST/DTL) - 추가
        /// </summary>
        [Route("popup/m")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetPopupInsert([FromBody] List<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                //MST
                Properties.EntityMapper.Insert("S22226InsertPopupMst", voList[0]);


                foreach (SaleVo _item in voList)
                {
                    //DTL
                    Properties.EntityMapper.Insert("S22226InsertPopupDtl", _item);
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



        //[Route("dtl")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetDtlSelect([FromBody] SaleVo vo)
        //{
        //    return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22225SelectDtlList", vo));
        //}

    }
}