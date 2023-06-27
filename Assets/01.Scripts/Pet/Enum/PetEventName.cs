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
    OnInputInteractAction,
    OnInteractArrive,

    OnSkillComplete,
    OnSkillOffComplete, // Sticky (ON/OFF)
    OnFly,
    OnFlyEnd,
    OnExplosion,
    OnExplosionSmall,
    OnExplosionEnd,
    OnDrawStart, // OilPet (OnSkillStart)
    OnOffPing, // OilPet (OnSkillStart)

    // Animation Event
    OnAfraid,

    Count
}