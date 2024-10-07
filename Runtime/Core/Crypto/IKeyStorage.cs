using System;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// 암호화 키와 IV를 저장하고 불러오는 기능을 정의한 인터페이스입니다.
    /// 다양한 저장 방식을 지원하기 위해 인터페이스로 추상화하였습니다.
    /// </summary>
    public interface IKeyStorage
    {
        /// <summary>
        /// 암호화 키와 IV를 저장합니다.
        /// </summary>
        /// <param name="key">AES 키</param>
        /// <param name="iv">AES IV</param>
        void SaveKey(byte[] key, byte[] iv);

        /// <summary>
        /// 저장된 암호화 키와 IV를 불러옵니다.
        /// </summary>
        /// <param name="key">불러온 AES 키를 반환합니다.</param>
        /// <param name="iv">불러온 AES IV를 반환합니다.</param>
        /// <returns>저장된 키와 IV가 존재하면 true, 그렇지 않으면 false</returns>
        bool LoadKey(out byte[] key, out byte[] iv);

        /// <summary>
        /// 저장된 암호화 키와 IV를 삭제합니다.
        /// </summary>
        void DeleteKey();
    }
}
