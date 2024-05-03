using QuanLyNhaHang.DAO;
using QuanLyNhaHang.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }
        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTableFoods();
            LoadCategory();
            LoadComboboxTable(cbSwitchTable);
        }

        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            TàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        void LoadCategory()
        {

            List<FoodCategory> listCategory = FoodCategoryDAO.Instance.LoadAllCategory();
            
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
            
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.LoadFoodWithCategory(id);
            if (listFood.Count > 0)
            {
                cbFood.DataSource = listFood;
                cbFood.DisplayMember = "Name";
            }
            else cbFood.DataSource = null;
            
        }
       

        private void LoadBillOnMenu(int idTable)
        {


            ListViewMenu.Items.Clear();
            Bill bill = BillDAO.LoadBill(idTable);
            float totalPrice = 0;
           
            if (bill.Id > 0)
            {
                foreach (BillInfo item in bill.BillInfos)
                {

                    ListViewItem lsvItem = new ListViewItem(item.Food.Name);

                    lsvItem.SubItems.Add(item.Count.ToString());
                    lsvItem.SubItems.Add(item.Food.Price.ToString());
                    lsvItem.SubItems.Add((item.Count * item.Food.Price).ToString());
                    totalPrice += item.Count * item.Food.Price;


                    ListViewMenu.Items.Add(lsvItem);
                }
            }

            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;
            totalTextbox.Text = totalPrice.ToString("c", culture);




        }
        private void LoadTableFoods()
        {
            FlowList.Controls.Clear();
            List<TableFood> listFoodTable = TableFoodDAO.Instance.LoadTableList();
            foreach (TableFood item in listFoodTable)
            {
                Button btn = new Button() { Width = TableFoodDAO.TableWidth, Height = TableFoodDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.White;
                        btn.ForeColor = Color.Black;
                        btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        break;
                    default:
                        btn.BackColor = Color.FromArgb(242,82,82);
                        btn.ForeColor = Color.White;
                        btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        break;
                }
                FlowList.Controls.Add(btn);

            }
        }

        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableFoodDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion


        #region Events

        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PayButton_Click(this, new EventArgs());
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddButton_Click(this, new EventArgs());
        }

        private void btn_Click(object sender, EventArgs e)
        {

            int idTable = ((sender as Button).Tag as TableFood).Id;
            LoadBillOnMenu(idTable);
            ListViewMenu.Tag = (sender as Button).Tag;
            string StatusTable = ((sender as Button).Tag as TableFood).Status;
            if (StatusTable == "Trống")
            {
                TableFoodDAO.Instance.UpdateStatusIn(idTable);
                LoadTableFoods();
            }


        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        void f_UpdateAccount(object sender, AccountEvent e)
        {
            TàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;

            f.InsertCategory += f_InsertCategory;
            f.UpdateCategory += f_UpdateCategory;
            f.DeleteCategory += f_DeleteCategory;
            
            f.InsertTable += f_InsertTable;
            f.UpdateTable += f_UpdateTable;
            f.DeleteTable += f_DeleteTable;
            f.ShowDialog();
        }
        void f_InsertCategory(object sender, EventArgs args)
        {
            LoadCategory();
            if (ListViewMenu.Tag != null)
                LoadBillOnMenu((ListViewMenu.Tag as TableFood).Id);
        }
        void f_UpdateCategory(object sender, EventArgs args)
        {
            LoadCategory();
            if (ListViewMenu.Tag != null)
                LoadBillOnMenu((ListViewMenu.Tag as TableFood).Id);
        }
        void f_DeleteCategory(object sender, EventArgs args)
        {
            LoadCategory();
            if (ListViewMenu.Tag != null)
                LoadBillOnMenu((ListViewMenu.Tag as TableFood).Id);
        }
        void f_InsertTable(object sender, EventArgs e)
        {
            LoadTableFoods();

            LoadComboboxTable(cbSwitchTable);
        }
        void f_UpdateTable(object sender, EventArgs e)
        {
            LoadTableFoods();
            LoadComboboxTable(cbSwitchTable);
        }
        void f_DeleteTable(object sender, EventArgs e)
        {
            LoadTableFoods();
            LoadComboboxTable(cbSwitchTable);
        }

        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as FoodCategory).Id);
            if (ListViewMenu.Tag != null)
                LoadBillOnMenu((ListViewMenu.Tag as TableFood).Id);
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as FoodCategory).Id);
            if (ListViewMenu.Tag != null)
                LoadBillOnMenu((ListViewMenu.Tag as TableFood).Id);
            LoadTableFoods();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as FoodCategory).Id);
            if (ListViewMenu.Tag != null)
                LoadBillOnMenu((ListViewMenu.Tag as TableFood).Id);
        }
        
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            FoodCategory selected = cb.SelectedItem as FoodCategory;
            id = selected.Id;

            LoadFoodListByCategoryID(id);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            TableFood tableFood = ListViewMenu.Tag as TableFood;

            if (tableFood == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }
            try
            {
                int idBill = BillDAO.Instance.getIdBillByIdTable(tableFood.Id);
                int foodID = (cbFood.SelectedItem as Food).Id;
                int count = (int)UpDownCount.Value;

                if (idBill == -1)
                {
                    BillDAO.Instance.InsertBill(tableFood.Id);
                    BillInfoDAO.Instance.InsertBillInfos(BillDAO.Instance.GetMaxIdBill(), foodID, count);
                }
                else
                {
                    BillInfoDAO.Instance.InsertBillInfos(idBill, foodID, count);
                }

                LoadBillOnMenu(tableFood.Id);
            }
            catch
            {
                MessageBox.Show("Có lỗi khi chọn món");
                return;               
            }
            
        }
        private void PayButton_Click(object sender, EventArgs e)
        {
            
            TableFood tableFood = ListViewMenu.Tag as TableFood;
            int idBill = BillDAO.Instance.getIdBillByIdTable(tableFood.Id);
            int discount = (int)UpDownDiscount.Value;
            
            double totalPrice = Convert.ToDouble(totalTextbox.Text.Split(',')[0]);
            if (totalPrice <= 0)
            {
                TableFoodDAO.Instance.UpdateStatusOut(tableFood.Id);
                LoadTableFoods();
                return;
            }
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Xác nhận thanh toán cho bàn {0}\n" +
                    "Với số tiền {2}đ (giảm giá {1}đ)",
                    tableFood.Name, (totalPrice / 100) * discount, finalTotalPrice),
                    "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.Pay(idBill, discount, (float)finalTotalPrice);
                    LoadBillOnMenu(tableFood.Id);
                    TableFoodDAO.Instance.UpdateStatusOut(tableFood.Id);

                    LoadTableFoods();
                }
            }
        }

        


        #endregion

        private void fTableManager_Load(object sender, EventArgs e)
        {

        }

        private void ListViewMenu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void totalTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (ListViewMenu.Tag as TableFood).Id;

            int id2 = (cbSwitchTable.SelectedItem as TableFood).Id;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (ListViewMenu.Tag as TableFood).Name, (cbSwitchTable.SelectedItem as TableFood).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableFoodDAO.Instance.SwitchTable(id1, id2);
                
                LoadTableFoods();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void FlowList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
