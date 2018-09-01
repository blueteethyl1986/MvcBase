#region using
using System;
using System.Configuration;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;


#endregion

namespace Esmart.Framework.DB
{
    /// <summary>
    /// <table style="font-size:12px">
    /// <tr><td><b>�� �� ��</b>��DbObject.cs</td></tr> 
    /// <tr><td><b>��������</b>�����ݲ���࣬�ṩ�Եײ����ݵĻ�������</td></tr>
    /// <tr><td><b>�� �� ��</b>�� </td></tr>
    /// <tr><td><b>����ʱ��</b>��</td></tr>
    /// </table>
    /// </summary>
    public class Oracledbhelp
    {
        #region ��Ա����
        /// <summary>
        /// <table style="font-size:12px">
        /// <tr><td><b>��������</b>��Oracle�������Ӷ���</td></tr> 
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table>
        /// </summary>

        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>�����������ַ���</td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr> 
        /// </table></summary>
        private static string connectionString;
        #endregion
        private OracleConnection ocon;
        private int timeout = 300000;


        #region ���캯��
        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>�����캯����ʹ�������ļ��е�Ĭ�����������ַ���ConnectionString����ʼ���������Ӷ��� </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>
        public Oracledbhelp()
        {
           // connectionString = connstr;//��Web.Config��ȡ�õ������ַ���
            ocon = new OracleConnection(connectionString);
        }
        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>�����캯��������ָ�������������ַ�������ʼ���������Ӷ���</td></tr> 
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>
        /// <param name="newConnectionString">���������ַ���</param>
        public Oracledbhelp(string newConnectionString)
        {
            connectionString = newConnectionString;
            ocon = new OracleConnection(connectionString);
        }
        #endregion

        #region ˽�з���

        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>������һ��OracleCommand������������OracleDataReader </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>
        /// <param name="storedProcName">�洢��������</param>
        /// <param name="parameters">�洢���̵Ĳ��������б����飩</param>
        /// <returns>OracleCommand����</returns>
        private OracleCommand BuildCommand(string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = new OracleCommand(storedProcName, ocon);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = timeout;

            foreach (OracleParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;

        }
        #endregion

        #region ���д洢����
        /// <summary>
        /// <table style="font-size:12px">
        /// <tr><td><b>��������</b>�����д洢���̣���ȡӰ�����������ش洢�������н�� </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table>
        /// </summary>
        /// <param name="storedProcName">�洢��������</param>
        /// <param name="parameters">�洢���̵Ĳ��������б����飩</param>
        /// <param name="rowsAffected">���Σ�ִ�д洢������Ӱ��ļ�¼����</param>
        /// <returns>�洢���̵����н��</returns>
        public object RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            object result;

            //if(ocon.State.ToString() == "Closed") Open();
            Open();
            OracleCommand command = BuildCommand(storedProcName, parameters);
            rowsAffected = command.ExecuteNonQuery();
            //�����"ReturnValue"�����򷵻�ֵ�����򷵻�null
            bool blnHasReturn = false;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Direction == ParameterDirection.ReturnValue)
                {
                    blnHasReturn = true;
                    break;
                }
            }
            if (blnHasReturn)
                result = command.Parameters["ReturnValue"].Value;
            else
                result = null;

            Close();
            return result;
        }

        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>�����д洢���̣����ز�����OracleDataReader����,���������ر����ӣ����һ��Ҫ����������ֶ��ر����� </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>
        /// <param name="storedProcName">�洢��������</param>
        /// <param name="parameters">�洢���̵Ĳ��������б����飩</param>
        /// <returns>OracleDataReader����</returns>
        public OracleDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            OracleDataReader returnReader;

            Open();
            OracleTransaction myOracleTransaction = ocon.BeginTransaction();
            try
            {
                OracleCommand command = BuildCommand(storedProcName, parameters);
                command.CommandType = CommandType.StoredProcedure;

                returnReader = command.ExecuteReader();
                myOracleTransaction.Commit();

            }
            catch (Exception e)
            {
                myOracleTransaction.Rollback();
                OracleCommand command2 = BuildCommand(storedProcName, parameters);
                command2.CommandType = CommandType.StoredProcedure;

                returnReader = command2.ExecuteReader();
            }
            //Close();
            return returnReader;
        }

        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>�����д洢���̣�����һ��DataSet����
        /// �����н������ָ����DataTable�У�����DataSet���� </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>
        /// <param name="storedProcName">�洢��������</param>
        /// <param name="parameters">�洢���̵Ĳ��������б����飩</param>
        /// <param name="tableName">���ݱ�����</param>
        /// <returns>DataSet����</returns>
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            DataSet dataSet = new DataSet();
            OracleDataAdapter sqlDA = new OracleDataAdapter();
            try
            {
                
                Open();
                OracleTransaction myOracleTransaction = ocon.BeginTransaction();

               
                sqlDA.SelectCommand = BuildCommand(storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                myOracleTransaction.Commit();
                Close();
            }
            catch (Exception e)
            {
                try
                {
                    Close();
                  //  connectionString = Constant.GetOracleConn();//��Web.Config��ȡ�õ������ַ���
                    ocon = new OracleConnection(connectionString);

                    Open();

                    //OracleDataAdapter sqlDA = new OracleDataAdapter();
                   // sqlDA.SelectCommand = BuildCommand(storedProcName, parameters);
                    sqlDA.Fill(dataSet, tableName);
                    
                   // HGERROR.log(e.ToString() + "OracleTransaction error,cancel transaction ,rebuild sqlDA.Fill! storedProcName:" + storedProcName);
                    Close();
                }
                catch (Exception ex)
                {
                    DataTable dt = new DataTable();
                    dataSet.Tables.Add(dt);
                    //HGERROR.log(ex.ToString() + "sqlDA.Fill error,fill null dt! storedProcName:" + storedProcName+",OconState:"+ocon.State.ToString());
                }
            }

            return dataSet;
        }

        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>�����д洢���̣������н����������DataSet�����ָ�����У��޷���ֵ </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>  
        /// <param name="storedProcName">�洢��������</param>
        /// <param name="parameters">�洢���̵Ĳ��������б����飩</param>
        /// <param name="dataSet">DataSet����</param>
        /// <param name="tableName">���ݱ�����</param>
        public void RunProcedure(string storedProcName, IDataParameter[] parameters, DataSet dataSet, string tableName)
        {
                OracleDataAdapter sqlDA = new OracleDataAdapter();
            try
            {
                Open();

                OracleTransaction myOracleTransaction = ocon.BeginTransaction();

                sqlDA.SelectCommand = BuildCommand(storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                myOracleTransaction.Commit();
                Close();
            }
            catch (Exception e)
            {
                Open();
                sqlDA.Fill(dataSet, tableName);
                Close();
               // HGERROR.log(e.ToString() + "OracleTransaction error,rebuild execute!");
            }
            
        }
        #endregion

        #region ִ�д������Ĵ洢����,����DataTable
        /// <summary>
        /// �������Ĵ洢����,����DataTable
        /// </summary>
        /// <param name="ProcName">�洢��������</param>
        /// <param name="parameters">��������</param>
        /// <returns>����datatable����</returns>
        public DataTable RunProcedure2DT(string ProcName, OracleParameter[] parameters)
        {
            DataTable dt;
            Oracledbhelp db = new Oracledbhelp();
            try
            {
                DataSet da = db.RunProcedure(ProcName, parameters, "t1");

                dt = da.Tables["t1"];

            }
            catch (Exception e)
            {
               // HGERROR.log(e + "�洢����:" + ProcName);
               
                dt = null;
            }
            finally
            {
                db.Close();
                db = null;
            }
            if (dt == null)
            {
                dt = new DataTable();
            }
            return dt;
        }

       

        #endregion

        /// <summary>
        /// ���ش洢���̵�����ַ���
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string RunProcedure2OutString(string ProcName, OracleParameter[] parameters)
        {
            string outpar = "";
            Oracledbhelp db = new Oracledbhelp();
            try
            {
                db.RunProcedure(ProcName, parameters);
                outpar = (string)parameters[parameters.Length - 1].Value.ToString();

            }
            catch (Exception e)
            {
                
            }
            finally
            {
                db.Close();
                db = null;
            }
            return outpar;

        }

        #region ����SQL���
        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>��������д���ݿ���ص�SQL��䣬����Ӱ������ </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>  
        /// <param name="sqlString">SQL���</param>
        /// <returns>Ӱ������</returns>
        public int ExecNonQuery(string sqlString)
        {
            int RowAffected;
            //if(ocon.State.ToString() == "Closed") Open();
            Open();
            OracleCommand command = new OracleCommand(sqlString, ocon);
            RowAffected = command.ExecuteNonQuery();
            Close();

            return RowAffected;

        }

        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>������SQL��䣬����OracleDataReader���� </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>
        /// <param name="sqlString">SQL���</param>
        /// <returns>SqlDataReader����</returns>
        public OracleDataReader ExecSqlString(string sqlString)
        {
            OracleDataReader returnReader;

            //if(ocon.State.ToString() == "Closed") Open();
            Open();
            OracleCommand command = new OracleCommand(sqlString, ocon);
            returnReader = command.ExecuteReader();
            //Close();

            return returnReader;
        }


        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>������SQL��䣬����DataSet���� </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>
        /// <param name="string">SQL���</param>
        /// <param name="tableName">���ݱ�����</param>
        /// <returns>DataSet����</returns>
        public DataSet ExecSqlString(string sqlString, string tableName)
        {
            DataSet dataSet = new DataSet();
            //if (ocon.State.ToString() == "Closed") Open();
            Open();
            OracleTransaction myOracleTransaction = ocon.BeginTransaction();
           
            OracleDataAdapter sqlDA = new OracleDataAdapter();
            sqlDA.SelectCommand = new OracleCommand(sqlString, ocon);
            sqlDA.Fill(dataSet, tableName);
            myOracleTransaction.Commit();
            Close();

            return dataSet;
        }

        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>������SQL��䣬�����н����������DataSet�����ָ�����У��޷���ֵ </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>  
        /// <param name="sqlString">SQL���</param>
        /// <param name="dataSet">DataSet����</param>
        /// <param name="tableName">���ݱ�����</param>
        public void ExecSqlString(string sqlString, DataSet dataSet, string tableName)
        {
            //if (ocon.State.ToString() == "Closed") Open();
            Open();
            OracleDataAdapter sqlDA = new OracleDataAdapter();
            sqlDA.SelectCommand = new OracleCommand(sqlString, ocon);
            sqlDA.Fill(dataSet, tableName);
            Close();
        }

        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>������SQL��䣬���ز�ѯ����ĵ�һ�еĵ�һ�У����������л��� </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>  
        /// <param name="sqlString">SQL���</param>
        /// <returns>Ӱ������</returns>
        public object ExecScalar(string sqlString)
        {
            object returnScalar;
            //if (ocon.State.ToString() == "Closed") Open();
            Open();
            OracleCommand command = new OracleCommand(sqlString, ocon);
            returnScalar = command.ExecuteScalar();
            //Close();

            return returnScalar;
        }
        #endregion







        #region �����������
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param></param>
        /// <param name="Size">������С</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>�µ� parameter ����</returns>
        public OracleParameter MakeInParam(string ParamName, OracleDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }
        #endregion




        #region ���뷵��ֵ����
        /// <summary>
        /// ���뷵��ֵ����
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <returns>�µ� parameter ����</returns>
        public OracleParameter MakeOutParam(string ParamName, OracleDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }
        #endregion

        #region ���뷵��ֵ����
        /// <summary>
        /// ���뷵��ֵ����
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <returns>�µ� parameter ����</returns>
        public OracleParameter MakeOutRecord(string ParamName, OracleDbType DbType)
        {
            return MakeParam(ParamName, OracleDbType.RefCursor, 0, ParameterDirection.Output, null);
        }


        #endregion





        #region ���뷵��ֵ����
        /// <summary>
        /// ���뷵��ֵ����
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <returns>�µ� parameter ����</returns>
        public OracleParameter MakeReturnParam(string ParamName, OracleDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.ReturnValue, null);
        }
        #endregion





        #region ���ɴ洢���̲���
        /// <summary>
        /// ���ɴ洢���̲���
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <param name="Direction">��������</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>�µ� parameter ����</returns>
        public OracleParameter MakeParam(string ParamName, OracleDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            OracleParameter param;

            if (Size > 0)
                param = new OracleParameter(ParamName, DbType, Size);
            else
                param = new OracleParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }
        #endregion

        #region  �����ݿ�����
        /// <summary>
        ///  �����ݿ�����
        /// </summary>
        public void Open()
        {
            try
            {
                // ʵ���� SqlConnection ����
                if (ocon == null)
                {
                    ocon = new OracleConnection(connectionString);
                }
                // �����ݿ�����
                if (ocon.State == System.Data.ConnectionState.Closed)
                {
                    //HGERROR.log("ocon.state:" + ocon.State.ToString());
                    ocon.Open();

                }

                // ���ǰ���ʧ�ܣ��ٴδ����ݿ�����
                if (ocon.State != System.Data.ConnectionState.Open)
                {
                   // HGERROR.log("ocon.state:" + ocon.State.ToString());
                    ocon.Close();
                    ocon.Open();

                }
            }
            catch (Exception e)
            {
               // HGERROR.log(e.ToString() + "ocon.state:" + ocon.State.ToString());
                ocon = new OracleConnection(connectionString);
                ocon.Open();
               // HGERROR.log("ocon rebuild:" + ocon.State.ToString());
            }

        }
        #endregion





        #region �ر����ݿ�����
        /// <summary>
        /// �ر����ݿ�����
        /// </summary>
        public void Close()
        {
            if (ocon != null)
            {
                if (ocon.State == System.Data.ConnectionState.Open)
                {
                    ocon.Close();
                    ocon.Dispose();
                    ocon = null;
                }
            }


        }
        # endregion



        #region ��������
        /// <summary><table style="font-size:12px">
        /// <tr><td><b>��������</b>�������������ƺ����ͷ��������� </td></tr>
        /// <tr><td><b>�� �� ��</b>�� </td></tr>
        /// <tr><td><b>����ʱ��</b>��</td></tr>
        /// </table></summary>
        ~Oracledbhelp()
        {
            if (ocon != null)
            {
                if (ocon.State.ToString() == "Open")
                    Close();
                ocon.Dispose();
            }
        }
        #endregion


    }
}

