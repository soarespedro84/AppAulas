using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace AppAulas.Models
{
    class Aula_Helper
    {
        private string _ligacao;

        public Aula_Helper(string ligacaoAUtilizar) {
            _ligacao = ligacaoAUtilizar;
        }

        public List<Aula> listar(Turma turma) {
            //criar lista de turmas a ver
            List<Aula> listaAulasAVer = new List<Aula>();
            DataTable listaBrutaAulas = new DataTable();

            //Sequência de Ligação à BD
            SqlDataAdapter telefone = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection ConexaoBD = new SqlConnection(_ligacao);

            comando.CommandType = CommandType.Text;
            //chamar todos os registos ordenados por data, do mais recente para o mais antigo
            string comandoParaSQL = "SELECT * FROM tAula ORDER BY data DESC";
            if (turma == null) comando.CommandText = comandoParaSQL; 
            else {
                comandoParaSQL = "SELECT * FROM tAula WHERE idTurma = @idTurma ORDER BY data DESC";
                comando.CommandText = comandoParaSQL; 
                comando.Parameters.AddWithValue("@idTurma", turma.IDTurma);
            }
            telefone.SelectCommand = comando;
            telefone.SelectCommand.Connection = ConexaoBD;
            ConexaoBD.Open();
            telefone.Fill(listaBrutaAulas);
            ConexaoBD.Close();
            //Final da Sequência de Ligação à BD

            //Para cada linha da tabela devolvida da pesquisa no SQL, criar um objecto Aula a partir do idAula do registo da tabela da BD
            foreach (DataRow linha in listaBrutaAulas.Rows) {                
                Aula a = new Aula(linha["idAula"].ToString());
                //Ler a Turma correspondente a essa Aula e preencher a informação com os valores respetivos 
                Turma_Helper th = new Turma_Helper(_ligacao);
                a.TurmaEmAula= th.ler(linha["idTurma"].ToString());
                a.Data = Convert.ToDateTime(linha["data"]);
                a.Modulo = linha["modulo"].ToString();
                a.Sumario = linha["sumario"].ToString();

                //Adicionar a Aula à lista de saída
                listaAulasAVer.Add(a);
            }
            return listaAulasAVer;
        }

        public Aula ler(string idAula) {
            Aula aulaSelecionada;
            DataTable listaBrutaAulas = new DataTable();

            SqlDataAdapter telefone = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection ConexaoBD = new SqlConnection(_ligacao);

            comando.CommandType = CommandType.Text;
            string comandoParaSQL = "SELECT * FROM tAula WHERE idAula = @idAula";
            comando.CommandText = comandoParaSQL;
            comando.Parameters.AddWithValue("@idAula", idAula);
            telefone.SelectCommand = comando;
            telefone.SelectCommand.Connection = ConexaoBD;
            ConexaoBD.Open();
            telefone.Fill(listaBrutaAulas);
            ConexaoBD.Close();

            //Se existir o registo, só pode haver uma Aula com o id indicado
            if (listaBrutaAulas.Rows.Count == 1) {
                //Criar um novo objeto Aula a partir do registo da tabela
                DataRow linha = listaBrutaAulas.Rows[0];
                aulaSelecionada = new Aula(linha["idAula"].ToString());
                //Ler a Turma correspondente a essa Aula e preencher a informação com os valores respetivos 
                Turma_Helper th = new Turma_Helper(_ligacao);
                aulaSelecionada.TurmaEmAula = th.ler(linha["idTurma"].ToString());
                
                aulaSelecionada.Data = Convert.ToDateTime(linha["data"]);
                aulaSelecionada.Modulo = linha["modulo"].ToString();
                aulaSelecionada.Sumario = linha["sumario"].ToString();
                
            } else {
                aulaSelecionada = null;
            }
            return aulaSelecionada;
        }

        public string atualizarAula(Aula aula) {
            string erros = "";
            string instrucaoSQL = "";
            //instruções diferentes se aula não existe mas existe a Turma Cria novo registo; se já existe, atualiza com os valores novos.
            if (aula.IDTurma == "" && aula.TurmaEmAula != null) {
                instrucaoSQL = "INSERT INTO tAula (idTurma, modulo, sumario, data)"
                    + " VALUES (@idturma, @modulo, @sumario, @data);";
            } else {
                instrucaoSQL = "UPDATE tAula SET " +
                    "idTurma = @idTurma, " +
                    "modulo = @modulo, " +
                    "sumario = @sumario, " +
                    "data = @data " +                    
                    "WHERE idAula = @idAula";
            }
            try {
                SqlCommand comando = new SqlCommand();
                comando.Connection = new SqlConnection(_ligacao);
                comando.CommandText = instrucaoSQL;
                comando.CommandType = CommandType.Text;
                comando.Parameters.AddWithValue("@idTurma", aula.TurmaEmAula.IDTurma);
                comando.Parameters.AddWithValue("@modulo", aula.Modulo);
                comando.Parameters.AddWithValue("@sumario", aula.Sumario);
                comando.Parameters.AddWithValue("@data", aula.Data);
                if (aula.IDTurma != "") {
                    comando.Parameters.AddWithValue("@idAula", aula.IDTurma);
                }
                comando.Connection.Open();                          //"marcar o número do serviço"
                comando.ExecuteNonQuery();                          //enviar a ordem para o serviço
                comando.Connection.Close();                         //desligar a chamada
                comando.Connection.Dispose();                       //descartar o telefone que ocupa muito espaço
                erros = "";
            } catch (Exception ex) {
                erros = ex.Message;
            }
            return erros;
        }

        public string EliminarAula(Aula aula) {
            string erros = "";
            string instrucaoSQL = "DELETE FROM tAula WHERE idAula = @idAula";
            try {
                SqlCommand comando = new SqlCommand();
                comando.Connection = new SqlConnection(_ligacao);
                comando.CommandText = instrucaoSQL;
                comando.CommandType = CommandType.Text;
                comando.Parameters.AddWithValue("@idAula", aula.IDTurma);
                comando.Connection.Open();
                comando.ExecuteNonQuery();
                comando.Connection.Close();
                comando.Connection.Dispose();
                erros = "";
            } catch (Exception ex) {
                erros = ex.Message;
            }
            return erros;
        }
    }
}
