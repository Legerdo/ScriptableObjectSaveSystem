using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// 간단한 데이터를 저장하는 ScriptableObject 클래스입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SimpleData", order = 1)]
    public class SimpleData : ScriptableObject
    {
            [Tooltip("데이터의 이름")]
            public string dataName;

            [Tooltip("데이터의 값")]
            public int dataValue;

            public Dictionary<string, string> dataDic = new Dictionary<string,string>();
    }
}
