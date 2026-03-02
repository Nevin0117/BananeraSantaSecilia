using System.Security.Cryptography;
using SantaSecilia.Domain.Entities;
using SantaSecilia.Infrastructure.Repositories;

namespace SantaSecilia.Application.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;
    private const string CurrentUserKey = "current_user_id";

    public AuthService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        
        if (user == null || !user.IsActive)
            return null;

        if (!VerifyPassword(password, user.PasswordHash))
            return null;

        return user;
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations: 100000,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: 32);

        var saltHex = Convert.ToHexString(salt);
        var hashHex = Convert.ToHexString(hash);
        
        return $"{saltHex}:{hashHex}";
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split(':');
        if (parts.Length != 2)
            return false;

        var salt = Convert.FromHexString(parts[0]);
        var storedHashBytes = Convert.FromHexString(parts[1]);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations: 100000,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: 32);

        return CryptographicOperations.FixedTimeEquals(hash, storedHashBytes);
    }

    public async Task SetCurrentUserAsync(int userId)
    {
        await SecureStorage.SetAsync(CurrentUserKey, userId.ToString());
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var userIdStr = await SecureStorage.GetAsync(CurrentUserKey);
        
        if (string.IsNullOrEmpty(userIdStr))
            return null;

        if (!int.TryParse(userIdStr, out var userId))
            return null;

        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<bool> HasActiveSessionAsync()
    {
        var userId = await SecureStorage.GetAsync(CurrentUserKey);
        return !string.IsNullOrEmpty(userId);
    }

    public async Task LogoutAsync()
    {
        SecureStorage.Remove(CurrentUserKey);
    }
}
