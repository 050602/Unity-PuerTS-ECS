
using System.IO;
using System.Security.Cryptography;
namespace UnityEngine.ResourceManagement.ResourceProviders
{
    public class AnStreamProcessor : IDataConverter
    {
        byte[] Key { get { return System.Text.Encoding.ASCII.GetBytes("An-8L7d2k1h0j8h8"); } }
        SymmetricAlgorithm m_algorithm;
        SymmetricAlgorithm Algorithm
        {
            get
            {
                if (m_algorithm == null)
                {
                    m_algorithm = new AesManaged();
                    m_algorithm.Padding = PaddingMode.Zeros;
                    var initVector = new byte[m_algorithm.BlockSize / 8];
                    for (int i = 0; i < initVector.Length; i++)
                        initVector[i] = (byte)i;
                    m_algorithm.IV = initVector;
                    m_algorithm.Key = Key;
                    m_algorithm.Mode = CipherMode.ECB;
                }
                return m_algorithm;
            }
        }
        public Stream CreateReadStream(Stream input, string id)
        {
            return new CryptoStream(input, Algorithm.CreateDecryptor(Algorithm.Key, Algorithm.IV), CryptoStreamMode.Read);
        }

        public Stream CreateWriteStream(Stream input, string id)
        {
            return new CryptoStream(input, Algorithm.CreateEncryptor(Algorithm.Key, Algorithm.IV), CryptoStreamMode.Write);
        }
    }
    }