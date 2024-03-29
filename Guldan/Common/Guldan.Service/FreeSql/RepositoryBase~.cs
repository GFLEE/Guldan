﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using Guldan.Service.FreeSql.Base;
using Org.BouncyCastle.Utilities.Collections;

namespace Guldan.Service.FreeSql
{
    public class RepositoryBase<TEntity, TKey> : BaseRepository<TEntity, TKey>,
        IRepositoryBase<TEntity, TKey> where TEntity : class, new()
    {
        public IUserContext User { get; set; }

        public RepositoryBase(IFreeSql freeSql) : base(freeSql, null, null)
        {
        }
        public RepositoryBase(IFreeSql fsql, Expression<Func<TEntity, bool>> filter, Func<string, string> asTable = null) : base(fsql, filter, asTable) { }

        public TEntity GetNew()
        {
            var newObj = default(TEntity);
             return newObj;
        }

        public virtual Task<TDto> GetAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToOneAsync<TDto>();
        }

        public virtual Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync<TDto>();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync();
        }

        public virtual async Task<bool> SoftDeleteAsync(TKey id)
        {
            await UpdateDiy
                .SetDto(new
                {
                    IsDeleted = true,
                    ModifiedUserId = User.Id,
                    ModifiedUserName = User.Name
                })
                .WhereDynamic(id)
                .ExecuteAffrowsAsync();

            return true;
        }

        public virtual async Task<bool> SoftDeleteAsync(TKey[] ids)
        {
            await UpdateDiy
                .SetDto(new
                {
                    IsDeleted = true,
                    ModifiedUserId = User.Id,
                    ModifiedUserName = User.Name
                })
                .WhereDynamic(ids)
                .ExecuteAffrowsAsync();

            return true;
        }

        public virtual async Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames)
        {
            await UpdateDiy
                .SetDto(new
                {
                    IsDeleted = true,
                    ModifiedUserId = User.Id,
                    ModifiedUserName = User.Name
                })
                .Where(exp)
                .DisableGlobalFilter(disableGlobalFilterNames)
                .ExecuteAffrowsAsync();

            return true;
        }

        public virtual async Task<bool> DeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames)
        {
            await Select
            .Where(exp)
            .DisableGlobalFilter(disableGlobalFilterNames)
            .AsTreeCte()
            .ToDelete()
            .ExecuteAffrowsAsync();

            return true;
        }

        public virtual async Task<bool> SoftDeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames)
        {
            await Select
            .Where(exp)
            .DisableGlobalFilter(disableGlobalFilterNames)
            .AsTreeCte()
            .ToUpdate()
            .SetDto(new
            {
                IsDeleted = true,
                ModifiedUserId = User.Id,
                ModifiedUserName = User.Name
            })
            .ExecuteAffrowsAsync();

            return true;
        }


    }

}
