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
    public class FoodCategory
    {
        private int id;
        private string name;

        public FoodCategory() { }

        public FoodCategory(int id, string name)
        {
            this.Id = id;
            this.name = name;
        }

        public FoodCategory(DataRow dr)
        {
            this.Id = (int)dr["id"];
            this.name = dr["name"].ToString();
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
    }
}
