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
    public partial class RegistrationForm : Form
    {
        private Thread _thread;
        private string _userName;

        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void пользователиBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.пользователиBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.testDataSet);

        }

        private void пользователиBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.пользователиBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.testDataSet);

        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "testDataSet.Пользователи". При необходимости она может быть перемещена или удалена.
            this.пользователиTableAdapter.Fill(this.testDataSet.Пользователи);
            пользователиBindingSource.AddNew();
        }

        private void SaveGoButton_Click(object sender, EventArgs e)
        {
            пользователиBindingSource.EndEdit();
            пользователиTableAdapter.Update(testDataSet);
            _userName = фИОTextBox.Text;
            this.Close();
            _thread = new Thread(OpenTestListForm);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }

        private void OpenTestListForm(object newObject)
        {
            Application.Run(new TestListForm(_userName));
        }
    }
}
