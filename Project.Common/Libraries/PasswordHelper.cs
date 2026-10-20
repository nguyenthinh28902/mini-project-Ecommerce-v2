using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Libraries
{
    public static class PasswordHelper
    {
        private const int SaltSize = 128 / 8; // 16 bytes
        private const int SubkeySize = 256 / 8; // 32 bytes
        private const int IterationCount = 100000;
        private const KeyDerivationPrf DefaultPrf = KeyDerivationPrf.HMACSHA256;

        /// <summary>
        /// Mã hóa mật khẩu thành chuỗi hash an toàn sử dụng PBKDF2.
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Mật khẩu không được để trống.");
            }

            // Tạo một mã muối (salt) ngẫu nhiên 128-bit
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            // Dùng thuật toán PBKDF2 với HMAC-SHA256, 100,000 vòng lặp
            byte[] subkey = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: DefaultPrf,
                iterationCount: IterationCount,
                numBytesRequested: SubkeySize);

            // Ghép cấu trúc: [Format Marker (1 byte)] + [PRF (4 bytes)] + [Iteration Count (4 bytes)] + [Salt Length (4 bytes)] + [Salt] + [Subkey]
            byte[] outputBytes = new byte[1 + 4 + 4 + 4 + salt.Length + subkey.Length];

            outputBytes[0] = 0x01; // Format marker phiên bản 1
            Buffer.BlockCopy(BitConverter.GetBytes((int)DefaultPrf), 0, outputBytes, 1, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(IterationCount), 0, outputBytes, 5, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(salt.Length), 0, outputBytes, 9, 4);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + salt.Length, subkey.Length);

            return Convert.ToBase64String(outputBytes);
        }

        /// <summary>
        /// Kiểm tra mật khẩu người dùng nhập vào so với chuỗi hash trong DB.
        /// </summary>
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
            {
                return false;
            }

            try
            {
                byte[] decodeBytes = Convert.FromBase64String(hashedPassword);

                // Kiểm tra độ dài tối thiểu của mảng byte (ít nhất phải chứa header 13 bytes + salt + subkey)
                if (decodeBytes.Length < 13)
                {
                    return false;
                }

                // Kiểm tra format marker (nếu có quy ước phiên bản)
                if (decodeBytes[0] != 0x01)
                {
                    return false;
                }

                // Đọc lại các thông số đã lưu trong chuỗi hash
                int prf = BitConverter.ToInt32(decodeBytes, 1);
                int iterCount = BitConverter.ToInt32(decodeBytes, 5);
                int saltLength = BitConverter.ToInt32(decodeBytes, 9);

                // Kiểm tra tính hợp lệ của độ dài salt
                if (saltLength <= 0 || saltLength > decodeBytes.Length - 13)
                {
                    return false;
                }

                // Lấy lại phần Salt
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(decodeBytes, 13, salt, 0, salt.Length);

                // Lấy lại phần Subkey (mật khẩu đã băm cũ)
                int subkeyLength = decodeBytes.Length - 13 - salt.Length;
                if (subkeyLength <= 0)
                {
                    return false;
                }

                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(decodeBytes, 13 + salt.Length, expectedSubkey, 0, subkeyLength);

                // Băm lại mật khẩu người dùng vừa nhập với cùng Salt và cấu hình cũ từ DB
                byte[] actualSubkey = KeyDerivation.Pbkdf2(
                    password: providedPassword,
                    salt: salt,
                    prf: (KeyDerivationPrf)prf,
                    iterationCount: iterCount,
                    numBytesRequested: expectedSubkey.Length);

                // So sánh hai mảng byte an toàn tuyệt đối (chống timing attack)
                return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
            }
            catch
            {
                // Nếu chuỗi hash trong DB bị lỗi định dạng Base64 hoặc lỗi xử lý byte
                return false;
            }
        }
    }
}
