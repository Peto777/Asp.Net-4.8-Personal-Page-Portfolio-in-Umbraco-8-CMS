using NPoco;
using System;
using System.Linq;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Repositories
{
    public class OsobnaStrankaUserPropRepository : _BaseRepository
    {
        public OsobnaStrankaUserProp Get(string sessionId, string propId)
        {
            System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
            if (user == null)
            {
                return Get(0, sessionId, propId);
            }

            return Get((int)user.ProviderUserKey, sessionId, propId);
        }

        public OsobnaStrankaUserProp Get(int userId, string sessionId, string propId)
        {
            var sql = GetBaseQuery().Where(GetBaseWhereClause(), new { PropId = propId });
            // Use session id identifier
            sql.Where(GetSessionWhereClause(), new { SessionId = sessionId });

            return Fetch<OsobnaStrankaUserProp>(sql).FirstOrDefault();
        }

        public bool Save(string sessionId, OsobnaStrankaUserProp dataRec)
        {
            if (IsNew(dataRec))
            {
                return Insert(sessionId, dataRec);
            }
            else
            {
                return Update(dataRec);
            }
        }

        bool Insert(string sessionId, OsobnaStrankaUserProp dataRec)
        {
            // Use session id identifier
            dataRec.UserId = 0;
            dataRec.SessionId = sessionId;

            dataRec.pk = Guid.NewGuid();
            dataRec.DateCreate = DateTime.Now;

            object result = InsertInstance(dataRec);
            if (result is Guid)
            {
                return (Guid)result == dataRec.pk;
            }

            return false;
        }

        bool Update(OsobnaStrankaUserProp dataRec)
        {
            dataRec.DateCreate = DateTime.Now; // update date create
            return UpdateInstance(dataRec);
        }

        public bool Delete(OsobnaStrankaUserProp dataRec)
        {
            return DeleteInstance(dataRec);
        }
        public void Delete(string sessionId, string propId)
        {
            OsobnaStrankaUserProp dataRec = Get(sessionId, propId);
            if (dataRec != null)
            {
                Delete(dataRec);
            }
        }

        Sql GetBaseQuery()
        {
            var sql = new Sql(string.Format("SELECT * FROM {0}", OsobnaStrankaUserProp.DbTableName));

            return sql;
        }

        string GetBaseWhereClause()
        {
            return string.Format("{0}.propId = @PropId", OsobnaStrankaUserProp.DbTableName);
        }
        string GetUserWhereClause()
        {
            return string.Format("{0}.userId = @UserId", OsobnaStrankaUserProp.DbTableName);
        }
        string GetSessionWhereClause()
        {
            return string.Format("{0}.sessionId = @SessionId", OsobnaStrankaUserProp.DbTableName);
        }

        public bool DeleteOldSessionData(DateTime dt)
        {
            var sql = new Sql();
            sql.Append(string.Format("DELETE FROM {0}", OsobnaStrankaUserProp.DbTableName));
            sql.Where(string.Format("{0}.sessionId IS NOT NULL AND {0}.dateCreate < @DateCreate", OsobnaStrankaUserProp.DbTableName), new { DateCreate = dt });
            Execute(sql);

            return true;
        }

    }

    [TableName(OsobnaStrankaUserProp.DbTableName)]
    [PrimaryKey("pk", AutoIncrement = false)]
    public class OsobnaStrankaUserProp : _BaseRepositoryRec
    {
        public const string DbTableName = "osUserProp";

        public int UserId { get; set; }
        public string SessionId { get; set; }
        public DateTime DateCreate { get; set; }
        public string PropId { get; set; }
        public string PropValue { get; set; }
    }
}
