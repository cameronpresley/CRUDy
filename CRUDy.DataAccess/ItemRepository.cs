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
        IResult<Exception, IEnumerable<Item>> GetAll();
        IResult<Exception, IOption<Item>> GetById(int id);
        IResult<Exception, Item> Add(Item item);
        IResult<Exception, Item> Edit(Item item);
        IResult<Exception, Item> Delete(Item item);
    }

    public class ItemRepository : IItemRepository
    {
        public IResult<Exception, IEnumerable<Item>> GetAll()
        {
            const string query = "SELECT Id, Title, Description FROM Item";
            Func<IDbConnection, IEnumerable<Item>> getAll = (connection) => connection.Query<Item>(query);
            return Execute(getAll);
        }

        private IResult<Exception, T> Execute<T>(Func<IDbConnection, T> method)
        {
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = ".\\SQLEXPRESS",
                InitialCatalog = "CRUDy",
                IntegratedSecurity = true,
            }.ConnectionString;
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    return Result.Success<Exception, T>(method(connection));
                }
            }
            catch (Exception ex)
            {
                return Result.Failure<Exception, T>(ex);
            }
        }

        public IResult<Exception, IOption<Item>> GetById(int id)
        {
            const string query = "SELECT Id, Title, Description FROM Item WHERE Id=@Id";
            Func<IDbConnection, IOption<Item>> getById = connection =>
            {
                var item = connection.Query<Item>(query, new { id }).SingleOrDefault();
                return item == null ? Option.No<Item>() : Option.Some(item);
            };
            return Execute(getById);
        }

        public IResult<Exception, Item> Add(Item item)
        {
            const string query = "INSERT INTO Item (Title, Description) VALUES (@Title, @Description); SELECT SCOPE_IDENTITY();";
            Func<IDbConnection, int> getNewId = connection => connection.ExecuteScalar<int>(query, new { item.Title, item.Description });
            return Execute(getNewId).Map(id =>
            {
                item.Id = id;
                return item;
            });
        }

        public IResult<Exception, Item> Edit(Item item)
        {
            const string query = "UPDATE Item SET Title=@Title, Description=@Description WHERE Id=@Id";
            Func<IDbConnection, Item> edit = connection =>
            {
                connection.Execute(query, new { item.Title, item.Description, item.Id });
                return item;
            };
            return Execute(edit);
        }

        public IResult<Exception, Item> Delete(Item item)
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
