using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenFixTrigger : MonoBehaviour
{
    public Sprite fixedSprite;
    public float updateDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DelayedSpriteUpdate(float delay)
    {
        yield return new WaitForSeconds(delay);

        GetComponent<SpriteRenderer>().sprite = fixedSprite;
        GetComponent<Collider2D>().isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DynamicTile"))
        {
            Object.Destroy(other.gameObject, updateDelay);
            StartCoroutine(DelayedSpriteUpdate(updateDelay));
        }
    }
}
