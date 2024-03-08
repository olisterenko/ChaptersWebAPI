using System.Security.Cryptography;
using Chapters.Options;
using Chapters.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Chapters.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasherSettings _settings;

    public PasswordHasher(IOptions<PasswordHasherSettings> options)
    {
        _settings = options.Value;
    }

    public string HashPassword(string password)
    {
        byte[] saltBuffer;
        byte[] hashBuffer;
    
        using (var keyDerivation = new Rfc2898DeriveBytes(password, _settings.SaltSize, _settings.Iterations, _settings.HashAlgorithmName))
        {
            saltBuffer = keyDerivation.Salt;
            hashBuffer = keyDerivation.GetBytes(_settings.HashSize);
        }
    
        byte[] result = new byte[_settings.HashSize + _settings.SaltSize];
        Buffer.BlockCopy(hashBuffer, 0, result, 0, _settings.HashSize);
        Buffer.BlockCopy(saltBuffer, 0, result, _settings.HashSize, _settings.SaltSize);
        return Convert.ToBase64String(result);
    }
    
    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
        if (hashedPasswordBytes.Length != _settings.HashSize + _settings.SaltSize)
        {
            return false;
        }

        byte[] hashBytes = new byte[_settings.HashSize];
        Buffer.BlockCopy(hashedPasswordBytes, 0, hashBytes, 0, _settings.HashSize);
        byte[] saltBytes = new byte[_settings.SaltSize];
        Buffer.BlockCopy(hashedPasswordBytes, _settings.HashSize, saltBytes, 0, _settings.SaltSize);

        byte[] providedHashBytes;
        using (var keyDerivation = new Rfc2898DeriveBytes(providedPassword, saltBytes, _settings.Iterations, _settings.HashAlgorithmName))
        {
            providedHashBytes = keyDerivation.GetBytes(_settings.HashSize);
        }

        return hashBytes.SequenceEqual(providedHashBytes);
    }
}