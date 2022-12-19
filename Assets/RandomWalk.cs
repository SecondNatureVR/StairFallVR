using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent _navmeshAgent;
    
    // Start is called before the first frame update
    void Awake()
    {
        _navmeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
       if (_navmeshAgent.enabled == false)  {
        return;
       }
       if (_navmeshAgent.hasPath == false || _navmeshAgent.remainingDistance < 1f)
        ChooseNewPosition();
    }

    private void ChooseNewPosition() {
        Vector3 randomOffset = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10,10));
        var destination = transform.position + randomOffset;
        _navmeshAgent.SetDestination(destination);
    }
}
