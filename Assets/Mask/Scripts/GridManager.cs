using UnityEngine;
using System.Collections.Generic;

namespace XingXing.GlobalGameJam.Y2026
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private string m_Header;
        [SerializeField] private bool m_Generator = false;
        [Space(15f)]
        [Header("Grid Size")]
        [SerializeField, Min(1)] private int m_Width = 10;
        [SerializeField, Min(1)] private int m_Height = 10;

        [Header("Block Settings")]
        [SerializeField] private GameObject m_BlockPrefab;
        [SerializeField] private Transform m_Parent;
        [SerializeField, Min(1)] private float m_Spacing = 1f;

        private GameObject[,] _grid;
        private List<GameObject> _blocks = new();
        private void OnValidate()
        {
            if (m_Generator)
            {
                GenerateGrid();
                m_Generator = false;
            }
        }

        private void GenerateGrid()
        {
            _grid = new GameObject[m_Width, m_Height];

            for (int x = 0; x < m_Width; x++)
            {
                for (int z = 0; z < m_Height; z++)
                {
                    Vector3 pos = new Vector3(
                        x * m_Spacing,
                        0,
                        z * m_Spacing
                    );

                    GameObject block = Instantiate(
                        m_BlockPrefab,
                        pos,
                        Quaternion.identity,
                        m_Parent ?? transform
                    );

                    _blocks.Add(block);
                    block.SetActive(true);

                    block.name = $"{m_Header} Block_{x}_{z}";
                    _grid[x, z] = block;
                }
            }
        }
    }
}
