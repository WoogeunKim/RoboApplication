using ModelsLibrary.Tec;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Tec
{
    [RoutePrefix("t91101")]
    public class T91101Controller : ApiController
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
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] TecVo vo)
        {
            return Ok<IEnumerable<TecVo>>(Properties.EntityMapper.QueryForList<TecVo>("T91101SelectMstList", vo));
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
                    Properties.EntityMapper.QueryForObject<int?>("T91101UpdateMst", item);
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
        /// 부자재 품질검사(불량) DTL - 조회
        /// </summary>
        [Route("bad")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] TecVo vo)
        {
            return Ok<IEnumerable<TecVo>>(Properties.EntityMapper.QueryForList<TecVo>("T91101SelectDtlList", vo));
        }


        /// <summary>
        /// 부자재 품질검사(불량) DTL - 추가
        /// </summary>
        [Route("bad/m")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody] IEnumerable<TecVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                IList<TecVo> _voList = new List<TecVo>(voList);
                Properties.EntityMapper.Delete("T91101DeleteDetail", _voList[0]);

                foreach (TecVo item in voList)
                {
                    Properties.EntityMapper.Insert("T91101InsertDetail", item);
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
        /// 부자재 품질검사 [시료수] MST - 조회
        /// </summary>
        [Route("popup/ok")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopupOkSelect([FromBody] TecVo vo)
        {
            return Ok<TecVo>(Properties.EntityMapper.QueryForObject<TecVo>("T91100SelectPopupOk", vo));
        }

        /// <summary>
        /// 부자재 품질검사(불량) DTL - 삭제
        /// </summary>
        [Route("bad/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] TecVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("T91101DeleteDetail", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}