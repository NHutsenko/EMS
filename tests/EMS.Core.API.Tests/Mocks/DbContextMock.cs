using EMS.Core.API.DAL;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EMS.Core.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class DbContextMock
    {
        public static bool ShouldThrowException { get; set; }
        public static int SaveChangesResult { get; set; } = 1;

        public static Mock<DbSet<T>> SetupCollectionMock<T>(List<T> data) where T : class
        {
            IQueryable<T> dataQueryable = data.AsQueryable();
            Mock<DbSet<T>> mockSet = new();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(dataQueryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(dataQueryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(dataQueryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => dataQueryable.GetEnumerator());

            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback((T item) =>
            {
                if((long)item.GetType().GetProperty("Id").GetValue(item) == 0)
                {
                    long index = 0;
                    foreach(object entity in data)
                    {
                        long currentIndex = (long)entity.GetType().GetProperty("Id").GetValue(entity);
                        if(currentIndex >= index)
                        {
                            index = ++currentIndex;
                        }
                    }
                    item.GetType().GetProperty("Id").SetValue(item, index);
                }
                data.Add(item);
            });

            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback((T item) =>
            {
                data.Remove(item);
            });

            mockSet.Setup(m => m.Update(It.IsAny<T>())).Callback((T item) =>
            {
                UpdateEntity(item, data);
            });
            return mockSet;
        }

        private static void ThrowExceptionIfNeeded()
        {
            if (ShouldThrowException)
            {
                ShouldThrowException = false;
                throw new DbUpdateException("DbContext test Exception");
            }
        }

        public static void UpdateEntity<T>(T entity, ICollection<T> data)
        {
            if (data == null || entity == null)
            {
                return;
            }
            T entityToUpdate = data.FirstOrDefault(((T x) => GetEntityId(x).ToString() == GetEntityId(entity).ToString()));
            if (entityToUpdate == null)
            {
                return;
            }
            PropertyInfo[] properties = entityToUpdate.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = entity.GetType().GetProperty(property.Name).GetValue(entity);
                entityToUpdate.GetType().GetProperty(property.Name).SetValue(entityToUpdate, value);
            }
        }

        private static object GetEntityId<T>(T entity)
        {
            return entity.GetType().GetProperty("Id").GetValue(entity);
        }

        public static Mock<IApplicationDbContext> SetupDbContext<T>() where T : class
        {
            Mock<IApplicationDbContext> dbContext = new Mock<IApplicationDbContext>();

            dbContext.SetupAllProperties();

            dbContext.Setup(m => m.SaveChangesAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns<bool, CancellationToken>((accept, token) =>
            {
                ThrowExceptionIfNeeded();
                return Task.FromResult(SaveChangesResult);
            });

            Type interfaceType = typeof(T);
            List<PropertyInfo> interfaceIDbSetProperties = interfaceType.GetProperties()
              .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
              .ToList();

            foreach (PropertyInfo propertyInfo in interfaceType.GetProperties())
            {
                Type[] arguments = propertyInfo.PropertyType.GetGenericArguments();

                if (arguments.Length > 0)
                {
                    Type argument = propertyInfo.PropertyType.GetGenericArguments()[0];
                    Type genericType = typeof(List<>).MakeGenericType(argument);
                    dynamic instance = Activator.CreateInstance(genericType);
                    dynamic collectionAsMock = SetupCollectionMock(instance);
                    propertyInfo.SetValue(dbContext.Object, collectionAsMock.Object);
                }
            }

            return dbContext;
        }
    }

}
