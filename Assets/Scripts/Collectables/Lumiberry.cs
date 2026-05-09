using System;
using UnityEngine;

public class Lumiberry : BaseCollectable
{
    protected override void OnCollected()
    {
        GameManager.Instance.AddLumiberry();
    }
}