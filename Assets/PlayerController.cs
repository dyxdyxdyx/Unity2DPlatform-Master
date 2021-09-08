using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Paras
    [Header("Move")]
    public float moveSpeed;

    [Space]
    [Header("Jump")]
    public int maxJumpCount = 2;
    private int jumpCount;
    public float jumpForce = 12f;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 8f;

    [SerializeField] private Vector2 dir;
    [SerializeField] private bool isJumpPressed;
    [SerializeField] private bool isJumpPressedDown;
    [SerializeField] private bool isDownPressedDown;
    [SerializeField] private bool isRising;
    [SerializeField] private bool isFalling;
    [SerializeField] private Platform bottomPlatformToIgnore;
    [SerializeField] private Platform upperPlatformToIgnore;

    private Rigidbody2D rb;
    private Collider2D[] colls;
    private CollisionCheck collCheck;

    #endregion

    #region Sys Funcs
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collCheck = GetComponent<CollisionCheck>();
        colls = GetComponents<Collider2D>();
        if(colls.Length == 0)
        {
            Debug.LogError("No Collision");
            Application.Quit();
        }

        jumpCount = maxJumpCount;
    }
    private void Update()
    {
        //Input
        dir = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        isJumpPressed = Input.GetButton("Jump");
        isJumpPressedDown = Input.GetButtonDown("Jump");
        isDownPressedDown = dir.y < 0;

        //Movement AND Jump
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
        isRising = rb.velocity.y > 0;
        isFalling = rb.velocity.y < 0;

        if (collCheck.OnPlatform)
        {
            jumpCount = maxJumpCount;
        }
        if (isJumpPressedDown && jumpCount > 0)
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //Fall
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !isJumpPressed)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }

        //Platform
        HandlePlatform();
    }

    private void HandlePlatform()
    {
        //Ignore
        if (isDownPressedDown && collCheck.OnPlatform && !isFalling)
        {
            GameObject platform = collCheck.GetBottomPlaform();
            bottomPlatformToIgnore = platform.GetComponent<Platform>();
            if (platform == null || !bottomPlatformToIgnore.canDown) return;
            for (int i = 0; i < colls.Length; i++) bottomPlatformToIgnore.IgnoreCollision(colls[i]);
        }
        if (isRising && collCheck.UnderPlatform)
        {
            GameObject platform = collCheck.GetUpperPlaform();
            upperPlatformToIgnore = platform.GetComponent<Platform>();
            if (platform == null || !upperPlatformToIgnore.canUp) return;
            for (int i = 0; i < colls.Length; i++) upperPlatformToIgnore.IgnoreCollision(colls[i]);
        }

        //Restore
        if (!collCheck.OnPlatform)
        {
            if(bottomPlatformToIgnore != null && 
                bottomPlatformToIgnore.gameObject != collCheck.GetInnerPlatform())
            {
                for (int i = 0; i < colls.Length; i++) 
                    bottomPlatformToIgnore.restoreCollision(colls[i]);
                bottomPlatformToIgnore = null;
            }
        }   
        if (!collCheck.UnderPlatform)
        {
            if(upperPlatformToIgnore != null &&
                upperPlatformToIgnore.gameObject != collCheck.GetInnerPlatform())
            {
                for (int i = 0; i < colls.Length; i++) 
                    upperPlatformToIgnore.restoreCollision(colls[i]);
                upperPlatformToIgnore = null;
            }
        }

    }

    #endregion
}
