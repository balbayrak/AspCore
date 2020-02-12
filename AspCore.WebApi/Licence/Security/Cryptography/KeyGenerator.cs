using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace AspCore.WebApi.Licence
{
    public class KeyGenerator
    {
        private readonly IAsymmetricCipherKeyPairGenerator keyGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGenerator"/> class
        /// with a key size of 256 bits.
        /// </summary>
        public KeyGenerator()
            : this(256)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGenerator"/> class
        /// with the specified key size and seed.
        /// </summary>
        /// <remarks>Following key sizes are supported:
        /// - 192
        /// - 224
        /// - 239
        /// - 256 (default)
        /// - 384
        /// - 521</remarks>
        /// <param name="keySize">The key size.</param>
        /// <param name="seed">The seed.</param>
        public KeyGenerator(int keySize)
        {
            var secureRandom = SecureRandom.GetInstance("SHA256PRNG");
            secureRandom.SetSeed(secureRandom.GenerateSeed(keySize));

            var keyParams = new KeyGenerationParameters(secureRandom, keySize);
            keyGenerator = new ECKeyPairGenerator();
            keyGenerator.Init(keyParams);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="KeyGenerator"/> class.
        /// </summary>
        public static KeyGenerator Create()
        {
            return new KeyGenerator();
        }

        /// <summary>
        /// Generates a private/public key pair for license signing.
        /// </summary>
        /// <returns>An <see cref="KeyPair"/> containing the keys.</returns>
        public KeyPair GenerateKeyPair()
        {
            return new KeyPair(keyGenerator.GenerateKeyPair());
        }
    }
}