using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

public class AESCrypto
{
    private static string Key;
    private static string IV;

    public static AESCrypto instance;

    public AESCrypto()
    {
        instance = this;

        Key = "Full Stack IT Service 198703Game";
        IV = "MatGoGameProject";

       // UnityEngine.Debug.Log("1. == Key Length : " + Key.Length);
     //   UnityEngine.Debug.Log("1. == Key : " + Key);
    }

    public string Encrypt(string message)
    {
        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        aes.BlockSize = 128;
        aes.KeySize = 256;

        UnityEngine.Debug.Log("2. == Key Length : " + Key.Length);
        UnityEngine.Debug.Log("2. == Key : " + Key);

        aes.IV = UTF8Encoding.UTF8.GetBytes(IV);
        aes.Key = UTF8Encoding.UTF8.GetBytes(Key);
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        byte[] data = Encoding.UTF8.GetBytes(message);
        using (ICryptoTransform encrypt = aes.CreateEncryptor())
        {
            byte[] dest = encrypt.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(dest);
        }
    }

    public string Decrypt(string encryptedText)
    {
        string plaintext = null;
        using (AesManaged aes = new AesManaged())
        {
            byte[] cipherText = Convert.FromBase64String(encryptedText);
            byte[] aesIV = UTF8Encoding.UTF8.GetBytes(IV);
            byte[] aesKey = UTF8Encoding.UTF8.GetBytes(Key);
            ICryptoTransform decryptor = aes.CreateDecryptor(aesKey, aesIV);
            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                        plaintext = reader.ReadToEnd();
                }
            }
        }

        return plaintext;
    }

    public static void SetKey( string strKey )
    {
        Key = strKey;
    }

    public static string GetKey()
    {
        return Key;
    }
}
