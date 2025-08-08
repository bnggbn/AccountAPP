using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountAPP
{
    internal abstract class DefineAccount : IType, IAccount
    {

    }
    internal interface IType
    {

    }

    internal interface IAccount
    {

    }

    internal enum TypeClass
    {
        income,
        expend,
        total,

        Unknow
    }
}
