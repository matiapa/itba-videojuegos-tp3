using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Automata: MonoBehaviour {
    
    [SerializeField] private PatternsEnum pattern = PatternsEnum.CUSTOM;
    [SerializeField] private int gridSize;
    [SerializeField] private PopulationRuleConfig populationRuleConfig;
    [SerializeField] private Point[] initialLiveCells;
    [SerializeField] private float stateDuration;
    [SerializeField] private int colorMaxAge = 10;

    private LineRenderer lr;
    private EvolutionRule rule;
    private Cell[,,] currentGrid;
    private float currentStateSeconds = 0;
    private int iteration = 0;
    private List<GameObject> cubes = new List<GameObject>();


    public void loadConfig() {
        if (pattern == PatternsEnum.OSCILLATOR) {
            gridSize = 5;
            populationRuleConfig = new PopulationRuleConfig(5, 5, 5, 6);
            initialLiveCells = new Point[] {
                new(2,1,1), new(2,1,1), new(2,1,2),
                new(2,1,3), new(2,2,2), new(2,3,1),
                new(2,3,2), new(2,3,3)
            };
        } else if (pattern == PatternsEnum.GLIDER) {
            gridSize = 15;
            populationRuleConfig = new PopulationRuleConfig(6, 6, 5, 7);
            initialLiveCells = new Point[] {
                new(1,1,1), new(1,2,1), new(1,2,3),
                new(1,3,1), new(1,3,2), new(2,1,1),
                new(2,2,1), new(2,2,3), new(2,3,1),
                new(2,3,2)
            };
            foreach(Point p in initialLiveCells) {
                p.x += 5;
                p.y += 5;
                p.z += 5;
            }
        }  else if (pattern == PatternsEnum.EXPANSION) {
            gridSize = 20;
            populationRuleConfig = new PopulationRuleConfig(2, 6, 4, 5);
            initialLiveCells = new Point[] {
                new(12,14,10), new(13,14,10), new(12,12,12),
                new(12,11,11), new(12,14,15), new(12,13,15)
            };
        }

        rule = new PopulationRule(populationRuleConfig.bornMin, populationRuleConfig.bornMax, populationRuleConfig.dieUnder, populationRuleConfig.dieOver);

        // Draw delimiting square

        Vector3[] positions = new Vector3[] {
            new(0, 0, 0), new(0, 0, gridSize), new(gridSize, 0, gridSize), new(gridSize, 0, 0), new(0, 0, 0),
        };
        lr.startWidth = 0.25f;
        lr.endWidth = 0.25f;
        lr.positionCount = 5;
        lr.SetPositions(positions);

        // Move camera to border

        if (Camera.main != null) {
            Transform cameraTransform = Camera.main.transform;
            cameraTransform.position = new Vector3(gridSize * 1.2f, gridSize * 1.2f, gridSize * 1.2f);
            cameraTransform.LookAt(new Vector3(0, 0, 0));
        }
    }

    public void initializeGrid() {
        Cell[,,] newGrid = new Cell[gridSize, gridSize, gridSize];

        for(int i=0; i<initialLiveCells.Length; i++) {
            Point point = initialLiveCells[i];
            newGrid[point.x, point.y, point.z] = new Cell(true);
        }

        for (int x=0; x<gridSize; x++) {
            for (int y=0; y < gridSize; y++) {
                for (int z=0; z < gridSize; z++) {
                    if (newGrid[x, y, z] == null)
                        newGrid[x, y, z] = new Cell(false);
                }
            }
        }

        currentGrid = newGrid;
    }

    public void updateGrid() {
        Cell[,,] newGrid = new Cell[gridSize, gridSize, gridSize];

        for(int x=0; x<gridSize; x++) {
            for(int y=0; y<gridSize; y++) {
                for(int z=0; z<gridSize; z++) {
                    newGrid[x, y, z] = rule.evaluate(iteration, x, y, z, currentGrid);
                }
            }
        }

        currentGrid = newGrid;
    }

    public void displayGrid() {
        // Remove currently displayed cubes
        cubes.ForEach((cube) => Destroy(cube));
        cubes.Clear();

        // Add a cube for each alive cell
        for(int x=0; x<gridSize; x++) {
            for(int y=0; y<gridSize; y++) {
                for(int z=0; z<gridSize; z++) {
                    Cell cell = currentGrid[x, y, z];

                    if (cell.isAlive()) {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        cube.transform.parent = gameObject.transform;
                        cube.transform.localPosition = new Vector3(x, y, z);

                        int age = iteration - cell.getBornIteration();
                        float redness = age / colorMaxAge;
                        float greenness = 1 - age / colorMaxAge;
                        cube.GetComponent<Renderer>().material.color = new Color(redness, greenness, 1);

                        cubes.Add(cube);
                    }
                }
            }
        }
    }


    void Start() {
        lr = GetComponent<LineRenderer>();

        loadConfig();
        initializeGrid();
        displayGrid();
    }

    void Update() {
        currentStateSeconds += Time.deltaTime;

        if (currentStateSeconds < stateDuration)
            return;
        
        updateGrid();
        displayGrid();

        currentStateSeconds = 0;
        iteration += 1;
    }


    [System.Serializable]
    public class Point {

        public Point(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int x;
        public int y;
        public int z;
    }

    [System.Serializable]
    public class PopulationRuleConfig {
        public int bornMin;
        public int bornMax;
        public int dieUnder;
        public int dieOver;

        public PopulationRuleConfig(int bornMin, int bornMax, int dieUnder, int dieOver) {
            this.bornMin = bornMin;
            this.bornMax = bornMax;
            this.dieUnder = dieUnder;
            this.dieOver = dieOver;
        }
    }

    [System.Serializable]
    public enum PatternsEnum {
        CUSTOM,
        OSCILLATOR,
        GLIDER,
        EXPANSION,
    }

}