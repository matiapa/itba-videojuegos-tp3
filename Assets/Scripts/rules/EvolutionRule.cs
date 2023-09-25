public interface EvolutionRule {

    Cell evaluate(int t, int x, int y, int z, Cell[,,] grid);

}
