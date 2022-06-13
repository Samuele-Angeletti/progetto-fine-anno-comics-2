using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavableEntity : MonoBehaviour
{
    [SerializeField] string id = string.Empty;

    public string Id => id;

    [ContextMenu("Generate Id")]
    private void GenerateId() => id = Guid.NewGuid().ToString();
}
