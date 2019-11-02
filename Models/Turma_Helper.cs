using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace AppAulas.Models
{
    class Turma_Helper
    {
        private string _ligacao;

        public Turma_Helper(string ligacaoAUtilizar) {
            _ligacao = ligacaoAUtilizar;
        }

        public List<Turma> listar(Turma.TipoEstado estadoAtual) {
            //criar lista de turmas a ver na ComboBox
            List<Turma> listaTurmasDrop = new List<Turma>();
            DataTable listaBrutaTurmas = new DataTable();
            
            //Sequência de Ligação à BD
            SqlDataAdapter telefone = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection ConexaoBD = new SqlConnection(_ligacao);

            comando.CommandType = CommandType.Text;
            //Chamar todos os registos com estado ativo (1)
            comando.CommandText = "SELECT * FROM tTurma WHERE estado= " + ((int)estadoAtual).ToString(); 

            telefone.SelectCommand = comando;
            telefone.SelectCommand.Connection = ConexaoBD;
            //Abrir a ligação
            ConexaoBD.Open();
            //preencher a lista
            telefone.Fill(listaBrutaTurmas);
            //Fechar a ligação
            ConexaoBD.Close();

            //Para cada linha da tabela devolvida da pesquisa em SQL, criar um objecto Turma a partir do registo existente na BD
            foreach (DataRow linha in listaBrutaTurmas.Rows) {                
                Turma t = new Turma(
                    //o novo objeto recebe como parametros os valores dos campos respetivos da BD
                        linha["idTurma"].ToString(),
                        linha["nomeTurma"].ToString(),
                        linha["ano"].ToString(),
                        linha["curso"].ToString(),
                        (Turma.TipoEstado)Convert.ToInt32(linha["estado"]));


                //Adicionar a turma à lista de saída
                listaTurmasDrop.Add(t);
            }
            return listaTurmasDrop;
        }

        public Turma ler(string idTurmaSelect) {             
            Turma turmaSelecionada;
            DataTable listaBrutaTurmas = new DataTable();

            //Sequência de Ligação à BD
            SqlDataAdapter telefone = new SqlDataAdapter();
            SqlCommand comando = new SqlCommand();
            SqlConnection ConexaoBD = new SqlConnection(_ligacao);

            comando.CommandType = CommandType.Text;
            //Procurar o id na lista de turmas
            comando.CommandText = "SELECT * FROM tTurma WHERE idTurma=@idTurma";
            comando.Parameters.AddWithValue("@idTurma", idTurmaSelect);
            telefone.SelectCommand = comando;
            telefone.SelectCommand.Connection = ConexaoBD;
            //Abrir, preencher a lista, fechar
            ConexaoBD.Open();
            telefone.Fill(listaBrutaTurmas);
            ConexaoBD.Close();

            //Se existir o registo, só pode haver uma turma com o id indicado
            if (listaBrutaTurmas.Rows.Count == 1) {               
                DataRow linha = listaBrutaTurmas.Rows[0];
                turmaSelecionada = new Turma(
                        //o novo objeto recebe os valores dos campos da linha
                        linha["idTurma"].ToString(),
                        linha["nomeTurma"].ToString(),
                        linha["ano"].ToString(),
                        linha["curso"].ToString(),
                        (Turma.TipoEstado)Convert.ToInt32(linha["estado"]));                 

            } else {
                turmaSelecionada = null;
            }
            return turmaSelecionada;
        }
        public string atualizarTurma(Turma turma) {
            string erros = "";
            string instrucaoSQL = "";
            //instruções diferentes se aula não existe mas existe a Turma Cria novo registo; se já existe, atualiza com os valores novos.
            if (turma.IDTurma == "" && turma.NomeTurma != null) {
                instrucaoSQL = "INSERT INTO tTurma (nomeTurma, ano, curso)"
                    + " VALUES (@nomeTurma, @ano, @curso);";
            } else {
                instrucaoSQL = "UPDATE tTurma SET " +
                    "nomeTurma = @nomeTurma, " +
                    "ano = @ano, " +
                    "curso = @curso " +
                    "WHERE idTurma = @idTurma";
            }
            try {
                SqlCommand comando = new SqlCommand();
                comando.Connection = new SqlConnection(_ligacao);
                comando.CommandText = instrucaoSQL;
                comando.CommandType = CommandType.Text;
                comando.Parameters.AddWithValue("@idTurma", turma.IDTurma);
                comando.Parameters.AddWithValue("@nomeTurma", turma.NomeTurma);
                comando.Parameters.AddWithValue("@ano", turma.Ano);
                comando.Parameters.AddWithValue("@curso", turma.Curso);
                //if (turma.IDTurma != "") {
                //    comando.Parameters.AddWithValue("@idAula", aula.IDTurma);
                //}
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

        public string EliminarTurma(Turma turma) {
            string erros = "";
            string instrucaoSQL = "DELETE FROM tTurma WHERE idTurma = @idTurma";
            try {
                SqlCommand comando = new SqlCommand();
                comando.Connection = new SqlConnection(_ligacao);
                comando.CommandText = instrucaoSQL;
                comando.CommandType = CommandType.Text;
                comando.Parameters.AddWithValue("@idTurma", turma.IDTurma);
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
