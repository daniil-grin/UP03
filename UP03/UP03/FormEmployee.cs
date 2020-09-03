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

namespace UP03
{
    public partial class FormEmployee : Form
    {
        DataSetEmployee dsBakery = new DataSetEmployee();
        DataSetEmployeeTableAdapters.cakecategoryTableAdapter daCakeCategory = new UP03.DataSetEmployeeTableAdapters.cakecategoryTableAdapter();
        DataSetEmployeeTableAdapters.cakeTableAdapter daCake = new UP03.DataSetEmployeeTableAdapters.cakeTableAdapter();
        DataSetEmployeeTableAdapters.orderTableAdapter daOrder = new UP03.DataSetEmployeeTableAdapters.orderTableAdapter();
        // DataSetEmployeeTableAdapters.TableAdapterManager daTable = new UP03.DataSetEmployeeTableAdapters.TableAdapterManager();
        DataSetEmployeeTableAdapters.themeTableAdapter daTheme = new UP03.DataSetEmployeeTableAdapters.themeTableAdapter();
        DataSetEmployeeTableAdapters.usersTableAdapter daUsers = new UP03.DataSetEmployeeTableAdapters.usersTableAdapter();
        DataSetEmployeeTableAdapters.employeeTableAdapter daEmployee = new UP03.DataSetEmployeeTableAdapters.employeeTableAdapter();
        DataSetEmployeeTableAdapters.customerTableAdapter daCustomer = new UP03.DataSetEmployeeTableAdapters.customerTableAdapter();
        BindingManagerBase bmOrder;
        static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=bakeryDB;Integrated Security=True";
        // Создание подключения
        SqlConnection connection = new SqlConnection(connectionString);
        public FormEmployee()
        {
            InitializeComponent();
            bmOrder = this.BindingContext[dsBakery, "order"];
            // Добавляем делегата  PositionChanged для события - изменение позиции в таблице Employee DataSet dsEmployee
            bmOrder.PositionChanged += new EventHandler(BindingManagerBase_PositionChanged);
        }
        private void BindingManagerBase_PositionChanged(object sender, EventArgs e)
        {
            int pos = ((BindingManagerBase)sender).Position;
        }
        public void tablesFill()
        {
            daCakeCategory.Fill(dsBakery.cakecategory);
            daCake.Fill(dsBakery.cake);
            daOrder.Fill(dsBakery.order);
            daTheme.Fill(dsBakery.theme);
            daUsers.Fill(dsBakery.users);
            daEmployee.Fill(dsBakery.employee);
            daCustomer.Fill(dsBakery.customer);
            // MessageBox.Show("Метод Fill отработал");
        }

        private void AddColumsFullName()
        {

            dsBakery.order.Columns.Add("Full", typeof(string),
             "orderdate+' '+cSurname+' '+cName");
            dsBakery.employee.Columns.Add("Full", typeof(string),
             "surname+' '+name+' '+patronymic");
            dsBakery.customer.Columns.Add("Full", typeof(string),
             "surname+' '+name+' '+patronymic");
        }

        private void FormEmployee_Load(object sender, EventArgs e)
        {
            tablesFill();
            AddColumsFullName();
            listBoxOrder.DataSource = this.dsBakery;
            listBoxOrder.DisplayMember = "order.Full";
            textBoxESurname.DataBindings.Add("Text", dsBakery, "order.eSurname");
            textBoxEName.DataBindings.Add("Text", dsBakery, "order.eName");
            textBoxEPatronymic.DataBindings.Add("Text", dsBakery, "order.ePatronymic");
            textBoxCSurname.DataBindings.Add("Text", dsBakery, "order.cSurname");
            textBoxCName.DataBindings.Add("Text", dsBakery, "order.cName");
            textBoxCPatronymic.DataBindings.Add("Text", dsBakery, "order.cPatronymic");
            textBoxCakeName.DataBindings.Add("Text", dsBakery, "order.cake");
            comboBoxCakeName.DataSource = this.dsBakery.cake;
            comboBoxCakeName.DisplayMember = "name";
            comboBoxCakeName.ValueMember = "id";
            comboBoxCakeName.DataBindings.Add("SelectedValue", dsBakery, "order.id_cake");

            comboBoxESurname.DataSource = this.dsBakery.employee;
            comboBoxESurname.DisplayMember = "Full";
            comboBoxESurname.ValueMember = "id";
            comboBoxESurname.DataBindings.Add("SelectedValue", dsBakery, "order.id_employee");

            comboBoxCSurname.DataSource = this.dsBakery.customer;
            comboBoxCSurname.DisplayMember = "Full";
            comboBoxCSurname.ValueMember = "id";
            comboBoxCSurname.DataBindings.Add("SelectedValue", dsBakery, "order.id_customer");
            textBoxCakeWeight.DataBindings.Add("Text", dsBakery, "order.weight");
            textBoxCakePrice.DataBindings.Add("Text", dsBakery, "order.price");
            ReadOnly(true);
            
        }
        public void ReadOnly(bool readOnly)
        {
            if (readOnly)
            {
                listBoxOrder.Enabled = true;
                listBoxOrder.Enabled = true;
                textBoxESurname.ReadOnly = true;
                textBoxEName.ReadOnly = true;
                textBoxEPatronymic.ReadOnly = true;
                textBoxCSurname.ReadOnly = true; 
                textBoxCName.ReadOnly = true; 
                textBoxCPatronymic.ReadOnly = true;
                comboBoxCakeName.Enabled = false;
                textBoxCakeName.Visible = true;
                textBoxCSurname.Visible = true;
                textBoxESurname.Visible = true;
            }
            else
            {
                listBoxOrder.Enabled = false;
                listBoxOrder.Enabled = false;
                textBoxESurname.ReadOnly = false;
                textBoxEName.ReadOnly = false;
                textBoxEPatronymic.ReadOnly = false;
                textBoxCSurname.ReadOnly = false;
                textBoxCName.ReadOnly = false;
                textBoxCPatronymic.ReadOnly = false;
                comboBoxCakeName.Enabled = true;
                textBoxCakeName.Visible = false;
                textBoxCSurname.Visible = false;
                textBoxESurname.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "Редактировать")
            {
                button1.Text = "Сохранить";
                ReadOnly(false);
            }
            else
            {
                button1.Text = "Редактировать";
                // Завершение текущих обновлений всех  связанных с помощью 
                // объектов Binding элементов управления 
                bmOrder.EndCurrentEdit();

                /// Формирование таблицы, в которую включаются только 
                // модифицированные строки
                DataSetEmployee.orderDataTable ds1 =
                    (DataSetEmployee.orderDataTable)dsBakery.order.GetChanges(DataRowState.Modified);

                if (ds1 != null)
                    try
                    {
                        this.daOrder.Update(ds1);
                        ds1.Dispose();
                        dsBakery.order.AcceptChanges();
                    }
                    catch (Exception x)
                    {
                        string mes = x.Message;
                        MessageBox.Show("Ошибка обновления базы данных Bakery " + mes, "Предупреждение");
                        this.dsBakery.order.RejectChanges();
                    }
                /// Формирование таблицы, в которую включаются только добавленные строки
                DataSetEmployee.orderDataTable ds2 = (DataSetEmployee.orderDataTable)dsBakery.order.
                GetChanges(DataRowState.Added);
                if (ds2 != null)
                {
                    try
                    {
                        daOrder.Update(ds2);
                        ds2.Dispose();
                        dsBakery.order.AcceptChanges();
                    }
                    catch (Exception x)
                    {
                        string mes = x.Message;
                        MessageBox.Show("Ошибка вставки записи в базу данных Employee " + mes, "Предупреждение");
                        this.dsBakery.order.RejectChanges();
                    }
                }
                ReadOnly(true);
                tablesFill();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Text = "Сохранить";
            ReadOnly(false);
            // Создать новую строку
            DataRow rowOrder = this.dsBakery.order.NeworderRow();
            // Сформировать начальные значения для элементов строки
            rowOrder["id_cake"] = 1;
            rowOrder["eSurname"] = "";
            rowOrder["cSurname"] = "";
            rowOrder["theme"] = "1";
            rowOrder["delivery"] = DateTime.Now;
            rowOrder["orderdate"] = DateTime.Now;

            // Добавить сформированную строку к таблице Employee
            dsBakery.order.Rows.Add(rowOrder);
            // Установить активную позицию в таблице Employee на добавленную строку
            int pos = this.dsBakery.order.Rows.Count - 1;
            this.BindingContext[dsBakery, "Employee"].Position = pos;
            // Задать режим редактирования формы
            ReadOnly(false);
            // Сделать список сотрудников недоступным для выбора
            listBoxOrder.Enabled = false;
            // Установить фокус на элементе textBoxSurname
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Определяется позиция, которую необходимо удалить в таблице Employee
            int pos = -1;
            pos = this.BindingContext[dsBakery, "Employee"].Position;
            // Затем формируется строка с фамилией, именем и отчеством, удаляемого сотрудника
            string mes = textBoxESurname.Text.ToString().Trim() + " " + textBoxEName.Text.ToString().Trim() + " " + textBoxEPatronymic.Text.ToString().Trim();
            DialogResult result = MessageBox.Show(" Удалить данные  \n по сотруднику \n" + mes + "?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            switch (result)
            {
                case DialogResult.Yes:
                    {
                        //выполнить действия по удалению данных по сотруднику 
                        this.dsBakery.order.Rows[pos].Delete();
                        if (this.dsBakery.order.GetChanges(DataRowState.Deleted) != null)
                        {
                            try
                            {
                                this.daOrder.Update(dsBakery.order);
                                this.dsBakery.order.AcceptChanges();
                            }
                            catch (Exception x)
                            {
                                string er = x.Message.ToString();
                                MessageBox.Show("Ошибка удаления записи в базе данных Employee " + er, "Предупреждение");
                                this.dsBakery.order.RejectChanges();
                            }
                        }
                        break;
                    }
                case DialogResult.No:
                    {
                        //отмена удаления данных по сотруднику   
                        this.dsBakery.order.RejectChanges();
                        break;
                    }
            }
            ReadOnly(true);
        }
    }

}
