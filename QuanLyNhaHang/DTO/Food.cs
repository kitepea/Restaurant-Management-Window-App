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
    public class Food
    {
        private int id;
        private string name;
        private float price;
        private FoodCategory foodCategory;

        public Food() { }
        public Food(int id, string name, float price, FoodCategory foodCategory)
        {
            this.Id = id;
            this.name = name;
            this.price = price;
            this.foodCategory = foodCategory;

        }
        public Food(DataRow dr)
        {
            this.Id = (int)dr["id"];
            this.name = dr["name"].ToString();
            this.price = (float)Convert.ToDouble(dr["price"].ToString());
            this.foodCategory = FoodCategoryDAO.Instance.LoadCategory((int)dr["idCategory"]);
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public float Price { get => price; set => price = value; }
        public FoodCategory FoodCategory { get => foodCategory; set => foodCategory = value; }
    }
}
