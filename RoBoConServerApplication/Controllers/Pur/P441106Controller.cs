using ModelsLibrary.Pur;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p441106")]
    public class P441106Controller : ApiController
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
        /// 고객사발주등록 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441106SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        ///  고객사발주등록 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] PurVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                vo.PUR_NO = Properties.EntityMapper.QueryForObject<string>("P441106SelectPurOrdNo", vo);  // 키생성
                Properties.EntityMapper.Update("P441106UpdatePurOrdNo", vo);                              // 생성된 키를 업데이트
                Properties.EntityMapper.Insert("P441106InsertMst", vo);                                   // 업데이트된걸 인서트

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
        /// 고객사발주등록 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P441106UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 고객사발주등록 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] PurVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                Properties.EntityMapper.Delete("P441106DeleteDtl", vo);
                Properties.EntityMapper.Delete("P441106DeleteMst", vo);

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
        /// 고객사발주등록 DTL(도면관리) - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441106SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 고객사발주등록 DTL - 추가(도면등록)
        /// </summary>
        /// 
        //[RequestFormLimits(MultipartBodyLengthLimit = 314_572_800)]
        //[RequestSizeLimit(314_572_800)]
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        //public async Task<IHttpActionResult> GetDtlInsert([FromBody] IList<PurVo> voList)
        public async Task<IHttpActionResult> GetDtlInsert([FromBody] PurVo vo)
        {
            try
            {
                vo.FLR_FILE_ID = System.Guid.NewGuid().ToString();

                string _dirPath = @"C:\WebService\www.robocon.ai_file\";
                string filePath = _dirPath + vo.FLR_FILE_ID + "_" + vo.FLR_NM;

                if (Directory.Exists(_dirPath) == false)
                {
                    Directory.CreateDirectory(_dirPath);
                }

                File.WriteAllBytes(filePath, vo.FLR_FILE);
                if (File.Exists(filePath) == true)
                {
                    vo.FLR_FILE = new byte[0];
                    Properties.EntityMapper.Insert("P441106InsertDtl", vo);
                }
                else
                {
                    return Ok<string>("파일이 저장되지 않았습니다.");
                }

                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// 원재료 발주 관리 DTL - 수정
        ///// </summary>
        //[Route("dtl/u")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetDtlUpdate([FromBody] PurVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Update("P441106UpdateDtl", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        [Route("dtl/file")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDtlFileSelect([FromBody] PurVo vo)
        {
            try
            {
                byte[] temp = new byte[0];
                string _dirPath = @"C:\WebService\www.robocon.ai_file\";

                if (Directory.Exists(_dirPath) == true)
                {
                    string filePath = _dirPath + vo.FLR_FILE_ID + "_" + vo.FLR_NM;
                    if(File.Exists(filePath) == true)
                    {
                        temp = File.ReadAllBytes(filePath);
                        vo.FLR_FILE = temp;
                        vo.isSuccess = true;
                    }
                    else
                    {
                        vo.Message = "[ " + vo.FLR_NM + " ] 파일이 존재 하지 않습니다";
                        vo.isSuccess = false;
                        //return Ok<string>("[ " + vo.FLR_NM + " ] 파일이 존재 하지 않습니다");
                    }
                }
                else 
                {
                    vo.Message = "[ " + vo.FLR_NM + " ] 파일이 존재 하지 않습니다";
                    vo.isSuccess = false;
                    //return Ok<string>("[ " + vo.FLR_NM + " ] 파일이 존재 하지 않습니다");
                }
                return Ok<PurVo>(vo);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 고객사발주등록 DTL - 삭제(도면삭제)
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] PurVo vo)
        {
            try
            {
                string _dirPath = @"C:\WebService\www.robocon.ai_file\";
                if (Directory.Exists(_dirPath) == true)
                {
                    string filePath = _dirPath + vo.FLR_FILE_ID + "_" + vo.FLR_NM;
                    if (File.Exists(filePath) == true)
                    {
                        File.Delete(filePath);
                    }
                }

                return Ok<int>(Properties.EntityMapper.Delete("P441106DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 고객사발주등록 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody] PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P441106UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}