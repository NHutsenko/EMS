using EMS.Gateway.API.DAL;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gateway.API.Test.Mocks
{
    public class DbContextMock
    {
        public static bool ShouldThrowException = false;

        public static Mock<DbSet<T>> SetupCollectionMock<T>(List<T> data) where T : class
        {
            IQueryable<T> dataQueryable = data.AsQueryable();
            Mock<DbSet<T>> mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(dataQueryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(dataQueryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(dataQueryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => dataQueryable.GetEnumerator());

            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback((T item) =>
            {
                ThrowExceptionIfNeeded(ShouldThrowException);
                data.Add(item);
            });

            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<T>>())).Callback((IEnumerable<T> items) =>
            {
                ThrowExceptionIfNeeded(ShouldThrowException);
                data.AddRange(items);

            });

            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback((T item) =>
            {
                ThrowExceptionIfNeeded(ShouldThrowException);
                data.Remove(item);
            });

            mockSet.Setup(m => m.Update(It.IsAny<T>())).Callback((T item) =>
            {
                ThrowExceptionIfNeeded(ShouldThrowException);
                UpdateEntity(item, data);
            });

            return mockSet;
        }

        private static void ThrowExceptionIfNeeded(bool shouldThrowException)
        {
            if (shouldThrowException)
            {
                ShouldThrowException = false;
                throw new Exception("DbContext test Exception");
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
