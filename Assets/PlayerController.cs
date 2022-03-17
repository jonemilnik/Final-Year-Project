using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{

    //public IEnumerator MoveTo(GameObject agent, GameObject waypoint)
    //{
    //    yield return agent.GetComponent<NavMeshAgent>().SetDestination(waypoint.transform.position);

    //}

    public IEnumerator MoveTo(GameObject character, GameObject destination)
    {
        character.transform.LookAt(destination.transform);

        while (Vector3.Distance(character.transform.position, destination.transform.position) > 0.1f)
        {
            character.transform.position = Vector3.Lerp(character.transform.position, destination.transform.position, 12 * Time.deltaTime);
            yield return null;
        }
    }


}
