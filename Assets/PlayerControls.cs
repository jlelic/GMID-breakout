using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float speed = 0.2f;

    // Use this for initialization
    void Start()
    {
    }

    void OnCollisionEnter(Collision col)
    {
        var tag = col.gameObject.tag;
        if (tag == "Wall")
        {
            //   transform.position = _lastPosition;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var pos = transform.position;
        int x = 0, y = 0;
        if (Input.GetKey(KeyCode.UpArrow) && pos.y < 7f)
        {
            y++;
        }
        if (Input.GetKey(KeyCode.DownArrow) && pos.y > 0.5f)
        {
            y--;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && pos.x > -7f)
        {
            x--;
        }
        if (Input.GetKey(KeyCode.RightArrow) && pos.x < 7f)
        {
            x++;
        }
        transform.position += new Vector3(x, y, 0) * speed * Time.timeScale;
    }

    public void ShrinkPlayer()
    {
        StartCoroutine(ShrinkPlayerCoroutine());
    }

    public void EnlargePlayer()
    {
        StartCoroutine(EnlargePlayerCoroutine());
    }

    private IEnumerator ShrinkPlayerCoroutine()
    {
        float stepSize = 0.035f;
        int stepCount = 20;
        for (int i = 0; i < stepCount; i++)
        {
            var s = transform.localScale;
            gameObject.transform.localScale = new Vector3(
                s.x - stepSize,
                s.y,
                s.z
            );
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(10);

        for (int i = 0; i < stepCount; i++)
        {
            var s = transform.localScale;
            gameObject.transform.localScale = new Vector3(
                s.x + stepSize,
                s.y,
                s.z
            );
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator EnlargePlayerCoroutine()
    {
        float stepSize = 0.04f;
        int stepCount = 20;
        for (int i = 0; i < stepCount; i++)
        {
            var s = transform.localScale;
            gameObject.transform.localScale = new Vector3(
                s.x + stepSize,
                s.y,
                s.z
            );
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(20);

        for (int i = 0; i < stepCount; i++)
        {
            var s = transform.localScale;
            gameObject.transform.localScale = new Vector3(
                s.x - stepSize,
                s.y,
                s.z
            );
            yield return new WaitForSeconds(0.05f);
        }
    }
}