public interface IEditableObject : IHighlightableObject
{
    void IncrementInput(bool wasPressedThisFrame);
    void DecrementInput(bool wasPressedThisFrame);
}
