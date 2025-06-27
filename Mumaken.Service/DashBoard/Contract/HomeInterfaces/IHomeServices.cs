using Mumaken.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Contract.HomeInterfaces
{
    public interface IHomeServices
    {
        DashBoardHomeModel HomeIndex();
    }
}
