using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Limpiar_Sessiones_BD.Clases
{
    public class clsSQL
    {

        public DataSet sp_Configuracion()
        {
          
            SqlConnection conexionSQL = new SqlConnection(Properties.Settings.Default.strConexionControlSeccionesSiesa);
            SqlCommand comandoSQL = new SqlCommand();
            SqlDataAdapter adaptador = new SqlDataAdapter();
            DataSet ds = new DataSet();

            comandoSQL.Connection = conexionSQL;
            comandoSQL.CommandType = CommandType.StoredProcedure;
            comandoSQL.CommandText = "sp_Configuracion";

            //comandoSQL.Parameters.AddWithValue("@id", id);

            try
            {
                adaptador.SelectCommand = comandoSQL;
                adaptador.Fill(ds);
            }
            catch (Exception ex)
            {
                string resultado;
                resultado = ex.Message.ToString();
            }

            return ds;

        }





        public void ejecutarComando(string id)
        {           
            SqlConnection conexionSQL = new SqlConnection(Properties.Settings.Default.strConexionControlSeccionesSiesa);
            SqlCommand comandoSQL = new SqlCommand();
            DataSet ds = new DataSet();

            comandoSQL.Connection = conexionSQL;
            comandoSQL.CommandType = CommandType.Text;
            comandoSQL.CommandText = "sp_Expediente_Eliminar";

            comandoSQL.Parameters.AddWithValue("@id", id);

            try
            {
                comandoSQL.Connection.Open();
                comandoSQL.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                string resultado;
                resultado = ex.Message.ToString();
            }
            finally
            {
                comandoSQL.Connection.Close();
            }

        }


    }
}
