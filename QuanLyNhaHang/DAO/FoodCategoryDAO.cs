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
    public class FoodCategoryDAO
    {
        private static FoodCategoryDAO instance;

        public static FoodCategoryDAO Instance
        {
            get { if (instance == null)instance = new FoodCategoryDAO(); return FoodCategoryDAO.instance; }
            private set { FoodCategoryDAO.instance = value; }
        }

        public FoodCategoryDAO() { }

        public FoodCategory LoadCategory(int idFoodCategory)
        {
            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT * FROM FoodCategory WHERE id = " + idFoodCategory.ToString());
            DataRow dr = dt.Rows[0];
            FoodCategory foodCategory = new FoodCategory(dr);
            return foodCategory;
        }
        public List<FoodCategory> LoadAllCategory()
        {
            List<FoodCategory> listFoodCategory = new List<FoodCategory>();
            DataTable dt = DataProvider.Instance.ExecuteQuery("SELECT * FROM FoodCategory");
            foreach (DataRow dr in dt.Rows)
            {
                FoodCategory foodCategory = new FoodCategory(dr);
                listFoodCategory.Add(foodCategory);
            }
            return listFoodCategory;
        }
        public bool InsertCategory(string name)
        {
            string query = string.Format("INSERT dbo.FoodCategory ( name ) VALUES  ( N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateCategory(int id, string name)
        {
            string query = string.Format("UPDATE dbo.FoodCategory SET name = N'{0}' WHERE id = {1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteCategory(int id)
        {
            FoodDAO.Instance.DeleteFoodByCategoryID(id);

            string query = string.Format("Delete dbo.FoodCategory where id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

    }
}
