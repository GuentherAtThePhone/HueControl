using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ColorHelper;
using HueControl.Classes.HueBridgeClasses;
using HueControl.Classes.Others;

namespace HueControl
{
    /// <summary>
    /// Interaktionslogik für HueControlTest.xaml
    /// </summary>
    public partial class HueControlTest : Window
    {

        List<LightHelper>? lights;
        readonly double constValue = 100 / 347.5;

        bool loop = false;
        bool firstStart = false;

        public HueControlTest()
        {
            InitializeComponent();

            Thread r = new Thread(OnStartUp);
            r.Start();
            
        }

        private void OnStartUp()
        {
            Dispatcher.Invoke(new System.Action(delegate
            {
                txtUsername.Text = Properties.Settings.Default.Usercode;
                txtIp.Text = Properties.Settings.Default.BridgeIP;

                HueLogic.Usercode = txtUsername.Text;
                HueLogic.BridgeIP = txtIp.Text;
            }));            

            if (HueLogic.Usercode == "" | HueLogic.Usercode == null | HueLogic.BridgeIP == "" | HueLogic.BridgeIP == null)
            {
                // create / search

                if (HueLogic.BridgeIP == "" | HueLogic.BridgeIP == null)
                {
                    if (HueLogic.Usercode == "" | HueLogic.Usercode == null)
                    {
                        // First Start (no ip and no username)
                        firstStart = true;
                        FirstStart();
                    }
                    else
                    {
                        // No ip but username
                        Dispatcher.Invoke(new System.Action(delegate
                        {                            
                            GridSettings.Visibility = Visibility.Visible;
                            GridMainView.Visibility = Visibility.Collapsed;
                        }));
                    }

                }
                else
                {
                    // IP but no username
                    string result2 = "";

                    try
                    {
                        result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights"));
                    }
                    catch (Exception)
                    {
                        // Ip not correct
                        firstStart = true;
                        // Act like first start
                        FirstStart();
                    }

                    if (result2 != "" && !result2.Contains("error"))
                    {
                        // IP and Username Correct
                        // Change view (visible grids)
                        LoadData();
                        Dispatcher.Invoke(new System.Action(delegate
                        {                            
                            GridSettings.Visibility = Visibility.Collapsed;
                            GridMainView.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        //username incorrect
                        Dispatcher.Invoke(new System.Action(delegate
                        {                            
                            GridSettings.Visibility = Visibility.Visible;
                            GridMainView.Visibility = Visibility.Collapsed;
                        }));
                    }

                }
            }
            else
            {
                // IP and username
                // Change view (visible grids)
                LoadData();
                Dispatcher.Invoke(new System.Action(delegate
                {                            
                    GridSettings.Visibility = Visibility.Collapsed;
                    GridMainView.Visibility = Visibility.Visible;
                }));
            }
        }

        private void FirstStart() 
        { 
            // Act like creating new Account
            BtnCreateUserCancel.Visibility = Visibility.Collapsed;
            CreatingUser();
        }

        private void LoadData()
        {
            // Load the data for the groups
            string result = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "groups"));
            List<GroupHelper> groups = GroupHelper.FromJson(result);

            // Set the data for the group lists
            Dispatcher.Invoke(new System.Action(delegate
            {
                LvRoomsList.ItemsSource = groups;
                ListViewRoomsOverview.ItemsSource = groups;
            }));            

            // Load the data for the lights
            string result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights"));
            lights = LightHelper.FromJson(result2);
            
            // Set the data for the light lists
            Dispatcher.Invoke(new System.Action(delegate
            {
                LvLightsList.ItemsSource = lights;
                LvLightsOverviewList.ItemsSource = lights;
            }));            
        }

        #region Selection Changed

        // Change in rooms List on left side
        private void LvRoomsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lights == null)
            {
                return;
            }
            GridRoomsOverview.Visibility = Visibility.Collapsed;
            GridSingleRoom.Visibility = Visibility.Visible;
            GridSingleLight.Visibility = Visibility.Collapsed;
            GridLightsOverview.Visibility = Visibility.Collapsed;
            GridSettings.Visibility = Visibility.Collapsed;

            GroupHelper groupHelper = (GroupHelper)LvRoomsList.SelectedItem;

            lblSingleRoomName.Content = groupHelper.Name;

            List<int> ints = groupHelper.Lights;
            string result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights"));
            lights = LightHelper.FromJson(result2);
            List<LightHelper> sinlgeRoomLights = lights;
            int i = 0;

            foreach (LightHelper lightHelper in sinlgeRoomLights.ToList())
            {
                if (!ints.Contains(Convert.ToInt32(lightHelper.ID)))
                {
                    sinlgeRoomLights.Remove(lightHelper);
                }
                i++;
            }

            LvSingleRoomsList.ItemsSource = sinlgeRoomLights;
        }

        // Change in rooms list overview right side
        private void ListViewRoomsOverview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lights == null)
            {
                return;
            }
            GridRoomsOverview.Visibility = Visibility.Collapsed;
            GridSingleRoom.Visibility = Visibility.Visible;
            GridSingleLight.Visibility = Visibility.Collapsed;
            GridLightsOverview.Visibility = Visibility.Collapsed;
            GridSettings.Visibility = Visibility.Collapsed;

            GroupHelper groupHelper = (GroupHelper)ListViewRoomsOverview.SelectedItem;

            lblSingleRoomName.Content = groupHelper.Name;

            List<int> ints = groupHelper.Lights;
            string result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights"));
            lights = LightHelper.FromJson(result2);
            List<LightHelper> sinlgeRoomLights = lights;
            int i = 0;

            foreach (LightHelper lightHelper in sinlgeRoomLights.ToList())
            {
                if (!ints.Contains(Convert.ToInt32(lightHelper.ID)))
                {
                    sinlgeRoomLights.Remove(lightHelper);
                }
                i++;
            }

            LvSingleRoomsList.ItemsSource = sinlgeRoomLights;
        }
        private void ListViewRoomsOverview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lights == null)
            {
                return;
            }
            GridRoomsOverview.Visibility = Visibility.Collapsed;
            GridSingleRoom.Visibility = Visibility.Visible;
            GridSingleLight.Visibility = Visibility.Collapsed;
            GridLightsOverview.Visibility = Visibility.Collapsed;
            GridSettings.Visibility = Visibility.Collapsed;

            GroupHelper groupHelper = (GroupHelper)ListViewRoomsOverview.SelectedItem;

            lblSingleRoomName.Content = groupHelper.Name;

            List<int> ints = groupHelper.Lights;
            string result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights"));
            lights = LightHelper.FromJson(result2);
            List<LightHelper> sinlgeRoomLights = lights;
            int i = 0;

            foreach (LightHelper lightHelper in sinlgeRoomLights.ToList())
            {
                if (!ints.Contains(Convert.ToInt32(lightHelper.ID)))
                {
                    sinlgeRoomLights.Remove(lightHelper);
                }
                i++;
            }

            LvSingleRoomsList.ItemsSource = sinlgeRoomLights;
        }


        // Change in lights List on left side
        private void LvLightsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridSettings.Visibility = Visibility.Collapsed;
            GridRoomsOverview.Visibility = Visibility.Collapsed;
            GridSingleRoom.Visibility = Visibility.Collapsed;
            GridSingleLight.Visibility = Visibility.Visible;
            GridLightsOverview.Visibility = Visibility.Collapsed;

            LightHelper lightHelper = (LightHelper)LvLightsList.SelectedItem;

            lblIdSingleLight.Content = lightHelper.ID;

            switch (lightHelper.Type)
            {

                case "Color temperature light":

                    CbSingleLightIsOn.IsChecked = lightHelper.State.On;
                    SlBrightnessSingleLight.Value = lightHelper.State.Bri;
                    lblSingleLightName.Content = lightHelper.Name;

                    SlColorCT.Maximum = lightHelper.Capabilities.Control.Ct.Max;
                    SlColorCT.Minimum = lightHelper.Capabilities.Control.Ct.Min;
                    SlColorCT.Value = lightHelper.State.Ct;

                    SlColorHue.Visibility = Visibility.Collapsed;
                    SlColorCT.Visibility = Visibility.Visible;
                    BdColorChangegable.Visibility = Visibility.Visible;
                    GridXYColorSelector.Visibility = Visibility.Collapsed;
                    TbBrightnessValueSingleLight.IsEnabled = true;
                    SlBrightnessSingleLight.IsEnabled = true;

                    try
                    {
                        double kelvin = ColorConversions.midToKelvin(SlColorCT.Value);
                        string rgb = ColorConversions.colorTemperatureToRGB(Convert.ToInt32(kelvin));
                        string hex = "#" + ColorConversions.rgbToHex(rgb);

                        var bc = new BrushConverter();
                        lblColorSampleSingleLight.Background = (Brush?)bc.ConvertFrom(hex);
                    }
                    catch (Exception) { }



                    break;

                // Simple on/off plug
                case "On/Off plug-in unit":

                    SlColorHue.Visibility = Visibility.Collapsed;
                    SlColorCT.Visibility = Visibility.Collapsed;
                    BdColorChangegable.Visibility = Visibility.Collapsed;
                    GridXYColorSelector.Visibility = Visibility.Collapsed;
                    TbBrightnessValueSingleLight.IsEnabled = false;
                    SlBrightnessSingleLight.IsEnabled = false;

                    break;

                // lights like the Ikea Color bulbs with XY-Color Control
                case "Color light":

                    CbSingleLightIsOn.IsChecked = lightHelper.State.On;
                    SlBrightnessSingleLight.Value = lightHelper.State.Bri;
                    lblSingleLightName.Content = lightHelper.Name;

                    SlColorHue.Visibility = Visibility.Collapsed;
                    SlColorCT.Visibility = Visibility.Collapsed;
                    BdColorChangegable.Visibility = Visibility.Visible;
                    GridXYColorSelector.Visibility = Visibility.Visible;
                    TbBrightnessValueSingleLight.IsEnabled = true;
                    SlBrightnessSingleLight.IsEnabled = true;

                    double zwischenX1 = lightHelper.State.Xy[0] * 100;
                    double zwischenY1 = lightHelper.State.Xy[1] * 100;
                    double zwischenX2 = constValue / zwischenX1;
                    double zwischenY2 = constValue / zwischenY1;
                    SliderX.Value = 1 / zwischenX2;
                    SliderY.Value = 1 / zwischenY2;

                    if (SliderX.Value != 0 && SliderY.Value != 0)
                    {
                        try
                        {
                            double zwischenX = SliderX.Value * constValue;
                            double zwischenY = SliderY.Value * constValue;
                            double X = zwischenX / 100;
                            double Y = zwischenY / 100;

                            string rgb = ColorConversions.XYZtoRGB(X, Y, SlBrightnessSingleLight.Value);
                            string hex = "#" + ColorConversions.rgbToHex(rgb);
                            var bc = new BrushConverter();
                            lblColorSampleSingleLight.Background = (Brush)bc.ConvertFrom(hex);
                        }
                        catch (Exception) { }
                    }
                    break;

                // lights like the Hue Spot with Hue-Color Control
                case "Extended color light":

                    CbSingleLightIsOn.IsChecked = lightHelper.State.On;
                    SlBrightnessSingleLight.Value = lightHelper.State.Bri;
                    lblSingleLightName.Content = lightHelper.Name;
                    SlColorHue.Value = lightHelper.State.Hue;

                    SlColorHue.Visibility = Visibility.Visible;
                    SlColorCT.Visibility = Visibility.Collapsed;
                    BdColorChangegable.Visibility = Visibility.Visible;
                    GridXYColorSelector.Visibility = Visibility.Collapsed;
                    TbBrightnessValueSingleLight.IsEnabled = true;
                    SlBrightnessSingleLight.IsEnabled = true;

                    break;

                default:
                    MessageBox.Show("The light/device you selected isn´t registered yet. \r\n Please contact the Developer!", "Error", MessageBoxButton.OK);
                    break;
            }

            CbSingleLightIsOn.IsChecked = lightHelper.State.On;
            SlBrightnessSingleLight.Value = lightHelper.State.Bri;
            lblSingleLightName.Content = lightHelper.Name;
        }


        private void TbRooms_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)TbRooms.IsChecked)
            {
                GridRoomsOverview.Visibility = Visibility.Visible;
                GridSingleRoom.Visibility = Visibility.Collapsed;
                GridSingleLight.Visibility = Visibility.Collapsed;
                GridLightsOverview.Visibility = Visibility.Collapsed;
                GridSettings.Visibility = Visibility.Collapsed;
            }
        }

        private void TbLights_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)TbLights.IsChecked)
            {
                GridRoomsOverview.Visibility = Visibility.Collapsed;
                GridSingleRoom.Visibility = Visibility.Collapsed;
                GridSingleLight.Visibility = Visibility.Collapsed;
                GridLightsOverview.Visibility = Visibility.Visible;
                GridSettings.Visibility = Visibility.Collapsed;
            }
        }


        private void LvSingleRoomsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LightHelper lightHelper = (LightHelper)LvSingleRoomsList.SelectedItem;

            if (lightHelper == null)
            {
                return;
            }

            lblIdSingleLight.Content = lightHelper.ID;

            GridSettings.Visibility = Visibility.Collapsed;
            GridRoomsOverview.Visibility = Visibility.Collapsed;
            GridSingleRoom.Visibility = Visibility.Collapsed;
            GridSingleLight.Visibility = Visibility.Visible;
            GridLightsOverview.Visibility = Visibility.Collapsed;

            switch (lightHelper.Type)
            {

                case "Color temperature light":

                    CbSingleLightIsOn.IsChecked = lightHelper.State.On;
                    SlBrightnessSingleLight.Value = lightHelper.State.Bri;
                    lblSingleLightName.Content = lightHelper.Name;

                    SlColorCT.Maximum = lightHelper.Capabilities.Control.Ct.Max;
                    SlColorCT.Minimum = lightHelper.Capabilities.Control.Ct.Min;
                    SlColorCT.Value = lightHelper.State.Ct;

                    SlColorHue.Visibility = Visibility.Collapsed;
                    SlColorCT.Visibility = Visibility.Visible;
                    BdColorChangegable.Visibility = Visibility.Visible;
                    GridXYColorSelector.Visibility = Visibility.Collapsed;
                    TbBrightnessValueSingleLight.IsEnabled = true;
                    SlBrightnessSingleLight.IsEnabled = true;

                    try
                    {
                        double kelvin = ColorConversions.midToKelvin(SlColorCT.Value);
                        string rgb = ColorConversions.colorTemperatureToRGB(Convert.ToInt32(kelvin));
                        string hex = "#" + ColorConversions.rgbToHex(rgb);

                        var bc = new BrushConverter();
                        lblColorSampleSingleLight.Background = (Brush)bc.ConvertFrom(hex);
                    }
                    catch (Exception) { }

                    break;

                // Simple on/off plug
                case "On/Off plug-in unit":

                    SlColorHue.Visibility = Visibility.Collapsed;
                    SlColorCT.Visibility = Visibility.Collapsed;
                    BdColorChangegable.Visibility = Visibility.Collapsed;
                    GridXYColorSelector.Visibility = Visibility.Collapsed;
                    TbBrightnessValueSingleLight.IsEnabled = false;
                    SlBrightnessSingleLight.IsEnabled = false;

                    break;

                // lights like the Ikea Color bulbs with XY-Color Control
                case "Color light":

                    CbSingleLightIsOn.IsChecked = lightHelper.State.On;
                    SlBrightnessSingleLight.Value = lightHelper.State.Bri;
                    lblSingleLightName.Content = lightHelper.Name;

                    SlColorHue.Visibility = Visibility.Collapsed;
                    SlColorCT.Visibility = Visibility.Collapsed;
                    BdColorChangegable.Visibility = Visibility.Visible;
                    GridXYColorSelector.Visibility = Visibility.Visible;
                    TbBrightnessValueSingleLight.IsEnabled = true;
                    SlBrightnessSingleLight.IsEnabled = true;

                    double zwischenX1 = lightHelper.State.Xy[0] * 100;
                    double zwischenY1 = lightHelper.State.Xy[1] * 100;
                    double zwischenX2 = constValue / zwischenX1;
                    double zwischenY2 = constValue / zwischenY1;
                    SliderX.Value = 1 / zwischenX2;
                    SliderY.Value = 1 / zwischenY2;

                    if (SliderX.Value != 0 && SliderY.Value != 0)
                    {
                        try
                        {
                            double zwischenX = SliderX.Value * constValue;
                            double zwischenY = SliderY.Value * constValue;
                            double X = zwischenX / 100;
                            double Y = zwischenY / 100;

                            string rgb = ColorConversions.XYZtoRGB(X, Y, SlBrightnessSingleLight.Value);
                            string hex = "#" + ColorConversions.rgbToHex(rgb);
                            var bc = new BrushConverter();
                            lblColorSampleSingleLight.Background = (Brush)bc.ConvertFrom(hex);
                        }
                        catch (Exception) { }
                    }
                    break;

                // lights like the Hue Spot with Hue-Color Control
                case "Extended color light":

                    CbSingleLightIsOn.IsChecked = lightHelper.State.On;
                    SlBrightnessSingleLight.Value = lightHelper.State.Bri;
                    lblSingleLightName.Content = lightHelper.Name;
                    SlColorHue.Value = lightHelper.State.Hue;

                    SlColorHue.Visibility = Visibility.Visible;
                    SlColorCT.Visibility = Visibility.Collapsed;
                    BdColorChangegable.Visibility = Visibility.Visible;
                    GridXYColorSelector.Visibility = Visibility.Collapsed;
                    TbBrightnessValueSingleLight.IsEnabled = true;
                    SlBrightnessSingleLight.IsEnabled = true;

                    break;

                default:
                    MessageBox.Show("The light/device you selected isn´t registered yet. \r\n Please contact the Developer!", "Error", MessageBoxButton.OK);
                    break;
            }

            CbSingleLightIsOn.IsChecked = lightHelper.State.On;
            SlBrightnessSingleLight.Value = lightHelper.State.Bri;
            lblSingleLightName.Content = lightHelper.Name;
        }

        private void LvLightsOverviewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LightHelper lightHelper = (LightHelper)LvLightsOverviewList.SelectedItem;

            if (lightHelper == null)
            {
                return;
            }

            lblIdSingleLight.Content = lightHelper.ID;

            GridSettings.Visibility = Visibility.Collapsed;
            GridRoomsOverview.Visibility = Visibility.Collapsed;
            GridSingleRoom.Visibility = Visibility.Collapsed;
            GridSingleLight.Visibility = Visibility.Visible;
            GridLightsOverview.Visibility = Visibility.Collapsed;

            switch (lightHelper.Type)
            {

                case "Color temperature light":

                    CbSingleLightIsOn.IsChecked = lightHelper.State.On;
                    SlBrightnessSingleLight.Value = lightHelper.State.Bri;
                    lblSingleLightName.Content = lightHelper.Name;

                    SlColorCT.Maximum = lightHelper.Capabilities.Control.Ct.Max;
                    SlColorCT.Minimum = lightHelper.Capabilities.Control.Ct.Min;
                    SlColorCT.Value = lightHelper.State.Ct;

                    SlColorHue.Visibility = Visibility.Collapsed;
                    SlColorCT.Visibility = Visibility.Visible;
                    BdColorChangegable.Visibility = Visibility.Visible;
                    GridXYColorSelector.Visibility = Visibility.Collapsed;
                    TbBrightnessValueSingleLight.IsEnabled = true;
                    SlBrightnessSingleLight.IsEnabled = true;

                    try
                    {
                        double kelvin = ColorConversions.midToKelvin(SlColorCT.Value);
                        string rgb = ColorConversions.colorTemperatureToRGB(Convert.ToInt32(kelvin));
                        string hex = "#" + ColorConversions.rgbToHex(rgb);

                        var bc = new BrushConverter();
                        lblColorSampleSingleLight.Background = (Brush)bc.ConvertFrom(hex);
                    }
                    catch (Exception) { }

                    break;

                // Simple on/off plug
                case "On/Off plug-in unit":

                    SlColorHue.Visibility = Visibility.Collapsed;
                    SlColorCT.Visibility = Visibility.Collapsed;
                    BdColorChangegable.Visibility = Visibility.Collapsed;
                    GridXYColorSelector.Visibility = Visibility.Collapsed;
                    TbBrightnessValueSingleLight.IsEnabled = false;
                    SlBrightnessSingleLight.IsEnabled = false;

                    break;

                // lights like the Ikea Color bulbs with XY-Color Control
                case "Color light":

                    CbSingleLightIsOn.IsChecked = lightHelper.State.On;
                    SlBrightnessSingleLight.Value = lightHelper.State.Bri;
                    lblSingleLightName.Content = lightHelper.Name;

                    SlColorHue.Visibility = Visibility.Collapsed;
                    SlColorCT.Visibility = Visibility.Collapsed;
                    BdColorChangegable.Visibility = Visibility.Visible;
                    GridXYColorSelector.Visibility = Visibility.Visible;
                    TbBrightnessValueSingleLight.IsEnabled = true;
                    SlBrightnessSingleLight.IsEnabled = true;

                    double zwischenX1 = lightHelper.State.Xy[0] * 100;
                    double zwischenY1 = lightHelper.State.Xy[1] * 100;
                    double zwischenX2 = constValue / zwischenX1;
                    double zwischenY2 = constValue / zwischenY1;
                    SliderX.Value = 1 / zwischenX2;
                    SliderY.Value = 1 / zwischenY2;

                    if (SliderX.Value != 0 && SliderY.Value != 0)
                    {
                        try
                        {
                            double zwischenX = SliderX.Value * constValue;
                            double zwischenY = SliderY.Value * constValue;
                            double X = zwischenX / 100;
                            double Y = zwischenY / 100;

                            string rgb = ColorConversions.XYZtoRGB(X, Y, SlBrightnessSingleLight.Value);
                            string hex = "#" + ColorConversions.rgbToHex(rgb);
                            var bc = new BrushConverter();
                            lblColorSampleSingleLight.Background = (Brush)bc.ConvertFrom(hex);
                        }
                        catch (Exception) { }
                    }
                    break;

                // lights like the Hue Spot with Hue-Color Control
                case "Extended color light":

                    CbSingleLightIsOn.IsChecked = lightHelper.State.On;
                    SlBrightnessSingleLight.Value = lightHelper.State.Bri;
                    lblSingleLightName.Content = lightHelper.Name;
                    SlColorHue.Value = lightHelper.State.Hue;

                    SlColorHue.Visibility = Visibility.Visible;
                    SlColorCT.Visibility = Visibility.Collapsed;
                    BdColorChangegable.Visibility = Visibility.Visible;
                    GridXYColorSelector.Visibility = Visibility.Collapsed;
                    TbBrightnessValueSingleLight.IsEnabled = true;
                    SlBrightnessSingleLight.IsEnabled = true;

                    break;

                default:
                    MessageBox.Show("The light/device you selected isn´t registered yet. \r\n Please contact the Developer!", "Error", MessageBoxButton.OK);
                    break;
            }

            CbSingleLightIsOn.IsChecked = lightHelper.State.On;
            SlBrightnessSingleLight.Value = lightHelper.State.Bri;
            lblSingleLightName.Content = lightHelper.Name;
        }
        #endregion

        #region Value Changed

        private void SliderXY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SliderX.Value != 0 && SliderY.Value != 0)
            {
                double X = 0;
                double Y = 0;
                try
                {
                    double zwischenX = SliderX.Value * constValue;
                    double zwischenY = SliderY.Value * constValue;
                    X = zwischenX / 100;
                    Y = zwischenY / 100;

                    string rgb = ColorConversions.XYZtoRGB(X, Y, SlBrightnessSingleLight.Value);
                    string hex = "#" + ColorConversions.rgbToHex(rgb);
                    var bc = new BrushConverter();
                    lblColorSampleSingleLight.Background = (Brush)bc.ConvertFrom(hex);
                }
                catch (Exception) { }

                try
                {
                    List<double> xy = new List<double>();
                    xy.Add(X);
                    xy.Add(Y);
                    var data = new ChangeColor(5, xy).ToJson();
                    HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", lblIdSingleLight.Content, "state"), data);
                }
                catch (Exception) { }
            }
        }

        private void SlColorCT_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                double kelvin = ColorConversions.midToKelvin(SlColorCT.Value);
                string rgb = ColorConversions.colorTemperatureToRGB(Convert.ToInt32(kelvin));
                string hex = "#" + ColorConversions.rgbToHex(rgb);

                var bc = new BrushConverter();
                lblColorSampleSingleLight.Background = (Brush)bc.ConvertFrom(hex);
            }
            catch (Exception) { }

            try
            {
                var data = new ChangeColor(Convert.ToInt64(SlColorCT.Value), 5).ToJson();
                HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", lblIdSingleLight.Content, "state"), data);
            }
            catch (Exception) { }
        }

        private void SlColorHue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double hue = SlColorHue.Value;

            double cons = 360.0 / 65535.0;

            double degree = cons * hue;

            double brightness = (100.0 / 254.0) * SlBrightnessSingleLight.Value;

            HSV hsv = new HSV(Convert.ToInt32(degree), Convert.ToByte(100), Convert.ToByte(brightness));
            HEX hex = ColorHelper.ColorConverter.HsvToHex(hsv);

            string hEx = "#" + hex.ToString();

            var bc = new BrushConverter();
            lblColorSampleSingleLight.Background = (Brush)bc.ConvertFrom(hEx);

            try
            {
                var data = new ChangeColor(5, Convert.ToInt64(SlColorHue.Value)).ToJson();
                HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", lblIdSingleLight.Content, "state"), data);
            }
            catch (Exception) { }
        }

        private void SlBrightnessSingleLight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                var data = new ChangeBrightness(5, Convert.ToInt64(SlBrightnessSingleLight.Value)).ToJson();
                HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", lblIdSingleLight.Content, "state"), data);
            }
            catch (Exception) { }
        }

        private void CbSingleLightIsOn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = new TurnOnOff((bool)CbSingleLightIsOn.IsChecked).ToJson();
                HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", lblIdSingleLight.Content, "state"), data);
            }
            catch (Exception) { }
        }



        // SingleRoom IsOnChanged
        private void CbIsOnSingleRoom_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            var item = GetAncestorOfType<ListViewItem>(sender as CheckBox);
            LightHelper light = item.DataContext as LightHelper;

            try
            {
                var data = new TurnOnOff((bool)cb.IsChecked).ToJson();
                HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", light.ID, "state"), data);
            }
            catch (Exception) { }
        }

        // SingleRoom BrightnessChanged
        private void SlBrightnessSingleRoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider sl = (Slider)sender;

            var item = GetAncestorOfType<ListViewItem>(sender as Slider);
            LightHelper light = item.DataContext as LightHelper;

            try
            {
                var data = new ChangeBrightness(5, Convert.ToInt64(sl.Value)).ToJson();
                HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", light.ID, "state"), data);
            }
            catch (Exception) { }
        }



        // RoomOverview BrightnessChanged
        private void SlBrightnessRoomOverview_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider sl = (Slider)sender;

            var item = GetAncestorOfType<ListViewItem>(sender as Slider);
            GroupHelper group = item.DataContext as GroupHelper;

            var data = new ChangeBrightness(5, Convert.ToInt64(sl.Value)).ToJson();
            HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "groups", group.ID, "action"), data);
        }

        // RoomOverview IsOnChanged
        private void CbIsOnRoomOverView_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            var item = GetAncestorOfType<ListViewItem>(sender as CheckBox);
            GroupHelper group = item.DataContext as GroupHelper;

            var data = new TurnOnOff((bool)cb.IsChecked).ToJson();
            HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "groups", group.ID, "action"), data);

        }

        // LightsOverview BrightnessChanged
        private void SlBrightnessLightsOverView_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider sl = (Slider)sender;

            var item = GetAncestorOfType<ListViewItem>(sender as Slider);
            LightHelper light = item.DataContext as LightHelper;

            var data = new ChangeBrightness(5, Convert.ToInt64(sl.Value)).ToJson();
            HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", light.ID, "state"), data);
        }

        // LightsOverview IsOnChanged
        private void CbIsOnLightsOverView_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            var item = GetAncestorOfType<ListViewItem>(sender as CheckBox);
            LightHelper? light = item.DataContext as LightHelper;

            var data = new TurnOnOff((bool)cb.IsChecked).ToJson();
            HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", light.ID, "state"), data);
        }

        #endregion
        
        #region Settings
        private void ImgSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridSettings.Visibility = Visibility.Visible;
            GridMainView.Visibility = Visibility.Collapsed;
        }


        private int TryIp(string IP)
        {
            string result = "";

            try
            {
                result = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, IP, "TestUserCode", "lights"));
                // Right IP
                return 0;
            }
            catch (Exception)
            {
                // Wrong IP
                return 1;
            }
        }

        private int TryUsername(string IP, string Username)
        {
            string result = "";

            try
            {
                result = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, IP, Username, "lights"));
            }
            catch (Exception)
            {
                // Ip not correct
                return 0;
            }

            if (result != "" && result != null && !result.Contains("error"))
            {
                // Success (IP and Username correct)
                return 1;
            }
            else
            {
                // Username not correct
                return 2;
            }
        }

        private string SearchBridge()
        {
            HueLogic.BridgeIP = "";

            var hueLogic = new HueLogic();
            hueLogic.FindBridgeIP();

            return HueLogic.BridgeIP;
        }

        private void BtnSearchBridge_Click(object sender, RoutedEventArgs e)
        {
            txtIp.Text = SearchBridge();
        }
        

        private void BtnCreateUser_Click(object sender, RoutedEventArgs e)
        {

            GridCreateUser.Visibility = Visibility.Visible;
            TbIpCreateUser.Visibility = Visibility.Collapsed;

            lblCreateUserState.Content = "Searching for Hue Bridge";
            TbIpCreateUser.Text = "";
            BtnCreateUserContinue.IsEnabled = true;
            BtnCreateUserContinue.Visibility = Visibility.Collapsed;

            Thread thread = new Thread(CreatingUser);
            thread.Start();
        }

        private void CreatingUser()
        {
            bool CreationSuccessfull = false;
            loop = false;
            string IP = "";

            // Search IP
            // Dispatcher, so that if-clause can access txtIP.Text
            Dispatcher.Invoke(new System.Action(delegate
            {
                lblCreateUserState.Content = "Searching for your Hue Bridge...";
                if(txtIp.Text != null && txtIp.Text != "" && !firstStart)
                {
                    int i = TryIp(txtIp.Text);

                    if(i == 0)
                    {
                        // IP correct
                        HueLogic.BridgeIP = txtIp.Text;
                        IP = HueLogic.BridgeIP;
                        // No action neccessary- wait for if-else to end
                    }
                    else
                    {
                        IP = SearchBridge();
                        if (IP.Contains("no bridge found"))
                        {
                            // No IP - enter manual
                            lblCreateUserState.Content = "Please enter the IP of your Bridge:";
                            BtnCreateUserContinue.Visibility = Visibility.Visible;
                            TbIpCreateUser.Visibility = Visibility.Visible;
                            return;
                        }
                        HueLogic.BridgeIP = IP;
                    }
                }
                else
                {
                    IP = SearchBridge();
                    if (IP.Contains("no bridge found"))
                    {
                        // No IP - enter manual
                        lblCreateUserState.Content = "Please enter the IP of your Bridge:";
                        BtnCreateUserContinue.Visibility = Visibility.Visible;
                        TbIpCreateUser.Visibility = Visibility.Visible;
                        return;
                    }
                    HueLogic.BridgeIP = IP;
                }
            }));
            

            // Trying to create user here in loop

            loop = true;

            Dispatcher.Invoke(new System.Action(delegate
            {
                lblCreateUserState.Content = "Please press the Button on your Bridge";
            }));
            while (loop)
            {
                try
                {
                    // Create random int for the username
                    Random rand = new Random();
                    int i = rand.Next(100);
                    string result = "";
                    try
                    {
                        result = HueLogic.ConnectBridge("HueControlID" + i.ToString());
                    }
                    catch (Exception) { }
                    if (!result.Contains("link button not pressed"))
                    {
                        loop = false;
                        CreationSuccessfull = true;
                    }
                }
                catch (Exception) { }
            }

            if (CreationSuccessfull)
            {
                // Actions after successfull creation
                Dispatcher.Invoke(new System.Action(delegate
                {
                    txtIp.Text = IP;
                    HueLogic.BridgeIP = IP;
                    txtUsername.Text = HueLogic.Usercode;
                    BtnCreateUserCancel.Visibility = Visibility.Visible;
                }));
                Properties.Settings.Default.BridgeIP = IP;
                Properties.Settings.Default.Usercode = HueLogic.Usercode;
                Properties.Settings.Default.Save();
            }
            else
            {
                // Creation canceled
                CreationSuccessfull = false;
                loop = false;
                Dispatcher.Invoke(new System.Action(delegate
                {
                    GridCreateUser.Visibility = Visibility.Collapsed;
                })); 
            }
        }

        private void BtnCreateUserContinue_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(CreateUserContinue);
            t.Start();
            BtnCreateUserContinue.IsEnabled = false;
        }

        private void CreateUserContinue()
        {
            bool CreationSuccessfull = false;
            loop = false;
            string IP = "";

            // The creating progress started with an IP-Check

            //IP Check
            int i = 0;
            Dispatcher.Invoke(new System.Action(delegate
            {
                i = TryIp(TbIpCreateUser.Text);
                lblCreateUserState.Content = "Checking the IP adress";
            }));

            if (i == 0)
            {
                // Right IP
                Dispatcher.Invoke(new System.Action(delegate
                {
                    IP = TbIpCreateUser.Text;
                }));
            }else
            {
                // Wrong IP
                Dispatcher.Invoke(new System.Action(delegate
                {
                    BtnCreateUserContinue.IsEnabled = true;
                    lblCreateUserState.Content = "Please enter the IP of your Bridge:";
                }));                
                return;
            }


            // Trying to create user here in loop

            loop = true;

            Dispatcher.Invoke(new System.Action(delegate
            {
                lblCreateUserState.Content = "Please press the Button on your Bridge";
            }));
            while (loop)
            {
                try
                {
                    // Create random int for the username
                    Random rand = new Random();
                    int j = rand.Next(100);
                    string result = "";
                    try
                    {
                        result = HueLogic.ConnectBridge("HueControlID" + j.ToString());
                    }
                    catch (Exception) { }
                    if (!result.Contains("link button not pressed"))
                    {
                        loop = false;
                        CreationSuccessfull = true;
                    }
                }
                catch (Exception) { }
            }

            if (CreationSuccessfull)
            {
                // Actions after successfull creation
                Dispatcher.Invoke(new System.Action(delegate
                {
                    txtIp.Text = IP;
                    HueLogic.BridgeIP = IP;
                    txtUsername.Text = HueLogic.Usercode;
                    BtnCreateUserContinue.IsEnabled = true;
                    BtnCreateUserCancel.Visibility = Visibility.Visible;
                }));
                Properties.Settings.Default.BridgeIP = IP;
                Properties.Settings.Default.Usercode = HueLogic.Usercode;
                Properties.Settings.Default.Save();
            }
            else
            {
                // Creation canceled
                CreationSuccessfull = false;
                loop = false;
                Dispatcher.Invoke(new System.Action(delegate
                {
                    GridCreateUser.Visibility = Visibility.Collapsed;
                    BtnCreateUserContinue.IsEnabled = true;                    
                })); 
            }
        }

        private void BtnCreateUserCancel_Click(object sender, RoutedEventArgs e)
        {
            loop = false;
            
            GridCreateUser.Visibility = Visibility.Collapsed;
            TbIpCreateUser.Visibility = Visibility.Collapsed;

            lblCreateUserState.Content = "Searching for Hue Bridge";
            TbIpCreateUser.Visibility = Visibility.Collapsed;

            BtnCreateUserContinue.IsEnabled = true;
            BtnCreateUserContinue.Visibility = Visibility.Collapsed;
            GridSettings.Visibility = Visibility.Visible;
            GridMainView.Visibility = Visibility.Collapsed;
        }

        private void BtnCheckConnection_Click(object sender, RoutedEventArgs e)
        {
            string result2 = "";

            Cursor = Cursors.Wait;

            try
            {
                result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights"));
            }
            catch (Exception)
            {
                lblCheckConnectionResult.Content = "IP not correct";
                lblCheckConnectionResult.Foreground = Brushes.Red;
            }

            if (result2 != "" && !result2.Contains("error"))
            {
                lblCheckConnectionResult.Content = "Connection successfull";
                lblCheckConnectionResult.Foreground = Brushes.Green;
            }
            else
            {
                lblCheckConnectionResult.Content = "Username not correct";
                lblCheckConnectionResult.Foreground = Brushes.Red;
            }

            Cursor = Cursors.Arrow;

        }

        private void BtnSettingsClose_Click(object sender, RoutedEventArgs e)
        {
            GridSettings.Visibility = Visibility.Collapsed;
            GridMainView.Visibility = Visibility.Visible;
        }

        private void BtnSettingsApply_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.BridgeIP = txtIp.Text;
            Properties.Settings.Default.Usercode = txtUsername.Text;
            HueLogic.BridgeIP = txtIp.Text;
            HueLogic.Usercode = txtUsername.Text;
            Properties.Settings.Default.Save();
        }

        private void BtnSettingsApllyClose_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.BridgeIP = txtIp.Text;
            Properties.Settings.Default.Usercode = txtUsername.Text;
            HueLogic.BridgeIP = txtIp.Text;
            HueLogic.Usercode = txtUsername.Text;
            Properties.Settings.Default.Save();
            GridSettings.Visibility = Visibility.Collapsed;
            GridMainView.Visibility = Visibility.Visible;
        }

        #endregion


        public T GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return (T)GetAncestorOfType<T>((FrameworkElement)parent);
            return (T)parent;
        }

    }
}
