using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{


    /* 二维码显示器
     * 
     * Winform 或 WPF
     * 
     * 文本框显示二维码
     * 
     * 扫一次，UI立刻更新本次扫到的二维码
     * 
     * 未扫码显示空
     * 
     * 事件 或 定时器 刷新文本框二维码
     */
    class Program
    {
        static void Main(string[] args)
        {

            using(BarcodeReader barcodeReader = new BarcodeReader())
            {
                barcodeReader.Initialize("COM2");

                Task.Run(() =>
                {
                    while (true)
                    {
                        Console.WriteLine(barcodeReader.GetBarcode());
                        Thread.Sleep(1000);
                    }
                });

                Console.ReadLine();

                barcodeReader.Dispose();
            }
        }
    }
}
