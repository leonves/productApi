namespace ProductApi.Models.Settings
{
    public class CategoryDatabaseSettings : ICategoryDatabaseSettings
    {
        public string CategoriesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICategoryDatabaseSettings
    {
        string CategoriesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
