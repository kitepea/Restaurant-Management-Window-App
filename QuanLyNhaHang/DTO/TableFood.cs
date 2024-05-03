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
    public class TableFood
    {
        private int id;
        private string name;
        private string status;

        public TableFood() { }
        public TableFood(int id, string name, string status)
        {
            this.Id = id;
            this.name = name;
            this.status = status;
        }
        public TableFood(DataRow dataRow)
        {
            this.Id = (int)dataRow["id"];
            this.status = dataRow["status"].ToString();
            this.name = dataRow["name"].ToString();
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Status { get => status; set => status = value; }
    }
}
