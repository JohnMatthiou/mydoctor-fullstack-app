
using Microsoft.EntityFrameworkCore;
using MyDoctorApp.Data;

namespace MyDoctorApp.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MyDoctorAppDbContext context;
        protected readonly DbSet<T> dbset;

        public BaseRepository(MyDoctorAppDbContext context)
        {
            this.context = context;
            dbset = context.Set<T>();
        }

        public virtual async Task AddAsync(T entity) => await dbset.AddAsync(entity);


        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await dbset.AddRangeAsync(entities);


        public virtual async Task<bool> DeleteAsync(int id)
        {
            T? existingEntity = await GetAsync(id);
            if (existingEntity is null) return false;
            dbset.Remove(existingEntity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await dbset.ToListAsync();

        public virtual async Task<T?> GetAsync(int id) => await dbset.FindAsync(id);
       
        public virtual Task UpdateAsync(T entity)
        {
            dbset.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
