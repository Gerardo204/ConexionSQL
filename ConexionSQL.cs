using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ConexionDB
{
    class ConexionSQL
    {

        private SqlConnection Conection;
        private String Cadconx = "";

        public ConexionSQL(String Servidor, String BD)
        {
            Cadconx = @"Data Source=" + Servidor + "; Initial Catalog=" + BD + "; Integrated Security=True";
        }

        public SqlConnection AbrirConexion_SQL(ref String Mensaje)
        {
            Conection = new SqlConnection();
            Conection.ConnectionString = Cadconx;
            try
            {
                Conection.Open();
                Mensaje = "Operacion  Exitosa";
            }
            catch (Exception e)
            {
                Mensaje = "Conexion No Exitosa\n\n" + e.Message;
                Conection = null;
            }
            return Conection;
        }

        public DataSet ConsultaDataSet_SQL(SqlConnection cn_abierta, String query, ref String mensaje)
        {
            SqlCommand carrito = new SqlCommand();
            DataSet CajaEnorme = new DataSet();
            SqlDataAdapter Trailer = new SqlDataAdapter();

            if (cn_abierta != null)
            {
                carrito.Connection = cn_abierta;
                carrito.CommandText = query;
                Trailer.SelectCommand = carrito;
                try
                {
                    Trailer.Fill(CajaEnorme);
                    mensaje = "Consulta correcta";
                }
                catch (Exception e)
                {
                    CajaEnorme = null;
                    mensaje = "Error ConsultaDataSet SQL\n\n" + e.Message;
                }
                cn_abierta.Dispose();
            }
            else
            {
                CajaEnorme = null;
                mensaje = "No hay conexion para la consulta";
            }
            return CajaEnorme;
        }

        public SqlDataReader ConsultaReader_SQL(SqlConnection cn_abierta, String query1, ref String Messag)
        {
            SqlCommand carrito = new SqlCommand();
            SqlDataReader caja = null;
            if (cn_abierta != null)
            {
                carrito.Connection = cn_abierta;
                carrito.CommandText = query1;
                try
                {
                    caja = carrito.ExecuteReader();
                    Messag = "Consulta correcta";
                }
                catch (Exception e)
                {
                    Messag = "Error ConsultaReader Access\n\n" + e.Message;
                    caja = null;
                }
            }
            else
                Messag = "No hay coneccion abierta\n\nConsultaReader()";

            return caja;
        }

        public Boolean Ejecuta_Consulta_SQL(SqlConnection Carretera, String SentenciaSQL, ref String Mensaje)
        {
            Boolean resultado = false;
            SqlCommand carrito = new SqlCommand();

            if (Carretera != null)
            {
                carrito.Connection = Carretera;
                carrito.CommandText = SentenciaSQL;
                try
                {
                    carrito.ExecuteNonQuery();
                    resultado = true;
                }
                catch (Exception e)
                {
                    Mensaje = "Error OperacionModifica SQL\n\n" + e.Message;
                    resultado = false;
                }
                Carretera.Dispose();
            }
            else
                Mensaje = "No hay coneccion abierta\n\nOperacionModifica()";

            return resultado;
        }

        public void EliminarParametro(SqlConnection cn, ref string M, string tabla, int id)
        {
            SqlCommand com = new SqlCommand("Eliminar_Trabajador2" + tabla, cn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", id);
            com.ExecuteNonQuery();
            M = "Eliminación exitosa";
        }

        public Boolean Operacion_modifica(SqlConnection carretera, string sentenciaSQL, ref string mesj)
        {
            Boolean resultado = false;
            SqlCommand carrito = new SqlCommand();
            if (carretera != null)
            {
                //está abierta la conexión
                carrito.Connection = carretera;
                carrito.CommandText = sentenciaSQL;
                try
                {
                    carrito.ExecuteNonQuery();
                    resultado = true;
                    mesj = "Modificación Correcta";
                }
                catch (Exception g)
                {
                    resultado = false;
                    mesj = "Error: " + g.Message;

                }
                carretera.Close();
                carretera.Dispose();
            }
            else
            {
                resultado = false;
                mesj = "No Hay Conexión Abierta";
            }
            return resultado;
        }

    }
}
