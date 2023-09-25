public class Cell {

    private bool alive;
    private int bornIteration;

    public Cell(bool alive) {
        this.alive = alive;
        this.bornIteration = 0;
    }

    public bool isAlive() {
        return alive;
    }

    public void setAlive(bool alive) {
        this.alive = alive;
    }

    public int getBornIteration() {
        return bornIteration;
    }

    public void setBornIteration(int bornIteration) {
        this.bornIteration = bornIteration;
    }

}