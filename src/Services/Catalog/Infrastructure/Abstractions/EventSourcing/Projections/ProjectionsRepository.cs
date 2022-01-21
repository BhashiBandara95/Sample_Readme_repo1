using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.EventSourcing.Projections;
using Application.Abstractions.EventSourcing.Projections.Pagination;
using Infrastructure.Abstractions.EventSourcing.Projections.Contexts;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Abstractions.EventSourcing.Projections;

public abstract class ProjectionsRepository : IProjectionsRepository
{
    private readonly IMongoDbContext _context;

    protected ProjectionsRepository(IMongoDbContext context)
    {
        _context = context;
    }

    public virtual Task<TProjection> GetAsync<TProjection, TId>(TId id, CancellationToken cancellationToken)
        where TProjection : IProjection
        => FindAsync<TProjection>(projection => projection.Id.Equals(id), cancellationToken);

    public virtual Task<TProjection> FindAsync<TProjection>(Expression<Func<TProjection, bool>> predicate, CancellationToken cancellationToken)
        where TProjection : IProjection
        => _context.GetCollection<TProjection>().AsQueryable().Where(predicate).FirstOrDefaultAsync(cancellationToken);

    public virtual Task<IPagedResult<TProjection>> GetAllAsync<TProjection>(Paging paging, Expression<Func<TProjection, bool>> predicate, CancellationToken cancellationToken)
        where TProjection : IProjection
    {
        var queryable = _context.GetCollection<TProjection>().AsQueryable().Where(predicate);
        return Pagination.PagedResult<TProjection>.CreateAsync(paging, queryable, cancellationToken);
    }

    public virtual async Task<IPagedResult<TProjectionResult>> GetAllNestedAsync<TProjection, TProjectionResult>(Paging paging, Expression<Func<TProjection, bool>> predicate, Expression<Func<TProjection, IEnumerable<TProjectionResult>>> selector, CancellationToken cancellationToken)
        where TProjection : IProjection
        where TProjectionResult : IProjection
    {
        
        //TODO
        var orderBy = "Title";
        var sort = "asc";
        var predicateField = "Title";
        var predicateValue = "test";

        var query = _context
            .GetCollection<TProjection>()
            .AsQueryable()
            .OrderBy(orderBy, sort)
            .Where($"{predicateField} == @0", predicateValue);

        var result = await query.ToDynamicListAsync();


        var queryable = _context
            .GetCollection<TProjection>()
            .AsQueryable()
            .Where(predicate)
            .Select(selector)
            .SelectMany(results => results);

        return await Pagination.PagedResult<TProjectionResult>.CreateAsync(paging, queryable, cancellationToken);
    }

    public virtual Task UpsertAsync<TProjection>(TProjection replacement, CancellationToken cancellationToken)
        where TProjection : IProjection
        => _context
            .GetCollection<TProjection>()
            .ReplaceOneAsync(
                filter: projection => projection.Id.Equals(replacement.Id),
                replacement: replacement,
                options: new ReplaceOptions { IsUpsert = true },
                cancellationToken: cancellationToken);

    public virtual Task DeleteAsync<TProjection>(Expression<Func<TProjection, bool>> filter, CancellationToken cancellationToken)
        where TProjection : IProjection
        => _context.GetCollection<TProjection>().DeleteOneAsync(filter, cancellationToken);
}