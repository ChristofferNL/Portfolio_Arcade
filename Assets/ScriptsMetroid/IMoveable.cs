/// <summary>
/// Interface used by player controlled objects
/// </summary>
public interface IMoveable
{
    public void YAxisMoveCalc();
    public void XAxisMoveCalc();
    public void ExecuteMove(float yVelocity, float xVelocity);
}
