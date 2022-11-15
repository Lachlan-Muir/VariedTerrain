using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Board dimensions
    int b_width = 10;
    int b_height = 10;

    [SerializeField]
    GameObject hex_obj;

    // Start is called before the first frame update
    void Start()
    {
        float hex_inner_radius = hex_obj.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2;
        print(hex_inner_radius);
        float hex_outer_radius = hex_obj.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2;
        print(hex_outer_radius);

        for (int row = 0; row < b_width; row++)
        {
            float y = 1.5f * hex_outer_radius * row;
            for (int col = 0; col < b_height; col++)
            {
                float x = hex_inner_radius * 2 * col + (row % 2) * hex_inner_radius - 0.01f * col;
                Instantiate(hex_obj, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

}
