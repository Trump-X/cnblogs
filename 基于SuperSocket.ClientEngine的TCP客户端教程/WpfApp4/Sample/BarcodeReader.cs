using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{
    public class BarcodeReader : IDisposable
    {
        private SerialPort _port;
        private string _barcode;


        public BarcodeReader()
        {
            _barcode = string.Empty;
        }


        public string GetBarcode()
        {
            return _barcode;
        }

        public void Initialize(string portName)
        {
            _port = new SerialPort()
            {
                PortName = portName,
                BaudRate = 9600,
                StopBits = StopBits.One,
                Parity = Parity.None,
                DataBits = 8,
            };

            _port.DataReceived += _port_DataReceived;

            if (!_port.IsOpen)
            {
                _port.Open();
            }
        }

        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = sender as SerialPort;

            Thread.Sleep(20);

            _barcode = sp.ReadExisting();

            sp.DiscardInBuffer();
        }

        public void Dispose()
        {
            if (_port.IsOpen)
            {
                _port.Close();
                _port.Dispose();
            }
        }
    }
}
