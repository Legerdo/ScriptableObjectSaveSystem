using System;
using UnityEngine;
using ScriptableObjectSaveSystem;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// 암호화 키와 IV를 관리하는 정적 클래스입니다. 저장소 인터페이스를 통해 키를 저장하고 불러옵니다.
    /// </summary>
    public static class KeyManager
    {
        private static IKeyStorage keyStorage;
        private static bool isInitialized = false;

        /// <summary>
        /// AES 키 (32 bytes for AES-256)
        /// </summary>
        public static byte[] Key { get; private set; }

        /// <summary>
        /// AES IV (16 bytes)
        /// </summary>
        public static byte[] IV { get; private set; }

        /// <summary>
        /// KeyManager를 초기화합니다. 저장소는 여기서 설정할 수 있습니다.
        /// </summary>
        /// <param name="storage">사용할 키 저장소</param>
        public static void Initialize(IKeyStorage storage = null)
        {
            if (isInitialized)
            {
                Debug.LogWarning("KeyManager가 이미 초기화되었습니다.");
                return;
            }

            keyStorage = storage ?? new PlayerPrefsKeyStorage();

            if (keyStorage.LoadKey(out byte[] loadedKey, out byte[] loadedIV))
            {
                Key = loadedKey;
                IV = loadedIV;
            }
            else
            {
                Debug.Log("암호화 키가 존재하지 않거나 로드에 실패했습니다. 새로운 키와 IV를 생성합니다.");
                GenerateAndSaveKey();
            }

            isInitialized = true;
        }

        /// <summary>
        /// 키와 IV를 생성하고 저장소에 저장합니다.
        /// </summary>
        private static void GenerateAndSaveKey()
        {
            Key = CryptoUtils.GenerateRandomBytes(32);
            IV = CryptoUtils.GenerateRandomBytes(16);

            keyStorage.SaveKey(Key, IV);
            Debug.Log("암호화 키와 IV가 생성되어 저장되었습니다.");
        }

        /// <summary>
        /// 키와 IV를 리셋하고 저장소에서 삭제합니다.
        /// </summary>
        public static void ResetKeys()
        {
            keyStorage.DeleteKey();
            Key = null;
            IV = null;
            isInitialized = false;
            Debug.Log("키가 리셋되었습니다.");
        }
    }
}
