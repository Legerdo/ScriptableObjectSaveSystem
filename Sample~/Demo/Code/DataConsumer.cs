using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace ScriptableObjectSaveSystem
{
    /// <summary>
    /// SimpleData를 사용하고 저장 및 로드하는 예제 클래스입니다.
    /// </summary>
    public class DataConsumer : MonoBehaviour
    {
        [SerializeField] SimpleData simpleData;

        [SerializeField]
        private PlayerStats playerStats;

        [SerializeField]
        private List<ScriptableObject> listStats;

        [SerializeField] SimpleData loadSimpleData;

        void Awake()
        {
            KeyManager.Initialize();            
        }

        public void SaveData1()
        {
            var saveDataIns = Instantiate(simpleData);        
            saveDataIns.dataDic.Add("TEST", "TEST1");

            saveDataIns.Save("SimpleData_JSON", SerializerManager.GetSerializer("NewtonsoftJson"));
        }

        public void LoadData1()
        {
            var saveDataIns = ScriptableObject.CreateInstance<SimpleData>();  

            saveDataIns.Load("SimpleData_JSON", SerializerManager.GetSerializer("NewtonsoftJson"));

            if ( saveDataIns.dataDic.ContainsKey("TEST"))
            {
                Debug.Log("Dic OK = " + saveDataIns.dataDic["TEST"]);
            }

            loadSimpleData = saveDataIns;
        }

        public void SaveData2()
        {
            playerStats.Save("PlayerStats_JSON");
        }

        public void LoadData2()
        {
            playerStats.Load("PlayerStats_JSON");
        }

        public void SaveDataAll()
        {   
            listStats.SaveAll("ListStats_JSON");
        }

        public void LoadDataAll()
        {
            listStats.LoadAll("ListStats_JSON");
        }
    }
}


