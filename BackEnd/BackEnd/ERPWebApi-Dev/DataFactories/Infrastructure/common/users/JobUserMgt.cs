﻿using DataFactories.BaseFactory;
using DataFactories.Infrastructure.common.processflow;

//using DataModel.EntityModels.OraModel;
using DataModel.JobEntityModel.JobOraModelTest;
using DataModel.ViewModels;
using DataModel.ViewModels.ERPViewModel.Common;
//using DataModels.ViewModels.ERPViewModel.HRM.employee;
using DataUtility;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataFactories.Infrastructure.common.users
{
    public class JobUserMgt
    {
        #region Variable declaration & initialization
        //dbRGLERPContext _ctx = null;
        ModelContext _ctxOra = null;
        Hashtable ht = null;
        OracleParameter[] oprm = null;
        OracleCommand ocmd = null;
        IGenericFactory<vmCmnParameters> Generic_vmCmnParameters = null;
        IGenericFactoryOracle<vmCmnParameters> OraGeneric_vmCmnParameters = null;
        private IGenericFactoryOracle<vmCmnParameter> OraGeneric_vmCmnParameter = null;
        //IGenericFactoryOracle<Cmnuser> OraGeneric_Cmnuser = null;
        IGenericFactoryOracle<vmCmnuser> OraGeneric_vmCmnuser = null;
        //IGenericFactoryEF<Cmnuser> Cmnuser_EF = null;
        #endregion

        #region All Methods





        //------------------------------------------End-----------------------------------

        public async Task<object> GetWithPagination(vmCmnParameter param) //vmCmnParameters cmnParam
        {
            OraGeneric_vmCmnParameters = new GenericFactoryOracle<vmCmnParameters>();
            string listUser = string.Empty;
            object result = null;
            try
            {
                ht = new Hashtable
                {
                    { "cresult", (0, OracleDbType.RefCursor, ParameterDirection.Output) },
                    { "PageNumber", (1, Convert.ToDecimal(param.pageNumber))},
                    { "PageSize", (2, Convert.ToDecimal(param.pageSize)) }
                };

                listUser = await OraGeneric_vmCmnParameters.ExecuteCommandString(StoredProcedure.Ora_SpGet_CmnUser, ht, StaticInfos.conStringOracle.ToString());
            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return result = new
            {
                listUser
            };
        }

        public async Task<string> getJobIdByMail(string strId)
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string empDetails = string.Empty;
            try
            {
                var PrcsFlowMgt = new ProcessFlowMgt();
                ocmd = new OracleCommand();
                ocmd.Parameters.Add("gresult", OracleDbType.Varchar2, 150).Direction = ParameterDirection.Output;
                ocmd.Parameters.Add("gLogedUserId", OracleDbType.Varchar2).Value = strId;

                //gQuotationId
                empDetails = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutString(StoredProcedure.Ora_SpGet_EmpDetails, ocmd, "gresult", StaticInfos.conStringOracle.ToString());

            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return empDetails;
        }





        public class vmCmnuser
        {
            public decimal USERID { get; set; }
            public string FIRSTNAME { get; set; }
            public string LASTNAME { get; set; }
            public string PHONE { get; set; }
        }


        public async Task<object> VerifyUser(object data)
        {
            object result = null; object loggeduser = null, resdatas = null; string message = string.Empty; bool resstate = false;
            dynamic loggeddata = JObject.Parse(data.ToString());
            string userId = loggeddata.EmpID.ToString();
            string password = loggeddata.UserPassw.ToString();
            string isredirect = loggeddata.IsRedirect.ToString();
            TSysUser SysUser = null;
            if (isredirect == "1")
            {
                string usrName = await getJobIdByMail(userId);
                using (_ctxOra = new ModelContext())
                {
                    loggeduser = await (from ur in _ctxOra.TUserRoles
                                        join r in _ctxOra.TRoleSetups on ur.RoleId equals r.Roleid
                                        where ur.UserId == userId
                                        select new
                                        {
                                            userId = ur.UserId,
                                            password = password,
                                            roleId = ur.RoleId,
                                            rolename = r.Rolename,
                                            displayName = usrName,
                                            fullName = usrName,
                                            //email = string.Empty,
                                            //designation = string.Empty,
                                            //photo = string.Empty,
                                            isSys = false
                                        }
                          ).FirstOrDefaultAsync();
                }

                message = MessageConstants.LoginSuccess;
                resstate = MessageConstants.SuccessState;
                
            }
            else
            {
                using (_ctxOra = new ModelContext())
                {
                    //SysUser = await _ctxOra.TSysUsers.Where(x => x.Userid.ToLower() == userId.Trim().ToLower() && x.Password == password).FirstOrDefaultAsync();
                    SysUser = await _ctxOra.TSysUsers.Where(x => x.Userid == userId && x.Password == password).FirstOrDefaultAsync();
                }

                if (SysUser != null)
                {
                    using (_ctxOra = new ModelContext())
                    {
                        loggeduser = await (from ur in _ctxOra.TUserRoles
                                            join r in _ctxOra.TRoleSetups on ur.RoleId equals r.Roleid
                                            where ur.UserId.ToLower() == userId.Trim().ToLower()
                                            select new
                                            {
                                                userId = ur.UserId,
                                                password = password,
                                                roleId = ur.RoleId,
                                                rolename = r.Rolename,
                                                displayName = SysUser.Username,
                                                fullName = SysUser.Fullname,
                                                //email=SysUser.Email,
                                                //designation=SysUser.Designation,
                                                //photo=SysUser.Photo,
                                                isSys = true
                                            }
                                  ).FirstOrDefaultAsync();
                    }

                    message = MessageConstants.LoginSuccess;
                    resstate = MessageConstants.SuccessState;
                }
                else
                {
                    using (var httpClientHandler = new HttpClientHandler())
                    {
                        httpClientHandler.ServerCertificateCustomValidationCallback = (messages, cert, chain, errors) => { return true; };
                        using (var objClient = new HttpClient(httpClientHandler))
                        {
                            var serializedPackage = JsonConvert.SerializeObject(loggeddata);
                            var content = new StringContent(serializedPackage, Encoding.UTF8, StaticInfos.contentTypeJson);
                            //objClient.DefaultRequestHeaders.TryAddWithoutValidation(StaticInfos.authorization, token);
                            using (HttpResponseMessage resapidata = await objClient.PostAsync(StaticInfos.loginUrl, content))
                            {
                                resdatas = resapidata.Content.ReadAsStringAsync().Result;
                                if (resapidata.IsSuccessStatusCode)
                                {
                                    string resdata = JsonConvert.DeserializeObject(resdatas.ToString()).ToString();
                                    string[] spdata = resdata.ToString().Split('~');

                                    if (spdata[0] == "1")
                                    {
                                        using (_ctxOra = new ModelContext())
                                        {
                                            loggeduser = await (from ur in _ctxOra.TUserRoles
                                                                join r in _ctxOra.TRoleSetups on ur.RoleId equals r.Roleid
                                                                where ur.UserId == userId
                                                                select new
                                                                {
                                                                    userId = ur.UserId,
                                                                    password = password,
                                                                    roleId = ur.RoleId,
                                                                    rolename = r.Rolename,
                                                                    displayName = spdata[1],
                                                                    fullName = spdata[1],
                                                                    //email = string.Empty,
                                                                    //designation = string.Empty,
                                                                    //photo = string.Empty,
                                                                    isSys = false
                                                                }
                                                      ).FirstOrDefaultAsync();
                                        }

                                        message = MessageConstants.LoginSuccess;
                                        resstate = MessageConstants.SuccessState;
                                    }
                                    else
                                    {
                                        message = spdata[0].ToString();
                                        resstate = MessageConstants.ErrorState;
                                    }
                                }
                                else
                                {
                                    message = MessageConstants.LoginFailed;
                                    resstate = MessageConstants.ErrorState;
                                }
                            }
                        }
                    }
                }
            }

            result = new
            {
                message,
                loggeduser,
                resstate
            };

            return result;
        }

        public async Task<object> LoggedUserDetails(vmCmnParameter param)
        {
            object result = null; string data = null; string Message = string.Empty; bool resstate = false;

            string getUrl = StaticInfos.loggedInfoUrl + param.UserID;

            if (param.IsTrue)
            {
                using (_ctxOra = new ModelContext())
                {
                    var dd = await (from su in _ctxOra.TSysUsers
                                    where su.Userid.ToLower() == param.UserID.Trim().ToLower()
                                    select new
                                    {
                                        EMP_OID = string.Empty,
                                        EMP_ID = su.Userid,
                                        EMP_NAME = su.Fullname,
                                        EMP_DSIG = su.Designation,
                                        LOC_OID = string.Empty, //null
                                        LOC_NAME = string.Empty, //null
                                        COMP_OID = string.Empty, //null
                                        COMP_NAME = "Corporate Head Office",
                                        DEPT_OID = string.Empty, //null
                                        DEPT_NAME = "IT Department",
                                        EMP_JOB_LOCATION = "Head Office",
                                        EMP_PHOTO = su.Photo,
                                        EMP_VALIDITY = string.Empty, //null
                                        EMP_JOIN_DATE = "2021-09-01T00:00:00",
                                        EMP_BIRTH_DATE = "1988-01-29T00:00:00",
                                        EMP_GRADE = "4A",
                                        EMP_BLOOD_GROUP = "A+",
                                        /*OFFICE_MAIL = "soft@citygroupbd.com",
                                        MOBILE_NO = string.Empty, //null*/
                                        OFFICE_MAIL = su.Email,
                                        MOBILE_NO = su.MobileNumber, //null
                                        LOS = 0.0,
                                        AGE = 0.0,
                                        SALARY = 0.0,
                                        IS_VOUCHER_PRINTED = string.Empty, //null
                                        LOS_CG = string.Empty, //null
                                        LOS_CP = string.Empty, //null
                                        LAST_YEAR_APPR_GRADE = string.Empty, //null
                                        LAST_YEAR_INCR_AMNT = 0.0,
                                        CURRENT_SALARY = 0.0,
                                        PROB_STATUS = string.Empty, //null
                                        LAST_YEAR_PROMOTION = string.Empty, //null
                                        INCR_GRAD = string.Empty, //null
                                        EDU = string.Empty, //null
                                        INCR_PRCT = 0.0,
                                        PROMO_PRCT = 0.0,
                                        INCR_MBCP = 0.0,
                                        PR_FLG = string.Empty, //null
                                        NEW_GRAD = string.Empty, //null
                                        NEW_DSIG_NAME = string.Empty, //null
                                        INCR_RMKS = string.Empty, //null
                                        INCR_AMNT = 0.0,
                                        BASIC = 0.0,
                                        HR = 0.0,
                                        OTHER = 0.0,
                                        CONS = 0.0,
                                        PRCNT_14 = 0.0,
                                        PRCNT_15 = 0.0,
                                        PRCNT_16 = 0.0,
                                        PRCNT_17 = 0.0,
                                        PRCNT_18 = 0.0,
                                        PRCNT_19 = 0.0,
                                        PRCNT_20 = 0.0,
                                        PRCNT_21 = 0.0,
                                        SAL_14 = 0.0,
                                        SAL_15 = 0.0,
                                        SAL_16 = 0.0,
                                        SAL_17 = 0.0,
                                        SAL_18 = 0.0,
                                        SAL_19 = 0.0,
                                        SAL_20 = 0.0,
                                        SAL_21 = 0.0,
                                        INCR_14 = 0.0,
                                        INCR_15 = 0.0,
                                        INCR_16 = 0.0,
                                        INCR_17 = 0.0,
                                        INCR_18 = 0.0,
                                        INCR_19 = 0.0,
                                        INCR_20 = 0.0,
                                        INCR_21 = 0.0,
                                        D_YR = 0.0,
                                        D_MN = 0.0,
                                        D_DAY = 0.0

                                    }
                              ).FirstOrDefaultAsync();

                    data = JsonConvert.SerializeObject(dd);
                    resstate = MessageConstants.SuccessState;
                }
            }
            else
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    using (var objClient = new HttpClient(httpClientHandler))
                    {
                        //objClient.DefaultRequestHeaders.TryAddWithoutValidation(StaticInfos.authorization, token);
                        using (var resapidata = await objClient.GetAsync(getUrl))
                        {
                            if (resapidata.IsSuccessStatusCode)
                            {
                                var jsonString = resapidata.Content.ReadAsStringAsync().Result;
                                data = jsonString;
                                resstate = MessageConstants.SuccessState;
                            }
                            else
                            {
                                //Message = resapidata.ToString();
                                resstate = MessageConstants.ErrorState;
                            }
                        }
                    }
                }
            }

            result = new
            {
                data,
                resstate
            };

            return result;
        }




        public async Task<object> SaveUpdate(string _JsonData, vmCmnParameter param)
        {
            string message = string.Empty; bool resstate = false;
            OraGeneric_vmCmnParameters = new GenericFactoryOracle<vmCmnParameters>();
            string result = string.Empty;
            try
            {
                ocmd = new OracleCommand();
                ocmd.Parameters.Add("mresult", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
                ocmd.Parameters.Add("JsonData", OracleDbType.Clob).Value = _JsonData;
                ocmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2).Value = param.LoggedUserId;
                ocmd.Parameters.Add("mCreatePC", OracleDbType.Varchar2).Value = Extension.Createpc();
                result = await OraGeneric_vmCmnParameters.ExecuteNonQueryOutString(StoredProcedure.Ora_SpSet_CmnUser, ocmd, "mresult", StaticInfos.conStringOracle.ToString());

                if (!string.IsNullOrEmpty(result) && result != "0")
                {
                    message = result == "-1" ? "User already exists!!!" : MessageConstants.Saved;
                    resstate = MessageConstants.SuccessState;
                }
                else
                {
                    message = MessageConstants.SavedWarning;
                }
            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                result,
                message,
                resstate
            };
        }


        public async Task<object> GetEmployeeDetailByID(string EmployeeId)
        {
            OraGeneric_vmCmnParameters = new GenericFactoryOracle<vmCmnParameters>();
            string empDetail = string.Empty;
            try
            {
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.RefCursor, ParameterDirection.Output) },
                    { "gEmployeeId", (1, EmployeeId) }
                };

                empDetail = await OraGeneric_vmCmnParameters.ExecuteCommandString(StoredProcedure.Ora_SpGet_EmployeeById, ht, StaticInfos.conStringOracle.ToString());
            }
            catch (Exception ex)
            {
                ex.ToString();
                Logs.Bug(ex);
            }
            return new
            {
                empDetail
            };
        }

        #endregion
    }
}
