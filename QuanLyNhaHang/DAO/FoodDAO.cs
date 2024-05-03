using QuanLyNhaHang.DTO;
using QuanLyNhaHang.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaHang.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null)instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        
        public  Food LoadFood(int idFood)
        {
            Food food = new Food();
            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT * FROM Food WHERE id = " + idFood.ToString());
            DataRow dr = dt.Rows[0];
            food.Id = (int)dr["id"];
            food.Name = dr["name"].ToString();
            food.FoodCategory = FoodCategoryDAO.Instance.LoadCategory((int)dr["idCategory"]);
            food.Price = (float)Convert.ToDouble(dr["price"].ToString());

            return food;
        }
        public  List<Food> LoadFoodWithCategory(int idCategory)
        {
            List<Food> listFood = new List<Food>();
            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT * FROM Food WHERE idCategory = " + idCategory.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                Food food = new Food(dr);
                listFood.Add(food);
            }
            return listFood;
        }

        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();

            string query = "SELECT * FROM Food";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }

            return list;
        }

        public DataTable SearchFoodByName(string name)
        {                     

            

            string query = string.Format("SELECT * FROM dbo.Food WHERE dbo.fuConvertToUnsign1(name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            

            return data;
        }

        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("INSERT INTO Food (name, idCategory, price) VALUES (N'{0}', {1}, {2})", name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            string query = string.Format("UPDATE Food SET name = N'{0}', idCategory = {1}, price = {2} WHERE id = {3}", name, id, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodID(idFood);

            string query = string.Format("Delete Food where id = {0}",idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public void DeleteFoodByCategoryID(int idCategory)
        {
            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT * FROM Food WHERE idCategory = " + idCategory.ToString());
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BillInfoDAO.Instance.DeleteBillInfoByFoodID((int)dr["id"]);
                }
            }
            DataProvider.Instance.ExecuteNonQuery("DELETE Food WHERE idCategory =" + idCategory.ToString());   
            
            
            
        }
    }
}
