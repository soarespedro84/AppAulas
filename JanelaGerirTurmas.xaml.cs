using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using AppAulas.Models;


namespace AppAulas
{
    /// <summary>
    /// Interaction logic for JanelaGerirTurmas.xaml
    /// </summary>
    public partial class JanelaGerirTurmas : Window
    {
        private ObservableCollection<Turma> _contentor;
        private Turma _TurmaAEditar;

        public JanelaGerirTurmas()
        {
            InitializeComponent();
            resetForm();
        }

        private void BtnApagarTurma_Click(object sender, RoutedEventArgs e) {
            if (_TurmaAEditar != null) {
                Turma_Helper th = new Turma_Helper(App.ligacaoBD);
                th.EliminarTurma(_TurmaAEditar);
            }
            resetForm();
        }

        private void BtnCancelarTurma_Click(object sender, RoutedEventArgs e) {
            resetForm();
        }

        private void BtnAtualizarTurma_Click(object sender, RoutedEventArgs e) {
            Turma T = lstTurma.SelectedItem as Turma;
            if (txtCurso.Text != "" && txtAno.Text != "" && txtTurma.Text != "") {
                Turma_Helper th = new Turma_Helper(App.ligacaoBD);
                Turma t;
                if (_TurmaAEditar == null) { t = new Turma(); } else {t = _TurmaAEditar; }
                t.Ano= txtAno.Text;
                t.Curso = txtCurso.Text;
                t.NomeTurma = txtTurma.Text;
                
                string estadoDaOperacao = th.atualizarTurma(t);
                if (estadoDaOperacao != "") MessageBox.Show("Erro:" + estadoDaOperacao); else { resetForm(); }
            } else {
                MessageBox.Show("Todos os campos são de preenchimento obrigatório.");
            }
        }

        private void BtnListarTurma_Click(object sender, RoutedEventArgs e) {
            resetForm();
            resetLista();
        }

        private void BtnSair_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void LstTurma_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            _TurmaAEditar = lstTurma.SelectedItem as Turma;
            if (_TurmaAEditar != null) setForm(_TurmaAEditar);
        }

        private void resetLista() {
            string liga = App.ligacaoBD;
            Turma_Helper th = new Turma_Helper(liga);
            _contentor = new ObservableCollection<Turma>(th.listar(Turma.TipoEstado.Ativo));
            lstTurma.ItemsSource = _contentor;
        }

        private void resetForm() {
            txtAno.Text = "";
            txtCurso.Text = "";
            txtTurma.Text = "";
            btnApagarTurma.IsEnabled = false;
            btnCancelarTurma.IsEnabled = false;
            btnAtualizarTurma.Content = "Criar Turma";
            _TurmaAEditar = null;
            resetLista();
        }

        private void setForm(Turma turmaAPreencher) {
            //cmbTurma.IsSynchronizedWithCurrentItem = true;
            txtCurso.Text = turmaAPreencher.Curso;
            txtAno.Text = turmaAPreencher.Ano;
            txtTurma.Text = turmaAPreencher.NomeTurma;
            //cmbTurma.SelectedIndex = findIndexForCombo(turmaAPreencher.TurmaEmAula);
            btnApagarTurma.IsEnabled = true;
            btnCancelarTurma.IsEnabled = true;
            btnAtualizarTurma.Content = "Atualizar Turma";
        }
        
    }
}
