using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public  class MsgType
{
    #region 客户机通信请求标志

    /// <summary>
    /// 客户端登录标识
    /// </summary>
    public  const int LOGIN = 1001;
    public  const int LOGIN_FAILED = 1002;
    public  const int LOGIN_SUCCESS = 1003;
    #endregion
}
