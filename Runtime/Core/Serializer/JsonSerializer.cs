using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// JSON 형식으로 ScriptableObject를 직렬화하여 저장하고, 복호화하여 로드합니다.
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        public string Extension => "JsonUtility";

        public void Serialize(ScriptableObject obj, string path)
        {
            string json = JsonUtility.ToJson(obj, true);
            string encryptedJson = SecurityHelper.EncryptString(json, KeyManager.Key, KeyManager.IV);
            File.WriteAllText(path, encryptedJson);
        }

        public async Task SerializeAsync(ScriptableObject obj, string path)
        {
            string json = JsonUtility.ToJson(obj, true);
            string encryptedJson = SecurityHelper.EncryptString(json, KeyManager.Key, KeyManager.IV);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteAsync(encryptedJson);
            }
        }

        public void Deserialize(ScriptableObject obj, string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"파일이 존재하지 않습니다: {path}");
                return;
            }

            string encryptedJson = File.ReadAllText(path);
            string json = SecurityHelper.DecryptString(encryptedJson, KeyManager.Key, KeyManager.IV);
            JsonUtility.FromJsonOverwrite(json, obj);
            Debug.Log($"암호화된 JSON 형식으로 데이터가 로드되었습니다: {path}");
        }

        public async Task DeserializeAsync(ScriptableObject obj, string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"파일이 존재하지 않습니다: {path}");
                return;
            }

            string encryptedJson;
            using (StreamReader reader = new StreamReader(path))
            {
                encryptedJson = await reader.ReadToEndAsync();
            }

            string json = SecurityHelper.DecryptString(encryptedJson, KeyManager.Key, KeyManager.IV);
            JsonUtility.FromJsonOverwrite(json, obj);
        }

        public string DeserializeToString(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"파일이 존재하지 않습니다: {path}");
                return null;
            }

            string encryptedJson = File.ReadAllText(path);
            string json = SecurityHelper.DecryptString(encryptedJson, KeyManager.Key, KeyManager.IV);
            return json;
        }        
    }
}
