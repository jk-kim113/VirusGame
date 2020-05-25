using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMaker : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private GameObject mEquipMakerUI;
    [SerializeField]
    private Transform mScrollTarget;
    [SerializeField]
    private CombinationElement mElement;
#pragma warning restore

    private Equip[] mEquipArr;

    private void Awake()
    {
        mEquipArr = new Equip[1];

        mEquipArr[0] = new Equip();
        mEquipArr[0].Name = "비커";
        mEquipArr[0].ID = 0;
        mEquipArr[0].Info = "혈액 채취를 하기 위해 필요한 도구";
    }

    private void Start()
    {
        //int[] needNum = new int[4];
        //needNum[0] = 3;
        //needNum[1] = 0;
        //needNum[2] = 0;
        //needNum[3] = 0;
        //int[] needID = new int[4];
        //needID[0] = 1001;
        //needID[1] = 0;
        //needID[2] = 0;
        //needID[3] = 0;

        //for (int i = 0; i < mEquipArr.Length; i++)
        //{
        //    CombinationElement element = Instantiate(mElement, mScrollTarget);
        //    element.Init(null, mEquipArr[i].ID, mEquipArr[i].Name, mEquipArr[i].Info, needNum, needID, null);
        //}
    }

}

public class Equip
{
    public string Name;
    public int ID;
    public string Info;
}