﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FuStudy_Repository.Entity;

namespace FuStudy_Repository.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        IEnumerable<TEntity> Get(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "",
    int? pageIndex = null,
    int? pageSize = null);

        TEntity GetByID(object id);
        void Insert(TEntity entity);
        Task<Order> GetOrderByPaymentAsync(long transactionId);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        Task<Order> GetOrderByTransactionIdAsync(long transactionId);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filterExpression);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(long id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<RolePermission>> GetRolePermissionsByRoleIdAsync(long roleId);
        Task<Token> GetUserToken(long id);
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null);
        Task SaveChangesAsync();

        Task<TEntity> GetByIdWithInclude(long id, string includeProperties = "");

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

    }
}
