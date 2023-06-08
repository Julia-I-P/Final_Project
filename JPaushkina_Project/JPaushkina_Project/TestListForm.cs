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
    public partial class TestListForm : Form
    {
        private Thread _thread;
        private string _testName;
        private string _userName; 
        private DataTable _user;
        private DateTime _userBirthdayDate;
        private DateTime _today;

        public TestListForm(string userName)
        {
            InitializeComponent();
            _userName = userName;
            _user = SQLSelect($"SELECT * FROM Пользователи WHERE ФИО = '{_userName}'");
            _userBirthdayDate = Convert.ToDateTime(_user.Rows[0][2]);
            _today = DateTime.Now;
            CheckAgePerson();
        }

        private void CheckAgePerson()
        {
            TimeSpan age = _today.Subtract(_userBirthdayDate);
            if (age.TotalDays < 6570)
            {
                StressButton.Visible = false;
            }
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

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            this.Close();
            _thread = new Thread(OpenLogInForm);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }

        private void ProgressButton_Click(object sender, EventArgs e)
        {
            this.Close();
            _thread = new Thread(OpenProgressForm);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }

        private void SelfesteemButton_Click(object sender, EventArgs e)
        {
            TimeSpan age = _today.Subtract(_userBirthdayDate);
            if (age.TotalDays > 6570)
            {
                _testName = "Опросник самооценки Уайнхолд";
            }
            else
            {
                _testName = "Шкала оценки своей компетентности";
            }
            this.Close();
            _thread = new Thread(OpenQuestionsForm);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }

        private void StressButton_Click(object sender, EventArgs e)
        {
            _testName = "Стресс-ФИЭ";
            this.Close();
            _thread = new Thread(OpenQuestionsForm);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();

        }

        private void AnxietyButton_Click(object sender, EventArgs e)
        {
            TimeSpan age = _today.Subtract(_userBirthdayDate);
            if (age.TotalDays > 6570)
            {
                _testName = "Интегративный тест тревожности";
            }
            else
            {
                _testName = "Шкала личностной тревожности учащихся (А.М. Прихожан)";
            }
            this.Close();
            _thread = new Thread(OpenQuestionsForm);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }

        private void OpenLogInForm(object newObject)
        {
            Application.Run(new LogInForm());
        }

        private void OpenQuestionsForm(object newObject)
        {
            Application.Run(new QuestionsForm(_userName, _testName));
        }

        private void OpenProgressForm(object newObject)
        {
            Application.Run(new ProgressForm(_userName));
        }
    }
}
