using System.Security.Cryptography;

namespace Chapters.Options;

public class PasswordHasherOptions
{
    public HashAlgorithmName HashAlgorithmName { get; set; } = HashAlgorithmName.SHA1;
    public int SaltSize { get; set; } = 16;
    public int HashSize { get; set; } = 256;
    public int Iterations { get; set; } = 8192;
}