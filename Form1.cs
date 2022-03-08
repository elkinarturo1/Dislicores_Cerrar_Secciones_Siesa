using Limpiar_Sessiones_BD.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Limpiar_Sessiones_BD
{
    public partial class Form1 : Form
    {

        string resultado = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {  

            try
            {

                //============================================================================================================
                //Paso 1: Consulto las configuraciones de los servidores que evaluare
                
                clsSQL objSQL = new clsSQL();
                DataSet dsConfiguraciones = new DataSet();

                dsConfiguraciones = objSQL.sp_Configuracion();
                //============================================================================================================


                if (dsConfiguraciones.Tables .Count > 0)
                {
                    if(dsConfiguraciones.Tables[0].Rows .Count > 0)
                    {

                        foreach (DataRow registro in dsConfiguraciones.Tables[0].Rows)
                        {

                            //============================================================================================================
                            //Paso 2: Asigna las variables leidas de las configuraciones y armos la consulta de oracle

                            string servidor = registro["servidor"].ToString();
                            string tiempoSegundos = registro["tiempoSegundos"].ToString();
                            string consultaOracle = $"SELECT TERMINAL, STATUS, USER, OSUSER, SID, SERIAL#, PROCESS FROM  v$session WHERE (TERMINAL = '{servidor}') AND (STATUS = 'INACTIVE') and (LAST_CALL_ET > {tiempoSegundos})";
                            //string consultaOracle = $"SELECT TERMINAL, STATUS, USER, OSUSER, SID, SERIAL#, PROCESS FROM  v$session  WHERE (TERMINAL = 'EUCLIDES') ";
                            //============================================================================================================

                            //============================================================================================================
                            //Paso 3: Ejecuto la consulta en oracle para ver cuantos proceso debo cerrar deacuerdo a la configuracion
                            clsOracle objOracle = new clsOracle();
                            DataSet dsSeccionesOracle = new DataSet();
                            dsSeccionesOracle = objOracle.ejecutarConsultaOracle(consultaOracle);
                            //============================================================================================================

                            //============================================================================================================
                            //Paso 4: Separo el proceso de oracle que deseo eliminar en Windows y realizo el KILL de manera remota en el servidor deseado
                            if (dsSeccionesOracle.Tables .Count > 0)
                            {
                                if (dsSeccionesOracle.Tables [0].Rows .Count > 0)
                                {
                                    foreach (DataRow  registroOracle in dsSeccionesOracle.Tables[0].Rows)
                                    {                                      

                                        string[] arregloPID;
                                        arregloPID = registroOracle["PROCESS"].ToString().Split(char.Parse(":"));

                                        string PID = arregloPID[0];     

                                        //Process.Start(comandoCMD);

                                        Process cmd = new Process();
                                        string comandoCMD = $"taskkill /s {servidor} /pid {PID}";

                                        cmd.StartInfo.FileName = "cmd.exe";
                                        cmd.StartInfo.RedirectStandardInput = true;
                                        cmd.StartInfo.RedirectStandardOutput = true;
                                        cmd.StartInfo.CreateNoWindow = true;
                                        cmd.StartInfo.UseShellExecute = false;
                                        cmd.Start();

                                        cmd.StandardInput.WriteLine(comandoCMD);
                                        cmd.StandardInput.Flush();
                                        cmd.StandardInput.Close();
                                        cmd.WaitForExit();
                                        //MessageBox.Show(cmd.StandardOutput.ReadToEnd());    
                                                              
                                    }
                                }
                            }
                            //============================================================================================================


                            ////============================================================================================================
                            ////Paso 5: Mensaje en pantalla

                            List<string> listadoUsuarios = new List<string>();

                            if (dsSeccionesOracle.Tables.Count > 0)
                            {
                                if (dsSeccionesOracle.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow registroOracle in dsSeccionesOracle.Tables[0].Rows)
                                    {
                                        string usuario = registroOracle["OSUSER"].ToString();

                                        if (!listadoUsuarios.Contains(usuario))
                                        {
                                            listadoUsuarios.Add(usuario);
                                        }
                                    }

                                    foreach (string usuario in listadoUsuarios)
                                    {
                                        //Process.Start(comandoCMD);

                                        Process cmd = new Process();
                                        string mensajePantalla = $"msg {usuario} Su seccion de Siesa se cerro por tiempo de inactividad";

                                        cmd.StartInfo.FileName = "cmd.exe";
                                        cmd.StartInfo.RedirectStandardInput = true;
                                        cmd.StartInfo.RedirectStandardOutput = true;
                                        cmd.StartInfo.CreateNoWindow = true;
                                        cmd.StartInfo.UseShellExecute = false;
                                        cmd.Start();

                                        cmd.StandardInput.WriteLine(mensajePantalla);
                                        cmd.StandardInput.Flush();
                                        cmd.StandardInput.Close();
                                        cmd.WaitForExit();
                                    }

                                }

                            }
                            ////============================================================================================================

                        }


                    }
                }
                                        
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }

            Close();

        }
    }
}
