using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace LivrariaApp
{
    public partial class employeeControl : UserControl
    {
        public employeeControl()
        {
            InitializeComponent();
        }

        SqlConnection cn = new SqlConnection(@"Persist Security Info=true;User ID=senac;Password=senac;
        Initial Catalog=db_Livraria;Server=TAU0588408W10-1;Encrypt=false;");

        SqlCommand cm = new SqlCommand();   

        SqlDataReader dt;

        private void desabilitaCampos()
        {
            lblNumero.Visible = false;
            lblCodigo.Visible = false;
            btnNovo.Enabled = true;
            tbxNome.Enabled= false;
            tbxLogin.Enabled= false;
            tbxSenha.Enabled= false;
            btnGravar.Enabled= false;
            btnAlterar.Enabled= false;
            btnRemover.Enabled= false;
            btnCancelar.Enabled= false;
        }
        private void habilitaCampos()
        {
            tbxNome.Enabled = true;
            tbxLogin.Enabled= true;
            tbxSenha.Enabled= true;
            btnGravar.Enabled= true;
            btnCancelar.Enabled= true;
            btnNovo.Enabled= false;
            tbxNome.Focus();
            tbxPesquisa.Text = "";
            dgvTabela.DataSource = null;
        }

        private void limparCampos()
        {
            tbxNome.Text = "";
            tbxLogin.Text = string.Empty;
            tbxSenha.Clear();
        }

        private void manipularDados()
        {
            lblNumero.Visible = true;
            lblCodigo.Visible = true;
            tbxNome.Enabled = true;
            tbxLogin.Enabled = true;
            tbxSenha.Enabled = true;
            btnGravar.Enabled = false;
            btnCancelar.Enabled = true;
            btnRemover.Enabled = true;
            btnAlterar.Enabled = true;
            btnNovo.Enabled = false;
         
        }

        private void employeeControl_Load(object sender, EventArgs e)
        {
            desabilitaCampos();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitaCampos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            desabilitaCampos();
            limparCampos();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            if (tbxNome.Text == "")
            {
                MessageBox.Show("Obrigatório informar o campo Nome", "Nome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxNome.Focus();
            }
           // else if(tbxLogin.Text == "")
           // {
              //  MessageBox.Show("Obrigatório informar o campo 'Login'", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
               // tbxLogin.Focus();
            //}
            else if (tbxSenha.Text == "")
            {
                MessageBox.Show("Obrigatório informar o campo 'Senha'", "Senha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxSenha.Focus();
            }
            else if(tbxSenha.Text.Length < 8)
            {
                MessageBox.Show("Obrigatório ter no mínimo 8 caracteres", "Senha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxSenha.Focus();
            }
            else
            {
                try
                {
                    string nome = tbxNome.Text;
                    string login = tbxLogin.Text;
                    string senha = tbxSenha.Text;

                    string strSql = "insert into tbl_atendente(ds_login,ds_senha,nm_atendente)values(@login,@senha,@atendente)";

                    cm.CommandText = strSql;
                    cm.Connection = cn;

                    //SqlCommand cm = new SqlCommand(sql, cn);

                    cm.Parameters.Clear();
                    
                    cm.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    cm.Parameters.Add("@senha", SqlDbType.Char).Value = senha;
                    cm.Parameters.Add("@atendente", SqlDbType.VarChar).Value = nome;
                    //cm.Parameters.AddWithValue("@Login",tbxLogin.Text);
                    //cm.Parameters.AddWithValue("@Login",login);

                    cn.Open();
                    cm.ExecuteNonQuery();

                    MessageBox.Show("Os dados foram gravados", "Inserção de dados", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    tbxNome.Focus();
                    limparCampos();
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    cn.Close();
                }
                finally
                {
                    cn.Close();
                }

            }
        }
        private void tbxPesquisa_TextChanged(object sender, EventArgs e)
        {
            if(tbxPesquisa.Text != "")
            {
                try
                {
                    cn.Open();
                    cm.CommandText = "select * from tbl_atendente where nm_atendente like ('%" + tbxPesquisa.Text + "%')";
                    cm.Connection = cn;

                    //Receber dados de uma tabela após a execução do Select
                    SqlDataAdapter da = new SqlDataAdapter();

                    //Objeto que pode representar uma ou mais tabelas de dados
                    // quais permanecem alocadas em memória
                    DataTable dt = new DataTable();

                    da.SelectCommand = cm;
                    //Método ou função como ato de abrir os DataTable
                    da.Fill(dt);//Preenchimento do DataTable
                    dgvTabela.DataSource = dt;
                    cn.Close();

                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
                
            }
            else
            {
                dgvTabela.DataSource = null;
            }
        }

        private void carregaAtendente()
        {
            
                lblNumero.Text = dgvTabela.SelectedRows[0].Cells[0].Value.ToString();
                tbxNome.Text = dgvTabela.SelectedRows[0].Cells[3].Value.ToString();
                tbxLogin.Text = dgvTabela.SelectedRows[0].Cells[1].Value.ToString();
                tbxSenha.Text = dgvTabela.SelectedRows[0].Cells[2].Value.ToString();
                manipularDados();
          
        }

        private void dgvTabela_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
                carregaAtendente();
            
        

    }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (tbxNome.Text == "")
            {
                MessageBox.Show("Obrigatório informar o campo Nome", "Nome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxNome.Focus();
            }            
            else if (tbxSenha.Text == "")
            {
                MessageBox.Show("Obrigatório informar o campo 'Senha'", "Senha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxSenha.Focus();
            }
            else if (tbxSenha.Text.Length < 8)
            {
                MessageBox.Show("Obrigatório ter no mínimo 8 caracteres", "Senha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxSenha.Focus();
            }
            else
            {
                try
                {
                    string nome = tbxNome.Text;
                    string login = tbxLogin.Text;
                    string senha = tbxSenha.Text;
                    int cd = Convert.ToInt32(lblNumero.Text);

                    string strSql = "update tbl_atendente set ds_login=@login, ds_senha=@senha, nm_atendente=@nome where id_atendente=@cd";

                    cm.CommandText = strSql;
                    cm.Connection = cn;

                    //SqlCommand cm = new SqlCommand(sql, cn);

                    cm.Parameters.Clear();

                    cm.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    cm.Parameters.Add("@senha", SqlDbType.Char).Value = senha;
                    cm.Parameters.Add("@nome", SqlDbType.VarChar).Value = nome;
                    cm.Parameters.Add("@cd", SqlDbType.Int).Value = cd;
                    //cm.Parameters.AddWithValue("@Login",tbxLogin.Text);
                    //cm.Parameters.AddWithValue("@Login",login);

                    cn.Open();
                    cm.ExecuteNonQuery();

                    MessageBox.Show("Os dados foram atualizados com sucesso", "Alteração de dados", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    tbxNome.Focus();
                    limparCampos();
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    cn.Close();
                }
                finally
                {
                    cn.Close();
                }

            }
        }
    }
}
