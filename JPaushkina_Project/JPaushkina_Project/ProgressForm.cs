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
    public partial class ProgressForm : Form
    {
        private Thread _thread;
        private string _testName;
        private string _userName;
        private int _userID;
        private DataTable _userTable;
        private string[] _scales = new string[12];
        private int[] _scalesScore = new int[12];
        private string[] _results = new string[12];
        private Label[] scalesLabels = new Label[12];
        private DateTime _userBirthdayDate;
        private DateTime _today;

        public ProgressForm(string UserName)
        {
            InitializeComponent();
            radioButton2.Checked = true;
            _userName = UserName;
            _userTable = SQLSelect($"SELECT * FROM Пользователи WHERE ФИО = '{_userName}'");
            _userID = int.Parse(_userTable.Rows[0][0].ToString());
            _userBirthdayDate = Convert.ToDateTime(_userTable.Rows[0][2]);
            _today = DateTime.Now;
            PrevTryButton.Visible = false;
            TimeSpan age = _today.Subtract(_userBirthdayDate);
            if (age.TotalDays < 6570)
            {
                radioButton3.Visible = false;
            }
            FillingScales();
            
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

        private void FillingScales()
        {
            if (radioButton1.Checked)
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
            }
            if (radioButton2.Checked)
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
            }
            if (radioButton3.Checked)
            {
                TimeSpan age = _today.Subtract(_userBirthdayDate);
                if (age.TotalDays > 6570)
                {
                    _testName = "Стресс-ФИЭ";
                }
            }
            int tryNumber = int.Parse(label13.Text);
            DataTable scalesTable = SQLSelect($"SELECT Шкала FROM Тесты WHERE " +
                $" Тест = '{_testName}'  ORDER BY Шкала ASC");
            string currentScale = scalesTable.Rows[0][0].ToString();
            _scales[0] = currentScale;
            int counter = 1;
            for (int i = 0; i < scalesTable.Rows.Count; i++)
            {
                if (scalesTable.Rows[i][0].ToString() != currentScale)
                {
                    _scales[counter] = scalesTable.Rows[i][0].ToString();
                    counter++;
                    currentScale = scalesTable.Rows[i][0].ToString();
                }
            }
            for (int i = counter; i < 12; i++)
            {
                _scales[i] = "nothing";
            }

            DataTable currentTry = SQLSelect($"SELECT * FROM Пройденные_тесты WHERE " +
                $"ID_Пользователя = '{_userID}' AND Номер_попытки = '{tryNumber}' " +
                $"AND Название_теста = '{_testName}'");
            for (int i = 0; i < currentTry.Rows.Count; i++)
            {
                _scalesScore[i] = int.Parse(currentTry.Rows[i][3].ToString());
                _results[i] = currentTry.Rows[i][5].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(currentTry.Rows[i][7]);
            }
            for (int i = currentTry.Rows.Count; i < 12; i++)
            {
                _scalesScore[i] = 0;
                _results[i] = "";
            }

            

            scalesLabels[0] = label1;
            scalesLabels[1] = label2;
            scalesLabels[2] = label3;
            scalesLabels[3] = label4;
            scalesLabels[4] = label5;
            scalesLabels[5] = label6;
            scalesLabels[6] = label7;
            scalesLabels[7] = label8;
            scalesLabels[8] = label9;
            scalesLabels[9] = label10;
            scalesLabels[10] = label11;
            scalesLabels[11] = label12;
            for(int i = 0; i < 12; i++)
            {
                scalesLabels[i].Visible = true;
            }
            for (int i = 0; i < 12; i++)
            {
                if (_scales[i] == "nothing")
                {
                    scalesLabels[i].Visible = false;
                }
                else
                {
                    scalesLabels[i].Text = _scales[i] + " - " + _scalesScore[i]
                        + " баллов."; // Результат: " + _results[i];
                }
            }
        }

        private void PrevTryButton_Click(object sender, EventArgs e)
        {
            label13.Text = (int.Parse(label13.Text) - 1).ToString();
            if (int.Parse(label13.Text) <= 1)
            {
                PrevTryButton.Visible = false;
            }
            FillingScales();
        }

        private void NextTryButton_Click(object sender, EventArgs e)
        {
            label13.Text = (int.Parse(label13.Text) + 1).ToString();
            if (int.Parse(label13.Text) > 1)
            {
                PrevTryButton.Visible = true;
            }
            FillingScales();
        }

        private void TestListButton_Click(object sender, EventArgs e)
        {
            this.Close();
            _thread = new Thread(OpenTestListForm);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }

        private void OpenTestListForm(object newObject)
        {
            Application.Run(new TestListForm(_userName));
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            FillingScales();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            FillingScales();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            FillingScales();
        }
    }
}
