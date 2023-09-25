// B3/S23 -> Standard ruleset.
// B1/S12 -> Generates Sierpinski triangles.
// B36/S23 -> Has frequent replicators.

using UnityEngine;

public abstract class LifeGameRule : EvolutionRule {

    public Cell evaluate(int t, int x, int y, int z, Cell[,,] grid) {
        int aliveNeighbours = 0;
        int L = grid.GetLength(0);

        for(int i = x - 1; i <= x + 1; i++) {
            for(int j = y - 1; j <= y + 1; j++) {
                for(int k = z - 1; k <= z + 1; k++) {
                    if (i < 0 || i >= L || j < 0 || j >= L || k < 0 || k >= L)
                        continue;
                    aliveNeighbours += grid[i, j, k].isAlive() && (i != x || j != y || k != z) ? 1 : 0;
                }
            }
        }

        return getNewState(t, grid[x, y, z], aliveNeighbours);
    }

    private Cell getNewState(int t, Cell cell, int aliveNeighbours) {
        bool lives = cellLives(t, cell, aliveNeighbours);
        Cell newCell = new Cell(lives);

        if(!cell.isAlive() && newCell.isAlive())
            newCell.setBornIteration(t);

        return newCell;
    }

    protected abstract bool cellLives(int t, Cell cell, int aliveNeighbours);
}
