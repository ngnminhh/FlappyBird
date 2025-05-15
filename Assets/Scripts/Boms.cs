using UnityEngine;

public class Boms : MonoBehaviour
{
    
    public float speed = 5f;
    

    private float leftEdge;
    private bool hasScored = false;


    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
        
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
    
        }

        //if (!hasScored && transform.position.x < GameObject.FindGameObjectWithTag("Player").transform.position.x)
        //{
        //    FindObjectOfType<GameManager>().IncreaseScore();
        //    hasScored = true;
        //}

    }



}
