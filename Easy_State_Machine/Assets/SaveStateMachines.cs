using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Availables Machines", menuName = "Easy State Machine/Machines", order = 3)]
public class SaveStateMachines : ScriptableObject
{
   public List<StateMachineScriptable> newAvailablesMachines = new List<StateMachineScriptable>();
}
