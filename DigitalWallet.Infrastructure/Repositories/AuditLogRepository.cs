using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;


    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }



    public async Task AddAsync(AuditLog auditLog)
    {
        await _context.AuditLogs.AddAsync(auditLog);
    }




    public async Task<List<AuditLog>> GetUserLogsAsync(int userId)
    {
        return await _context.AuditLogs
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _context.AuditLogs
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<AuditLog>> GetPagedAsync(
        PaginationRequest request)
    {
        var query =
            _context.AuditLogs
                .Include(a => a.User)
                .AsQueryable();


        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            if (int.TryParse(search, out int searchId))
            {
                query = query.Where(a =>
                    a.Id == searchId ||
                    a.UserId == searchId ||
                    a.Action.Contains(search) ||
                    a.Description.Contains(search));
            }
            else
            {
                query = query.Where(a =>
                    a.Action.Contains(search) ||
                    a.Description.Contains(search));
            }
        }


        if (request.SortOrder.ToLower() == "asc")
        {
            query = query.OrderBy(a => a.CreatedAt);
        }
        else
        {
            query = query.OrderByDescending(a => a.CreatedAt);
        }


        return await query
            .Skip(
                (request.Page - 1) *
                request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
    }


    public async Task<int> CountAsync(
     PaginationRequest request)
    {
        var query =
            _context.AuditLogs
                .AsQueryable();


        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            if (int.TryParse(search, out int searchId))
            {
                query = query.Where(a =>
                    a.Id == searchId ||
                    a.UserId == searchId ||
                    a.Action.Contains(search) ||
                    a.Description.Contains(search));
            }
            else
            {
                query = query.Where(a =>
                    a.Action.Contains(search) ||
                    a.Description.Contains(search));
            }
        }


        return await query.CountAsync();
    }

}