using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System.Configuration;
using System.Linq.Expressions;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL
{




    public class DAL<DataBaseType> where DataBaseType : DbContext
    {
        private const string FIND_BY_KEY = "Error searching an entity of type '{0}'.\nProbably parameters aren't univocal.{1}{2}";
        private const string DATABASE_ERROR = "Database Operation '{0}()' failed for an entity of type '{1}'.{2}{3}";
        private DbContextOptionsBuilder<DataBaseType> optionsBuilder { get; set; }
        public DataBaseType EntityModel { get; set; }
        private string privateEntityName = null;
        protected string EntityName
        {
            get
            {
                return privateEntityName;
            }
        }
        public DAL(string connectionStringName, DataBaseType _entityModel = null)
        {
            CheckAndCreateModel(connectionStringName, _entityModel);
            privateEntityName = typeof(DataBaseType).Name;
        }
        private void CheckAndCreateModel(string connectionStringName, DataBaseType _entityModel)
        {
            try
            {
                if (_entityModel == null)
                {
                    if (string.IsNullOrWhiteSpace(connectionStringName))
                    {
                        throw new Exception("La connection string è vuota");
                    }

                    CreateModel(connectionStringName);
                }
                else
                {
                    _entityModel = _entityModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void CreateModel(string connectionStringName)
        {
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<DataBaseType>();
                optionsBuilder.UseSqlServer(connectionStringName);
                EntityModel = (DataBaseType)Activator.CreateInstance(typeof(DataBaseType), optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Database error, CreateModel() exception: ", ex.Message, ex.InnerException != null ? (" " + ex.InnerException.Message) : string.Empty));
            }


            if (EntityModel == null || EntityModel.Database == null /*|| !entityModel.Database.Exists()*/)
            {

                throw new Exception("Database error: Cannot access to database, connectionString may not be valid or server is inaccessible.");
            }
        }       
        protected string GetDatabaseErrorMessage(string methodName, string[] parametersArray, Exception ex)
        {
            string parameters = string.Empty;

            if (parametersArray != null && parametersArray.Length > 0)
            {
                parameters = "\nUsed Parameters:\n(";

                for (int index = 0; index <= parametersArray.Length - 1; index++)
                {
                    if (index > 0)
                        parameters = string.Concat(parameters, ",");

                    parameters = string.Concat(parameters, parametersArray[index]);
                }

                parameters = string.Concat(parameters, ").");
            }

            Exception currentException = ex;
            string exceptionMessage = string.Empty;
            int i = 0;
            do
            {
                if (currentException != null)
                {
                    exceptionMessage = string.Concat(exceptionMessage, $"\nException level {i + 1}: ", currentException.Message, "; ");
                    currentException = currentException.InnerException;
                    i++;
                }

            } while (currentException != null);

            return string.Format(DATABASE_ERROR, methodName, EntityName, parameters, exceptionMessage);
        }
        private static void ThrowDBError(string errorMessage, Exception ex)
        {
            throw new Exception(errorMessage, ex);
        }
        public IQueryable<T> Read<T>(Expression<Func<T, bool>> predicate = null, bool includiEliminati = false) where T : class
        {
            IQueryable<T> result = null;

            try
            {
                if (predicate != null)
                {
                    result = EntityModel.Set<T>().Where(predicate);
                }
                else
                {
                    result = EntityModel.Set<T>();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = GetDatabaseErrorMessage(nameof(Read), null, ex);
                // LogServerManager.LogError(errorMessage)
                ThrowDBError(errorMessage, ex);
            }

            return result;
        }
        public void SaveChanges()
        {
            try
            {
                EntityModel.SaveChanges();
            }
            catch (Exception ex)
            {
                string errorMessage = GetDatabaseErrorMessage(nameof(SaveChanges), null, ex);
                ThrowDBError(errorMessage, ex);
            }
        }
        public void Create<TableType>(TableType entityToCreate, bool mustSaveChanges = false) where TableType : class
        {
            try
            {
                //DalContext.CreateObjectSet<T>().AddObject(entityToCreate);
                EntityModel.Set<TableType>().Add(entityToCreate);
                if (mustSaveChanges == true)
                    SaveChanges();
            }
            catch (Exception ex)
            {
                string errorMessage = GetDatabaseErrorMessage(nameof(Create), null, ex);
                ThrowDBError(errorMessage, ex);
            }
        }
        public void Delete<T>(IEnumerable<T> entitiesToDelete, bool mustSaveChanges = false) where T : class
        {
            try
            {
                foreach (T entity in entitiesToDelete)
                    Delete(entity, false);

                if (mustSaveChanges == true)
                    SaveChanges();
            }
            catch (Exception ex)
            {
                string errorMessage = GetDatabaseErrorMessage("DeleteCollection", null, ex);
                ThrowDBError(errorMessage, ex);
            }
        }
        public void Delete<T>(T entityToDelete, bool mustSaveChanges = false) where T : class
        {
            try
            {
                EntityModel.Set<T>().Attach(entityToDelete);
                EntityModel.Entry(entityToDelete).State = EntityState.Deleted;

                if (mustSaveChanges == true)
                    SaveChanges();
            }
            catch (Exception ex)
            {
                string errorMessage = GetDatabaseErrorMessage(nameof(Delete), null, ex);
                ThrowDBError(errorMessage, ex);
            }
        }
        private DataBaseType GetEntities(string connectionStringName = null)
        {
            try
            {
                var entityModel = (DataBaseType)Activator.CreateInstance(typeof(DataBaseType), connectionStringName);
                return entityModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Refresh()
        {
            try
            {
                //DalContext.Refresh(RefreshMode.StoreWins, DalContext.CreateObjectSet<T>());

                EntityModel.Dispose();
                EntityModel = GetEntities();

            }
            catch (Exception ex)
            {
                string errorMessage = GetDatabaseErrorMessage(nameof(Refresh), null, ex);
                ThrowDBError(errorMessage, ex);
            }
        }
    }
}
