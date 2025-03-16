namespace Pcf.Administration.DataAccess
{
    public class MongoDBSettings
    {
        public const string EMPLOYEES_COLLECTION_NAME = "Employees";
        public const string ROLES_COLLECTION_NAME = "Roles";

        public string Connection { get; set; }
        public string DatabaseName { get; set; }
    }
}