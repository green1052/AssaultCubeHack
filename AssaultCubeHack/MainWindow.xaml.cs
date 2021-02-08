using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Memory;

namespace AssaultCubeHack
{
    /// <summary>
    ///     MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string LocalPlayer = "base+0x10F4F4";
        private readonly Mem _mem = new Mem();
        private bool _isOpen;

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(Loop);
        }

        private void Loop()
        {
            while (true)
            {
                Thread.Sleep(500);

                if (!_isOpen)
                {
                    _isOpen = _mem.OpenProcess("ac_client");

                    if (!_isOpen)
                        continue;

                    Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(delegate { FindGame.Content = "게임을 찾았습니다."; }));
                }

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    if (Invincibility.IsChecked == true) _mem.WriteMemory($"{LocalPlayer},0x00F8", "int", "100");

                    if (InfiniteVest.IsChecked == true) _mem.WriteMemory($"{LocalPlayer},0x00FC", "int", "100");

                    if (InfiniteAmmo.IsChecked == true)
                    {
                        _mem.WriteMemory($"{LocalPlayer},0x013C", "int", "99");
                        _mem.WriteMemory($"{LocalPlayer},0x140", "int", "99");
                        _mem.WriteMemory($"{LocalPlayer},0x14C", "int", "99");
                        _mem.WriteMemory($"{LocalPlayer},0x144", "int", "99");
                        _mem.WriteMemory($"{LocalPlayer},0x148", "int", "99");
                        _mem.WriteMemory($"{LocalPlayer},0x0150", "int", "99");
                        _mem.WriteMemory($"{LocalPlayer},0x0158", "int", "99");
                    }
                }));
            }
        }
    }
}