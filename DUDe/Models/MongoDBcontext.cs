using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Threading.Tasks;

namespace DUDe.Models
{
    public class MongoDBcontext
    {
        IMongoDatabase database; // база данных
        IGridFSBucket gridFS;   // файловое хранилище

        public MongoDBcontext()
        {
            // строка подключения
            string connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            database = client.GetDatabase(connection.DatabaseName);
            // получаем доступ к файловому хранилищу
            gridFS = new GridFSBucket(database);
        }
         public IMongoCollection<MongoDB> Test
        {
            get { return database.GetCollection<MongoDB>("Test"); }
        }
        // получаем все документы, используя критерии фильтрации
        public async Task<IEnumerable<MongoDB>> GetComputers(int? year, string name)
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<MongoDB>();
            var filter = builder.Empty; // фильтр для выборки всех документов
            // фильтр по имени
            if (!String.IsNullOrWhiteSpace(name))
            {
                filter = filter & builder.Regex("Name", new BsonRegularExpression(name));
            }
            if (year.HasValue)  // фильтр по году
            {
                filter = filter & builder.Eq("Year", year.Value);
            }

            return await Test.Find(filter).ToListAsync();
        }

        // получаем один документ по id
        public async Task<MongoDB> GetComputer(string id)
        {
            return await Test.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        // добавление документа
        public async Task Create(MongoDB c)
        {
            await Test.InsertOneAsync(c);
        }
        // обновление документа
        public async Task Update(MongoDB c)
        {
            await Test.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(c.Id)), c);
        }
        // удаление документа
        public async Task Remove(string id)
        {
            await Test.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        // получение изображения
        public async Task<byte[]> GetImage(string id)
        {
            return await gridFS.DownloadAsBytesAsync(new ObjectId(id));
        }
        // сохранение изображения
        public async Task StoreImage(string id, Stream imageStream, string imageName)
        {
            MongoDB c = await GetComputer(id);
            if (c.HasImage())
            {
                // если ранее уже была прикреплена картинка, удаляем ее
                await gridFS.DeleteAsync(new ObjectId(c.ImageId));
            }
            // сохраняем изображение
            ObjectId imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
            // обновляем данные по документу
            c.ImageId = imageId.ToString();
            var filter = Builders<MongoDB>.Filter.Eq("_id", new ObjectId(c.Id));
            var update = Builders<MongoDB>.Update.Set("ImageId", c.ImageId);
            await Test.UpdateOneAsync(filter, update);
        }
    }
}