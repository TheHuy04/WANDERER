using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] Transform diemDichChuyen;
    
    public Transform GetDiemDichChuyen()
    {
        return diemDichChuyen;
    }
}
