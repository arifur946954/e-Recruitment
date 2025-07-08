﻿using DataFactories.DBService;
using DataFactories.Infrastructure.common.users;
//using DataModels.EntityModels.ERPModel;
using DataModel.ViewModels;
using DataModel.ViewModels.ERPViewModel.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTG_ERPWebApi.api.common.user
{
    [Route("api/[controller]"), Produces("application/json"), EnableCors("AppPolicy")]
    [ApiController]
    public class jobusersController : ControllerBase
    {
        #region Variable Declaration & Initialization
        private JobUserMgt _manager = null;
        #endregion

        #region Constructor
        public jobusersController()
        {
            _manager = new JobUserMgt();
        }
        #endregion

        #region All Http Methods




        // GET: api/users/getbypage
        [HttpGet("[action]"), BasicAuthorization]
        public async Task<object> getbypage([FromQuery] string param)
        {
            object result = null; object resdata = null;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                //var cmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                var cmnParam = JsonConvert.DeserializeObject<vmCmnParameter>(data[0].ToString());
                resdata = await _manager.GetWithPagination(cmnParam);
            }
            catch (Exception) { }
            return result = new
            {
                resdata
            };
        }


        // Post: api/users/login
        [HttpPost("[action]")]
        public async Task<object> login([FromBody] object[] data)
 {
            object result = null; object resdata = null;
            try
            {
                resdata = await _manager.VerifyUser(data[0]);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result = new
            {
                resdata
            };
        }

        // GET: api/users/loggeduserdetails
        [HttpGet("[action]")]
        public async Task<object> loggeduserdetails([FromQuery] string param)
        {
            object result = null; object resdata = null;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                vmCmnParameter cmnParam = JsonConvert.DeserializeObject<vmCmnParameter>(data[0].ToString());
                resdata = await _manager.LoggedUserDetails(cmnParam);
            }
            catch (Exception) { }
            return result = new
            {
                resdata
            };
        }




        // POST: api/menu/saveupdate
        [HttpPost("[action]")]//, BasicAuthorization
        public async Task<object> saveupdate([FromBody] object[] data)
        {
            object result = null; object resdata = null;
            try
            {
                vmCmnParameter cparam = JsonConvert.DeserializeObject<vmCmnParameter>(data[0].ToString());
                string mstr = data[1].ToString();
                if (mstr != null)
                {
                    resdata = await _manager.SaveUpdate(mstr, cparam);
                }
            }
            catch (Exception) { }

            return result = new
            {
                resdata
            };
        }

        // GET: api/jobusers/getemployeedetailbyid
        [HttpGet("[action]")]//BasicAuthorization
        public async Task<object> getemployeedetailbyid([FromQuery] string param)
        {
            object result = null; object resdata = null;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                vmCmnParameter cmnParam = JsonConvert.DeserializeObject<vmCmnParameter>(data[0].ToString());
                resdata = await _manager.GetEmployeeDetailByID(cmnParam.strId);
            }
            catch (Exception) { }
            return result = new
            {
                resdata
            };
        }

        #endregion
    }
}