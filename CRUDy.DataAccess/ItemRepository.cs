using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CRUDy.Domain;
using Dapper;
using Optionally;

namespace CRUDy.DataAccess
{
    public interface IItemRepository
    {
        IDatabaseResult<IEnumerable<Item>> GetAll();
        IDatabaseResult<IOption<Item>> GetById(int id);
        IDatabaseResult<Item> Add(Item item);
        IDatabaseResult<Item> Edit(Item item);
        IDatabaseResult<Item> Delete(Item item);
    }

    public class ItemRepository : IItemRepository
    {
        public IDatabaseResult<IEnumerable<Item>> GetAll()
        {
            const string query = "SELECT Id, Title, Description FROM Item";
            Func<IDbConnection, IEnumerable<Item>> getAll = (connection) => connection.Query<Item>(query);
            return Execute(getAll);
        }

        private IDatabaseResult<T> Execute<T>(Func<IDbConnection, T> method)
        {
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = ".\\SQLEXPRESS",
                InitialCatalog = "CRUDy.Database",
                IntegratedSecurity = true,
            }.ConnectionString;
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    return DatabaseResult.Success(method(connection));
                }
            }
            catch (Exception ex)
            {
                return DatabaseResult.Failure<T>(ex);
            }
        }

        public IDatabaseResult<IOption<Item>> GetById(int id)
        {
            const string query = "SELECT Id, Title, Description FROM Item WHERE Id=@Id";
            Func<IDbConnection, IOption<Item>> getById = connection =>
            {
                var item = connection.Query<Item>(query, new { id }).SingleOrDefault();
                return item == null ? Option.No<Item>() : Option.Some(item);
            };
            return Execute(getById);
        }

        public IDatabaseResult<Item> Add(Item item)
        {
            const string query = "INSERT INTO Item (Title, Description) VALUES (@Title, @Description); SELECT SCOPE_IDENTITY();";
            Func<IDbConnection, int> getNewId = connection => connection.ExecuteScalar<int>(query, new { item.Title, item.Description });
            return Execute(getNewId).Map(x =>
            {
                item.Id = x;
                return item;
            });
        }

        public IDatabaseResult<Item> Edit(Item item)
        {
            const string query = "UPDATE Item SET Title=@Title, Description=@Description WHERE Id=@Id";
            Func<IDbConnection, Item> edit = connection =>
            {
                connection.Execute(query, new { item.Title, item.Description, item.Id });
                return item;
            };
            return Execute(edit);
        }

        public IDatabaseResult<Item> Delete(Item item)
        {
            const string query = "DELETE FROM Item WHERE Id=@Id";
            Func<IDbConnection, Item> delete = connection =>
            {
                connection.Execute(query, new { item.Id });
                return item;
            };
            return Execute(delete);
        }
    }
}
