using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL
{
    /// <summary>
    /// 组合算法，从M个数中取出N个，无序
    /// </summary>
    public class CombineAlgorithm
    {
        //src数组的长度
        private int m;

        //需要获取n个数
        private int n;

        //存放结果的二维数组
        private Object[][] obj = null;

        //临时变量，ogj数组的行数
        private int objLineIndex;

        public CombineAlgorithm(Object[] src,int getNum)
        {
            if (src == null)
                throw new Exception("原数组为空");
            if (src.Count() < getNum)
                throw new Exception("要取的数据比原数组还大");
            m = src.Count();
            n = getNum;

            //obj数组初始化
            objLineIndex = 0;
            obj = new Object[combination(m, n)][];
            for (int i = 0; i < obj.GetLength(0); i++)
            {
                obj[i] = new Object[n];
            }

            Object[] temp = new Object[n];
            combine(src, 0, 0, n, temp);
        }
        
        /// <summary>
        /// * 计算 C(m,n)个数 = (m!)/(n!*(m-n)!) 即从M中选N个数，函数返回有多少种选法（参数M必须大于等于n）
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns>返回有C(m,n)种选法</returns>
        public int combination(int m,int n)
        {
            if (m < n)
                return 0;

            int k = 1, j = 1;

            //约掉分母的（m-n）!
            for (int i = n; i >=1; i--)
            {
                k = k * m;
                j = j * n;
                m--;
                n--;
            }
            return k / j;
        }

        /// <summary>
        /// 递归算法，把组合结果写进obj数组
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcIndex"></param>
        /// <param name="i"></param>
        /// <param name="n"></param>
        /// <param name="temp"></param>
        public void combine(Object[] src,int srcIndex,int i,int n,Object[] temp)
        {
            int j;
            for (j = srcIndex; j < src.Count()-(n-1); j++)
            {
                temp[i] = src[j];
                if (n==1)
                {
                    Array.Copy(temp, 0, obj[objLineIndex], 0, temp.Count());
                    objLineIndex++;
                }
                else
                {
                    n--;
                    i++;
                    combine(src, j + 1, i, n, temp);
                    n++;
                    i--;
                }
            }
        }

        /// <summary>
        /// 得到结果数组
        /// </summary>
        /// <returns></returns>
        public Object[][] getResult()
        {
            return obj;
        }
    }
}
