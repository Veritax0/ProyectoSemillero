using System.Collections;
using UnityEngine;

public class SonarSpawner : MonoBehaviour
{
    public GameObject scanner;
    public Transform player;
    public float sonarTimeInterval;
    
    private GameObject _activeScanner;
    private bool _isSpawn;

    void Update()
    {
        Scan();
    }

    private void Scan()
    {
        if (!_isSpawn && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(SpawnSonar());
        }
    }

    private IEnumerator SpawnSonar()
    {
        _isSpawn = true;
        _activeScanner = Instantiate(scanner, player);
        yield return new WaitForSeconds(sonarTimeInterval);
        _isSpawn = false;
    }
}
