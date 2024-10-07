using System;
using System.Security.Cryptography;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// 암호화 관련 공통 유틸리티 메서드를 제공하는 클래스입니다.
    /// </summary>
    public static class CryptoUtils
    {
        /// <summary>
        /// 지정된 길이의 랜덤 바이트 배열을 생성합니다.
        /// </summary>
        /// <param name="length">바이트 배열의 길이</param>
        /// <returns>생성된 랜덤 바이트 배열</returns>
        public static byte[] GenerateRandomBytes(int length)
        {
            byte[] bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }

        /// <summary>
        /// 두 개의 바이트 배열을 하나로 결합합니다.
        /// </summary>
        /// <param name="first">첫 번째 바이트 배열</param>
        /// <param name="second">두 번째 바이트 배열</param>
        /// <returns>결합된 바이트 배열</returns>
        public static byte[] CombineBytes(byte[] first, byte[] second)
        {
            byte[] combined = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, combined, 0, first.Length);
            Buffer.BlockCopy(second, 0, combined, first.Length, second.Length);
            return combined;
        }
    }
}
