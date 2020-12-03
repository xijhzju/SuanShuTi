using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SuanShuTi
{
    public partial class MainPage : ContentPage
    {
        TestPaper testPaper;//试卷
        Arithmetic currentArithmetic;//当前题目
        int totalNum { get; set; }//试卷总题数
        int currentNum { get; set; }//目前做到了第几题(从0开始计数）
        int leftNum { get; set; } //记录做到第几题了
        int wrongNum { get; set; }//记录做错的次数（一道题错了需要重做，同一题错误数可以大于1

        class TestPaper
        {
            public int numberOfQuestions { get; }//试卷共有几道题
            public Arithmetic[] papers { get; }
            public TestPaper(int n, int range, int length, bool random)
            {
                numberOfQuestions = n;
                papers = new Arithmetic[numberOfQuestions];
                for (int i = 0; i < numberOfQuestions; i++)
                {
                    Arithmetic arith = new Arithmetic(range, length, !random);

                    papers[i] = arith;
                }
            }
        }

        public MainPage()
        {
            InitializeComponent();


            InitializeTestPaper();
            //currentNum = 0;
            //leftNum = totalNum - currentNum;
            //wrongNum = 0;
            //testPaper = new TestPaper(totalNum);
            //currentArithmetic = testPaper.papers[currentNum];
            //BindingContext = testPaper;
            //labelTotalNum.Text = "Total Number: " + totalNum.ToString();
            //RefleshStatus();
        }

        void InitializeTestPaper()
        {
            totalNum = TotalPara.TOTALNUMBEROFQUESTIONS;
            currentNum = 0;
            leftNum = totalNum - currentNum;
            wrongNum = 0;
            testPaper = new TestPaper(GetTotalNum(), GetRange(), GetLength(), GetIsRandom());//初始化从entry里取值string to int怎么搞。
            currentArithmetic = testPaper.papers[currentNum];
            BindingContext = testPaper;
            labelTotalNum.Text = "Total Number: " + totalNum.ToString();
            RefleshStatus();
        }

        //从radiobuttom得到length
        int GetLength()
        {
            if (length2RadioButton.IsChecked)
            {
                return 2;
            }
            else if (length3RadioButton.IsChecked)
            {
                return 3;
            }
            else return 0;
        }

        //从radiobuttom得到试卷的题目总数
        int GetTotalNum()
        {
            if (total10RadioButton.IsChecked)
            {
                return 10;
            }
            else if (total20RadioButton.IsChecked)
            {
                return 20;
            }
            else if (total50RadioButton.IsChecked)
            {
                return 50;
            }
            else return 0;
        }

        //从radiobutton得到range
        int GetRange()
        {
            if (range10RadioButtom.IsChecked)
            {
                return 10;
            }
            else if (range20RadioButtom.IsChecked)
            {
                return 20;
            }
            else if (range100RadioButtom.IsChecked)
            {
                return 100;
            }
            else return 0;
        }

        //从radiobutton得到是否随机
        bool GetIsRandom()
        {
            if (randomYesRadiobuttom.IsChecked)
            {
                return true;
            }
            else if (randomNoRadiobuttom.IsChecked)
            {
                return false;
            }
            else return true;
        }

        //刷新界面显示状态，以后用binding就不用这么傻的操作了
        void RefleshStatus()
        {
            labelLeftNum.Text = "Left: " + leftNum.ToString();
            labelWrongNum.Text = "Wrong: " + wrongNum.ToString();
            labelQuestion.Text = currentArithmetic.ToString(true);
            entryInputAnswer.Text = "";

        }
        //得重写，乱七八糟
        void buttomConfirmInput_Clicked(System.Object sender, System.EventArgs e)
        {
            //testLabel.Text = entryInputAnswer.Text;

            if (currentArithmetic.Answer() == entryInputAnswer.Text)
            {
                if (currentNum < totalNum - 1)
                {
                    currentNum++;
                    leftNum = totalNum - currentNum;
                    currentArithmetic = testPaper.papers[currentNum];
                    RefleshStatus();
                }
                else
                {
                    DisplayAlert("DONE", $"Total:{totalNum},Wrong:{wrongNum},Score:{(int)(100 * (totalNum - wrongNum) / totalNum)}", "YES");
                    InitializeTestPaper();
                    RefleshStatus();
                }

            }
            else if (testPaper.papers[currentNum].ArithmeticString()[testPaper.papers[currentNum].EmptySite() + 1] == "0")//要填写的是+-，但后面一个是0，那么+-都正确
            {
                if (entryInputAnswer.Text == "+" || entryInputAnswer.Text == "-")
                {       //复制的上面正确的做法，得重写。
                    if (currentNum < totalNum - 1)
                    {
                        currentNum++;
                        leftNum = totalNum - currentNum;
                        currentArithmetic = testPaper.papers[currentNum];
                        RefleshStatus();
                    }
                    else
                    {
                        DisplayAlert("DONE", $"Total:{totalNum},Wrong:{wrongNum},Score:{(int)(100 * (totalNum - wrongNum) / totalNum)}", "YES");
                        InitializeTestPaper();
                        RefleshStatus();
                    }
                }
            }
            else
            {
                wrongNum++;
                RefleshStatus();
            }

        }

        void StartButton_Clicked(System.Object sender, System.EventArgs e)
        {
            InitializeTestPaper();
        }
    }
}
