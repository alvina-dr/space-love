using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    Vector3 direction;
    [SerializeField] private Rigidbody2D rb2d;
    void Start()
    {
        
    }

    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
    }

    private void FixedUpdate()
    {
        rb2d.velocity = direction * Time.deltaTime * 200;
    }
}
