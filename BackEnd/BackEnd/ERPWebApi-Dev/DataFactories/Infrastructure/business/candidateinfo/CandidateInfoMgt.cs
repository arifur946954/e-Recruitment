using DataFactories.BaseFactory;
using DataFactories.Infrastructure.common.processflow;
using DataModel.EntityModels.OraModel;
using DataModel.ViewModels;
using DataModel.ViewModels.ERPViewModel.Business;
using DataModel.ViewModels.ERPViewModel.Common;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace DataFactories.Infrastructure.business.candidateinfo
{
    public class CandidateInfoMgt
    {
        #region Variable declaration & initialization
        ModelContext _ctxOra = null;
        private IGenericFactoryOracle<vmCmnParameter> OraGeneric_vmCmnParameter = null;
        OracleCommand ocmd = null;
        Hashtable ht = null; OracleParameter[] oprm = null;
        #endregion
   





        public async Task<object> GetWithPagination(vmCmnParameter param) //vmCmnParameters cmnParam vmCmnParameter
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string listJobPost = string.Empty;
            param.Role = Regex.Replace(param.Role ?? "", @"\s*,\s*", " ");
            object result = null;
            try
            {
                        ht = new Hashtable
                        {
                            { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                              { "JobTitle", (1, OracleDbType.Varchar2, param.JobTitle)},
                              { "Company", (2, OracleDbType.Varchar2, param.Company)},
                             { "Department", (3, OracleDbType.Varchar2, param.Department)},
                             { "Post", (4, OracleDbType.Varchar2, param.Post)},
                             { "Role", (5, OracleDbType.Varchar2, param.Role)},
                             { "UserID", (6, OracleDbType.Varchar2, param.UserID)},
                             
                             // { "StartDate", (4, OracleDbType.Date, param.StartDate)},
                            // { "EndDate", (5, OracleDbType.Date, param.EndDate)}
                   



                        };
          


                listJobPost = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_candidateInfoByPage, ht, "gresult", StaticInfos.conStringOracle.ToString());
               

            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return result = new
            {
                listJobPost
            };
        }


        public async Task<object> getallapplication(vmCmnParameter param) //vmCmnParameters cmnParam vmCmnParameter
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string listApplicant = string.Empty;
            param.Role = Regex.Replace(param.Role ?? "", @"\s*,\s*", " ");
            object result = null;
            try
            {
                ht = new Hashtable
                        {
                            { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                              { "JobTitle", (1, OracleDbType.Varchar2, param.JobTitle)},
                              { "Company", (2, OracleDbType.Varchar2, param.Company)},
                             { "Department", (3, OracleDbType.Varchar2, param.Department)},
                             { "Post", (4, OracleDbType.Varchar2, param.Post)},
                             { "Role", (5, OracleDbType.Varchar2, param.Role)},
                             { "UserID", (6, OracleDbType.Varchar2, param.UserID)},
                             
                             // { "StartDate", (4, OracleDbType.Date, param.StartDate)},
                            // { "EndDate", (5, OracleDbType.Date, param.EndDate)}
                   



                        };



                listApplicant = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_ApplicantListByParam, ht, "gresult", StaticInfos.conStringOracle.ToString());


            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return result = new
            {
                listApplicant
            };
        }
        

        public async Task<object> GetByID(vmCmnParameter cparam)
        {
            OraGeneric_vmCmnParameter = new GenericFactoryOracle<vmCmnParameter>();
            string jobPostMaster = string.Empty, jobSkill = string.Empty, jobBenefit = string.Empty, jobRequirement = string.Empty, jobOtherRequirement = string.Empty, jobResponsibility = string.Empty; 
            try
            {
               
                ht = new Hashtable
                {
                    { "gresult", (0, OracleDbType.Clob, ParameterDirection.Output) },
                    { "gJobPostId", (1, OracleDbType.Varchar2, cparam.strId)}//
                };
                //gQuotationId
                jobPostMaster = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_JobPostMasterById, ht, "gresult", StaticInfos.conStringOracle.ToString());
              if (!string.IsNullOrEmpty(jobPostMaster) )

                {
                    jobSkill = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_JobPostSkillById, ht, "gresult", StaticInfos.conStringOracle.ToString());
                    jobBenefit = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_JobBenifitById, ht, "gresult", StaticInfos.conStringOracle.ToString());
                    jobRequirement = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_JobRequirementById, ht, "gresult", StaticInfos.conStringOracle.ToString());
                    jobOtherRequirement = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_JobOtherRequirementById, ht, "gresult", StaticInfos.conStringOracle.ToString());
                    jobResponsibility = await OraGeneric_vmCmnParameter.ExecuteNonQueryOutClob(StoredProcedure.Ora_SpGet_JobResponsibilityById, ht, "gresult", StaticInfos.conStringOracle.ToString());
                }





            


            }
            catch (Exception ex)
            {
                Logs.Bug(ex);
            }
            return new
            {
                jobPostMaster,
                jobSkill,
                jobBenefit,
                jobRequirement,
                jobOtherRequirement,
                jobResponsibility

            };
        }












































    }
}