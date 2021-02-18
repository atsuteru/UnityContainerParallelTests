using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using UnityContainerParallelTest.Services;

namespace UnityContainerParallelTest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool Testinig { get; set; }

        private async void TestStopButton_Click(object sender, RoutedEventArgs e)
        {
            Testinig = false;
        }

        private async void TestStartButton_Click(object sender, RoutedEventArgs e)
        {
            Exception error = null;
            Testinig = true;
            ResultTextBlock.Text = "(Testing...)";

            await Task.Run(() =>
            {
                while (Testinig)
                {
                    var container = new UnityContainer();
                    container.RegisterType<IService0, Service0>();

                    Parallel
                        .ForEach(new Tuple<Type, Type>[]
                        {
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService1), typeof(Service1)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService2), typeof(Service2)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService3), typeof(Service3)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService4), typeof(Service4)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService1), typeof(Service1)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService2), typeof(Service2)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService3), typeof(Service3)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService4), typeof(Service4)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService5), typeof(Service5)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService6), typeof(Service6)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService7), typeof(Service7)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService8), typeof(Service8)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                            Tuple.Create(typeof(IService9), typeof(Service9)),
                            Tuple.Create(typeof(IService0), typeof(Service0)),
                        },
                        new ParallelOptions() { MaxDegreeOfParallelism = 12 },
                        types =>
                        {
                            try
                            {
                                if (container.IsRegistered(types.Item1))
                                {
                                    container.Resolve(types.Item1);
                                }
                                else
                                {
                                    container.RegisterType(types.Item1, types.Item2);
                                }
                            }
                            catch (Exception ex)
                            {
                                error = ex;
                                Testinig = false;
                            }
                        });
                    Thread.Sleep(1);
                }
            });

            if (error != null)
            {
                ResultTextBlock.Text = error.ToString();
            }
            else
            {
                ResultTextBlock.Text = "No Error";
            }
        }
    }
}
