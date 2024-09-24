using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace LivrariaApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        //Estabelecer conexao com banco de dados sql server
        SqlConnection cn = new SqlConnection(@"Persist Security Info=true;User ID=senac;Password=senac;
        Initial Catalog=db_Livraria;Server=TAU0588422W10-1;Encrypt=false;"

);

        SqlCommand cm = new SqlCommand();

        SqlDataReader dt;

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pbxPassword_MouseDown(object sender, MouseEventArgs e)
        {
            tbxPassword.UseSystemPasswordChar = false;
        }

        private void pbxPassword_MouseUp(object sender, MouseEventArgs e)
        {
            tbxPassword.UseSystemPasswordChar = true;
        }

        private void btnAcessar_Click(object sender, EventArgs e)
        {
            if(tbxName.Text == "" || tbxPassword.Text == "")
            {
                MessageBox.Show("Preencher o nome e a senha do usuário","Login",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                try {//Tentar 
                    cn.Open();
                    cm.CommandText = "select * from tbl_Atendente1 where ds_Login = ('" + tbxName.Text + "') and ds_Senha = ('" + tbxPassword.Text + "')";
                    cm.Connection = cn;
                    dt = cm.ExecuteReader();

                    if(dt.HasRows)
                    {
                        frmMenu menu= new frmMenu();
                        menu.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Preencher o nome ou a senha do usuário corretamente", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tbxName.Clear();
                        tbxPassword.Clear();
                        tbxName.Focus();
                    }
                }
                catch(Exception erro)//Pegar
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
