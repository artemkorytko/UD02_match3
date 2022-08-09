using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "ProjectConfigInstallers", menuName = "Installers/ProjectConfigInstallers")]
    public class ProjectConfigInstallers : ScriptableObjectInstaller<ProjectConfigInstallers>
    {
        public override void InstallBindings()
        {
        }
    }
}