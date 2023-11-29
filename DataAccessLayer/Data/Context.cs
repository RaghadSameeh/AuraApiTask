using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class Context : DbContext
    {
        public Context() { }

        public Context (DbContextOptions <Context> options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var isDeletedProperty = entityType.FindProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
                {
                    // Apply soft delete filter for entities with the "IsDeleted" property
                    var parameter = Expression.Parameter(entityType.ClrType);
                    var softDeleteFilter = Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, isDeletedProperty.PropertyInfo),
                            Expression.Constant(false, typeof(bool))
                        ),
                        parameter
                    );

                    entityType.SetQueryFilter(softDeleteFilter);
                }
            }

        }

        public DbSet<Product> products { get; set; }
    
        
    }
}
