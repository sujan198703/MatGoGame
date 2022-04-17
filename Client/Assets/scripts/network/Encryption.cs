using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

public class Encryption : MonoBehaviour
{
    public static Encryption instance;
    private AesCryptoServiceProvider AES_CRYPTO;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        AES_CRYPTO = new AesCryptoServiceProvider();
        AES_CRYPTO.BlockSize = 128;
        AES_CRYPTO.KeySize = 256;
        AES_CRYPTO.GenerateIV();
        AES_CRYPTO.Mode = CipherMode.CBC;
        AES_CRYPTO.Padding = PaddingMode.PKCS7;
    }

    public string encrypt(string data)
    {
        ICryptoTransform transform = AES_CRYPTO.CreateEncryptor();

        byte[] encrypted_bytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(data), 0, data.Length);
        string str = Convert.ToBase64String(encrypted_bytes);
        print(str);
        string key = ASCIIEncoding.ASCII.GetString(AES_CRYPTO.Key);
       // print("Key used: " + key);
        return str;
    }

    public string decrypt(string data)
    {
        ICryptoTransform transform = AES_CRYPTO.CreateDecryptor();

        byte[] encrypted_bytes = Convert.FromBase64String(data);
        byte[] decrypted_bytes = transform.TransformFinalBlock(encrypted_bytes, 0, encrypted_bytes.Length);

        string str = ASCIIEncoding.ASCII.GetString(decrypted_bytes);

        return str;
    }

    public void SetKey(string key)
    {
        print("Key used: " + key);
        byte[] enc_key = Convert.FromBase64String(key);
        AES_CRYPTO.Key = enc_key;
    }

    public string GetKey()
    {
        return Convert.ToBase64String(AES_CRYPTO.Key);
    }
}
