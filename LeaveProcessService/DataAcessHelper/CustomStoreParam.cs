using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace LeaveProcessService.DataAcessHelper
{
    public class CustomStoreParam
    {
        public List<ParamObject> parameterInput { set; get; }

        public class ParamObject
        {
            public string ParamName { set; get; }
            public string ParamType { set; get; }
            public ParameterDirection ParamInOut { set; get; }
            public string ParamLength { set; get; }
            public string InputValue { set; get; }
        }
    }
}
