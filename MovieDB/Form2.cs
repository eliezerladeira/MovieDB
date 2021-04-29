using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace MovieDB
{
    public partial class Form2 : Form
    {
        public string ano, publicadora, titulo, assistido, categoria;
        public int itemID;

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public Form2()
        { 
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            txtTitulo.Text = titulo;
            txtPublicadora.Text = publicadora;
            txtAno.Text = ano;
            cboCategoria.Text = categoria;

            if (assistido == "Sim") rdbSim.Checked = true;
            else if (assistido == "Não") rdbNao.Checked = true;
        }

        #region Update
        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            //sql query 
            Form1 f1 = new Form1();

            string categoriaString;

            titulo = txtTitulo.Text.ToString();
            publicadora = txtPublicadora.Text.ToString();
            ano = txtAno.Text.ToString();

            int _ano = 0;
            if (ano != "")
            {
                _ano = f1.VerificaAno(ano);
            }
            try
            {
                categoriaString = cboCategoria.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Selecione uma categoria!\nErro: " + ex.Message + "");
                return;
            }

            int categoria = 0;

            if (rdbSim.Checked == true)
            {
                assistido = "Sim";
            }
            else
            {
                assistido = "Não";
            }

            if (_ano != 1)
            {
                if (categoriaString == "Aventura") categoria = 1;
                if (categoriaString == "Comédia") categoria = 2;
                if (categoriaString == "Ação") categoria = 3;
                if (categoriaString == "Desenho") categoria = 4;
                if (categoriaString == "Romance") categoria = 5;
                if (categoriaString == "Fantasia") categoria = 6;
                if (categoriaString == "Suspense") categoria = 7;
                if (categoriaString == "Histórico") categoria = 8;
                if (categoriaString == "Drama") categoria = 9;
                if (categoriaString == "Horror") categoria = 10;
                if (categoriaString == "Ficção") categoria = 11;
                if (categoriaString == "Crime") categoria = 12;
                if (categoriaString == "Biografia") categoria = 13;
                if (categoriaString == "Documentário") categoria = 14;
                if (categoriaString == "Livro") categoria = 15;

                string SQLUpdateString;
                if (ano == "")
                {
                    SQLUpdateString = "UPDATE movie SET Title ='" + titulo.Replace("'", "''") + "', MovieYear=NULL, Publisher='" + publicadora + "', typeID=" + categoria + ", Previewed='" + assistido + "' WHERE movieID=" + itemID + "";
                }
                else
                {
                    SQLUpdateString = "UPDATE movie SET Title ='" + titulo.Replace("'", "''") + "', MovieYear=" + _ano + ", Publisher='" + publicadora + "', typeID=" + categoria + ", Previewed='" + assistido + "' WHERE movieID=" + itemID + "";
                }
                OleDbCommand SQLCommand = new OleDbCommand();
                SQLCommand.CommandText = SQLUpdateString;
                SQLCommand.Connection = f1.database;
                int response = SQLCommand.ExecuteNonQuery();
                MessageBox.Show("Atualização feita com sucesso!","Message",MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else 
            {
                MessageBox.Show("Formato do ano esta incorreto!\nInforme um ano válido.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAno.Clear();
                txtAno.Focus();
            }
        }
        #endregion

        private void button6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAtualizar_Click(null, null);
            }
        }

    }
}