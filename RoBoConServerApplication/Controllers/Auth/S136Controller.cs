using ModelsLibrary.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Auth
{
    [RoutePrefix("s136")]
    public class S136Controller : ApiController
    {
        #region MyRegion
        //[Route("")]
        //[HttpGet]
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //[Route("")]
        //[HttpGet]
        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //[Route("")]
        //[HttpPost]
        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //[Route("")]
        //[HttpPut]
        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //[Route("")]
        //[HttpDelete]
        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //} 
        #endregion

        /// <summary>
        /// 사용자 권한 관리 GROUP & USER - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetGroupAndUserSelect([FromBody]GroupUserVo vo)
        {
            return Ok<IEnumerable<GroupUserVo>>(Properties.EntityMapper.QueryForList<GroupUserVo>("S136SelectGroupUserTreeList", vo));
        }


        /// <summary>
        /// 사용자 권한 관리 GROUP - 조회
        /// </summary>
        [Route("g")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetGroupSelect([FromBody]GroupUserVo vo)
        {
            return Ok<IEnumerable<GroupUserVo>>(Properties.EntityMapper.QueryForList<GroupUserVo>("S136SelectGroupList", vo));
        }

        /// <summary>
        /// 사용자 권한 관리 GROUP - 추가
        /// </summary>
        [Route("g/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetGroupInsert([FromBody]GroupUserVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S136InsertGroupCode", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 GROUP - 수정
        /// </summary>
        [Route("g/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetGroupUpdate([FromBody]GroupUserVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S136UpdateGroupCode", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 GROUP - 삭제
        /// </summary>
        [Route("g/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetGroupDelete([FromBody]GroupUserVo vo)
        {
            try
            {
                ////#CHNL_CD#, #SQL#, #TB_NM#, #CRE_USR_ID#
                //vo.SQL = "DELETE FROM TB_SYS_GRP WHERE 1 = 1 AND GRP_ID = '" + vo.GRP_ID + "' AND CHNL_CD = '" + vo.CHNL_CD + "' AND 0 = (SELECT COUNT(*) FROM TB_SYS_USR WHERE GRP_ID = '" + vo.GRP_ID + "' AND CHNL_CD = '" + vo.CHNL_CD + "')";
                //vo.TB_NM = "TB_SYS_GRP";

                //return Ok<int>(Properties.EntityMapper.Delete("system_ProcDelete", vo));

                return Ok<int>(Properties.EntityMapper.Delete("S136DeleteGroupCode", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 사용자 권한 관리 USER - 조회
        /// </summary>
        [Route("u")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetUserSelect([FromBody]GroupUserVo vo)
        {
            return Ok<IEnumerable<GroupUserVo>>(Properties.EntityMapper.QueryForList<GroupUserVo>("S136SelectUserList", vo));
        }

        /// <summary>
        /// 사용자 권한 관리 USER - 추가
        /// </summary>
        [Route("u/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetUserInsert([FromBody]GroupUserVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S136InsertUserInfo", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 USER - 수정
        /// </summary>
        [Route("u/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetUserUpdate([FromBody]GroupUserVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S136UpdateUserInfo", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 USER - 삭제
        /// </summary>
        [Route("u/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetUserDelete([FromBody]GroupUserVo vo)
        {
            try
            {
                //#CHNL_CD#, #SQL#, #TB_NM#, #CRE_USR_ID#
                //vo.SQL = "DELETE FROM TB_SYS_USR  WHERE 1 = 1 AND USR_ID = '" + vo.USR_ID + "' AND CHNL_CD = '" + vo.CHNL_CD + "'";
                //vo.TB_NM = "TB_SYS_USR";

                //return Ok<int>(Properties.EntityMapper.Delete("system_ProcDelete", vo));

                return Ok<int>(Properties.EntityMapper.Delete("S136DeleteUserInfo", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 GROUP 메뉴 - 조회
        /// </summary>
        [Route("g/menu")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetGroupMenuSelect([FromBody]ProgramVo vo)
        {
            return Ok<IEnumerable<ProgramVo>>(Properties.EntityMapper.QueryForList<ProgramVo>("S136SelectProgramGroupList", vo));
        }

        /// <summary>
        /// 사용자 권한 관리 GROUP 메뉴 - 추가
        /// </summary>
        [Route("g/menu/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetGroupMenuInsert([FromBody]ProgramVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S136InsertGroupProgram", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 GROUP 메뉴 - 수정
        /// </summary>
        [Route("g/menu/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetGroupMenuUpdate([FromBody]ProgramVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S136UpdateGroupProgram", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 GROUP 메뉴 - 삭제
        /// </summary>
        [Route("g/menu/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetGroupMenuDelete([FromBody]ProgramVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S136DeleteGroupProgram", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 USER 메뉴 - 조회
        /// </summary>
        [Route("u/menu")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetUserMenuSelect([FromBody]ProgramVo vo)
        {
            return Ok<IEnumerable<ProgramVo>>(Properties.EntityMapper.QueryForList<ProgramVo>("S136SelectProgramUserList", vo));
        }

        /// <summary>
        /// 사용자 권한 관리 USER 메뉴 - 추가
        /// </summary>
        [Route("u/menu/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetUserMenuInsert([FromBody]ProgramVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S136InsertUserProgram", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 USER 메뉴 - 수정
        /// </summary>
        [Route("u/menu/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetUserMenuUpdate([FromBody]ProgramVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S136UpdateUserProgram", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 사용자 권한 관리 USER 메뉴 - 삭제
        /// </summary>
        [Route("u/menu/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetUserMenuDelete([FromBody]ProgramVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S136DeleteUserProgram", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 전체사용자 - 조회
        /// </summary>
        [Route("usr")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetUsr([FromBody]GroupUserVo vo)
        {
            return Ok<IEnumerable<GroupUserVo>>(Properties.EntityMapper.QueryForList<GroupUserVo>("S136SelectUserList", vo));
        }

        /// <summary>
        /// 채널 코드 체크
        /// </summary>
        [Route("chnl/{chnl_cd}")]
        [HttpGet]
        // GET api/<controller>/5
        public async Task<IHttpActionResult> GetChnl(string chnl_cd)
        {
            return Ok<ProgramVo> (Properties.EntityMapper.QueryForObject<ProgramVo>("SelectChnlList", new ProgramVo() { CHNL_CD = chnl_cd, DELT_FLG = "N" }));
        }

        /// <summary>
        /// 채널 코드 - 메인 이미지/리포트 도장 등록
        /// </summary>
        [Route("chnl/u")]
        [HttpPost]
        // GET api/<controller>/5
        public async Task<IHttpActionResult> GetChnlUpdate([FromBody]ProgramVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("UpdateChnl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 전체 채널 - 조회
        /// </summary>
        [Route("chnl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetChnl([FromBody] ProgramVo vo)
        {
            return Ok<IEnumerable<ProgramVo>>(Properties.EntityMapper.QueryForList<ProgramVo>("SelectChnlList", vo));
        }


        /// <summary>
        /// 로그인 체크
        /// </summary>
        [Route("login/{chnl_cd}/{usr_id}")]
        [HttpGet]
        // GET api/<controller>/5
        public async Task<IHttpActionResult> GetLogin(string chnl_cd, string usr_id)
        {
            return Ok<GroupUserVo>(Properties.EntityMapper.QueryForObject<GroupUserVo>("S136SelectUserList", new GroupUserVo() { CHNL_CD = chnl_cd, USR_ID = usr_id }));
        }

        /// <summary>
        /// 메뉴 체크
        /// </summary>
        [Route("menu/{chnl_cd}/{usr_id}")]
        [HttpGet]
        // GET api/<controller>/5
        public async Task<IHttpActionResult> GetMenu(string chnl_cd, string usr_id)
        {
            return Ok< IEnumerable<ProgramVo>>(Properties.EntityMapper.QueryForList<ProgramVo>("SelectProgramMenuList", new ProgramVo() { CHNL_CD = chnl_cd, USR_ID = usr_id }));
        }


    }
}