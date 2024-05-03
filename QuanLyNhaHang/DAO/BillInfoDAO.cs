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
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }

        private BillInfoDAO() { }

        public void DeleteBillInfoByFoodID(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete dbo.BillInfo WHERE idFood = " + id);
        }
        

       
        public  List<BillInfo> LoadBillInfos(int idBill)
        {
            List<BillInfo> billInfos = new List<BillInfo>();
            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BillInfo WHERE idBill = " + idBill);
            foreach (DataRow dr in dt.Rows)
            {
                BillInfo billInfo = new BillInfo(dr);
                billInfos.Add(billInfo);
            }
            return billInfos;
        }
        public void InsertBillInfos(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("USP_InsertBillInfo @idBill= " + idBill + " , @idFood =" + idFood + ", @count =" + count.ToString());
        }

        public void DeleteBillInfoByBillID(int idBill)
        {
            DataProvider.Instance.ExecuteNonQuery("DELETE BillInfo WHERE idBill = " + idBill.ToString());  
        }

    }
}
