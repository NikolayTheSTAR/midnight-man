using System;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
using UnityEngine.Purchasing;

[CreateAssetMenu(menuName = "Data/Shop", fileName = "ShopConfig")]
public class ShopConfig : ScriptableObject
{
    public ProductData[] products;
}

[Serializable]
public struct ProductData
{
    public ProductCostType costType;
    //[LabelText("costValue ($)")]
    public float costValue;

    public ShopRewardType rewardType;

    //[ShowIf("@costType == ProductCostType.Real")]
    public string SKU;

    //[ShowIf("@costType == ProductCostType.Real")]
    public ProductType ProductType;
}

public enum ProductCostType
{
    Real
}

public enum ShopRewardType
{
    AdsFree
}