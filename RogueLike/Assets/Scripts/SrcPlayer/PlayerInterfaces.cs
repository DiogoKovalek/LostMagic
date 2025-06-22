public interface IAtributesComunique {
    float CalculateSpeed();
    int CalculateAtack(Element project, Element enemy);
}
public interface IManaManager {
    void expendMana(int value);
    int getTotalMana();
}
