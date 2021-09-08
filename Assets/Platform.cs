using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    #region Paras
    [Header("Attributes")]
    public bool canDown;
    public bool canUp;

    private Collider2D coll;
    #endregion

    #region Sys funcs
    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }
    #endregion

    public void IgnoreCollision(Collider2D theColl)
    {
        if(!canDown && !canUp)
        {
            Debug.LogError("Error");
            return;
        }
        Physics2D.IgnoreCollision(theColl, coll,true);
    }

    public void restoreCollision(Collider2D theColl)
    {
        Physics2D.IgnoreCollision(theColl, coll,false);
    }
}
