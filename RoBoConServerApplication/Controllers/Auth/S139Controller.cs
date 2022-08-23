using ModelsLibrary.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Auth
{
    [RoutePrefix("s139")]
    public class S139Controller : ApiController
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
        /// 프로그램 메뉴 관리 MST - 조회
        /// </summary>
        [Route("{chnl_cd}")]
        [HttpGet]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect(string chnl_cd)
        {
            return Ok<IEnumerable<ProgramVo>>(Properties.EntityMapper.QueryForList<ProgramVo>("S139SelectProgramMenuTotalList", new ProgramVo() { CHNL_CD = chnl_cd }));
        }


        /// <summary>
        /// 프로그램 메뉴 관리 MST - 추가
        /// </summary>
        [Route("i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]ProgramVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S139InsertProgram", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 프로그램 메뉴 관리 MST - 수정
        /// </summary>
        [Route("u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]ProgramVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S139UpdateProgram", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        ///// <summary>
        ///// 프로그램 메뉴 관리 MST - 삭제
        ///// </summary>
        //[Route("d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstDelete([FromBody]ProgramVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Update("S139DeleteProgram", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}





        /// <summary>
        /// 프로그램 메뉴 관리 MST -  상위 메뉴 조회
        /// </summary>
        [Route("prntmenu")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstPrntMenu([FromBody]ProgramVo vo)
        {
            return Ok<IEnumerable<ProgramVo>>(Properties.EntityMapper.QueryForList<ProgramVo>("S139SelectProgramMenuTotalList", vo));
        }

    }
}