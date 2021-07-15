using RPG.UI;

namespace RPG.Controllers
{
    public interface IRaycastable
    {
        bool HandleRaycast(PlayerController controller);
        CursorType GetCursorType();
    }
}
