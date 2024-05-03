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
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }

        /// <summary>
        /// Thành công: bill ID
        /// thất bại: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       
        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

        public DataTable GetBillListByDateAndPage(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDateAndPage @checkIn , @checkOut , @page", new object[] { checkIn, checkOut, pageNum });
        }

        public int GetNumBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return (int)DataProvider.Instance.ExecuteScalar("exec USP_GetNumBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }
        public static Bill LoadBill(int idTable)
        {
            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE  idTable = " + idTable + " AND status = 0");


            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                Bill bill = new Bill(dr);
                return bill;

            }
            else
            {
                return new Bill();
            }

        }
        public  int getIdBill(int idTable)
        {
            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT id FROM dbo.Bill WHERE idTable = " + idTable + " AND status = 0");
            if (dt.Rows.Count > 0)
            {
                return (int)dt.Rows[0]["id"];
            }
            return -1;
        }
        public int getIdBillByIdTable(int idTable)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idTable = " + idTable + " AND status = 0");

            if (data.Rows.Count > 0)
            {
                DataRow dr = data.Rows[0];
                return (int)dr["id"];
            }

            return -1;
        }
        public  int GetMaxIdBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }
        public int InsertBill(int idTableFood)
        {
            return DataProvider.Instance.ExecuteNonQuery("INSERT dbo.Bill (DateCheckIn , DateCheckOut, idTable, status, discount) " +
                "VALUES  ( GETDATE() , NULL ," + idTableFood + " , 0,0)");
        }
        public void Pay(int idBill, int discount, float totalPrice)
        {
            DataProvider.Instance.ExecuteNonQuery("UPDATE dbo.Bill SET dateCheckOut = GETDATE(), status = 1, " + "discount = " + discount + ", totalPrice = " + totalPrice + " WHERE id = " + idBill.ToString());
        }
        public void DeleteBillByTableID(int idTable)
        {

            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idTable = " + idTable.ToString());
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BillInfoDAO.Instance.DeleteBillInfoByBillID((int)dr["id"]);

                }


            }
            DataProvider.Instance.ExecuteNonQuery("DELETE dbo.Bill WHERE idTable = " + idTable.ToString());



        }
    }
}
