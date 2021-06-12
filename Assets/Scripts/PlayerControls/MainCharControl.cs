using System;
using System.Collections;
using UnityEngine;

public class MainCharControl : MonoBehaviour
{
    float xValAdj;
    float yValAdj;
    public float moveSpeed;

    public static Vector3 mainCharPos;
    [SerializeField] GameObject explosionObj;

    //Input actions
    PlayerInputAction inputAction;
    Vector2 movementInput;

    public event Action OnDeath;

    void Awake()
    {
        inputAction = new PlayerInputAction();
        inputAction.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
    }

    void Update()
    {
        //Get input
        xValAdj = movementInput.x;
        yValAdj = movementInput.y;

        //Move character
        Vector3 position = transform.position;
        position.x = position.x + moveSpeed * xValAdj * Time.deltaTime;
        position.y = position.y + moveSpeed * yValAdj * Time.deltaTime;
        transform.position = position;

        mainCharPos = transform.position;
    }

    //Enable and disable the input action
    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyShot") || other.CompareTag("Enemy"))
        {
            //Play explosion
            if (explosionObj is GameObject)
            {
                Instantiate(explosionObj, transform.position, explosionObj.transform.rotation);
            }
            AudioManager.Instance.Play("PlayerDeath");

            StartCoroutine(LateDeathInvoke());

            GetComponent<CircleCollider2D>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    IEnumerator LateDeathInvoke()
    {
        yield return new WaitForSeconds(0.5f);

        OnDeath?.Invoke();

        // Destroy the player object
        Destroy(gameObject);
    }
}
