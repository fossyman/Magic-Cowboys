using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    public SO_CharacterData CharacterData;
    [SerializeField]
    private MultiDirectionalSprite Sprite;
    [SerializeField]
    private GameObject SelectedSprite;

    public int CurrentTeam = 0;

    public int Health;

    public enum CharacterState{Idle,Moving,PlanningAttack,Attacking,Dead};

    [SerializeField]
    public CharacterState State;


    private Vector3 MovementVelocity;

    public void Start()
    {
        Health = CharacterData.HealthCapacity;
    }

    public void AttemptMoveToNewPoint(Vector3 _NewPosition)
    {
        Vector3[] Positions = CombatGridManager.Instance.CalculateGridDistance(transform.position, _NewPosition, CharacterData.MoveDistance);
        if (Positions.Length <= 1)
            return;
        else
        {

            if (Positions[1] != null)
            {
                SmoothMovePosition(Positions[1]);
                CombatSceneManager.Instance.Selector.transform.position = Positions[1];
            }
        }
    }

    public void SmoothMovePosition(Vector3 _targetPosition)
    {
        transform.position = _targetPosition;
        //while (transform.position != _targetPosition)
        //{
        //    transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref MovementVelocity, 0.5f);
        //}
    }

}
