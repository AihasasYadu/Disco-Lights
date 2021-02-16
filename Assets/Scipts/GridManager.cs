using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridManager : MonoSingletonGeneric<GridManager>
{
    [SerializeField] private Dropdown rowsDD;
    [SerializeField] private Dropdown colsDD;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private RectTransform gridPanelObj;
    [SerializeField] private RectTransform sizeSelectionPanel;
    [SerializeField] private Button enterButton;

    private const int MIN_COL = 4, MAX_COL = 10, MIN_SPACING = 50;
    private GridLayoutGroup gridProperties;
    private Button[,] grid;
    private int rows;
    private int colunms;
    void Start()
    {
        gridProperties = gridPanelObj.GetComponent<GridLayoutGroup>();
        enterButton.onClick.AddListener(SetSize);
        EventManager.DisableInteractability += CheckForGameCompletion;
        SetupDropDown();
    }

    private void SetupDropDown()
    {
        rowsDD.options.Add(new Dropdown.OptionData() { text = "None" });
        colsDD.options.Add(new Dropdown.OptionData() { text = "None" });
        for (int i = 2; i <= 10; i++)
        {
            rowsDD.options.Add(new Dropdown.OptionData() { text = i.ToString() });
            colsDD.options.Add(new Dropdown.OptionData() { text = i.ToString() });
        }
        GenerateGrid();
    }

    private void SetSize()
    {
        if(!rowsDD.value.Equals(0) && !colsDD.value.Equals(0))
        {
            rows = rowsDD.value + 1;
            colunms = colsDD.value + 1;
            grid = new Button[rows, colunms];
            GenerateGrid();
            sizeSelectionPanel.gameObject.SetActive(false);
        }
    }

    private void GenerateGrid()
    {
        gridProperties.constraintCount = colunms;
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < colunms; j++)
            {
                Button prefabObj = Instantiate(buttonPrefab);
                prefabObj.transform.parent = gridPanelObj.transform;
                prefabObj.transform.localScale = GetCellSize();
                grid[i, j] = prefabObj;
            }
        }
        SetCellSpacing();
    }

    private Vector3 GetCellSize()
    {
        int size = MIN_COL + (MAX_COL - colunms);
        return new Vector3(size, size, 0);
    }

    private void SetCellSpacing()
    {
        int spacing = MIN_SPACING + ((MAX_COL - colunms) + 1) * 15;
        gridProperties.spacing = new Vector2(spacing, spacing);
    }

    private void CheckForGameCompletion()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colunms; j++)
            {
                if (grid[i, j].GetComponent<LightsController>().GetCurrentState != LightStatesEnum.Dead)
                {
                    return;
                }
            }
        }
        Debug.Log("Game Ended");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
