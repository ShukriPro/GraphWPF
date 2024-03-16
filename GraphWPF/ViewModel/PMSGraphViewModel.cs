//using Newtonsoft.Json;
//using OxyPlot.Axes;
//using OxyPlot.Series;
//using OxyPlot;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace GraphWPF.ViewModel
//{
//    public class PMSGraphViewModel : INotifyPropertyChanged
//    {
//        public PlotModel PMSGraph { get; set; }
//        public event PropertyChangedEventHandler PropertyChanged;
//        protected void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//        public PMSGraphViewModel() 
//        { 
//            SetUpPMSGraph();
//            PMSGraph = new PlotModel { Title = "PMS Features Importance (Sorted)", TitleFontSize = 10 };
//        }
//        //Creating the bar graph for PMSGraph
//        private void SetUpPMSGraph()
//        {
//            if(PatientDataStore.CurrentPatientData != null || string.IsNullOrEmpty(PatientDataStore.CurrentPatientData.description))
//            {
//                MessageBox.Show("Patient data is not available. Please check the API response.");
//                return;
//            }
//            var description = PatientDataStore.CurrentPatientData.description;
//            try
//            {
//                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(description);
//                var sortedData = data.OrderByDescending(kv => Math.Abs((double)kv.Value["importance"])).ToList();

//                var barSeries = new BarSeries
//                {
//                    LabelPlacement = LabelPlacement.Inside,
//                    LabelFormatString = "{0:0.00}",
//                    Title = "Importance",
//                    FillColor = OxyColors.SteelBlue
//                };

//                foreach (var item in sortedData)
//                {
//                    barSeries.Items.Add(new BarItem { Value = Math.Abs((double)item.Value["importance"]) });
//                }

//                PMSGraph.Series.Clear();
//                PMSGraph.Series.Add(barSeries);

//                var categoryAxis = new CategoryAxis
//                {
//                    Position = AxisPosition.Left,
//                    IsZoomEnabled = false,
//                    IsPanEnabled = false
//                };

//                foreach (var item in sortedData)
//                {
//                    categoryAxis.Labels.Add(item.Key);
//                }

//                PMSGraph.Axes.Clear();
//                PMSGraph.Axes.Add(categoryAxis);

//                var valueAxis = new LinearAxis
//                {
//                    Position = AxisPosition.Bottom,
//                    MinimumPadding = 0.06,
//                    MaximumPadding = 0.06,
//                    AbsoluteMinimum = 0,
//                    Minimum = 0,
//                    AbsoluteMaximum = 0.06,
//                    Maximum = 0.06,
//                    MajorStep = 0.01,
//                    MinorStep = 0.005
//                };

//                PMSGraph.Axes.Add(valueAxis);

//                PMSGraph.Title = "PMS Features Importance (Sorted)";
//                PMSGraph.TitleFontSize = 14;

//                PMSGraph.InvalidatePlot(true);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error parsing PMS data: {ex.Message}");
//            }
//        }
//    }
//}
