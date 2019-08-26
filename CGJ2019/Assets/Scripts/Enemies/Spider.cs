using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider :MonoBehaviour
{
    Rigidbody2D rbody;

    Vector3 OriginalPosition;

    bool OnGround;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        
    }
    private void Update()
    {
        if (!OnGround) {
            StartCoroutine("DropDown");
        }
        if (OnGround) {
            rbody.velocity = new Vector3(0, 1, 0) * speed * Time.deltaTime;
        }
    }
    //spider drops down
    IEnumerator DropDown() {
        yield return new WaitForSeconds(3.0f);
        OriginalPosition = transform.position;
        rbody.velocity = new Vector3(0, -1, 0) * speed * Time.deltaTime;
        StopAllCoroutines();
    }
    //spider goes back up
    //IEnumerator GoBackUp() {

        
    //    yield return new WaitForSeconds(3.0f);
    //    StopAllCoroutines();
    //    if (!OnGround)
    //    {
    //       StartCoroutine("DropDown");
    //    }
        
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            //PlayerDeath Functions
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            rbody.velocity = Vector3.zero;
            OnGround = true;
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            rbody.velocity = Vector3.zero;
            OnGround = false;
        }
    }


}
