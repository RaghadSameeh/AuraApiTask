using DataAccessLayer.Data;
using DataAccessLayer.Reposatries;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Reposatories
{
    public class GenericReposatory<T> : IGenericReposatory<T> where T : class
    {
        private readonly Context context;

        public GenericReposatory(Context context)
        {
            this.context = context;
        }
        public async Task<List<T>> GetAllAsync (params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var item in includeProperties)
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync().ConfigureAwait(false);
        }

        public T GetById(int id)
        {
           
             T Entity = context.Set<T>().Find(id);
            if (Entity != null && Entity is ISoftDeletable softDeletable && softDeletable.IsDeleted)
            {
                return null;
            }
            return Entity;
        }

        public void insert(T Entity)
        {
            context.Set<T>().Add(Entity);
        }

        public async Task <int> saveAsync()
        {
            return await context.SaveChangesAsync();

        }
        public async Task<string> SoftDelete (int id)
        {
            T Entity = GetById(id);
            if (Entity != null)
            {
                if (Entity is ISoftDeletable softDeletableEntity)
                {
                    softDeletableEntity.IsDeleted = true;
                    return "item deleted Successfully";
                    
                }
                else
                {
                   
                    return "Soft delete is not supported for this entity.";
                }

            }
            return "Item not found";


        }


    }
}
