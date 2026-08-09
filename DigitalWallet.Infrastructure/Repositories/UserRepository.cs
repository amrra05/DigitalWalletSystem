using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;


    public UserRepository(AppDbContext context)
    {
        _context = context;
    }



    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Email == email);
    }



    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }



    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                u => u.Id == id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Wallet)
            .ToListAsync();
    }

    public async Task<List<User>> GetPagedAsync(
    PaginationRequest request)
    {
        var query =
            _context.Users
            .Include(u => u.Wallet)
            .AsQueryable();



        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(u =>
                u.Id.ToString().Contains(request.Search) ||
                u.Name.Contains(request.Search) ||
                u.Email.Contains(request.Search) ||
                u.Role.ToString().Contains(request.Search));
        }



        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            query = query.Where(u =>
                u.Role.ToString() == request.Role);
        }

        if (request.SortBy.ToLower() == "name")
        {
            query = request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(u => u.Name)
                : query.OrderByDescending(u => u.Name);
        }
        else if (request.SortBy.ToLower() == "email")
        {
            query = request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(u => u.Email)
                : query.OrderByDescending(u => u.Email);
        }
        else
        {
            query = request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(u => u.CreatedAt)
                : query.OrderByDescending(u => u.CreatedAt);
        }

        return await query
            .OrderBy(u => u.Id)
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
            _context.Users
            .AsQueryable();



        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(u =>
                u.Name.Contains(request.Search) ||
                u.Email.Contains(request.Search));
        }



        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            query = query.Where(u =>
                u.Role.ToString() == request.Role);
        }



        return await query.CountAsync();
    }
}