using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace GraphWPF
{
    public partial class MainWindow : Window
    {
        public PlotModel PMSGraph { get; private set; }
        public PlotModel GISGraph { get; private set; }
        string apiServerAddr = "";
        public MainWindow()
        {
            InitializeComponent();
            LoadDataPatientData();
            PMSGraph = new PlotModel { Title = "PMS Features Importance (Sorted)", TitleFontSize = 10 };
            GISGraph = new PlotModel { Title = "GIS Features Importance (Sorted)", TitleFontSize = 10 };
            
        }
        //Load data from API
        private async Task LoadPatientDataAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //create var to get response fro mclient api
                    var jsonResponse = await client.GetStringAsync(apiServerAddr);
                    //The Api return a single PatientData object
                    var patientData = JsonConvert.DeserializeObject<PatientData>(jsonResponse);
                    


                    //parse the description field to extract the PMS data
                    var descriptionObject = JObject.Parse(patientData.description);
                    var pmsData = descriptionObject["PMS"].ToString();
                    //Update the description field with the PMS data only
                    patientData.description = pmsData;

                    PatientDataStore.CurrentPatientData = patientData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data: {ex.Message}");
            }
        }
        //Getting the data from the LoadPatientAPI
        private async void LoadDataPatientData()
        {
            await LoadPatientDataAsync();
            SetUpGISGraph();
            SetUpPMSGraph();
        }
        //Setup the graph
        public void SetUpPMSGraph()
        {
            if (PatientDataStore.CurrentPatientData == null || string.IsNullOrEmpty(PatientDataStore.CurrentPatientData.description))
            {
                MessageBox.Show("Patient data is not available. Please check the API response.");
                return;
            }

            var description = PatientDataStore.CurrentPatientData.description;

            try
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(description);
                var sortedData = data.OrderByDescending(kv => Math.Abs((double)kv.Value["importance"])).ToList();

                var barSeries = new BarSeries
                {
                    LabelPlacement = LabelPlacement.Inside,
                    LabelFormatString = "{0:0.00}",
                    Title = "Importance",
                    FillColor = OxyColors.SteelBlue
                };

                foreach (var item in sortedData)
                {
                    barSeries.Items.Add(new BarItem { Value = Math.Abs((double)item.Value["importance"]) });
                }

                PMSGraph.Series.Clear();
                PMSGraph.Series.Add(barSeries);

                var categoryAxis = new CategoryAxis
                {
                    Position = AxisPosition.Left,
                    IsZoomEnabled = false,
                    IsPanEnabled = false
                };

                foreach (var item in sortedData)
                {
                    categoryAxis.Labels.Add(item.Key);
                }

                PMSGraph.Axes.Clear();
                PMSGraph.Axes.Add(categoryAxis);

                var valueAxis = new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    MinimumPadding = 0.06,
                    MaximumPadding = 0.06,
                    AbsoluteMinimum = 0,
                    Minimum = 0,
                    AbsoluteMaximum = 0.06,
                    Maximum = 0.06,
                    MajorStep = 0.01,
                    MinorStep = 0.005
                };

                PMSGraph.Axes.Add(valueAxis);

                PMSGraph.Title = "PMS Features Importance (Sorted)";
                PMSGraph.TitleFontSize = 14;

                PMSGraph.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing PMS data: {ex.Message}");
            }
        }
        public void SetUpGISGraph()
        {
            string jsonData = @"
                    {
          ""GIS"": {
    ""harvested_dist"": {""value"": ""Above median"", ""importance"": 0.019175805214451034},
    ""urban_park_dist"": {""value"": ""Below median"", ""importance"": -0.0053671332955729355},
    ""exotic_frst_dist"": {""value"": ""Above median"", ""importance"": -0.0071074721624162064},
    ""exotic_grass_dist"": {""value"": ""Above median"", ""importance"": -8.202734162724573e-05},
    ""exotic_shrub_dist"": {""value"": ""Below median"", ""importance"": 0.042104820586337216},
    ""crop_dist"": {""value"": ""Above median"", ""importance"": -0.014455372197672718},
    ""indg_frst_dist"": {""value"": ""Below median"", ""importance"": -0.01044941004642363},
    ""broadleaf_dist"": {""value"": ""Above median"", ""importance"": -0.020613242614407303},
    ""tree_count"": {""value"": ""Above median"", ""importance"": 0.04110080164072492},
    ""solar_watts_mean"": {""value"": ""Below median"", ""importance"": 0.02544001872006331},
    ""wetness_index_mean"": {""value"": ""Below median"", ""importance"": 0.008203060322007258}
  }
                    }";

            var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(jsonData);

            var sortedData = data["GIS"].OrderByDescending(kv => Math.Abs((double)kv.Value["importance"])).ToList();



            var barSeries = new BarSeries
            {
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:0.00}",
                Title = "Importance",
                FillColor = OxyColors.SteelBlue
            };

            foreach (var item in sortedData)
            {
                barSeries.Items.Add(new BarItem { Value = Math.Abs((double)item.Value["importance"]) });
            }

            GISGraph.Series.Add(barSeries);

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                IsZoomEnabled = false,
                IsPanEnabled = false
            };

            foreach (var item in sortedData)
            {
                categoryAxis.Labels.Add(item.Key);
            }

            GISGraph.Axes.Add(categoryAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0.06,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0,
                Minimum = 0,
                AbsoluteMaximum = 0.06,
                Maximum = 0.06,
                MajorStep = 0.01,
                MinorStep = 0.005
            };

            GISGraph.Axes.Add(valueAxis);

            DataContext = this;
        }
    }



   
}