using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAulas.Models {
    class Turma {
        #region "Constantes"
        public const int MAXNOMETURMA = 50;
        public enum TipoEstado { Inativo, Ativo };
        #endregion 

        #region "Atributos"
        private string _idTurma;
        private string _nomeTurma;
        private string _ano;
        private string _curso;
        private TipoEstado _estado;
        #endregion

        #region "Propriedades/Métodos"
        public string IDTurma {
            get { return _idTurma; }
        }

        public string NomeTurma {
            get { return _nomeTurma; }
            set {
                _nomeTurma = value;
                if (_nomeTurma.Length > MAXNOMETURMA) _nomeTurma = _nomeTurma.Substring(0, MAXNOMETURMA);
            }
        }

        public string Ano {
            get { return _ano; }
            set { _ano = value;}
        }

        public string Curso {
            get { return _curso; }
            set { _curso=value;}
        }        

        public TipoEstado Estado {
            get { return _estado; }
            set { _estado = value; }
        }
        #endregion

        #region "Construtores"
        public Turma() {
            _idTurma = "";
            NomeTurma = "";
            Ano = "";
            Curso = "";
            Estado = TipoEstado.Ativo;
        }

        public Turma(string idTurmaBD, string nomeTurma, string ano, string curso, TipoEstado estado) {
            _idTurma = idTurmaBD;
            NomeTurma = nomeTurma;
            Ano = ano;
            Curso = curso;
            Estado = estado;
        }
        #endregion

        #region "Outros Métodos"
        public string getInfo() {
            return $"ID: {IDTurma}\nNome:{NomeTurma}\nAno:{Ano}\nCurso:{Curso}";
        }

        public override string ToString() {
            return NomeTurma;
        }
        #endregion
    }
}

