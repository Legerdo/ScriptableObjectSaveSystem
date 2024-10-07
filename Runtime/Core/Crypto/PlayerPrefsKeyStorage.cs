using System;
using UnityEngine;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// PlayerPrefs를 이용하여 암호화 키와 IV를 저장하고 불러오는 클래스입니다.
    /// </summary>
    public class PlayerPrefsKeyStorage : IKeyStorage
    {
        private const string KeyPrefix = "EncryptionKey_";
        private const string KeyStorageKey = KeyPrefix + "Key";
        private const string IVStorageKey = KeyPrefix + "IV";

        /// <inheritdoc/>
        public void SaveKey(byte[] key, byte[] iv)
        {
            string keyBase64 = Convert.ToBase64String(key);
            string ivBase64 = Convert.ToBase64String(iv);
            PlayerPrefs.SetString(KeyStorageKey, keyBase64);
            PlayerPrefs.SetString(IVStorageKey, ivBase64);
            PlayerPrefs.Save();
        }

        /// <inheritdoc/>
        public bool LoadKey(out byte[] key, out byte[] iv)
        {
            key = null;
            iv = null;

            if (PlayerPrefs.HasKey(KeyStorageKey) && PlayerPrefs.HasKey(IVStorageKey))
            {
                try
                {
                    string keyBase64 = PlayerPrefs.GetString(KeyStorageKey);
                    string ivBase64 = PlayerPrefs.GetString(IVStorageKey);
                    key = Convert.FromBase64String(keyBase64);
                    iv = Convert.FromBase64String(ivBase64);

                    if (key.Length == 32 && iv.Length == 16)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"PlayerPrefs에서 암호화 키 로드에 실패했습니다: {e.Message}");
                    return false;
                }
            }

            Debug.LogWarning("PlayerPrefs에 암호화 키가 존재하지 않습니다.");
            return false;
        }

        /// <inheritdoc/>
        public void DeleteKey()
        {
            PlayerPrefs.DeleteKey(KeyStorageKey);
            PlayerPrefs.DeleteKey(IVStorageKey);
            PlayerPrefs.Save();
            
            Debug.Log("PlayerPrefs에 저장된 암호화 키와 IV가 삭제되었습니다.");
        }
    }
}
