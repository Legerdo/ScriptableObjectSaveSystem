using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// ScriptableObject에 저장 및 로드 기능을 확장합니다.
    /// </summary>
    public static partial class ScriptableObjectExtensions
    {
        private static readonly string Extension = ".dat";

        /// <summary>
        /// ScriptableObject를 지정된 파일 이름으로 저장합니다. 기본적으로 JSON 형식을 사용합니다.
        /// 동일한 파일 이름이 존재할 경우 기존 파일을 덮어씁니다.
        /// </summary>
        /// <param name="obj">저장할 ScriptableObject</param>
        /// <param name="fileName">저장할 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        public static void Save(this ScriptableObject obj, string fileName, ISerializer serializer = null)
        {
            if (!ValidateScriptableObject(obj) || !TryGetSerializer(ref serializer))
                return;

            string path = GetFilePath(fileName);

            try
            {
                serializer.Serialize(obj, path);
            }
            catch (IOException e)
            {
                Debug.LogError($"파일 저장 중 오류 발생: {e.Message}");
            }
        }

        /// <summary>
        /// ScriptableObject를 지정된 파일 이름으로 비동기적으로 저장합니다. 기본적으로 JSON 형식을 사용합니다.
        /// 동일한 파일 이름이 존재할 경우 기존 파일을 덮어씁니다.
        /// </summary>
        /// <param name="obj">저장할 ScriptableObject</param>
        /// <param name="fileName">저장할 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        /// <returns>비동기 작업</returns>
        public static async Task SaveAsync(this ScriptableObject obj, string fileName, ISerializer serializer = null)
        {
            if (!ValidateScriptableObject(obj) || !TryGetSerializer(ref serializer))
                return;

            string path = GetFilePath(fileName);

            try
            {
                await serializer.SerializeAsync(obj, path);
            }
            catch (IOException e)
            {
                Debug.LogError($"파일 저장 중 오류 발생: {e.Message}");
            }
        }

        /// <summary>
        /// 지정된 파일 이름에서 ScriptableObject를 로드합니다. 기본적으로 JSON 형식을 사용합니다.
        /// </summary>
        /// <param name="obj">로드할 ScriptableObject</param>
        /// <param name="fileName">로드할 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        public static void Load(this ScriptableObject obj, string fileName, ISerializer serializer = null)
        {
            if (!ValidateScriptableObject(obj) || !TryGetSerializer(ref serializer))
                return;

            string path = GetFilePath(fileName);

            try
            {
                serializer.Deserialize(obj, path);
            }
            catch (IOException e)
            {
                Debug.LogError($"파일 로드 중 오류 발생: {e.Message}");
            }
        }

        /// <summary>
        /// 지정된 파일 이름에서 ScriptableObject를 비동기적으로 로드합니다. 기본적으로 JSON 형식을 사용합니다.
        /// </summary>
        /// <param name="obj">로드할 ScriptableObject</param>
        /// <param name="fileName">로드할 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        /// <returns>비동기 작업</returns>
        public static async Task LoadAsync(this ScriptableObject obj, string fileName, ISerializer serializer = null)
        {
            if (!ValidateScriptableObject(obj) || !TryGetSerializer(ref serializer))
                return;

            string path = GetFilePath(fileName);

            try
            {
                await serializer.DeserializeAsync(obj, path);
            }
            catch (IOException e)
            {
                Debug.LogError($"파일 로드 중 오류 발생: {e.Message}");
            }
        }

        /// <summary>
        /// ScriptableObject의 유효성을 검사합니다.
        /// </summary>
        private static bool ValidateScriptableObject(ScriptableObject obj)
        {
            if (obj == null)
            {
                Debug.LogError("ScriptableObject이 null입니다.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Serializer를 얻거나 기본 JSON Serializer를 설정합니다.
        /// </summary>
        private static bool TryGetSerializer(ref ISerializer serializer)
        {
            if (serializer == null)
            {
                serializer = SerializerManager.GetSerializer("JsonUtility");

                if (serializer == null)
                {
                    Debug.LogError("기본 JSON Serializer를 찾을 수 없습니다.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 파일 경로를 반환합니다.
        /// </summary>
        private static string GetFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, $"{fileName} + {Extension}");
        }

        // --- List<ScriptableObject> 확장 메서드 시작 ---

        /// <summary>
        /// ScriptableObject 리스트를 지정된 기본 파일 이름으로 일괄 저장합니다. 기본적으로 JSON 형식을 사용합니다.
        /// 각 ScriptableObject는 기본 파일 이름에 고유한 식별자를 추가하여 저장됩니다.
        /// </summary>
        /// <param name="objects">저장할 ScriptableObject 리스트</param>
        /// <param name="baseFileName">기본 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        public static void SaveAll(this List<ScriptableObject> objects, string baseFileName, ISerializer serializer = null)
        {
            if (objects == null || objects.Count == 0)
            {
                Debug.LogError("저장할 ScriptableObject 리스트가 비어있습니다.");
                return;
            }

            if (!TryGetSerializer(ref serializer))
                return;

            foreach (var obj in objects)
            {
                if (!ValidateScriptableObject(obj))
                    continue;

                string fileName = $"{baseFileName}_{obj.name}";
                string path = GetFilePath(fileName);

                try
                {
                    serializer.Serialize(obj, path);
                }
                catch (IOException e)
                {
                    Debug.LogError($"파일 '{fileName}' 저장 중 오류 발생: {e.Message}");
                }
            }
        }

        /// <summary>
        /// ScriptableObject 리스트를 지정된 기본 파일 이름으로 비동기적으로 일괄 저장합니다. 기본적으로 JSON 형식을 사용합니다.
        /// 각 ScriptableObject는 기본 파일 이름에 고유한 식별자를 추가하여 저장됩니다.
        /// </summary>
        /// <param name="objects">저장할 ScriptableObject 리스트</param>
        /// <param name="baseFileName">기본 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        /// <returns>비동기 작업</returns>
        public static async Task SaveAllAsync(this List<ScriptableObject> objects, string baseFileName, ISerializer serializer = null)
        {
            if (objects == null || objects.Count == 0)
            {
                Debug.LogError("저장할 ScriptableObject 리스트가 비어있습니다.");
                return;
            }

            if (!TryGetSerializer(ref serializer))
                return;

            List<Task> saveTasks = new List<Task>();

            foreach (var obj in objects)
            {
                if (!ValidateScriptableObject(obj))
                    continue;

                string fileName = $"{baseFileName}_{obj.name}";
                string path = GetFilePath(fileName);

                try
                {
                    saveTasks.Add(serializer.SerializeAsync(obj, path));
                }
                catch (IOException e)
                {
                    Debug.LogError($"파일 '{fileName}' 저장 중 오류 발생: {e.Message}");
                }
            }

            try
            {
                await Task.WhenAll(saveTasks);
                Debug.Log($"총 {saveTasks.Count}개의 ScriptableObject가 비동기적으로 저장되었습니다.");
            }
            catch (Exception e)
            {
                Debug.LogError($"일괄 저장 중 오류 발생: {e.Message}");
            }
        }

        /// <summary>
        /// ScriptableObject 리스트를 지정된 기본 파일 이름으로 일괄 로드합니다. 기본적으로 JSON 형식을 사용합니다.
        /// 각 ScriptableObject는 기본 파일 이름에 고유한 식별자를 추가하여 로드됩니다.
        /// </summary>
        /// <param name="objects">로드할 ScriptableObject 리스트</param>
        /// <param name="baseFileName">기본 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        public static void LoadAll(this List<ScriptableObject> objects, string baseFileName, ISerializer serializer = null)
        {
            if (objects == null || objects.Count == 0)
            {
                Debug.LogError("로드할 ScriptableObject 리스트가 비어있습니다.");
                return;
            }

            if (!TryGetSerializer(ref serializer))
                return;

            foreach (var obj in objects)
            {
                if (!ValidateScriptableObject(obj))
                    continue;

                string fileName = $"{baseFileName}_{obj.name}";
                string path = GetFilePath(fileName);

                try
                {
                    serializer.Deserialize(obj, path);
                }
                catch (IOException e)
                {
                    Debug.LogError($"파일 '{fileName}' 로드 중 오류 발생: {e.Message}");
                }
            }
        }

        /// <summary>
        /// ScriptableObject 리스트를 지정된 기본 파일 이름으로 비동기적으로 일괄 로드합니다. 기본적으로 JSON 형식을 사용합니다.
        /// 각 ScriptableObject는 기본 파일 이름에 고유한 식별자를 추가하여 로드됩니다.
        /// </summary>
        /// <param name="objects">로드할 ScriptableObject 리스트</param>
        /// <param name="baseFileName">기본 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        /// <returns>비동기 작업</returns>
        public static async Task LoadAllAsync(this List<ScriptableObject> objects, string baseFileName, ISerializer serializer = null)
        {
            if (objects == null || objects.Count == 0)
            {
                Debug.LogError("로드할 ScriptableObject 리스트가 비어있습니다.");
                return;
            }

            if (!TryGetSerializer(ref serializer))
                return;

            List<Task> loadTasks = new List<Task>();

            foreach (var obj in objects)
            {
                if (!ValidateScriptableObject(obj))
                    continue;

                string fileName = $"{baseFileName}_{obj.name}";
                string path = GetFilePath(fileName);

                try
                {
                    loadTasks.Add(serializer.DeserializeAsync(obj, path));
                }
                catch (IOException e)
                {
                    Debug.LogError($"파일 '{fileName}' 로드 중 오류 발생: {e.Message}");
                }
            }

            try
            {
                await Task.WhenAll(loadTasks);
                Debug.Log($"총 {loadTasks.Count}개의 ScriptableObject가 비동기적으로 로드되었습니다.");
            }
            catch (Exception e)
            {
                Debug.LogError($"일괄 로드 중 오류 발생: {e.Message}");
            }
        }

      /// <summary>
        /// 지정된 파일 이름에서 ScriptableObject의 JSON 데이터를 문자열로 로드합니다.
        /// </summary>
        /// <param name="fileName">로드할 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        /// <returns>로드된 JSON 문자열</returns>
        public static string LoadToString(this ScriptableObject obj, string fileName, ISerializer serializer = null)
        {
            if (!ValidateScriptableObject(obj) || !TryGetSerializer(ref serializer))
                return null;

            string path = GetFilePath(fileName);

            try
            {
                return serializer.DeserializeToString(path);
            }
            catch (IOException e)
            {
                Debug.LogError($"파일 로드 중 오류 발생: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 지정된 기본 파일 이름에서 ScriptableObject 리스트의 JSON 데이터를 문자열로 일괄 로드합니다.
        /// </summary>
        /// <param name="objects">로드할 ScriptableObject 리스트</param>
        /// <param name="baseFileName">기본 파일 이름 (확장자 제외)</param>
        /// <param name="serializer">사용할 Serializer (기본값: JSON)</param>
        /// <returns>로드된 JSON 문자열 리스트</returns>
        public static List<string> LoadAllToString(this List<ScriptableObject> objects, string baseFileName, ISerializer serializer = null)
        {
            if (objects == null || objects.Count == 0)
            {
                Debug.LogError("로드할 ScriptableObject 리스트가 비어있습니다.");
                return null;
            }

            if (!TryGetSerializer(ref serializer))
                return null;

            List<string> jsonDataList = new List<string>();

            foreach (var obj in objects)
            {
                if (!ValidateScriptableObject(obj))
                    continue;

                string fileName = $"{baseFileName}_{obj.name}";
                string path = GetFilePath(fileName);

                try
                {
                    string jsonData = serializer.DeserializeToString(path);
                    jsonDataList.Add(jsonData);
                }
                catch (IOException e)
                {
                    Debug.LogError($"파일 '{fileName}' 로드 중 오류 발생: {e.Message}");
                }
            }

            return jsonDataList;
        }        
    }
}