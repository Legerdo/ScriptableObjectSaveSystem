using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// Newtonsoft.Json을 사용하여 JSON 형식으로 ScriptableObject를 직렬화하여 저장하고, 복호화하여 로드합니다.
    /// </summary>
    public class NewtonsoftJsonSerializer : ISerializer
    {
        public string Extension => "NewtonsoftJson";

        public class CustomContractResolver : DefaultContractResolver
        {
            // 필드 이름을 제거하기 위한 리스트
            private readonly HashSet<string> _propertiesToIgnore;

            public CustomContractResolver(IEnumerable<string> propertiesToIgnore)
            {
                _propertiesToIgnore = new HashSet<string>(propertiesToIgnore);
            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                // 모든 필드와 프로퍼티를 가져옴
                var properties = base.CreateProperties(type, memberSerialization);

                // 제거할 필드 제외
                properties = properties.Where(p => !_propertiesToIgnore.Contains(p.PropertyName)).ToList();

                return properties;
            }
        }

        private JsonSerializerSettings settings;

        public NewtonsoftJsonSerializer()
        {
            settings = new JsonSerializerSettings
            {
                ContractResolver = new CustomContractResolver(new[] { "name", "hideFlags" }), // 숨길 필드 지정
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore                
            };
        }

        public void Serialize(ScriptableObject obj, string path)
        {
            string json = JsonConvert.SerializeObject(obj, settings);
            string encryptedJson = SecurityHelper.EncryptString(json, KeyManager.Key, KeyManager.IV);
            File.WriteAllText(path, encryptedJson);
        }

        public async Task SerializeAsync(ScriptableObject obj, string path)
        {
            string json = JsonConvert.SerializeObject(obj, settings);
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
            JsonConvert.PopulateObject(json, obj, settings);
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
            JsonConvert.PopulateObject(json, obj, settings);
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
            Debug.Log($"DeserializeToString JSON: {json}");
            return json;
        }
    }
}
