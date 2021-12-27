using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBehavior : MonoBehaviour
{
    [SerializeField] private Transform chunkToLoad;
    [SerializeField] private Transform chunkToUnload;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(chunkToLoad !=null)
            chunkToLoad.gameObject.SetActive(!chunkToLoad.gameObject.active);
        if(chunkToUnload != null)
            chunkToUnload.gameObject.SetActive(!chunkToLoad.gameObject.active);
    }

}
