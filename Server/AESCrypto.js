const crypto = require('crypto');
const encryptionType = 'aes-256-cbc';
const encryptionEncoding = 'base64';
const bufferEncryption = 'utf-8';

class AESCrypto 
{
    AesKey;
    AesIV;

    constructor() {
        this.AesKey = 'Full Stack IT Service 198703Game';
        this.AesIV = 'MatGoGameProject';
    }

    encrypt(jsonObject) {
        const val = JSON.stringify(jsonObject);
        const key = Buffer.from(this.AesKey, bufferEncryption);
        const iv = Buffer.from(this.AesIV, bufferEncryption);
        const cipher = crypto.createCipheriv(encryptionType, key, iv);
        var encrypted = cipher.update(val, bufferEncryption, encryptionEncoding);
        encrypted += cipher.final(encryptionEncoding);
        return encrypted;
    }

    decrypt(base64String) {
        const buff = Buffer.from(base64String, encryptionEncoding);
        const key = Buffer.from(this.AesKey, bufferEncryption);
        const iv = Buffer.from(this.AesIV, bufferEncryption);
        const decipher = crypto.createDecipheriv(encryptionType, key, iv);
        var deciphered = decipher.update(buff) + decipher.final();
        return deciphered;
    }
}

module.exports = AESCrypto 