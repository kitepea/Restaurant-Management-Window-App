using QuanLyNhaHang.DTO;
using QuanLyNhaHang.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaHang.DAO
{
    public class TableFoodDAO
    {
        private static TableFoodDAO instance;

        public static TableFoodDAO Instance
        {
            get { if (instance == null) instance = new TableFoodDAO(); return TableFoodDAO.instance; }
            private set { TableFoodDAO.instance = value; }
        }

        public static int TableWidth = 160;
        public static int TableHeight = 160;

        private TableFoodDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTabel @idTable1 , @idTabel2", new object[] { id1, id2 });
            
        }


        public List<TableFood> LoadTableList()
        {
            List<TableFood> listFoodTable = new List<TableFood>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM TableFood");

            foreach (DataRow row in data.Rows)
            {
                TableFood table = new TableFood(row);
                listFoodTable.Add(table);
            }
            return listFoodTable;
        }
        public void UpdateStatusOut(int idTableFood)
        {
            DataProvider.Instance.ExecuteNonQuery("UPDATE dbo.TableFood SET status= N'Trống' WHERE id = " + idTableFood.ToString());
        }
        public void UpdateStatusIn(int idTableFood)
        {
            DataProvider.Instance.ExecuteNonQuery("UPDATE dbo.TableFood SET status= N'Có Người' WHERE id = " + idTableFood.ToString());
        }
        public bool InsertTable(string name, string status)
        {
            string query = string.Format("INSERT INTO TableFood (name, status) VALUES (N'{0}',N'{1}')", name, status);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteTable(int idTable)
            
        {
            BillDAO.Instance.DeleteBillByTableID(idTable);
            string query = string.Format("DELETE dbo.TableFood WHERE id = {0}", idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool EditTable(int idTable, string name, string status)
        {
            return false;
        }
        public bool UpdateTable(int idTable, string name,string status)
        {
            string query = string.Format("UPDATE dbo.TableFood SET name =N'{0}', status = N'{1}' WHERE id = {2} ", name, status, idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;

        }

    }
}
