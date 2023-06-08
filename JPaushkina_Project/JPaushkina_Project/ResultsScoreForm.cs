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
    public partial class ResultsScoreForm : Form
    {
        private Thread _thread;
        private string _testName;
        private string _userName;
        private string[] _scales;
        private int[] _scalesScore;
        private Label[] scalesLabels = new Label[12];

        public ResultsScoreForm(string UserName, string TestName,
            string[] scales, int[] scalesScore)
        {
            InitializeComponent();
            label2.Text = TestName;
            _userName = UserName;
            _testName = TestName;
            _scales = scales;
            _scalesScore = scalesScore;
            FillingScales();
            ResultEstimation();
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
            scalesLabels[0] = label3;
            scalesLabels[1] = label4;
            scalesLabels[2] = label5;
            scalesLabels[3] = label6;
            scalesLabels[4] = label7;
            scalesLabels[5] = label8;
            scalesLabels[6] = label9;
            scalesLabels[7] = label10;
            scalesLabels[8] = label11;
            scalesLabels[9] = label12;
            scalesLabels[10] = label13;
            scalesLabels[11] = label14;
            for(int i = 0; i < 12; i++)
            {
                if (_scales[i] == "nothing")
                {
                    scalesLabels[i].Visible = false;
                    textBox1.Top -= 27;
                    BackToTestsButton.Top -= 27;
                }
                else
                {
                    scalesLabels[i].Text = _scales[i] + " - " + _scalesScore[i];
                }                
            }
        }

        private void ResultEstimation()
        {
            DataTable userTable = SQLSelect($"SELECT * FROM Пользователи WHERE " +
                $"ФИО = '{_userName}'");
            int userID = int.Parse(userTable.Rows[0][0].ToString());
            DataTable previousAttempts = SQLSelect($"SELECT Номер_попытки FROM " +
                $"Пройденные_тесты WHERE ID_Пользователя = '{userID}' AND Название_теста = '{_testName}'");
            int lastTrial;
            if (previousAttempts.Rows.Count > 0)
            {
                lastTrial = int.Parse(previousAttempts.Rows[previousAttempts.Rows.Count - 1][0].ToString())+1;
            }
            else
            {
                lastTrial = 1;
            }
            for (int i = 0; i < 12; i++)
            {
                if (_scales[i] != "nothing")
                {
                    DataTable resultTable = SQLSelect($"SELECT Результат, " +
                        $"Минимальное_количество_баллов, Максимальное_количество_баллов " +
                        $"FROM Оценка_результатов WHERE Шкала = '{_scales[i]}' AND " +
                        $"Название_теста = '{_testName}'");
                    for (int j = 0; j < resultTable.Rows.Count; j++)
                    {
                        if (int.Parse(resultTable.Rows[j][1].ToString()) <= _scalesScore[i]
                            && int.Parse(resultTable.Rows[j][2].ToString()) >= _scalesScore[i])
                        {
                            string result = resultTable.Rows[j][0].ToString();
                            SaveResults(_scales[i], _scalesScore[i], result, lastTrial);
                            result += "\r\n";
                            textBox1.Text += result;
                            break;
                        }
                    }
                }
            }
        }

        private void SaveResults(string scale, int score, string result, int tryNumber)
        {
            SqlConnection connection = new
            SqlConnection(Properties.Settings.Default.Test_baseConnectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Пройденные_тесты" +
                " (ID_Пользователя, Название_теста, Балл, Шкала, Результат, Номер_попытки, " +
                "Дата) VALUES (@IDUserValue, @TestNameValue, @ScoreValue, " +
                "@ScaleValue, @ResultValue, @NumberOfTrial, @Date)";
            DataTable userTable = SQLSelect($"SELECT * FROM Пользователи WHERE " +
                $"ФИО = '{_userName}'");
            int userID = int.Parse(userTable.Rows[0][0].ToString());

            command.Parameters.AddWithValue("@IDUserValue", userID);
            command.Parameters.AddWithValue("@TestNameValue", _testName);
            command.Parameters.AddWithValue("@ScoreValue", score);
            command.Parameters.AddWithValue("@ScaleValue", scale);
            command.Parameters.AddWithValue("@ResultValue", result);
            command.Parameters.AddWithValue("@NumberOfTrial", tryNumber);
            command.Parameters.AddWithValue("@Date", DateTime.Now);

            command.ExecuteNonQuery();
            connection.Close();
        }

        private void BackToTestsButton_Click(object sender, EventArgs e)
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
