using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GraphWPF
{
    public partial class MainWindow : Window
    {
        public PlotModel PMSGraph { get; private set; }
        public PlotModel GISGraph { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            PMSGraph = new PlotModel { Title = "PMS Features Importance (Sorted)", TitleFontSize = 10 };
            GISGraph = new PlotModel { Title = "GIS Features Importance (Sorted)", TitleFontSize = 10 };
            SetUpPMSGraph();
            SetUpGISGraph();
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
        public void SetUpPMSGraph()
        {
            string jsonData = @"
                    {
                        'PMS': {
                            'assigned_sex': {'value': 'F', 'importance': -0.023010441674045447},
                            'age_group_new': {'value': '71-80', 'importance': -0.04093950002731588},
                            'Sitting Sys': {'value': 'Elevated', 'importance': -0.039559692270079785},
                            'Sitting Dia': {'value': 'Normal', 'importance': 0.002149668302948061},
                            'Haemoglobin': {'value': 'Normal', 'importance': 0.002886379500404963},
                            'eGFR': {'value': 'Mild to Moderate', 'importance': 0.05230030449512338},
                            'Triglyceride': {'value': 'Borderline High', 'importance': 0.011994537524905013},
                            'Chol/HDL Ratio': {'value': 'Normal', 'importance': -0.006383770788743084},
                            'mmol': {'value': 'Severely High', 'importance': 0.04275261967005567},
                            'Ferritin': {'value': 'Normal', 'importance': -0.008558809672715995},
                            'LDL Cholesterol': {'value': 'Moderate to High risk', 'importance': -0.05498969754887217},
                            'Uric Acid': {'value': 'Low risk', 'importance': -0.02727666840239492},
                            'Albumin/Creatinine Ratio': {'value': 'Low risk', 'importance': -0.021599894630545527},
                            'BMI': {'value': 'BMI not good', 'importance': -0.05737224210105597},
                            'cms': {'value': 2, 'importance': 0.01940919562254926}
                        }
                    }";

            var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(jsonData);

            var sortedData = data["PMS"].OrderByDescending(kv => Math.Abs((double)kv.Value["importance"])).ToList();



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

            DataContext = this;
        }

        //public void SetUpModel1()
        //{
        //    string json = @"
        //    {
        //        ""PMS"": {
        //            ""assigned_sex"": {""value"": ""F"", ""importance"": -0.023010441674045447},
        //            ""age_group_new"": {""value"": ""71-80"", ""importance"": -0.04093950002731588},
        //            ""Sitting Sys"": {""value"": ""Elevated"", ""importance"": -0.039559692270079785},
        //            ""Sitting Dia"": {""value"": ""Normal"", ""importance"": 0.002149668302948061},
        //            ""Haemoglobin"": {""value"": ""Normal"", ""importance"": 0.002886379500404963},
        //            ""eGFR"": {""value"": ""Mild to Moderate"", ""importance"": 0.05230030449512338},
        //            ""Triglyceride"": {""value"": ""Borderline High"", ""importance"": 0.011994537524905013},
        //            ""Chol/HDL Ratio"": {""value"": ""Normal"", ""importance"": -0.006383770788743084},
        //            ""mmol"": {""value"": ""Severely High"", ""importance"": 0.04275261967005567},
        //            ""Ferritin"": {""value"": ""Normal"", ""importance"": -0.008558809672715995},
        //            ""LDL Cholesterol"": {""value"": ""Moderate to High risk"", ""importance"": -0.05498969754887217},
        //            ""Uric Acid"": {""value"": ""Low risk"", ""importance"": -0.02727666840239492},
        //            ""Albumin/Creatinine Ratio"": {""value"": ""Low risk"", ""importance"": -0.021599894630545527},
        //            ""BMI"": {""value"": ""BMI not good"", ""importance"": -0.05737224210105597},
        //            ""cms"": {""value"": 2, ""importance"": 0.01940919562254926}
        //        }
        //    }";

        //    // Parse the JSON string into a dictionary
        //    var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(json);

        //    var barSeries = new BarSeries
        //    {
        //        ItemsSource = data["PMS"].OrderByDescending(kv => Math.Abs((double)kv.Value["importance"])),
        //        LabelPlacement = LabelPlacement.Inside,
        //        LabelFormatString = "{0}",
        //        ValueField = "Value.importance",
        //        Title = "Importance"
        //    };

        //    BarModel.Series.Add(barSeries);

        //    var categoryAxis = new CategoryAxis
        //    {
        //        Position = AxisPosition.Left,
        //        Key = "Features",
        //        ItemsSource = data["PMS"].OrderByDescending(kv => Math.Abs((double)kv.Value["importance"])),
        //        LabelField = "Key"
        //    };

        //    BarModel.Axes.Add(categoryAxis);

        //    var valueAxis = new LinearAxis
        //    {
        //        Position = AxisPosition.Bottom,
        //        Key = "Importance",
        //        AbsoluteMinimum = 0
        //    };

        //    BarModel.Axes.Add(valueAxis);

        //    DataContext = this;
        //}
    }
}