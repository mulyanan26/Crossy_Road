using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] GameObject titlePrefab;
    protected int horizontalSize;

    // private void Start()
    // {
    //     Generate(9);
    // }

    public virtual void Generate(int size)
    {
        horizontalSize = size;

        if (size == 0)
        {
            return;
        }
        if ((float)size % 2 == 0)
        {
            size -= 1;
        }

        int moveLimit = Mathf.FloorToInt((float)size / 2);
        for (int i = -moveLimit; i <= moveLimit; i++)
        {
            SpawnTile(i);
        }

        //untuk menggelapkan sisi kiri dan kanan tile
        var leftBoundaryTile = SpawnTile(-moveLimit - 1);
        var rightBoundaryTile = SpawnTile(moveLimit + 1);
        DarkenObject(leftBoundaryTile);
        DarkenObject(rightBoundaryTile);
    }

    private GameObject SpawnTile(int xPos)
    {
        var go = Instantiate(
                        titlePrefab,
                        transform
                    );
        go.transform.localPosition = new Vector3(xPos, 0, 0);

        return go;
    }

    private void DarkenObject(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<MeshRenderer>(true);
        foreach (var rend in renderers)
        {
            rend.material.color = rend.material.color * Color.grey;
        }
    }
}
