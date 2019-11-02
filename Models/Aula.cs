using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAulas.Models
{
    class Aula
    {
        #region "Constantes"
        public const int MAXSUMARIO = 255;
        #endregion

        #region "Atributos"
        private string _idAula;
        private Turma _nomeTurma;
        private string _modulo;
        private string _sumario;
        private DateTime _data;
        #endregion

        #region "Propriedades/Métodos"
        public string IDTurma {
            get { return _idAula; }
        }
        public Turma TurmaEmAula {
            get { return _nomeTurma; }
            set { _nomeTurma = value; }
        }
        public string Modulo {
            get { return _modulo; }
            set {_modulo = value;}
        }
        public string Sumario {
            get { return _sumario; }
            set {
                _sumario = value;
                if (_sumario.Length > MAXSUMARIO) _sumario = _sumario.Substring(0, MAXSUMARIO);
            }
        }
        public DateTime Data {
            get { return _data; }
            set { _data = value; }
        }
        #endregion

        #region "Construtores"
        public Aula() {
            _idAula = "";
            TurmaEmAula = null;            
            Modulo = "";
            Sumario = "";
            Data = Convert.ToDateTime("1900/01/01");
            
        }
        public Aula(string guidDaBD) {
            _idAula = guidDaBD;
            TurmaEmAula = null;
            Modulo = "";
            Sumario = "";            
            Data = Convert.ToDateTime("1900/01/01");            
        }
        #endregion

        #region "Outros Métodos"
        public string getInfo() {
            return $"ID: {IDTurma}\nTurma:{TurmaEmAula}\nData:{Data}\nMódulo:{Modulo}\nSumário:{Sumario}";
        }
        #endregion
    }
}