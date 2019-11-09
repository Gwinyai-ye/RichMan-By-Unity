using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class UIPanelInfo :ISerializationCallbackReceiver {
    [NonSerialized]
    public UIPanelType panelType;
    public string panelTypeString;

    public string path;

    // 反序列化   从文本信息 到对象
    public void OnAfterDeserialize()
    {
        Debug.Log("Love");
        UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
        panelType = type;
    }

    public void OnBeforeSerialize()
    {
        
    }
}