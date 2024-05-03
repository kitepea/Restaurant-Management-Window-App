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
    public class BillInfo
    {
        private int id;
        private int idBill;

        private int count;
        private Food food;
        public BillInfo() { }
        public BillInfo(int id, int idBill, int count, int idFood)
        {
            this.Id = id;
            this.IdBill = idBill;

            this.count = count;

            this.Food = FoodDAO.Instance.LoadFood(idFood);
        }
        public BillInfo(DataRow dr)
        {
            this.Id = (int)dr["id"];
            this.IdBill = (int)dr["idBill"];
            this.count = (int)dr["count"];

            this.Food = FoodDAO.Instance.LoadFood((int)dr["idFood"]);
        }

        public int Id { get => id; set => id = value; }

        public int IdBill { get => idBill; set => idBill = value; }

        public int Count { get => count; set => count = value; }
        internal Food Food { get => food; set => food = value; }

    }
}
