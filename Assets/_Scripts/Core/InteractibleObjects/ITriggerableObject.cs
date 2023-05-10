public interface ITriggerableObject : IHighlightableObject
{
    void OnTriggerInput();
    void OnResetObject();
    bool CanTriggerAgain();
}
