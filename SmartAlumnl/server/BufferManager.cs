using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class BufferManager
    {
        private byte[] buffer;                      //缓冲区操作的基本字节组
        private int numSize;                       //缓冲区的总大小
        private int bufferSize;                     //传入字节的大小
        private int currentIndex;                //相当于一个游标，判断缓冲区存储字节的位置
        private Stack<int> indexPool;        // 用来存储每次保存的buffer大小的栈结构

        public BufferManager(int totalbytes, int buffsize)
        {
            numSize = totalbytes;
            currentIndex = 0;
            bufferSize = buffsize;
            indexPool = new Stack<int>(totalbytes / buffsize);
        }

        public void InitBuffer()
        {
            buffer = new byte[numSize];         //初始化buffer对象
        }

        /// <summary>
        /// 分配缓冲区从缓冲池到指定的SocketAsyncEventArgs对象
        /// </summary>
        /// <param name="args">SocketAsyncEventArgs对象</param>
        /// <returns>分配成功则返回true，失败则false</returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            //判断缓冲区内是否有是内容，有就直接从里面取
            if (indexPool.Count > 0)
            {
                args.SetBuffer(buffer, indexPool.Pop(), bufferSize);
            }
            else
            {
                if ((numSize - bufferSize) < currentIndex)      //判断缓冲区是否还能装的下内容
                {
                    return false;
                }
                args.SetBuffer(buffer, currentIndex, bufferSize);
                currentIndex += bufferSize;
            }
            return true;
        }

        /// <summary>
        /// /移除一个SocketAsyncEventArg对象的缓冲器，释放缓冲区回到缓冲池
        /// </summary>
        /// <param name="args"></param>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            indexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }

    }
}
