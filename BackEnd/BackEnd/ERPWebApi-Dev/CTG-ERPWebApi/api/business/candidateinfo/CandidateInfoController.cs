using DataFactories.Infrastructure.business.businessconfigure;
using DataFactories.Infrastructure.business.jobpost;
using DataModel.ViewModels.ERPViewModel.Business;
using DataModel.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using DataModel.ViewModels.ERPViewModel.Common;
using DataFactories.Infrastructure.business.workorder;
using DataFactories.Infrastructure.business.candidateinfo;

namespace CTG_ERPWebApi.api.business.candidateinfo
{
   // [Route("api/[controller]")]
[Route("api/[controller]"), Produces("application/json"), EnableCors("AppPolicy")]
[ApiController]
public class CandidateInfoController : ControllerBase
{
        private IWebHostEnvironment _hostingEnvironment;

        #region Variable Declaration & Initialization
        private CandidateInfoMgt _manager = null;
        private BusinessSetupMgt _srvManager = null;
        #endregion

        #region Constructor
        public CandidateInfoController(IWebHostEnvironment hostingEnvironment)
        {
           
            _manager = new CandidateInfoMgt();
            _srvManager = new BusinessSetupMgt();
            _hostingEnvironment = hostingEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        #endregion






        // GET: api/candidateinfo/getbypages
        [HttpGet("[action]")]//BasicAuthorization
        public async Task<object> getbypages([FromQuery] string param)
        {
             object result = null; object resdata = null;
            try
             {
                dynamic data = JsonConvert.DeserializeObject(param);
                vmCmnParameter cmnParam = JsonConvert.DeserializeObject<vmCmnParameter>(data[0].ToString());
                resdata = await _manager.GetWithPagination(cmnParam);
            }
            catch (Exception) { }
            return result = new
            {
                resdata
            };
        }

        [HttpGet("[action]")]//BasicAuthorization check
        public async Task<object> getallapplication([FromQuery] string param)
        {
            object result = null; object resdata = null;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                vmCmnParameter cmnParam = JsonConvert.DeserializeObject<vmCmnParameter>(data[0].ToString());
                resdata = await _manager.getallapplication(cmnParam);
            }
            catch (Exception) { }
            return result = new
            {
                resdata
            };
        }


        //Job Post  Get by id 
        // GET: api/candidateinfo/getbyid
        [HttpGet("[action]")]//BasicAuthorization
        public async Task<object> getbyid([FromQuery] string param)
        {
            object result = null; object resdata = null;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                vmCmnParameter cmnParam = JsonConvert.DeserializeObject<vmCmnParameter>(data[0].ToString());
                resdata = await _manager.GetByID(cmnParam);
            }
            catch (Exception) { }
            return result = new
            {
                resdata
            };
        }


      














    }


}
