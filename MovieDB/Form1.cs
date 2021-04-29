using System;
using System.Data;
using System.Data.OleDb; 
using System.Windows.Forms;

namespace MovieDB
{
    public partial class Form1 : Form
    {
        public OleDbConnection database;
        DataGridViewButtonColumn editButton;
        DataGridViewButtonColumn deleteButton;

        int movieIDInt;

        #region constructor formulário
        public Form1()
        {

            InitializeComponent();
            // inicializa string de conexão
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=moviedb.mdb";
            try
            {
                database = new OleDbConnection(connectionString);
                database.Open();
                //SQL consulta para lista de itens
                string queryString = "SELECT movieID, Title, Publisher, Previewed, MovieYear, Type FROM movie,movieType WHERE movietype.typeID = movie.typeID";
                carregaDataGrid(queryString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion

        #region carrega datagridview
        public void carregaDataGrid(string sqlQueryString) {

            OleDbCommand SQLQuery = new OleDbCommand();
            DataTable data = null;
            dgvItens.DataSource = null;
            SQLQuery.Connection = null;
            OleDbDataAdapter dataAdapter = null;
            dgvItens.Columns.Clear(); // <-- limpa colunas
            //---------------------------------
            SQLQuery.CommandText = sqlQueryString;
            SQLQuery.Connection = database;
            data = new DataTable();
            dataAdapter = new OleDbDataAdapter(SQLQuery);
            dataAdapter.Fill(data);
            dgvItens.DataSource = data;
            dgvItens.AllowUserToAddRows = false; // remove linha nula
            dgvItens.ReadOnly = true;
            dgvItens.Columns[0].Visible = false;
            dgvItens.Columns[1].Width = 340;
            dgvItens.Columns[3].Width = 55;
            dgvItens.Columns[4].Width = 50;
            dgvItens.Columns[5].Width = 80;
            // insere botão edita no datagridview
            editButton = new DataGridViewButtonColumn();
            editButton.HeaderText = "Edita";
            editButton.Text = "Edita";
            editButton.UseColumnTextForButtonValue = true;
            editButton.Width = 80;
            dgvItens.Columns.Add(editButton);
            // insere botão delete no datagridview
            deleteButton = new DataGridViewButtonColumn();
            deleteButton.HeaderText = "Deleta";
            deleteButton.Text = "Deleta";
            deleteButton.UseColumnTextForButtonValue = true;
            deleteButton.Width = 80;
            dgvItens.Columns.Add(deleteButton);
        }
        #endregion

        private void izlazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        #region fecha conexão database
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            database.Close();
        }
        #endregion

        #region botão atualiza
        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            string queryString = "SELECT movieID, Title, Publisher, Previewed, MovieYear, Type FROM movie,movieType WHERE movietype.typeID = movie.typeID";
            carregaDataGrid(queryString);
        }
        #endregion

        #region Input
        private void button6_Click(object sender, EventArgs e)
        {
            string categoriaString;
            try
            {
                categoriaString = cboCategoria.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Informe a Categoria\nErro: " + ex.Message + "");
                return;
            }

            int categoria = 0;

            string nome = txtTitulo.Text.ToString();
            string publicadora = txtPublicadora.Text.ToString();
            string ano = txtAno.Text.ToString();

            int _ano = 0;
            if (ano != "")
            {
                _ano = VerificaAno(ano);
            }
            string assistido;
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

                string SQLString ="";
     
                    if (ano == "")
                    {
                        SQLString = "INSERT INTO movie(Title, Publisher, Previewed, typeID) VALUES('" + nome.Replace("'", "''") + "','" + publicadora + "','" + assistido + "'," + categoria + ");";
                    }
                    else
                    {
                        //MessageBox.Show(_ano.ToString());
                        SQLString = "INSERT INTO movie(Title, Publisher, Previewed, MovieYear, typeID) VALUES('" + nome.Replace("'", "''") + "','" + publicadora + "','" + assistido + "'," + _ano + "," + categoria + ");";
                    }


                OleDbCommand SQLCommand = new OleDbCommand();
                SQLCommand.CommandText = SQLString;
                SQLCommand.Connection = database;
                int resposta = -1;
                try
                {
                    resposta = SQLCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (resposta >= 1)
                    MessageBox.Show("Item foi incluído no banco de dados","Sucesso",MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtTitulo.Clear();
                txtPublicadora.Clear();
                txtAno.Clear();
                cboCategoria.ResetText();
                rdbSim.Checked = rdbNao.Checked = false;
            }
            else
            {
                MessageBox.Show("O formato do ano não esta correto!\nInforme um ano válido.", "Alerta",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAno.Clear();
                txtAno.Focus();
            }
        }

        public int VerificaAno(string ano)
        {
                int _ano = int.Parse(ano);
                if (_ano >= 2100 || _ano <= 1900)
                {
                    return 1;
                }
                else
                {
                    return _ano;
                }
        }
        #endregion

        #region trata o botão Deleta/Edita 
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string queryString = "SELECT movieID, Title, Publisher, Previewed, MovieYear, Type FROM movie,movieType WHERE movietype.typeID = movie.typeID";

            int linhaAtual = int.Parse(e.RowIndex.ToString());

            if (linhaAtual < 0)
                return;
            
            try
            {
                string movieIDString = dgvItens[0, linhaAtual].Value.ToString();
                movieIDInt = int.Parse(movieIDString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            // editar 
            if (dgvItens.Columns[e.ColumnIndex] == editButton && linhaAtual >= 0)
            {
                try
                {
                    string titulo = dgvItens[1, linhaAtual].Value.ToString();
                    string publicadora = dgvItens[2, linhaAtual].Value.ToString();
                    string assistido = dgvItens[3, linhaAtual].Value.ToString();
                    string ano = dgvItens[4, linhaAtual].Value.ToString();
                    string categoria = dgvItens[5, linhaAtual].Value.ToString();

                    //executa o form2 para edição
                    Form2 f2 = new Form2();
                    f2.titulo = titulo;
                    f2.publicadora = publicadora;
                    f2.assistido = assistido;
                    f2.ano = ano;
                    f2.categoria = categoria;
                    f2.itemID = movieIDInt;
                    f2.Show();
                    dgvItens.Update();
                }
                catch { }
            }
            // botão deletar
            else if (dgvItens.Columns[e.ColumnIndex] == deleteButton && linhaAtual >= 0)
            {
                if (MessageBox.Show("Confirma ?", "Deleta", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    // consulta sql delete sql
                    string queryDeleteString = "DELETE FROM movie where movieID = " + movieIDInt + "";
                    OleDbCommand sqlDelete = new OleDbCommand();
                    sqlDelete.CommandText = queryDeleteString;
                    sqlDelete.Connection = database;
                    sqlDelete.ExecuteNonQuery();
                    carregaDataGrid(queryString);
                }
            }
         }
        #endregion
         
        #region procurar por titulo
        private void button1_Click(object sender, EventArgs e)
        {
            string titulo = textBox4.Text.ToString();
            if (titulo != "")
            {
                string queryString = "SELECT movieID, Title, Publisher, Previewed, MovieYear, Type FROM movie,movietype WHERE movietype.typeID = movie.typeID AND movie.title LIKE '" + titulo + "%'";
                carregaDataGrid(queryString);
            }
            else
            {
                MessageBox.Show("Informe o título do item","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region procurar por Categoria
        private void button5_Click(object sender, EventArgs e)
        {
            int categoria = 0;
            string categoriaString = comboBox2.SelectedItem.ToString();
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
            
            string queryString = "SELECT movieID, Title, Publisher, Previewed, MovieYear, Type FROM movie,movietype WHERE movietype.typeID = movie.typeID AND movie.typeID = " + categoria + "";
            carregaDataGrid(queryString);
        }
        #endregion

        #region procurar por ano
        private void button4_Click(object sender, EventArgs e)
        {
            string primeiroAno = textBox5.Text.ToString();
            string segundoAno = textBox6.Text.ToString();;
            int ano1 = VerificaAno(primeiroAno);
            int ano2 = VerificaAno(segundoAno);

            if ((ano1 != 1 && ano2 != 1) && ano1 <= ano2)
            {
                string queryString = "SELECT movieID, Title, Publisher, Previewed, MovieYear, Type FROM movie,movietype WHERE movietype.typeID = movie.typeID AND movie.MovieYear BETWEEN " + ano1 + " AND " + ano2 + "";
                carregaDataGrid(queryString);
            }
            else
            {
                MessageBox.Show("O formato do ano não esta correto, inclua um ano válido.","Alerta",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox5.Clear();
                textBox5.Focus();
                textBox6.Clear();
            }
        }
        #endregion

        #region procurar itens lidos/assistidos
        private void button3_Click(object sender, EventArgs e)
        {
            string assistido;

            if (radioButton3.Checked == true)
                assistido = "Sim";
            else
                assistido = "Não";

            string queryString = "SELECT movieID, Title, Publisher, Previewed, MovieYear, Type FROM movie,movietype WHERE movietype.typeID = movie.typeID AND Previewed ='" + assistido + "'";
            carregaDataGrid(queryString);
        }
        #endregion

        private void button6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6_Click(null, null);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string queryString = "SELECT movieID, Title, Publisher, Previewed, MovieYear, Type FROM movie,movieType WHERE movietype.typeID = movie.typeID";
            carregaDataGrid(queryString);
        }

        private void txtAno_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}