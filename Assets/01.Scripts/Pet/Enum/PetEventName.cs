public enum PetEventName
{
    OnSetDestination,
    OnStop, //�������� �������� ���ϰ� ��������
    OnArrive,
    OnRecallKeyPress,
    OnRecallArrive,
    OnSkillKeyPress,
    OnSkillKeyUp,
    OnSkillCancel,
    OnHold,
    OnThrew,
    OnDrop,
    OnLanding,
    OnActiveInteract,
    OnInteractEnd,

    OnSkillComplete,
    OnSkillOffComplete, // Sticky (ON/OFF)
    OnFly,
    OnFlyEnd,
    OnExplosion,
    OnExplosionSmall,
    OnExplosionEnd,
    OnDrawStart, // OilPet (OnSkillStart)

    Count
}