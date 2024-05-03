using QuanLyNhaHang.DTO;
using QuanLyNhaHang.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaHang.DTO
{
    public class Bill
    {
        private int id;
        private DateTime? DateCheckIn;
        private DateTime? DateCheckOut;
        private List<BillInfo> billInfos;
        private int idTable;
        private int status;
        private float totalPrice;
        private int discount;
        public Bill()
        {
            this.Id = -1;
            this.DateCheckIn = null;
            this.DateCheckOut = null;
            this.billInfos = null;
            this.IdTable = -1;
            this.status = -1;
            this.totalPrice = 0;
            this.discount = 0;
        }

        public Bill(DataRow dr)
        {
            this.Id = (int)dr["id"];
            this.DateCheckIn = (DateTime?)dr["dateCheckin"];

            var dateCheckOutTemp = dr["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;

            this.Status = (int)dr["status"];

            if (dr["discount"].ToString() != "")
                this.Discount = (int)dr["discount"];


            this.billInfos = BillInfoDAO.Instance.LoadBillInfos(this.Id);

        }

        public int Id { get => id; set => id = value; }
        public DateTime? DateCheckIn1 { get => DateCheckIn; set => DateCheckIn = value; }
        public DateTime? DateCheckOut1 { get => DateCheckOut; set => DateCheckOut = value; }
        public int IdTable { get => idTable; set => idTable = value; }
        public int Status { get => status; set => status = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
        public int Discount { get => discount; set => discount = value; }
        internal List<BillInfo> BillInfos { get => billInfos; set => billInfos = value; }
    }

}
