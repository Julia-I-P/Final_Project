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
    public partial class QuestionsForm : Form
    {
        private Thread _thread;
        private string _testName;
        private string _userName;
        private DataTable _questionsTable;
        private string _currentQuestion;
        private int _counter = 0;
        private int _mainScaleNumber = 0;
        private int _mainScaleCount = 1;
        private string _mainScale;
        private string _mainScaleTwo;

        private RadioButton[] answers = new RadioButton[5];
        
        //Переменные для подсчёта баллов по шкалам
        private string[] _scales = new string[12];
        private int[] _scoreForScales = new int[12];

        public QuestionsForm(string UserName, string TestName)
        {
            InitializeComponent();
            TestNamelabel.Text = TestName;
            _testName = TestName;
            _userName = UserName;
            _questionsTable = SQLSelect($"SELECT * FROM Тесты WHERE Тест = '{_testName}'");
            QuestionTextBox.Text = (_questionsTable.Rows[0][3]).ToString();
            _currentQuestion = (_questionsTable.Rows[0][3]).ToString();
            TaskLabelFill();
            FillingAnswerOptions();
            FillingScalesNamesAndScores();
            InstructionTextBoxFill();
        }

        private void InstructionTextBoxFill()
        {
            if (_testName == "Опросник самооценки Уайнхолд")
            {
                textBox1.Text = "Оцените каждый из приведенных ниже пунктов в зависимости" +
                    " от того, насколько последовательно вы способны придерживаться  указанной" +
                    " линии поведения.";
            }
            if (_testName == "Шкала оценки своей компетентности")
            {
                textBox1.Text = "Внимательно прочитай пары утверждений и выбери то," +
                    " с которым согласен больше всего – «Всегда верно 1 или 2».\r\nЕсли" +
                    " это утверждение верно для тебя, но не во всех ситуациях – выбери" +
                    " ответ «Скорее 1 или 2».";
            }
            if (_testName == "Интегративный тест тревожности")
            {
                textBox1.Text = "Вам будут предложены несколько утверждений, касающихся" +
                    " вашего эмоционального состояния.\r\nВ первой части опросника в " +
                    "отношении каждого из них вам нужно будет решить, насколько данное " +
                    "состояние выражено именно сейчас, в данный момент, сегодня. Варианты" +
                    " ответов – «Совсем нет», «Слабо выражено», «Выражено», «Очень выражено»." +
                    "\r\nВо второй части опросника в отношении тех же утверждений нужно решить" +
                    " – как часто на протяжении последнего времени вы испытывали эти состояния." +
                    " Варианты ответов – «Почти никогда», «Редко», «Часто», «Почти всё время».";
            }
            if (_testName == "Шкала личностной тревожности учащихся (А.М. Прихожан)")
            {
                textBox1.Text = "Внимательно прочти каждое предложение, представь себя в " +
                    "этих обстоятельствах и выбери один из предложенных ответов в зависимости" +
                    " от того, насколько эта ситуация для тебя неприятна, насколько она может" +
                    " вызвать у тебя беспокойство, опасения или страх.\r\nЕсли ситуация " +
                    "совершенно не кажется тебе неприятной, выбери ответ «Нет».\r\nЕсли она " +
                    "немного тревожит, беспокоит тебя, отвечай «Немного».\r\nЕсли беспокойство" +
                    " и страх достаточно сильны, и тебе хотелось бы не попадать в такую ситуацию," +
                    " выбери ответ «Достаточно».\r\nЕсли ситуация очень неприятна и с ней " +
                    "связаны сильные беспокойство, тревога, страх, отвечай «Значительно»." +
                    "\r\nПри очень сильном беспокойстве, очень сильном страхе выбери ответ «Очень».";
            }
            if (_testName == "Стресс-ФИЭ")
            {
                textBox1.Text = "Оцените, как часто у вас бывают следующие мысли, чувства и ощущения.";
            }
        }

        private void TaskLabelFill()
        {
            if (_testName == "Интегративный тест тревожности")
            {
                TaskLabel.Text = "Сейчас, в данный момент:";
            }
            else
            {
                TaskLabel.Visible = false;
            }
        }

        private void FillingAnswerOptions()
        {
            answers[0] = radioButton1;
            answers[1] = radioButton2;
            answers[2] = radioButton3;
            answers[3] = radioButton4;
            answers[4] = radioButton5;
            DataTable currentQuestionTable = SQLSelect($"SELECT Вариант_ответа" +
                $" FROM Тесты WHERE Вопрос LIKE '{_currentQuestion}' AND Тест = '{_testName}'");
            for (int i = 0; i < currentQuestionTable.Rows.Count; i++)
            {
                answers[i].Text = currentQuestionTable.Rows[i][0].ToString();
            }
            for(int i = currentQuestionTable.Rows.Count; i < 5; i++)
            {
                answers[i].Visible = false;
            }
        }

        private void FillingScalesNamesAndScores()
        {
            DataTable scalesTable = SQLSelect($"SELECT Шкала FROM Тесты WHERE Тест = '{_testName}'" +
                $"ORDER BY Шкала ASC");
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
            if (_testName == "Шкала личностной тревожности учащихся (А.М. Прихожан)")
            {
                _mainScale = "Шкала личностной тревожности учащихся";
                _mainScaleCount = 1;
            }
            if (_testName == "Интегративный тест тревожности")
            {
                _mainScale = "Личностная тревожность (СТ-Л)";
                _mainScaleTwo = "Ситуационная тревожность (СТ-С)";
                _mainScaleCount = 2;
            }
            if (_testName == "Шкала оценки своей компетентности")
            {
                _mainScale = "Общая шкала оценки компетентности";
                _mainScaleCount = 1;
            }
            if (_testName == "Опросник самооценки Уайнхолд")
            {
                _mainScale = "Общая шкала самооценки";
                _mainScaleCount = 1;
            }
            if (_testName == "Стресс-ФИЭ")
            {
                _mainScale = "Общая шкала стресса";
                _mainScaleCount = 1;
            }

            if (_mainScaleCount == 1)
            {
                _mainScaleNumber = counter;
                _scales[_mainScaleNumber] = _mainScale;
                for (int i = counter + 1; i < 12; i++)
                {
                    _scales[i] = "nothing";
                }
                for (int i = 0; i < 12; i++)
                {
                    _scoreForScales[i] = 0;
                }
            }
            else
            {
                _mainScaleNumber = counter;
                _scales[_mainScaleNumber] = _mainScale;
                _scales[_mainScaleNumber+1] = _mainScaleTwo;

                for (int i = 0; i < 12; i++)
                {
                    _scoreForScales[i] = 0;
                }
            }
        }

        private void CalculationsScores(int numberOfAnswer)
        {
            DataTable currentQuestionTable = SQLSelect($"SELECT Шкала, Вариант_ответа, Балл" +
                $" FROM Тесты WHERE Вопрос LIKE '{_currentQuestion}' AND Тест = '{_testName}'");
            string temporaryScaleName = "";
            for (int i = 0; i < 12; i++)
            {
                if (_scales[i] == currentQuestionTable.Rows[0][0].ToString())
                {
                    _scoreForScales[i] += int.Parse(currentQuestionTable.Rows[numberOfAnswer][2].ToString());
                    temporaryScaleName = _scales[i];
                }
            }
            if (_mainScaleCount == 2)
            {
                if (_counter >= 15)
                {
                    _scoreForScales[_mainScaleNumber] += int.Parse(currentQuestionTable.Rows[numberOfAnswer][2].ToString());
                }
                else
                {
                    _scoreForScales[_mainScaleNumber + 1] += int.Parse(currentQuestionTable.Rows[numberOfAnswer][2].ToString());
                }
            }
            else
            { 
                _scoreForScales[_mainScaleNumber] += int.Parse(currentQuestionTable.Rows[numberOfAnswer][2].ToString());
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

        private void NextQuestionButton_Click(object sender, EventArgs e)
        {
            
            if (radioButton1.Checked && radioButton1.Visible)
            {
                CalculationsScores(0);
            }
            if (radioButton2.Checked && radioButton2.Visible)
            {
                CalculationsScores(1);
            }
            if (radioButton3.Checked && radioButton3.Visible)
            {
                CalculationsScores(2);
            }
            if (radioButton4.Checked && radioButton4.Visible)
            {
                CalculationsScores(3);
            }
            if (radioButton5.Checked && radioButton5.Visible)
            {
                CalculationsScores(4);
            }
            if(!radioButton1.Checked &&
                !radioButton2.Checked &&
                !radioButton3.Checked &&
                !radioButton4.Checked &&
                !radioButton5.Checked)
            {
                return;
            }
            bool counter = false;
            string temporaryQuestion = _currentQuestion;
            foreach (DataRow row in _questionsTable.Rows)
            {
                if ((row[3].ToString() != _currentQuestion)
                    && (counter == false))
                {
                    continue;
                }
                else
                {
                    counter = true;
                }
                if (row[3].ToString() != _currentQuestion)
                {
                    _currentQuestion = row[3].ToString();
                    QuestionTextBox.Text = _currentQuestion;
                    break;
                }
            }
            if (_currentQuestion == temporaryQuestion)
            {
                this.Close();
                _thread = new Thread(OpenResultsScoreForm);
                _thread.SetApartmentState(ApartmentState.STA);
                _thread.Start();
            }
            _counter++;
            if (_counter == 15)
            {
                TaskLabel.Text = "В последнее время:";
            }
        }

        public void OpenResultsScoreForm(object newObject)
        {
            Application.Run(new ResultsScoreForm(_userName, _testName, _scales, _scoreForScales));
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
