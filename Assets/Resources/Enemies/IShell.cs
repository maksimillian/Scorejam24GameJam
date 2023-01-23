using UnityEngine;
using UnityEngine.UI;

public interface IShell
{
    void Init(Sprite sprite);
    void Fire(Vector3 targetDirection);
}
