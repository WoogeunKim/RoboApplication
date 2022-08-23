using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using ModelsLibrary.Code;
using ModelsLibrary.Fproof;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Fproof
{
    [Route("api/PurRegWebApi/{action}", Name = "PurRegWebApi")]
    public class PurRegWebApiController : ApiController
    {
        /// <summary>
        /// [WebApi]원재료 매입등록 - 조회
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetMstSelect(string CHNL_CD, string CRE_USR_ID, DataSourceLoadOptions loadOptions)
        {
            return Request.CreateResponse(DataSourceLoader.Load(Properties.EntityMapper.QueryForList<FproofVo>("SelectPurRegMstList", new FproofVo() { CHNL_CD = CHNL_CD, CRE_USR_ID = CRE_USR_ID }), loadOptions));
        }
        /// <summary>
        /// [WebApi]원재료 매입등록 - 추가
        /// </summary>
        [HttpPost]
        public HttpResponseMessage GetMstInsert(FormDataCollection form)
        {
            var values = form.Get("values");

            var newOrder = new FproofVo();
            JsonConvert.PopulateObject(values, newOrder);

            Validate(newOrder);
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "저장 실패 했습니다");
            }

            //저장
            if(Properties.EntityMapper.Insert("InsertPurRegMst", newOrder) != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "저장 실패 했습니다");
            }
            return Request.CreateResponse(HttpStatusCode.Created, newOrder);
        }
        /// <summary>
        /// [WebApi]원재료 매입등록 - 수정
        /// </summary>
        [HttpPut]
        public HttpResponseMessage GetMstUpdate(FormDataCollection form)
        {
            var key = form.Get("key");
            var values = form.Get("values");

            var employee = new FproofVo();
            JsonConvert.PopulateObject(values, employee);

            Validate(employee);
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "저장 실패 했습니다");
            }

            employee.MTRL_LOT_NO = key;
            //저장
            if (Properties.EntityMapper.Update("UpdatePurRegMst", employee) == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "저장 실패 했습니다");
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }











        /// <summary>
        /// [WebApi]원료  투입 - 조회
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetDtlSelect(string CHNL_CD, string CRE_USR_ID, string PROC_LOT_NO,  DataSourceLoadOptions loadOptions)
        {
            if (string.IsNullOrEmpty(PROC_LOT_NO))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "PROC_LOT_NO 선택 해 주세요");
            }

            return Request.CreateResponse(DataSourceLoader.Load(Properties.EntityMapper.QueryForList<FproofVo>("SelectMaterialInputMstList", new FproofVo() { CHNL_CD = CHNL_CD, CRE_USR_ID = CRE_USR_ID, PROC_LOT_NO = PROC_LOT_NO }), loadOptions));
        }
        /// <summary>
        /// [WebApi]원료 투입 - 추가
        /// </summary>
        [HttpPost]
        public HttpResponseMessage GetDtlInsert(FormDataCollection form)
        {
            var values = form.Get("values");

            var newOrder = new FproofVo();
            JsonConvert.PopulateObject(values, newOrder);

            Validate(newOrder);
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "저장 실패 했습니다");
            }

            //저장
            if (Properties.EntityMapper.Insert("InsertMaterialInputMst", newOrder) != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "저장 실패 했습니다");
            }
            return Request.CreateResponse(HttpStatusCode.Created, newOrder);
        }
        /// <summary>
        /// [WebApi]원료 투입 - 삭제
        /// </summary>
        [HttpDelete]
        public HttpResponseMessage GetDtlDelete(FormDataCollection form)
        {
            var values = form.Get("key");

           string[] parms = values.Split('_');

            FproofVo newOrder = new FproofVo();
            newOrder.CHNL_CD = parms[0];
            newOrder.PROC_LOT_NO = parms[1];
            newOrder.PROC_LOT_SEQ = Convert.ToInt16(parms[2]);
            newOrder.PRNT_LOT_NO = parms[3];

            //JsonConvert.PopulateObject(values, newOrder);

            //Validate(newOrder);
            //if (!ModelState.IsValid)
            //{
            //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "삭제 실패 했습니다");
            //}

            //삭제
            if (Properties.EntityMapper.Delete("DeleteMaterialInputMst", newOrder) == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "삭제 실패 했습니다");
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// [WebApi]원료 투입 - 수정
        /// </summary>
        [HttpPut]
        public HttpResponseMessage GetDtlUpdate(FormDataCollection form)
        {

            var values = form.Get("values");

            FproofVo newOrder = new FproofVo();
            JsonConvert.PopulateObject(values, newOrder);

            var keys = form.Get("key");

            string[] parms = keys.Split('_');

            newOrder.CHNL_CD = parms[0];
            newOrder.PROC_LOT_NO = parms[1];
            newOrder.PROC_LOT_SEQ = Convert.ToInt16(parms[2]);
            newOrder.PRNT_LOT_NO = parms[3];

            //JsonConvert.PopulateObject(values, newOrder);

            //Validate(newOrder);
            //if (!ModelState.IsValid)
            //{
            //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "저장 실패 했습니다");
            //}

            //저장
            if (Properties.EntityMapper.Update("UpdateMaterialInputMst", newOrder) != 1)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "저장 실패 했습니다");
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }









        /// <summary>
        /// [WebApi]원재료 매입등록 - 품명
        /// </summary>
        [HttpGet]
        public HttpResponseMessage ItmNmLookup(string CHNL_CD, DataSourceLoadOptions loadOptions)
        {
            //var lookup = from i in _nwind.Customers
            //             let text = i.CompanyName + " (" + i.Country + ")"
            //             orderby i.CompanyName
            //             select new
            //             {
            //                 Value = i.CustomerID,
            //                 Text = text
            //             };
            return Request.CreateResponse(DataSourceLoader.Load(Properties.EntityMapper.QueryForList<SystemCodeVo>("S141SelectItemList", new SystemCodeVo() { CHNL_CD = CHNL_CD, DELT_FLG = "N"/*, ITM_GRP_CLSS_CD = "M"*/ }), loadOptions));
        }

        /// <summary>
        /// [WebApi]원재료 매입등록 - 매입처
        /// </summary>
        [HttpGet]
        public HttpResponseMessage CoNmLookup(string CHNL_CD, DataSourceLoadOptions loadOptions)
        {
            //var lookup = from i in _nwind.Customers
            //             let text = i.CompanyName + " (" + i.Country + ")"
            //             orderby i.CompanyName
            //             select new
            //             {
            //                 Value = i.CustomerID,
            //                 Text = text
            //             };
            return Request.CreateResponse(DataSourceLoader.Load(Properties.EntityMapper.QueryForList<SystemCodeVo>("S143SelectCodeList", new SystemCodeVo() { CHNL_CD = CHNL_CD }), loadOptions));
        }













        /// <summary>
        /// [WebApi]설비 통합 모니터링 - 조회
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetEqMstSelect(string CHNL_CD, DataSourceLoadOptions loadOptions)
        {
            return Request.CreateResponse(DataSourceLoader.Load(Properties.EntityMapper.QueryForList<FproofVo>("SelectEqMstList", new FproofVo() { CHNL_CD = CHNL_CD }), loadOptions));
        }

        ///// <summary>
        ///// [WebApi]설비 통합 모니터링 - 조회
        ///// </summary>
        //[HttpGet]
        //public HttpResponseMessage GetEqMonitoringSelect(string SENSOR_NO, DataSourceLoadOptions loadOptions)
        //{
        //    return Request.CreateResponse(DataSourceLoader.Load(Properties.EntityMapper.QueryForList<FproofVo>("SelectEqMonitoringList", new FproofVo() { /*SENSOR_NO = SENSOR_NO*/ }), loadOptions));
        //}


        /// <summary>
        /// [WebApi]설비 통합 모니터링 - LOT NO
        /// </summary>
        [HttpGet]
        public HttpResponseMessage ProcLotNoookup(string CHNL_CD, DataSourceLoadOptions loadOptions)
        {
            //var lookup = from i in _nwind.Customers
            //             let text = i.CompanyName + " (" + i.Country + ")"
            //             orderby i.CompanyName
            //             select new
            //             {
            //                 Value = i.CustomerID,
            //                 Text = text
            //             };
            return Request.CreateResponse(DataSourceLoader.Load(Properties.EntityMapper.QueryForList<FproofVo>("SelectProcLotNoList", new FproofVo() { CHNL_CD = CHNL_CD, FM_DT = System.DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"), TO_DT = System.DateTime.Now.ToString("yyyyMMdd") }), loadOptions));
        }



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
    }
}