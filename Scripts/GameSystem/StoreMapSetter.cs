using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreMapSetter : MonoBehaviour
{
    [SerializeField] private KitchenInteraction[] TeaPotsObj;
    [SerializeField] private KitchenInteraction[] CounterTopObj;
    [SerializeField] private KitchenInteraction[] SteamerObj;
    [SerializeField] private KitchenInteraction[] PotObj;
    [SerializeField] private GameObject IceMaker;
    [SerializeField] private GameObject[] IceMakerTable;
    [SerializeField] private KitchenInteraction Mortal;
    
    [SerializeField] private GameObject[] TeapotsTables;
    [SerializeField] private GameObject[] CounterTables;
    
    private PlayerData playerData;
    private GameManager gameManager;
    public void Start()
    {
        gameManager = GameManager.Instance;
        playerData = gameManager.GetPlayerData();
        CheckValiable();
        ApplyUpgradeContent();
    }

    

    private void CheckValiable()
    {
        Debug.Assert(gameManager,"DataManager를 받아올 수 없습니다.");
        Debug.Assert(playerData != null,"PlayerData를 받아올 수 없었습니다");
        Debug.Assert(TeaPotsObj != null && TeaPotsObj.Length >0,"위치된 티폿의 정보를 받아 올 수 없습니다.");
        Debug.Assert(CounterTopObj != null && CounterTopObj.Length >0,"위치된 가판대의 정보들을 받아올 수 없습니다.");
        Debug.Assert(SteamerObj != null && SteamerObj.Length >0,"위치된 찜기의 정보들을 받아올 수 없습니다.");
        Debug.Assert(PotObj != null && PotObj.Length >0,"위치된 냄비의 정보들을 받아올 수 없습니다");
        Debug.Assert(TeapotsTables != null && TeapotsTables.Length >0,"위치된 티폿테이블의 정보들을 받아올 수 없습니다");
        Debug.Assert(CounterTables != null && CounterTables.Length >0,"위치된 조리대테이블의 정보들을 받아올 수 없습니다");
        Debug.Assert(IceMaker != null,"위치된 얼음기계의 정보들을 받아올 수 없습니다");
        Debug.Assert(Mortal != null,"위치된 절구의 정보들을 받아올 수 없습니다");
    }

    private void ApplyUpgradeContent()
    {
        IceMaker.SetActive(false);
        Mortal.gameObject.SetActive(false);
        List<KitchenUtensilInfoData> utensil = playerData.GetInventory<KitchenUtensilInfoData>();
        for (int i = 0; i < utensil.Count; ++i)
        {
            KitchenInteraction[] applyList = null;
            GameObject[] table = null;
            int level = utensil[i].Level;
            bool isUpOne = utensil[i].Level > 0;
            if (utensil[i].DefaultData.ID == 4)
            {
                applyList = TeaPotsObj;
                table = TeapotsTables;
                if (IceMaker != null)
                {
                    IceMaker.gameObject.SetActive(isUpOne);
                    for (int j = 0; j < IceMakerTable.Length; ++j)
                    {
                        IceMakerTable[j].SetActive(!isUpOne);
                    }
                }
                
            }else if (utensil[i].DefaultData.ID == 3)
            {
                applyList = CounterTopObj;
                table = CounterTables;
            }else if (utensil[i].DefaultData.ID == 0)
            {
                applyList = SteamerObj;
            }else if (utensil[i].DefaultData.ID == 1)
            {
                applyList = PotObj;
            }else if (utensil[i].DefaultData.ID == 2)
            {
                if(IceMaker !=null)
                    Mortal.gameObject.SetActive(isUpOne);
            }

            int count;
            if (level > 0)
                count = utensil[i].DefaultData.QuantityUpgradeInfo[level - 1];
            else
                count = 0;
            
            if(applyList != null)
                ApplyLevelToSpawn(applyList,table,count);
        }

       
    }

    private void ApplyLevelToSpawn(KitchenInteraction[] utensil, GameObject[] table, int count)
    {
        int i = 0;
        for (; i < count; ++i)
        {
            utensil[i].gameObject.SetActive(true);
        }

        if (table != null)
        {
            for (; i < table.Length; ++i)
            {
                table[i].gameObject.SetActive(true);   
            }
        }
    }
    
}
