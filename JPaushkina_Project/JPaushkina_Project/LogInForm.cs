using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPaushkina_Project
{
    public partial class LogInForm : Form
    {
        private Thread _thread;
        private string _userName;

        public LogInForm()
        {
            InitializeComponent();
        }

        private DataTable SQLSelect(string sqlSelect)
        {
            //Создаем объект connection класса SqlConnection для соединения с БД 
            //StorageConnectionString – строка описания соединения с источником данных 
            SqlConnection connection = new
            SqlConnection(Properties.Settings.Default.Test_baseConnectionString);
            //Создаем объект command для SQL команды 
            SqlCommand command = connection.CreateCommand();
            //Заносим текст SQL запроса через параметр sqlSelect
            command.CommandText = sqlSelect;
            //Создаем объект adapter класса SqlDataAdapter 
            SqlDataAdapter adapter = new SqlDataAdapter();
            //Задаем адаптеру нужную команду, в данном случае команду Select 
            adapter.SelectCommand = command;
            //Создаем объект table для последующего отображения результата запроса
            DataTable table = new DataTable();
            //заполним набор данных результатом запроса
            adapter.Fill(table);
            return table;
        }

        private void пользователиBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.пользователиBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.testDataSet);

        }

        private void LogInForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Пользователи". При необходимости она может быть перемещена или удалена.
            //this.пользователиTableAdapter.Fill(this.testDataSet.Пользователи);

        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            _userName = фИОTextBox.Text;
            DataTable usersListTable = SQLSelect("SELECT * FROM Пользователи");
            if (usersListTable.Rows.Count == 0) 
            {
                MessageBox.Show("Отсутсвуют зарегистрированные пользователи." +
                    " Нажмите кнопку регистрации");
            }
            else
            {
                foreach (DataRow row in usersListTable.Rows)
                {
                    if ((row[1].Equals(фИОTextBox.Text)) &&
                        (row[4].Equals(парольTextBox.Text)))
                    {
                        this.Close();
                        _thread = new Thread(OpenTestListForm);
                        _thread.SetApartmentState(ApartmentState.STA);
                        _thread.Start();
                    }
                }
                MessageBox.Show("Неправильное имя пользователя или пароль");
            }
        }

        private void RegistrationButton_Click(object sender, EventArgs e)
        {
            this.Close();
            _thread = new Thread(OpenRegistrationForm);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }

        private void OpenRegistrationForm(object newObject)
        {
            Application.Run(new RegistrationForm());
        }

        private void OpenTestListForm(object newObject)
        {
            Application.Run(new TestListForm(_userName));
        }
    }
}
