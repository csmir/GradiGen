using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Commands
{
    /// <summary>
    /// 
    /// </summary>
    internal class ModuleInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public ConstructorInfo Constructor { get; }

        /// <summary>
        /// 
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructor"></param>
        /// <param name="method"></param>
        public ModuleInfo(ConstructorInfo constructor, MethodInfo method)
        {
            Constructor = constructor;
            Method = method;
        }
    }
}
