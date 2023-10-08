using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Configuration
{
    public static class MongoDBConfiguration
    {

        public static void ConfigureProductCollectionValidation(IMongoDatabase database)
        {

            var options = new CreateCollectionOptions<BsonDocument>
            {
                Validator = new BsonDocument
                {
                    { "$jsonSchema", new BsonDocument
                        {
                            { "bsonType", "object" },
                            { "required", new BsonArray { "Name", "Description", "Price", "CategoryId" } },
                            { "properties", new BsonDocument
                                {
                                    { "Name", new BsonDocument
                                        {
                                            { "bsonType", "string" },
                                        }
                                    },
                                    { "Description", new BsonDocument
                                        {
                                            { "bsonType", "string" },
                                        }
                                    },
                                    { "Price", new BsonDocument
                                        {
                                            { "bsonType", "decimal" }
                                        }
                                    },
                                    { "CategoryId", new BsonDocument
                                        {
                                            { "bsonType", "objectId" }
                                        }
                                    },
                                    { "Color", new BsonDocument
                                        {
                                            { "bsonType", "string" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            if (!database.ListCollectionNames().ToList().Contains("Product"))
            {
                database.CreateCollection("Product", options);
            }
        }

        public static void ConfigureCategoryCollectionValidation(IMongoDatabase database)
        {
            var options = new CreateCollectionOptions<BsonDocument>
            {
                Validator = new BsonDocument
                {
                    { "$jsonSchema", new BsonDocument
                        {
                            { "bsonType", "object" },
                            { "required", new BsonArray { "Name", "Description" } },
                            { "properties", new BsonDocument
                                {
                                    { "Name", new BsonDocument
                                        {
                                            { "bsonType", "string" }
                                        }
                                    },
                                    { "Description", new BsonDocument
                                        {
                                            { "bsonType", "string" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            if (!database.ListCollectionNames().ToList().Contains("Category")) {
                database.CreateCollection("Category", options);
            }
            
        }
    }
}
