using System;
using System.Collections.Generic;


namespace SuanShuTi
{
    //存储程序所需的参数，比如算式长度、数字范围等
    static class TotalPara
    {
        public static int TOTALNUMBEROFQUESTIONS = 10;
        public static int LENGTH = 2;
        public static int RANGE = 10;
        public static bool ANSWERRESULT = false;//默认空出来的位置随机（包括加减号）；为true时为求结果，如3+2=（  ）。
    }

    //自定义异常，处理算式长度、范围。
    class LenghLessThanTwo : ApplicationException
    {
        public LenghLessThanTwo()
        {
            Console.WriteLine("算式的长度应该大于等于2");
        }
    }

    class OutofRange : ApplicationException
    {
        public OutofRange()
        {
            Console.WriteLine("数值的范围应该大于等于0");
        }
    }

    public class Arithmetic
    {
        int length;//算式的长度，比如2+3=5（length=2）；2+3-1=4（length=3）
        int range;//算式数字范围（包括结果）,范围包括range本身
        bool answerResult;//是否只是求最后结果。
        public int emptySite;//出题时空出来让做题者填写的

        List<int> numbers;//加减的操作数，如2+3=5中的2、3；
        List<char> operators;//加减号,最后一个为=
        List<string> arithmetic;//以string形式储存整个算式，包括=和结果；
        int result = 0;//算式的结果，如2+3=5中的5；
        

        //上面那些应该设置成get的形式的，但之前没弄，不知道怎么不改动其他代码重新弄了。
        public int Length() { return length; }
        public int Range() { return range; }
        public bool AnswerResult() { return answerResult; }
        public int EmptySite() { return emptySite; }

        public List<int> Numbers() { return numbers; }
        public List<char> Operators() { return operators; }
        public List<string> ArithmeticString() { return arithmetic; }
        public int Result() { return result; }
        public string Answer() { return arithmetic[emptySite]; }//返回问题的答案。

        //构造函数
        public Arithmetic(int ran=10, int len=2, bool b=true)
        {
            if (len < 2)
            {
                throw new LenghLessThanTwo();
            }

            if (ran < 0)
            {
                throw new OutofRange();
            }

            length = len;
            range = ran;
            answerResult = b;
            numbers = new List<int>();
            operators = new List<char>();
            arithmetic = new List<string>();

            //从0开始，最后的结果的序数是2L   
            if (!answerResult)
            {
                Random rand = new Random();
                emptySite = rand.Next(2 * length - 1);
            }
            else
            { emptySite = 2 * length; }

            //产生长度为2的算式
            while (true)
            {
                Random rand = new Random();
                int no1 = rand.Next(range + 1);
                int no2 = rand.Next(range + 1);

                // i=0或1，+-随机先后，没有这个的话，总是先尝试+，导致最后的题目以+为主。
                int i = rand.Next(2);
                if (i==0)
                {   //代码重复，应该做个内部小函数
                    if (InOrOutRange(no1 + no2))
                    {
                        result = no1 + no2;
                        StoreNum(no1);
                        StoreOperator('+');
                        StoreNum(no2);
                        break;
                    }
                    else if (InOrOutRange(no1 - no2))
                    {
                        result = no1 - no2;
                        StoreNum(no1);
                        StoreOperator('-');
                        StoreNum(no2);
                        break;
                    }
                }
                else 
                {
                    if (InOrOutRange(no1 - no2))
                    {
                        result = no1 - no2;
                        StoreNum(no1);
                        StoreOperator('-');
                        StoreNum(no2);
                        break;
                    }
                    else if (InOrOutRange(no1 + no2))
                    {
                        result = no1 + no2;
                        StoreNum(no1);
                        StoreOperator('+');
                        StoreNum(no2);
                        break;
                    }
                }
            }

            //更长的算式，循环产生第3第4....的数。
            int lengthLeft = length;
            while (lengthLeft - 2 > 0)
            {
                Random rand = new Random();
                int newNum = rand.Next(range + 1);

                //没有随机+-
                //应该写个函数，结果作为第一操作数，随机产生一个数作为第2个操作数，与result进行操作，符合要求的结果再存为新的result。
                //如此循环产生新的操作数与result进行操作。
                if (InOrOutRange(result + newNum))
                {
                    result += newNum;
                    StoreOperator('+');
                    StoreNum(newNum);
                    break;
                }
                else if (InOrOutRange(result - newNum))
                {
                    result -= newNum;
                    StoreOperator('-');
                    StoreNum(newNum);
                    break;
                }
                lengthLeft--;
            }
            //最后加上=和结果。
            arithmetic.Add("=");
            arithmetic.Add(result.ToString());
        }

        //判断一个数是否在算式要求的大小范围内，【0，Range】
        bool InOrOutRange(int i)
        {
            return i >= 0 && i <= range;
        }

        //把数值存入numbers
        void StoreNum(int i)
        {
            numbers.Add(i);
            arithmetic.Add(i.ToString());
        }

        //把符号存入operators
        void StoreOperator(char c)
        {
            operators.Add(c);
            arithmetic.Add(c.ToString());
        }

        //把string list当作一个字符串输出,每个string间隔一个mediumStr（默认没有）
        string ListToString(string[] listString, string mediumStr = "")
        {
            string str = "";
            foreach (var s in listString)
            {
                str += s;
                str += mediumStr;
            }
            return str;
        }

        //参数true输出有空格的，false输出没有空格的
        //true:3+(  )=5
        //false:3+2=5
        public string ToString(bool withEmpty) 
        {
            int count = arithmetic.Count;
            string[] stringArith = new string[count];
            arithmetic.CopyTo(stringArith);
            if (withEmpty)
            {
                stringArith[emptySite] = "(  )";
            }
            return ListToString(stringArith);
        }
    }
}
