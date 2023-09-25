public class PopulationRule: LifeGameRule {

    int bornMin;
    int bornMax;
    int dieUnder;
    int dieOver;

    public PopulationRule(int bornMin, int bornMax, int dieUnder, int dieOver) {
        this.bornMin = bornMin;
        this.bornMax = bornMax;
        this.dieUnder = dieUnder;
        this.dieOver = dieOver;
    }

    protected override bool cellLives(int t, Cell cell, int aliveNeighbours) {
        if(cell.isAlive() && (aliveNeighbours < dieUnder || aliveNeighbours > dieOver)) {
            return false;
        } else if(!cell.isAlive() && aliveNeighbours >=bornMin && aliveNeighbours <=bornMax) {
            return true;
        }
        return cell.isAlive();
    }

}