using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limpiar_Sessiones_BD.Clases
{
    public class clsOracle
    {

        public DataSet ejecutarConsultaOracle(string query)
        {
            OdbcConnection conn = new OdbcConnection(Properties.Settings.Default.strConexionOracle);
            OdbcDataAdapter Adaptador = new OdbcDataAdapter();
            DataSet ds = new DataSet();
            OdbcCommand comando = new OdbcCommand();      


            try
            {

                comando.Connection = conn;
                //comando.InitialLONGFetchSize = 1000;
                comando.CommandType = CommandType.Text;
                comando.CommandText = query;

                //OracleComando.Parameters.Add("parametro1", OracleDbType.Int32).Value = parametro1;
                //OracleComando.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                Adaptador.SelectCommand = comando;
                Adaptador.Fill(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            {
                comando.Connection.Close();
            }

            return ds;

        }





        public DataSet consultarSesionesOracle()
        {
            OdbcConnection conn = new OdbcConnection(Properties.Settings.Default.strConexionOracle);
            OdbcDataAdapter Adaptador = new OdbcDataAdapter();
            DataSet ds = new DataSet();
            OdbcCommand comando = new OdbcCommand();

            string comand = Properties.Settings.Default.Query;


            try
            {

                comando.Connection = conn;
                //comando.InitialLONGFetchSize = 1000;
                comando.CommandType = CommandType.Text;
                comando.CommandText = comand;

                //OracleComando.Parameters.Add("parametro1", OracleDbType.Int32).Value = parametro1;
                //OracleComando.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                Adaptador.SelectCommand = comando;
                Adaptador.Fill(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            {
                comando.Connection.Close();
            }

            return ds;

        }

        public void matarProcesosOracle(string sid, string serial)
        {
            
            OdbcConnection conn = new OdbcConnection(Properties.Settings.Default.strConexionOracle);   
            OdbcCommand comando = new OdbcCommand();

            string comand = "alter system kill session '" + sid + "," + serial + "' IMMEDIATE";


            try
            {

                comando.Connection = conn;              
                comando.CommandType = CommandType.Text;
                comando.CommandText = comand;

                comando.Connection.Open();
                comando.ExecuteNonQuery();               

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                comando.Connection.Close();
            }

        }





        //SELECT TERMINAL, STATUS SID, SERIAL# FROM  v$session
        //WHERE(TERMINAL = 'EUCLIDES')
        //AND(STATUS = 'INACTIVE')

        //WHERE 
        //--alter system kill session '650,63695';
        //--alter system kill session 'sid,serial#';


    }
}
