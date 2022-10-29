using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace v12club.Models
{
  public interface IOpenAppService
  {
    Task<bool> Launch(string stringUri);
  }
}
