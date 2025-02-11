using System.Collections;
using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    [SerializeField] private int maxHitsToSink = 5;
    [SerializeField] private int currentHits = 0;

    private ShipFloating shipFloatingScript; //Õ»◊≈√Œ ”ÃÕ≈≈ “Œ Õ≈ œ–»ƒ”Ã¿“‹!!!
    private bool isSinking = false;

    [SerializeField] private GameObject sinkingEffectPrefab;

    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float turnSpeed = 5f;

    private void Start()
    {
        shipFloatingScript = GetComponent<ShipFloating>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (transform.position == Vector3.zero)
        {
            transform.position = new Vector3(0f, 0f, 0f);
        }
    }

   private  void Update()
    {
        if (!isSinking && player != null)
        {
            FollowPlayer();
        }
    }

    public void TakeHit()
    {
        currentHits++;


        if (currentHits >= maxHitsToSink && !isSinking)
        {
            isSinking = true;
            SinkShip();
        }
    }

 
    private void SinkShip()
    {
        Debug.Log(" Ó‡·Î¸ ÚÓÌÂÚ!");

  
        if (sinkingEffectPrefab != null)
        {
            Instantiate(sinkingEffectPrefab, transform.position, Quaternion.identity);
        }


        if (shipFloatingScript != null)
        {
            shipFloatingScript.enabled = false;
        }


        StartCoroutine(DestroyShipAfterDelay(10f));

         IEnumerator DestroyShipAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            DestroyShip();
        }
    }

   private  void FollowPlayer()
    {

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }


    private void DestroyShip()
    {
        Destroy(gameObject);  
    }
}
