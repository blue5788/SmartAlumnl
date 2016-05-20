using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class SocketAsyncEventArgsPool
    {
        Stack<SocketAsyncEventArgs> _saeaPool;          //存储SocketAsyncEventArgs对象的栈结构

        /// <summary>
        /// 初始化线程池
        /// </summary>
        /// <param name="capacity">指定池的大小</param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            _saeaPool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        /// 添加SocketAsyncEventArgs对象的操作
        /// </summary>
        /// <param name="item"></param>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) { throw new ArgumentNullException("SocketAsyncEventArgs的对象不能为空"); }
            lock (_saeaPool)
            {
                _saeaPool.Push(item);
            }
        }

        /// <summary>
        /// 移除SocketAsyncEventArgs对象的操作
        /// </summary>
        /// <returns>移除对象的实例</returns>
        public SocketAsyncEventArgs Pop()
        {
            lock (_saeaPool)
            {
                return _saeaPool.Pop();
            }
        }

        /// <summary>
        /// 获取当前存储SocketAsyncEventArgs对象的数量
        /// </summary>
        public int Count
        {
            get { return _saeaPool.Count; }
        }
    }
}
