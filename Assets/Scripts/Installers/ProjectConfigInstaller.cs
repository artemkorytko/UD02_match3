using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "ProjectConfigInstaller", menuName = "Installers/ProjectConfigInstaller")]
    public class ProjectConfigInstaller : ScriptableObjectInstaller<ProjectConfigInstaller>
    {
        public override void InstallBindings()
        {
        }
    }
}