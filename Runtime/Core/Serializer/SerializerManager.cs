using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// 다양한 Serializer를 관리하고 제공합니다.
    /// </summary>
    public static class SerializerManager
    {
        private static Dictionary<string, ISerializer> serializers = new Dictionary<string, ISerializer>();

        // 정적 생성자를 통해 기본 Serializer를 등록합니다.
        static SerializerManager()
        {
            // 기본 제공 Serializer 등록
            RegisterSerializer(new NewtonsoftJsonSerializer());
            RegisterSerializer(new JsonSerializer());

            // 추후 다른 Serializer를 등록할 수 있습니다.
            // 예: RegisterSerializer(new MemorypackSerializer());
            // 예: RegisterSerializer(new MessagePackSerializer());
            // 예: RegisterSerializer(new XmlSerializer());
        }

        /// <summary>
        /// 새로운 Serializer를 등록합니다.
        /// </summary>
        /// <param name="serializer">등록할 Serializer</param>
        public static void RegisterSerializer(ISerializer serializer)
        {
            string key = serializer.Extension.ToLower();
            if (!serializers.ContainsKey(key))
            {
                serializers.Add(key, serializer);
                Debug.Log($"Serializer 등록됨: {serializer.Extension}");
            }
            else
            {
                Debug.LogWarning($"이미 등록된 Serializer 형식입니다: {serializer.Extension}");
            }
        }

        /// <summary>
        /// 지정된 확장자에 해당하는 Serializer를 반환합니다.
        /// </summary>
        /// <param name="extension">Serializer 형식의 확장자 (예: "json")</param>
        /// <returns>해당하는 ISerializer 인스턴스 또는 null</returns>
        public static ISerializer GetSerializer(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                Debug.LogError("확장자가 유효하지 않습니다.");
                return null;
            }

            extension = extension.ToLower();

            if (serializers.TryGetValue(extension, out ISerializer serializer))
            {
                return serializer;
            }

            Debug.LogError($"지원되지 않는 Serializer 형식입니다: {extension}");
            return null;
        }
    }
}
