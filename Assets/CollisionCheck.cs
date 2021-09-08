using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    #region Paras
    [Header("Platform")]
    public LayerMask whatIsPlatform;

    [Space]
    [Header("Boolean")]
    [SerializeField] private bool _OnPlatform;
    public bool OnPlatform => (_OnPlatform);
    [SerializeField] private bool _UnderPlatform;
    public bool UnderPlatform => (_UnderPlatform);

    [Space]
    [Header("Collision")]
    public float collisionRadius = 0.1f;
    public Vector2 bottomOffset;
    public Vector2 upOffset;

    public Vector2 bodySize;
    public Vector2 bodyOffset;
    #endregion

    #region Sys Funcs
    private void Update()
    {
        _OnPlatform = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, whatIsPlatform);
        _UnderPlatform = Physics2D.OverlapCircle((Vector2)transform.position + upOffset, collisionRadius,whatIsPlatform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + upOffset, collisionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + bodyOffset, bodySize);
    }
    #endregion

    public GameObject GetBottomPlaform()
    {
        if (!_OnPlatform)
        {
            Debug.LogWarning("Warning");
            return null;
        }
        return Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, whatIsPlatform).gameObject;
    }

    public GameObject GetUpperPlaform()
    {
        if (!_UnderPlatform)
        {
            Debug.LogWarning("Warning");
            return null;
        }
        return Physics2D.OverlapCircle((Vector2)transform.position + upOffset, collisionRadius, whatIsPlatform).gameObject;
    }

    public GameObject GetInnerPlatform()
    {
        Collider2D platform = Physics2D.OverlapBox((Vector2)transform.position + bodyOffset,bodySize,0,whatIsPlatform);
        if (platform == null) return null;
        return platform.gameObject;
    }
}
