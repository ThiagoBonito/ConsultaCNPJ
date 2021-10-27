using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace ConsultaCNPJ
{
    class DAL
    {
        private static String strConexao = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=BDFarinha.mdb";
        private static OleDbConnection conn = new OleDbConnection(strConexao);
        private static OleDbCommand strSQL;
        private static OleDbDataReader result;
        private static OleDbDataAdapter adaptador;
        private static DataTable dt = new DataTable();
        private static int i = -1;

        public static void conecta()
        {
            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                Erro.setMsg("Problemas ao se conectar ao Banco de Dados");
            }

        }

        public static void desconecta()
        {
            conn.Close();
        }

        public static void consultaUmCliente()
        {
            String aux = "select * from TabClientes where cnpj = @cnpj";
            strSQL = new OleDbCommand(aux, conn);
            strSQL.Parameters.Add("@cnpj", OleDbType.VarChar).Value = Cliente.getCNPJ();
            result = strSQL.ExecuteReader();
            Erro.setErro(false);
            if (result.Read())
            {
                Cliente.setNome(result.GetString(1));
            }
            else
            {
                Erro.setMsg("CNPJ não Cadastrado");
            }
        }
        public static void consultaVendas()
        {
            String aux = "select * from TabVendasCliente where cnpj = '" + Cliente.getCNPJ() + "'";
            adaptador = new OleDbDataAdapter(aux, conn);
            adaptador.Fill(dt);
            adaptador.Dispose();
        }
        public static void getProximo()
        {
            Erro.setErro(false);
            ++i;
            if (i >= dt.Rows.Count)
                Erro.setErro(true);
            else
            {
                VendaCliente.setCNPJ("" + dt.Rows[i].ItemArray[0]);
                VendaCliente.setCodigo("" + dt.Rows[i].ItemArray[1]);
                VendaCliente.setData("" + dt.Rows[i].ItemArray[2]);
                VendaCliente.setToneladas("" + dt.Rows[i].ItemArray[3]);
                VendaCliente.setvalor("" + dt.Rows[i].ItemArray[4]);
            }
        }
    }
}