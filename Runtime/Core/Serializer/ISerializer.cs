using System.Threading.Tasks;
using UnityEngine;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// 모든 Serializer가 구현해야 하는 인터페이스입니다.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 직렬화 형식의 파일 확장자 (예: "json")
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// ScriptableObject를 지정된 경로에 직렬화하여 저장합니다.
        /// </summary>
        /// <param name="obj">저장할 ScriptableObject</param>
        /// <param name="path">저장할 파일의 전체 경로</param>
        void Serialize(ScriptableObject obj, string path);

        /// <summary>
        /// ScriptableObject를 지정된 경로에 비동기적으로 직렬화하여 저장합니다.
        /// </summary>
        /// <param name="obj">저장할 ScriptableObject</param>
        /// <param name="path">저장할 파일의 전체 경로</param>
        Task SerializeAsync(ScriptableObject obj, string path);

        /// <summary>
        /// 지정된 경로에서 ScriptableObject를 역직렬화하여 로드합니다.
        /// </summary>
        /// <param name="obj">로드할 ScriptableObject</param>
        /// <param name="path">로드할 파일의 전체 경로</param>
        void Deserialize(ScriptableObject obj, string path);

        /// <summary>
        /// 지정된 경로에서 ScriptableObject를 비동기적으로 역직렬화하여 로드합니다.
        /// </summary>
        /// <param name="obj">로드할 ScriptableObject</param>
        /// <param name="path">로드할 파일의 전체 경로</param>
        Task DeserializeAsync(ScriptableObject obj, string path);

        /// <summary>
        /// 지정된 경로에서 JSON 문자열을 역직렬화하여 반환합니다.
        /// </summary>
        /// <param name="path">로드할 파일의 전체 경로</param>
        /// <returns>파일에서 로드된 JSON 문자열</returns>
        string DeserializeToString(string path);        
    }
}
