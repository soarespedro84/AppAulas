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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using AppAulas.Models;

namespace AppAulas {
    
    public partial class MainWindow : Window {

        private ObservableCollection<Aula> _contentor;
        private Aula _AulaAEditar;

        public MainWindow() {
            InitializeComponent();
            resetForm();
        }

        private void BtnApagar_Click(object sender, RoutedEventArgs e) {
            if (_AulaAEditar != null) {
                Aula_Helper ah = new Aula_Helper(App.ligacaoBD);
                ah.EliminarAula(_AulaAEditar);
            }
            resetForm();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) {
            resetForm();
        }

        private void BtnAtualizar_Click(object sender, RoutedEventArgs e) {
            Turma T = cmbTurma.SelectedItem as Turma;
            if (txtSumario.Text != "" && txtModulo.Text != ""  && T.NomeTurma != "") {
                Aula_Helper ah = new Aula_Helper(App.ligacaoBD);
                Aula a;
                if (_AulaAEditar == null) {a = new Aula();} else {a = _AulaAEditar;}
                a.Modulo = txtModulo.Text;
                a.Sumario = txtSumario.Text;
                a.TurmaEmAula = T;                
                try { a.Data = Convert.ToDateTime(dtpickerData.SelectedDate); } catch { a.Data = Convert.ToDateTime("1900/01/01"); }
                string estadoDaOperacao = ah.atualizarAula(a);
                if (estadoDaOperacao != "") MessageBox.Show("Erro:" + estadoDaOperacao); else {resetForm();}
            } else {
                MessageBox.Show("Todos os campos são de preenchimento obrigatório.");
            }
        }
        private void BtnListar_Click(object sender, RoutedEventArgs e) {
            resetForm();
            resetLista();
        }

        private void BtnSair_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void LstAula_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            _AulaAEditar = lstAula.SelectedItem as Aula;
            if (_AulaAEditar != null) setForm(_AulaAEditar);
        }

        private void resetLista() {
            string liga = App.ligacaoBD;
            Aula_Helper ah = new Aula_Helper(liga);
            _contentor = new ObservableCollection<Aula>(ah.listar(null));
            lstAula.ItemsSource = _contentor;
        }
       
        private void resetForm() {
            preencherComboTurma();
            txtModulo.Text = "";
            txtSumario.Text = "";
            cmbTurma.SelectedIndex = 0;
            dtpickerData.SelectedDate = DateTime.Now;
            btnApagar.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnAtualizar.Content = "Criar Aula";
            _AulaAEditar = null;
            resetLista();
        }
        
        private void preencherComboTurma() {
            Turma_Helper th = new Turma_Helper(App.ligacaoBD);
            if (cmbTurma.ItemsSource != null) cmbTurma.ItemsSource = null;
            if (cmbTurma.Items.Count > 0) cmbTurma.Items.Clear();
            List<Turma> listaTurmas = th.listar(Turma.TipoEstado.Ativo);
            if (listaTurmas.Count > 0) {cmbTurma.ItemsSource = listaTurmas;} 
            else {
                Turma turma = new Turma();
                turma.NomeTurma = "Sem Turmas";
                cmbTurma.Items.Add(turma);
            }
            cmbTurma.SelectedIndex = 0;
        }

        private void setForm(Aula aulaAPreencher) {
            cmbTurma.IsSynchronizedWithCurrentItem = true;         
            txtSumario.Text = aulaAPreencher.Sumario;
            txtModulo.Text = aulaAPreencher.Modulo;
            dtpickerData.SelectedDate = aulaAPreencher.Data;
            cmbTurma.SelectedIndex = findIndexForCombo(aulaAPreencher.TurmaEmAula);
            btnApagar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            btnAtualizar.Content = "Atualizar Aula";
        }

        private int findIndexForCombo(Turma turma) {
            int indexOut = -1;
            int iCnt = 0;
            int MaxItens = cmbTurma.Items.Count;
            while (indexOut == -1 && iCnt < MaxItens) {
                Turma onCombo = cmbTurma.Items[iCnt] as Turma;
                if (turma.NomeTurma == onCombo.NomeTurma) indexOut = iCnt;
                else iCnt++;
            }
            return indexOut;
        }

        private void BtnGerirTurmas_Click(object sender, RoutedEventArgs e) {
            JanelaGerirTurmas j = new JanelaGerirTurmas();
            j.ShowDialog();
        }
    }           
}
