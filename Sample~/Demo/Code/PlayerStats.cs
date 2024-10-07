using UnityEngine;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// 플레이어의 스탯을 저장하는 ScriptableObject 클래스입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 2)]
    public class PlayerStats : ScriptableObject
    {
        [Tooltip("플레이어의 이름")]
        public string playerName;

        [Tooltip("플레이어의 체력")]
        public int health;

        [Tooltip("플레이어의 경험치")]
        public int experience;
    }
}
