using DigitalWallet.Application.DTOs;
using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task AddAsync(User user);

    Task SaveChangesAsync();

    Task<User?> GetByIdAsync(int id);

    Task<List<User>> GetAllAsync();

    Task<List<User>> GetPagedAsync(
    PaginationRequest request);

    Task<int> CountAsync(
        PaginationRequest request);

}