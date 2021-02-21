using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Dropdown rowsDD;
    [SerializeField] private Dropdown colsDD;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private RectTransform gridPanelObj;
    [SerializeField] private RectTransform sizeSelectionPanel;
    [SerializeField] private Button enterButton;
    [SerializeField] private RectTransform diagCheck;
    [SerializeField] private RectTransform gameOverPanel;
    [SerializeField] private Button restartButton;

    private const int MIN_COL = 2, MAX_COL = 10, MIN_SPACING = 5, DIAG_SCALE = 100;
    private GridLayoutGroup gridProperties;
    private Button[,] grid;
    private int rows;
    private int colunms;
    void Start()
    {
        diagCheck.gameObject.SetActive(false);
        gridProperties = gridPanelObj.GetComponent<GridLayoutGroup>();
        enterButton.onClick.AddListener(SetSize);
        EventManager.DisableInteractability += CheckForGameCompletion;
        EventManager.ButtonClickEvent += UpdateCheckDiagonal;
        restartButton.onClick.AddListener(RestartGame);
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
                SetCellSize(prefabObj.transform);
                grid[i, j] = prefabObj;
            }
        }
        SetCellSpacing();
    }

    private void SetCellSize(Transform tr)
    {
        tr.localScale = new Vector3(1, 1, 0);
        int size = MAX_COL - ((colunms - MIN_COL)+1);
        tr.localScale = new Vector3(size, size, 0);
    }

    private void SetCellSpacing()
    {
        int spacing = MIN_SPACING + (MAX_COL - colunms + MIN_COL) * 10;
        gridProperties.spacing = new Vector2(spacing, spacing);
    }

    private void UpdateCheckDiagonal(Transform tr)
    {
        diagCheck.gameObject.SetActive(true);
        diagCheck.transform.localScale = new Vector3(0, 0, 0);
        diagCheck.transform.position = tr.position;
        StartCoroutine(ToggleScale());
    }

    private IEnumerator ToggleScale()
    {
        diagCheck.transform.localScale = new Vector3(DIAG_SCALE, 1, 0);
        yield return new WaitForFixedUpdate();
        diagCheck.transform.localScale = new Vector3(1, DIAG_SCALE, 0);
    }

    private void CheckForGameCompletion()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colunms; j++)
            {
                LightsController temp = grid[i, j].GetComponent<LightsController>();
                Debug.Log("State : " + temp.GetCurrentState);
                if (temp.GetCurrentState != LightStatesEnum.Dead)
                {
                    return;
                }
            }
        }
        gameOverPanel.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        Debug.Log("Game Ended");
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        EventManager.DisableInteractability -= CheckForGameCompletion;
        EventManager.ButtonClickEvent -= UpdateCheckDiagonal;
    }
}
