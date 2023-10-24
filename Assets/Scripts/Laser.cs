using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : InteractableObject
{
    [SerializeField]
    LineRenderer line;

    [SerializeField]
    LayerMask mask;

    [SerializeField]
    GameObject particles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, line.transform.position);

        RaycastHit2D hit = Physics2D.Raycast(line.transform.position, line.transform.up * -1, 100, mask);

        var endPoint = line.transform.position + line.transform.up * -100;

        if (hit)
        {
            endPoint = hit.point;
            var player = hit.transform.gameObject.GetComponent<PlayerLogic>();
            if (player != null)
            {
                Debug.Log("HIT PLAYER");
                player.DeathByLaser();
            }
        }

        line.SetPosition(1, endPoint);
        particles.transform.position = endPoint;
        
    }
}
