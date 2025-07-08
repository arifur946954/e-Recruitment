using DataFactories.BaseFactory;
using DataFactories.Infrastructure.common.processflow;
using DataModel.EntityModels.OraModel;
using DataModel.ViewModels;
using DataModel.ViewModels.ERPViewModel.Business;
using DataModel.ViewModels.ERPViewModel.Common;
using DataUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace DataFactories.Infrastructure.business.reqform
{
    public class ReqFormMgt
    {
        #region Variable declaration & initialization
        ModelContext _ctxOra = null;
        private IGenericFactoryOracle<vmCmnParameter> OraGeneric_vmCmnParameter = null;
        OracleCommand ocmd = null;
        Hashtable ht = null; OracleParameter[] oprm = null; Hashtable gt = null;
        #endregion



        public async Task<object> GetUserDetailsByID(vmCmnParameter cparam)//GetApplicantProfileByID
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string userDetails = string.Empty;
            try
            {
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gLogedUserId", (1, OracleDbType.Varchar2, cparam.strId)}
                };
                userDetails = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_LoggedUserDetails, ht, "gresult", StaticInfos.conStringOracle.ToString());

            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                userDetails

            };
        }


        public async Task<object> GetApplicantProfileByID(vmCmnParameter cparam)
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string userDetails = string.Empty;
            try
            {
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gLogedUserId", (1, OracleDbType.Varchar2, cparam.strId)}
                };
                userDetails = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_ApplicantProfileDetails, ht, "gresult", StaticInfos.conStringOracle.ToString());

            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                userDetails

            };
        }

        /*        //START ------------------------------------------------------
                public List<dynamic> GetDistrict(string DivisionId)
                {
                    OracleCommand objCmd = new OracleCommand();
                    objCmd.CommandText = "hr_job_app.Address_Pkg.GetDistrictList";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_OID", OracleDbType.Varchar2).Value = DivisionId;
                    //ConvertDataTableToGeniric classDt = new ConvertDataTableToGeniric();
                    DataTable dt =GetData(objCmd);

                    List<dynamic> list = new List<dynamic>();
                    foreach (DataRow row in dt.Rows)
                    {
                        dynamic item = new ExpandoObject();
                        foreach (DataColumn col in dt.Columns)
                        {
                            ((IDictionary<string, object>)item)[col.ColumnName] = row[col];
                        }
                        list.Add(item);
                    }
                    return list;
                }


                //RETRIVE Thana BY ID
                public List<dynamic> GetThanaList(string DistrictID)
                {
                    OracleCommand objCmd = new OracleCommand();
                    objCmd.CommandText = "hr_job_app.Address_Pkg.GetUpzlList";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_OID", OracleDbType.Varchar2).Value = DistrictID;
                    //ConvertDataTableToGeniric classDt = new ConvertDataTableToGeniric();
                    DataTable dt = GetData(objCmd);

                    List<dynamic> list = new List<dynamic>();
                    foreach (DataRow row in dt.Rows)
                    {
                        dynamic item = new ExpandoObject();
                        foreach (DataColumn col in dt.Columns)
                        {
                            ((IDictionary<string, object>)item)[col.ColumnName] = row[col];
                        }
                        list.Add(item);
                    }
                    return list;
                }


                //ARIF EXTRRA---------------------START
                private string _connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=erpprd)(PORT=1525))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=hrdpdb)));User ID=hr_job_app;Password=hr_job_it;Persist Security Info=True";
                public DataTable GetData(OracleCommand objCmd)
                {
                    using (OracleConnection con = new OracleConnection(_connectionString))
                    {
                        con.Open();
                        DataTable dt = new DataTable();
                        using (OracleDataAdapter sda = new OracleDataAdapter())
                        {
                            objCmd.Connection = con;
                            sda.SelectCommand = objCmd;
                            sda.Fill(dt);
                            con.Close();
                            return dt;
                        }

                    }
                }

                //ARIF END ---------------------------------------------------*/

        //END------------------------------------------------------------------------------------
        public async Task<object> GetDistrictByID(vmCmnParameter cparam)
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string districtList = string.Empty;
            try
            {
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gJobDivId", (1, OracleDbType.Varchar2, cparam.id)}
                };
                districtList = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGetDistrict, ht, "gresult", StaticInfos.conStringOracle.ToString());

            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                districtList

            };
        }


        public async Task<object> GetThanaByID(vmCmnParameter cparam)
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string thanaList = string.Empty;
            try
            {
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gJobDisId", (1, OracleDbType.Varchar2, cparam.id)}
                };
                thanaList = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGetThana, ht, "gresult", StaticInfos.conStringOracle.ToString());
            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                thanaList

            };
        }






        public async Task<object> SaveUpdate(string _mJsonData, string _dJsonData, string _tcJsonData,string _proCirtificate, vmCmnParameter param)
    {
        object referenceId = 0; string message = string.Empty; bool resstate = false;
        OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
        string result1 = string.Empty, mstrRes = string.Empty;string result2 = string.Empty; string result3 = string.Empty; string result4 = string.Empty;
            try
        {
            
            ocmd = new OracleCommand();
            ocmd.Parameters.Add("mresult", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
            ocmd.Parameters.Add("JsonData_Mstr", OracleDbType.Clob).Value = _mJsonData;
            ocmd.Parameters.Add("JsonData_TCN", OracleDbType.Clob).Value = _tcJsonData;
            ocmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2).Value = param.LoggedUserId;
            ocmd.Parameters.Add("mCreatePC", OracleDbType.Varchar2).Value = Extension.Createpc();

            mstrRes = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutString(StoredProcedure.Ora_SpSet_ApplicantFormMaster, ocmd, "mresult", StaticInfos.conStringOracle.ToString());

                if(!string.IsNullOrEmpty(mstrRes) && !string.IsNullOrEmpty(param.JobOid) && string.IsNullOrEmpty(param.mstrOid)){
                    ocmd = new OracleCommand();
                    ocmd.Parameters.Add("mresult", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
                    ocmd.Parameters.Add("Email", OracleDbType.Varchar2).Value = param.LoggedUserId;
                    ocmd.Parameters.Add("JobId", OracleDbType.Varchar2).Value = param.JobOid;
                    ocmd.Parameters.Add("ProfileId", OracleDbType.Varchar2).Value = mstrRes;
                    ocmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2).Value = param.LoggedUserId;
                    result4 = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutString(StoredProcedure.Ora_SpSetApplyApplicant, ocmd, "mresult", StaticInfos.conStringOracle.ToString());
                }
            if (!string.IsNullOrEmpty(mstrRes) && mstrRes != "null")
            {
                ocmd = new OracleCommand();
                ocmd.Parameters.Add("mresult", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
                ocmd.Parameters.Add("Mstr_OID", OracleDbType.Varchar2).Value = mstrRes;
                ocmd.Parameters.Add("JsonData_Dtl", OracleDbType.Clob).Value = _dJsonData;
                ocmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2).Value = param.LoggedUserId;
                ocmd.Parameters.Add("mCreatePC", OracleDbType.Varchar2).Value = Extension.Createpc();
                  result1 = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutString(StoredProcedure.Ora_SpSet_ApplicantFormAcademicQlf, ocmd, "mresult", StaticInfos.conStringOracle.ToString());
                 
           
            }

                if (!string.IsNullOrEmpty(mstrRes) && mstrRes != "null")
                {
                    ocmd = new OracleCommand();
                    ocmd.Parameters.Add("mresult", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
                    ocmd.Parameters.Add("Mstr_OID", OracleDbType.Varchar2).Value = mstrRes;
                    ocmd.Parameters.Add("JsonData_Dtl", OracleDbType.Clob).Value = _tcJsonData;
                    ocmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2).Value = param.LoggedUserId;
                    ocmd.Parameters.Add("mCreatePC", OracleDbType.Varchar2).Value = Extension.Createpc();
                    result2 = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutString(StoredProcedure.Ora_SpSet_ApplicantFormWorkExp, ocmd, "mresult", StaticInfos.conStringOracle.ToString());
                }

                if (!string.IsNullOrEmpty(mstrRes) && mstrRes != "null")
                {
                    ocmd = new OracleCommand();
                    ocmd.Parameters.Add("mresult", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
                    ocmd.Parameters.Add("Mstr_OID", OracleDbType.Varchar2).Value = mstrRes;
                    ocmd.Parameters.Add("JsonData_Dtl", OracleDbType.Clob).Value = _proCirtificate;
                    ocmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2).Value = param.LoggedUserId;
                    ocmd.Parameters.Add("mCreatePC", OracleDbType.Varchar2).Value = Extension.Createpc();
                    result3 = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutString(StoredProcedure.Ora_SpSet_ApplicantProfCirtifacate, ocmd, "mresult", StaticInfos.conStringOracle.ToString());
                }




                if (!string.IsNullOrEmpty(mstrRes) && mstrRes != "0" && mstrRes != "null")
            {

                    message = MessageConstants.Saved;
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
            message,
            resstate
        };
    }




        public async Task<object> SaveUpdateMessage(string _mJsonData, vmCmnParameter param)
        {
            object referenceId = 0; string message = string.Empty; bool resstate = false;
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string result1 = string.Empty, mstrRes = string.Empty; string result2 = string.Empty; string result3 = string.Empty;
            try
            {

                ocmd = new OracleCommand();
                ocmd.Parameters.Add("mresult", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
                ocmd.Parameters.Add("JsonData_Mstr", OracleDbType.Varchar2,3000).Value = _mJsonData;
               // ocmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2).Value = param.LoggedUserId;
                //ocmd.Parameters.Add("mCreatePC", OracleDbType.Varchar2).Value = Extension.Createpc();

                mstrRes = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutString(StoredProcedure.Ora_SpSetUpdateMessage, ocmd, "mresult", StaticInfos.conStringOracle.ToString());
              
                if (!string.IsNullOrEmpty(mstrRes) && mstrRes != "0" && mstrRes != "null")
                {

                    message = MessageConstants.Saved;
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
                message,
                resstate
            };
        }

        public async Task<object> saveApplicantJobApply(vmCmnParameter param)
        {
            object referenceId = 0; string message = string.Empty; bool resstate = false;
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string result1 = string.Empty, mstrRes = string.Empty; string result2 = string.Empty; string result3 = string.Empty;
            try
            {

                ocmd = new OracleCommand();
                ocmd.Parameters.Add("mresult", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
                ocmd.Parameters.Add("Email", OracleDbType.Varchar2).Value = param.LoggedUserId;
                ocmd.Parameters.Add("JobId", OracleDbType.Varchar2).Value = param.JobOid;
                ocmd.Parameters.Add("ProfileId", OracleDbType.Varchar2).Value = param.strId2;
                ocmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2).Value = param.LoggedUserId;
                if (!string.IsNullOrEmpty(param.JobOid) && !string.IsNullOrEmpty(param.strId2))
                {
                    mstrRes = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutString(StoredProcedure.Ora_SpSetApplyApplicant, ocmd, "mresult", StaticInfos.conStringOracle.ToString());
                }
               
                if (!string.IsNullOrEmpty(mstrRes) && mstrRes != "0" && mstrRes != "null")
                {

                    message = MessageConstants.Saved;
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
                message,
                resstate
            };
        }



        public async Task<object> GetApplcntByID(vmCmnParameter cparam)
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string regApplicantDetails = string.Empty, wrkOrdrDetail = string.Empty, wrkDtl = string.Empty, termsCondition = string.Empty, objFlowDetail = string.Empty;
            try
            {
                var PrcsFlowMgt = new ProcessFlowMgt();
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gApplicantMobNum", (1, OracleDbType.Varchar2, cparam.strId)}
                };
                //gQuotationId
                regApplicantDetails = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGetApplicantDetailByMobNum, ht, "gresult", StaticInfos.conStringOracle.ToString());
           
            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                regApplicantDetails
             
            };
        }




        public async Task<object> GetWithPaginationById(vmCmnParameter param) //vmCmnParameters cmnParam
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string listApplication = string.Empty;
            object result = null;
            try
            {
                ht = new Hashtable
                {
                    { "qresult", (0, OracleDbType.RefCursor, ParameterDirection.Output) },
                    { "PageNumber", (1, Convert.ToDecimal(param.pageNumber))},
                    { "PageSize", (2, Convert.ToDecimal(param.pageSize)) },
                    { "SearchVal", (3, param.SearchVal.Trim().ToLower()) },
                    { "LoggedUserId", (4, param.LoggedUserId) }
                };

                listApplication = await OraGeneric_vmCmnParameter.ExecuteCommandString(StoredProcedure.Ora_SpGet_AlldataByPageById, ht, StaticInfos.conStringOracle.ToString());
            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return result = new
            {
                listApplication
            };
        }





        public async Task<object> getcandiDatedetailsByid(vmCmnParameter cparam)
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string regApplicantMaster = string.Empty, accQlfDetail = string.Empty, wrkExperience = string.Empty, profCertificate = string.Empty;
            try
            {
                var PrcsFlowMgt = new ProcessFlowMgt();
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gCandiDateMstById", (1, OracleDbType.Varchar2, cparam.strId)}
                };
                //gQuotationId
                regApplicantMaster = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_CandidateDetailsById, ht, "gresult", StaticInfos.conStringOracle.ToString());
                accQlfDetail = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_CandidatAccQualificatonById, ht, "gresult", StaticInfos.conStringOracle.ToString());
                wrkExperience = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_CandidateDeWorkExperienceById, ht, "gresult", StaticInfos.conStringOracle.ToString());
                profCertificate = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_CandidateProfCirtificateById, ht, "gresult", StaticInfos.conStringOracle.ToString());

            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                regApplicantMaster,
                accQlfDetail,
                wrkExperience,
                profCertificate

            };
        }


        public async Task<object> getApplicationDetalByid(vmCmnParameter cparam)
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string regApplicantMaster = string.Empty, accQlfDetail = string.Empty, wrkExperience = string.Empty, profCertificate = string.Empty;
            try
            {
                var PrcsFlowMgt = new ProcessFlowMgt();
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gCandiDateMstById", (1, OracleDbType.Varchar2, cparam.strId)}
                };
                //gQuotationId
                
                regApplicantMaster = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_ApplicationDetailsById, ht, "gresult", StaticInfos.conStringOracle.ToString());
                if (regApplicantMaster != "null")
                {
                    var applicants = System.Text.Json.JsonSerializer.Deserialize<List<JsonElement>>(regApplicantMaster);
                    string profileOid = applicants[0].GetProperty("profileOid").GetString();

                    gt = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gCandiDateMstById", (1, OracleDbType.Varchar2, profileOid)}
                };
                    accQlfDetail = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_CandidatAccQualificatonById, gt, "gresult", StaticInfos.conStringOracle.ToString());
                    wrkExperience = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_CandidateDeWorkExperienceById, gt, "gresult", StaticInfos.conStringOracle.ToString());
                    profCertificate = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_CandidateProfCirtificateById, gt, "gresult", StaticInfos.conStringOracle.ToString());



                }

            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                regApplicantMaster,
                accQlfDetail,
                wrkExperience,
                profCertificate

            };
        }



        public async Task<object> GetRegularReport(vmReportModel rptm, vmCmnParameter cparam)
        {
            object bytes = null; bool resstate = false;
            try
            {
                dynamic objRes = await getcandiDatedetailsByid(cparam);
                var dataListMstr = Extension.GetReportDataTable(objRes.regApplicantMaster.ToString());
                var dataListAccQlf = Extension.GetReportDataTable(objRes.accQlfDetail.ToString());
                var dataListWrkExp = Extension.GetReportDataTable(objRes.wrkExperience.ToString());
                var dataListProfCir = Extension.GetReportDataTable(objRes.profCertificate.ToString());


                dataListMstr = Extension.AddDataSetName(dataListMstr, "DataSet1");
                rptm.DataTableList.Add(dataListMstr);

                dataListAccQlf = Extension.AddDataSetName(dataListAccQlf, "DataSet2");
                rptm.DataTableList.Add(dataListAccQlf);

                dataListWrkExp = Extension.AddDataSetName(dataListWrkExp, "DataSet3");
                rptm.DataTableList.Add(dataListWrkExp);

                dataListProfCir = Extension.AddDataSetName(dataListProfCir, "DataSet4");
                rptm.DataTableList.Add(dataListProfCir);

                bytes = ReportingService.Report(rptm).Result;

                if (bytes != null && bytes != string.Empty)
                {
                    resstate = true;
                }
            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                bytes,
                resstate
            };
        }


        //GET REPORT FOR INDIVIDUAL ALL APPLICATION
        public async Task<object> GetApplicantReport(vmReportModel rptm, vmCmnParameter cparam)
        {
            object bytes = null; bool resstate = false;
            try
            {
                dynamic objRes = await getApplicationDetalByid(cparam);
                var dataListMstr = Extension.GetReportDataTable(objRes.regApplicantMaster.ToString());
                var dataListAccQlf = Extension.GetReportDataTable(objRes.accQlfDetail.ToString());
                var dataListWrkExp = Extension.GetReportDataTable(objRes.wrkExperience.ToString());
                var dataListProfCir = Extension.GetReportDataTable(objRes.profCertificate.ToString());


                dataListMstr = Extension.AddDataSetName(dataListMstr, "DataSet1");
                rptm.DataTableList.Add(dataListMstr);

                dataListAccQlf = Extension.AddDataSetName(dataListAccQlf, "DataSet2");
                rptm.DataTableList.Add(dataListAccQlf);

                dataListWrkExp = Extension.AddDataSetName(dataListWrkExp, "DataSet3");
                rptm.DataTableList.Add(dataListWrkExp);

                dataListProfCir = Extension.AddDataSetName(dataListProfCir, "DataSet4");
                rptm.DataTableList.Add(dataListProfCir);

                bytes = ReportingService.Report(rptm).Result;

                if (bytes != null && bytes != string.Empty)
                {
                    resstate = true;
                }
            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                bytes,
                resstate
            };
        }


        //GET JOB ID FROM JOB POST MASTER
        public async Task<object> getJobIdByMail(vmCmnParameter cparam)
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string jobIDList = string.Empty;
            try
            {
                var PrcsFlowMgt = new ProcessFlowMgt();
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gUserId", (1, OracleDbType.Varchar2, cparam.strId)}
                };
                //gQuotationId
                jobIDList = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_FindJobIdFrmMasterData, ht, "gresult", StaticInfos.conStringOracle.ToString());
               
            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                jobIDList,
         

            };
        }
        //------------------------------------------End-----------------------------------

       

















    }
}