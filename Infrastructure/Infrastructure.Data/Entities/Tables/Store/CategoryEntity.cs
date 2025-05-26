using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class CategoryEntity
    {
		public int CategoryId { get; set; }
		public int? CategoryRanking { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public int? ParentCategoryId { get; set; }
		public string Slug { get; set; }
		public string Title { get; set; }
		public DateTime? UpdatedAt { get; set; }

        public CategoryEntity() { }

        public CategoryEntity(DataRow dataRow)
        {
			CategoryId = Convert.ToInt32(dataRow["CategoryId"]);
			CategoryRanking = (dataRow["CategoryRanking"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CategoryRanking"]);
			CreatedAt = (dataRow["CreatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedAt"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Name = Convert.ToString(dataRow["Name"]);
			ParentCategoryId = (dataRow["ParentCategoryId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ParentCategoryId"]);
			Slug = Convert.ToString(dataRow["Slug"]);
			Title = Convert.ToString(dataRow["Title"]);
			UpdatedAt = (dataRow["UpdatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdatedAt"]);
        }
    }
}

