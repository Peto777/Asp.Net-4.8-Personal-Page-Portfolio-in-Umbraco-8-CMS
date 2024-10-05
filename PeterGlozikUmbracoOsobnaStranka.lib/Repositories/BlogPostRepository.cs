using NPoco;
using PeterGlozikUmbracoOsobnaStranka.lib.Models.DashboardBlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Repositories
{
    public class BlogPostRepository : _BaseRepository
    {
        public Page<OsobnaStrankaBlogPost> GetPage(long page, long itemsPerPage, string sortBy = "BlogName", string sortDir = "ASC", OsobnaStrankaBlogPostFilter filter = null)
        {
            var sql = GetBaseQuery();
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchText))
                {
                    sql.Where(GetSearchTextWhereClause(filter.SearchText), new { SearchText = filter.SearchText });
                }
            }
            sql.Append(string.Format("ORDER BY {0} {1}", sortBy, sortDir));

            return GetPage<OsobnaStrankaBlogPost>(page, itemsPerPage, sql);
        }

        public OsobnaStrankaBlogPost Get(Guid key)
        {
            var sql = GetBaseQuery().Where(GetBaseWhereClause(), new { Key = key });

            return Fetch<OsobnaStrankaBlogPost>(sql).FirstOrDefault();
        }

        public bool Save(OsobnaStrankaBlogPost dataRec)
        {
            if (IsNew(dataRec))
            {
                return Insert(dataRec);
            }
            else
            {
                return Update(dataRec);
            }
        }

        bool Insert(OsobnaStrankaBlogPost dataRec)
        {
            dataRec.pk = Guid.NewGuid();

            object result = InsertInstance(dataRec);
            if (result is Guid guidResult)
            {
                return guidResult == dataRec.pk;
            }

            return false;
        }

        bool Update(OsobnaStrankaBlogPost dataRec)
        {
            return UpdateInstance(dataRec);
        }

        public bool Delete(OsobnaStrankaBlogPost dataRec)
        {
            return DeleteInstance(dataRec);
        }

        Sql GetBaseQuery()
        {
            return new Sql(string.Format("SELECT * FROM {0}", OsobnaStrankaBlogPost.DbTableName));
        }

        string GetBaseWhereClause()
        {
            return string.Format("{0}.pk = @Key", OsobnaStrankaBlogPost.DbTableName);
        }
        string GetSearchTextWhereClause(string searchText)
        {
            return string.Format("{0}.blogName LIKE '%{1}%' collate Latin1_general_CI_AI OR {0}.blogDescription LIKE '%{1}%' collate Latin1_general_CI_AI OR {0}.blogWeb LIKE '%{1}%' collate Latin1_general_CI_AI", OsobnaStrankaBlogPost.DbTableName, searchText);
        }
    }

    [TableName(OsobnaStrankaBlogPost.DbTableName)]
    [PrimaryKey("pk", AutoIncrement = false)]
    public class OsobnaStrankaBlogPost : _BaseRepositoryRec
    {
        public const string DbTableName = "osBlogPost";

        public string BlogName { get; set; }
        public string BlogDescription { get; set; }
        public string BlogWeb { get; set; }

        public string BlogImagePath { get; set; }

        public static string GetFileUploadCategory(Guid productKey)
        {
            return string.Format(@"Product/{0}", productKey);
        }

    }

    public class OsobnaStrankaBlogPostFilter
    {
        public string SearchText { get; set; }
    }
}
