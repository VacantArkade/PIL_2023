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

    [SerializeField]
    int points = 5;

    [SerializeField]
    float amplitude = 1;

    [SerializeField]
    float waveSpeed = 1;

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

        //line.SetPosition(1, endPoint);

        Draw(endPoint);

        particles.transform.position = endPoint;
        
    }

    void Draw(Vector3 endPoint)
    {
        float start = transform.position.y;
        float finish = endPoint.y;

        //line.SetPosition(0, transform.position);

        line.positionCount = points + 1;
        for (int currentPoint = 1; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float y = Mathf.Lerp(start, finish, progress);
            float x = amplitude * Mathf.Sin(y + (Time.timeSinceLevelLoad * waveSpeed));
            line.SetPosition(currentPoint, new Vector3(transform.position.x + x, y, 0));
        }

        //line.SetPosition(0, transform.position);
        line.SetPosition(line.positionCount - 1, endPoint);
    }
}
