using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("S22227")]
    public class S22227Controller : ApiController
    {
        /// <summary>
        /// 주문별 생산공장/GR확정 MST - 발주번호, 고객사 조회
        /// </summary>
        [Route("mst/co")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstLeftSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22227LeftTopMstSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 주문별 생산공장/GR확정 DTL - 발주번호별 DTL조회
        /// </summary>
        [Route("dtl/right/top")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlRightTopSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22227RightTopDtlSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 주문별 생산공장/GR확정 DTL - GR번호, LOC번호 확정리스트 조회
        /// </summary>
        [Route("dtl/left/bottom")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlLeftBottomSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22227LeftBottomDtlSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 주문별 생산공장/GR확정 DTL - 공사별 DTL조회
        /// </summary>
        [Route("dtl/right/bottom")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlRightBottomSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22227RightBottomDtlSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 주문별 생산공장/GR확정 DTL - 공사별 DTL조회
        /// </summary>
        [Route("dtl/grlist")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlGrListSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22227GrListDtlSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 주문별 생산공장/GR확정 - GR 신규번호 불러오기
        /// </summary>
        [Route("dtl/gr/new")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlGrNo([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<string>(Properties.EntityMapper.QueryForObject<string>("S22227SelectGRNo", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 주문별 생산공장/GR확정 DTL - GR 신규번호 부여 및 리스트 묶음
        /// </summary>
        [Route("dtl/gr/new/u")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDtlGrNewUpdate([FromBody] IList<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                // 새로운 GR번호 생성
                string _RLSE_CMD_NO = Properties.EntityMapper.QueryForObject<string>("S22227SelectGRNo", voList[0]);

                foreach (SaleVo _item in voList)
                {
                    // 현재 데이터 리스트에 GR번호 부여
                    _item.RLSE_CMD_NO = _RLSE_CMD_NO;
                    Properties.EntityMapper.Update("S22227UpdateGRApply", _item);
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
        /// 주문별 생산공장/GR확정 DTL - GR 기존번호 부여 및 리스트 묶음에 포함
        /// </summary>
        [Route("dtl/gr/orgin/u")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDtlGrOriginUpdate([FromBody] IList<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                foreach (SaleVo _item in voList)
                {
                    Properties.EntityMapper.Update("S22227UpdateGRApply", _item);
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
        /// 주문별 생산공장/GR확정- LOC창고 콤보박스 조회
        /// </summary>
        [Route("pop/loc")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopLocSelect([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22227SelectLocList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 주문별 생산공장/GR확정- LOC창고 업데이트
        /// </summary>
        [Route("pop/loc/u")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopLocUpdate([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S22227UpdateLoc", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 주문별 생산공장/GR확정 - GR확정 리스트에 있는 GR번호 삭제
        /// </summary>
        [Route("dtl/gr/d")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlGrDelete([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S22227UpdateGR_RollBack", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 주문별 생산공장/GR확정 - GR확정 리스트에 있는 창고LOC 삭제
        /// </summary>
        [Route("dtl/loc/d")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlLocDelete([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S22227UpdateLoc_RollBack", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
    }
}