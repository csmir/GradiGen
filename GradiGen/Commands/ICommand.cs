using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ExecuteAsync(CommandContext context);
    }
}
