using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JuryData.Data;
using JuryData.Entities;

/// <summary>
/// Summary description for clsUtilities
/// </summary>
public class clsUtilities
{
    public clsUtilities()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region SQL Direct Proc Calls
    public static Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase GetDB()
    {
        JuryData.Data.SqlClient.SqlNetTiersProvider p = DataRepository.Provider as JuryData.Data.SqlClient.SqlNetTiersProvider;

        if (null != p)
        {
            // get the SqlDatabase 
            Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase db = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(p.ConnectionString);
            return db;
        }
        else
            return null;
    }

    public static void AddParameter(ref DbCommand dbCmd, string strParameterName, ParameterDirection prmDirection, DbType dbTypeExpr, int intValue)
    {
        DbParameter dbParam = dbCmd.CreateParameter();
        dbParam.ParameterName = strParameterName;
        dbParam.Direction = prmDirection;
        dbParam.DbType = dbTypeExpr;
        dbParam.Value = intValue;

        dbCmd.Parameters.Add(dbParam);
    }

    public static void AddParameter(ref DbCommand dbCmd, string strParameterName, ParameterDirection prmDirection, DbType dbTypeExpr, string strValue)
    {
        DbParameter dbParam = dbCmd.CreateParameter();
        dbParam.ParameterName = strParameterName;
        dbParam.Direction = prmDirection;
        dbParam.DbType = dbTypeExpr;
        dbParam.Value = strValue;

        dbCmd.Parameters.Add(dbParam);
    }


    public static System.Data.Common.DbCommand GetDBCommand(string strSQLCommand)
    {
        JuryData.Data.SqlClient.SqlNetTiersProvider p = DataRepository.Provider as JuryData.Data.SqlClient.SqlNetTiersProvider;

        if (null != p)
        {
            // get the SqlDatabase 
            Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase db = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(p.ConnectionString);

            // create a command for the slow sproc
            System.Data.Common.DbCommand cmd = db.GetStoredProcCommand(strSQLCommand);

            return cmd;
        }
        else
            return null;


    }

    public static DataSet SQLExecuteDataSet(System.Data.Common.DbCommand cmd)
    {

        JuryData.Data.SqlClient.SqlNetTiersProvider p = DataRepository.Provider as JuryData.Data.SqlClient.SqlNetTiersProvider;

        if (null == p)
        {
            // provider is not a SQL provider, do something else appropriate
            return DataRepository.Provider.ExecuteDataSet(cmd);
        }
        else
        {
            // get the SqlDatabase 
            Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase db = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(p.ConnectionString);

            // create a command for the slow sproc
            // set the timeout as set on the provider (or as needed)
            cmd.CommandTimeout = DataRepository.Provider.DefaultCommandTimeout;

            // execute the command (any of the Execute* family)
            return DataRepository.Provider.ExecuteDataSet(cmd);
        }
    }


    public static void SQLExecuteNonQuery(string strSQLCommand)
    {

        JuryData.Data.SqlClient.SqlNetTiersProvider p = DataRepository.Provider as JuryData.Data.SqlClient.SqlNetTiersProvider;

        if (null == p)
        {
            // provider is not a SQL provider, do something else appropriate
            DataRepository.Provider.ExecuteNonQuery(strSQLCommand);
        }
        else
        {
            // get the SqlDatabase 
            Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase db = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(p.ConnectionString);

            // create a command for the slow sproc
            System.Data.Common.DbCommand cmd = db.GetStoredProcCommand(strSQLCommand);

            // set the timeout as set on the provider (or as needed)
            cmd.CommandTimeout = DataRepository.Provider.DefaultCommandTimeout;

            // execute the command (any of the Execute* family)
            DataRepository.Provider.ExecuteNonQuery(cmd);
        }
    }

    public static void SQLExecuteNonQuery(System.Data.Common.DbCommand cmd)
    {

        JuryData.Data.SqlClient.SqlNetTiersProvider p = DataRepository.Provider as JuryData.Data.SqlClient.SqlNetTiersProvider;

        if (null == p)
        {
            // provider is not a SQL provider, do something else appropriate
            DataRepository.Provider.ExecuteNonQuery(cmd);
        }
        else
        {
            // get the SqlDatabase 
            Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase db = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(p.ConnectionString);

            // create a command for the slow sproc
            // set the timeout as set on the provider (or as needed)
            cmd.CommandTimeout = DataRepository.Provider.DefaultCommandTimeout;

            // execute the command (any of the Execute* family)
            DataRepository.Provider.ExecuteNonQuery(cmd);
        }
    }


    #endregion

}
