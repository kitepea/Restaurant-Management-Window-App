using QuanLyNhaHang.DAO;
using QuanLyNhaHang.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyNhaHang
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();

        BindingSource accountList = new BindingSource();

        BindingSource categoryList = new BindingSource();

        BindingSource tableList = new BindingSource();  

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            LoadData();
        }

        #region methods

        DataTable SearchFoodByName(string name)
        {
            DataTable listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        void LoadData()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadListTable();

            LoadAccount();
            LoadListCategory();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();
            AddAccountBinding();
            AddCategoryBinding();
            AddTableBinding();
            data_7day_Click(null, null);
        }

        void LoadListCategory()
        {
            categoryList.DataSource = FoodCategoryDAO.Instance.LoadAllCategory();
        }
        void LoadListTable()
        {
            tableList.DataSource = TableFoodDAO.Instance.LoadTableList();
        }
        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            numericUpDown1.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
            
            
        }
        void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource,"Id", true,DataSourceUpdateMode.Never));
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }
        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "name", true, DataSourceUpdateMode.Never));
            cbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "status", true, DataSourceUpdateMode.Never));

        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Id", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = FoodCategoryDAO.Instance.LoadAllCategory();
            cb.DisplayMember = "Name";
        }
        void LoadListFood()
        {
            foodList.DataSource = DataProvider.Instance.ExecuteQuery("SELECT * FROM Food");
            
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        #endregion

        #region events
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)numericUpDown1.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)numericUpDown1.Value;

            EditAccount(userName, displayName, type);
        }


        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            ResetPass(userName);
        }


        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }
        

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                LoadListFood();
                MessageBox.Show("Thêm món thành công");
                
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                LoadListFood();
                MessageBox.Show("Sửa món thành công");
               
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                LoadListFood();
                MessageBox.Show("Xóa món thành công");                
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
       
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }

        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
            
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }
        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }
        #endregion              

        private void btnFristBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
                lastPage++;

            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPrevioursBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);

            if (page > 1)
                page--;

            txbPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page < sumRecord)
                page++;

            txbPageBill.Text = page.ToString();
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'QuanLyNhaHangDataSet2.USP_GetListBillByDateForReport' table. You can move, or remove it, as needed.
            this.USP_GetListBillByDateForReportTableAdapter.Fill(this.QuanLyNhaHangDataSet2.USP_GetListBillByDateForReport, dtpkFromDate.Value, dtpkToDate.Value);           

            this.rpViewer.RefreshReport();
        }

        private void dtgvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtpkFromDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void month_Click(object sender, EventArgs e)
        {
            //dtpkFromDate.Value = DateTime.Now;
            //dtpkFromDate.Value.AddDays(1);
            //if (DateTime.Now.Month == 12)
            //{
            //dtpkToDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 31);
            // LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            // return;
            //}

            ///dtpkToDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);
            ///LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value.AddDays(-1));
            //
            dtpkFromDate.Value = dtpkToDate.Value = DateTime.Now;
            if (dtpkFromDate.Value.Day > 1)
            {
                dtpkFromDate.Value = dtpkFromDate.Value.AddDays(-(dtpkFromDate.Value.Day - 1));
            }

            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void day_now_Click(object sender, EventArgs e)
        {
            dtpkFromDate.Value = dtpkToDate.Value = DateTime.Now;

            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        DataTable LoadListdata_day(DateTime checkIn, DateTime checkOut)
        {
            return BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        private void data_7day_Click(object sender, EventArgs e)
        {
            //char1.clear();
            chart1.Series.RemoveAt(0);
            chart1.ChartAreas[0].Axes[0].MajorGrid.Enabled = false;//x axis
            chart1.ChartAreas[0].AxisX.Title = "Ngày";
            chart1.ChartAreas[0].AxisY.Title = "Tiền";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].Axes[1].MajorGrid.Enabled = true;//y axis
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy";

            //Series
            Series totalPrice = new Series();
            chart1.Series.Add(totalPrice);
            totalPrice.AxisLabel = "111";
            chart1.Series[0].LegendText = "Tiền trong ngày";
            totalPrice.LabelForeColor = Color.Gray;
            DataTable data = LoadListdata_day(DateTime.Now.AddDays(-6), DateTime.Now);

            for (int i = 0; i < 7; i++)
            {
                DateTime d = DateTime.Now.AddDays(i-6);
                int sum = 0;
                if (data.Compute("SUM(totalPrice)", "DateCheckIn = #" + d.ToString("yyyy-MM-dd") + "#") != System.DBNull.Value)
                {
                    sum = Convert.ToInt32(data.Compute("SUM(totalPrice)", "DateCheckIn = #" + d.ToString("yyyy-MM-dd") + "#"));
                }
                totalPrice.Points.AddXY(d.ToString("dd-MM-yyyy"), sum);
            }
            




        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void data_month_Click(object sender, EventArgs e)
        {
            chart1.Series.RemoveAt(0);
            chart1.ChartAreas[0].Axes[0].MajorGrid.Enabled = false;//x axis
            chart1.ChartAreas[0].AxisX.Title = "Ngày";
            chart1.ChartAreas[0].AxisY.Title = "Tiền";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].Axes[1].MajorGrid.Enabled = true;//y axis
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy";

            //Series
            Series totalPrice = new Series();
            totalPrice.ChartType = SeriesChartType.Line;  // type
            totalPrice.BorderWidth = 2;
            totalPrice.Color = Color.Green;
            chart1.Series.Add(totalPrice);
            chart1.Series[0].LegendText = "Tiền trong ngày";
            totalPrice.LabelForeColor = Color.Gray;

            DateTime start, end;
            start = end = DateTime.Now;
            if (start.Day > 1)
            {
                start = start.AddDays(-(start.Day - 1));
            }

            end = start.AddMonths(1).AddDays(-1);
            LoadListBillByDate(start, end);

            DataTable data = LoadListdata_day(start, end);
            
            for(int i = 0; i < end.Day; i++)
            {
                DateTime d = DateTime.Now.AddDays(-DateTime.Now.Day + 1 + i);
                int sum = 0;
                if (data.Compute("SUM(totalPrice)", "DateCheckIn = #" + d.ToString("yyyy-MM-dd") + "#") != System.DBNull.Value)
                {
                    sum = Convert.ToInt32(data.Compute("SUM(totalPrice)", "DateCheckIn = #" + d.ToString("yyyy-MM-dd") + "#"));
                }
                totalPrice.Points.AddXY(d.ToString("dd-MM-yyyy"), sum);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series.RemoveAt(0);
            chart1.ChartAreas[0].Axes[0].MajorGrid.Enabled = false;//x axis
            chart1.ChartAreas[0].AxisX.Title = "Tháng";
            chart1.ChartAreas[0].AxisY.Title = "Tiền";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].Axes[1].MajorGrid.Enabled = true;//y axis
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM";

            //Series
            Series totalPrice = new Series();
            //totalPrice.ChartType = SeriesChartType.Line;  // type
            totalPrice.BorderWidth = 2;
            totalPrice.Color = Color.Green;
            chart1.Series.Add(totalPrice);
            chart1.Series[0].LegendText = "Tiền trong tháng của năm";
            totalPrice.LabelForeColor = Color.Gray;
         
            DataTable data = LoadListdata_day(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, 12, 31));
            for (int i = 1; i <= 12; i++)
            {
                int sum = 0;
                DateTime d = new DateTime(DateTime.Now.Year, i, 1);
                string filterExpression = "DateCheckIn >= #" + d.ToString() + "# AND DateCheckIn < #" + d.AddMonths(1).ToString() + "#";
                if (data.Compute("SUM(totalPrice)", filterExpression) != System.DBNull.Value)
                {
                    sum = Convert.ToInt32(data.Compute("SUM(totalPrice)", filterExpression));
                }
                totalPrice.Points.AddXY(new DateTime(DateTime.Now.Year, i, 1).ToString("MM"), sum);
            }
        }

        private void dtgvFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {

        }

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            cbFoodCategory.SelectedIndex = FoodDAO.Instance.LoadFood(int.Parse(txbFoodID.Text)).FoodCategory.Id - 1 ;  
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;
           

            if (FoodCategoryDAO.Instance.InsertCategory( name))
            {
                LoadListCategory();
                MessageBox.Show("Thêm danh mục thành công");
                
                if (insertCategory != null)
                    insertCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {           
            int categoryID = int.Parse(txbCategoryID.Text);

            if (FoodCategoryDAO.Instance.DeleteCategory(categoryID))
            {
                LoadListCategory();
                MessageBox.Show("Xóa danh mục thành công");                
                if (deleteCategory != null)
                    deleteCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa danh mục");
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;
            int categoryID = int.Parse(txbCategoryID.Text);

            if (FoodCategoryDAO.Instance.UpdateCategory(categoryID, name))
            {
                LoadListCategory();
                MessageBox.Show("Sửa danh mục thành công");
                
                if (updateCategory != null)
                    updateCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa danh mục");
            }
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void cbTableStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            string status = cbTableStatus.Text;
            

            if (TableFoodDAO.Instance.InsertTable(name,status))
            {
                LoadListTable();
                MessageBox.Show("Thêm bàn ăn thành công");
                
                if (insertTable != null)
                    insertTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn ăn");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int idTable = int.Parse(txbTableID.Text);


            if (TableFoodDAO.Instance.DeleteTable(idTable))
            {
                LoadListTable();
                
                MessageBox.Show("Xóa bàn ăn thành công");
                
                if (deleteTable != null)
                    deleteTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            int idTable = int.Parse(txbTableID.Text); 
            string name = txbTableName.Text;
            string status = cbTableStatus.Text;


            if (TableFoodDAO.Instance.UpdateTable(idTable,name, status))
            {
                LoadListTable();
                MessageBox.Show("Sửa bàn ăn thành công");
                
                if (updateTable != null)
                    updateTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn ăn");
            }
        }

        private void test_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
