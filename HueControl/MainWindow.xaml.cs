﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<LightHelper> lights;

        double constValue = 100 / 347.5;


        public MainWindow()
        {
            InitializeComponent();

            HueLogic.Usercode = txtUsername.Text;
            HueLogic.BridgeIP = txtIp.Text;
            string result = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "groups"));
            List<GroupHelper> groups = GroupHelper.FromJson(result);
            LvRoomsList.ItemsSource = groups;

            ListViewRoomsOverview.ItemsSource = groups;

            string result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights"));
            lights = LightHelper.FromJson(result2);
            LvLightsList.ItemsSource = lights;
            LvLightsOverviewList.ItemsSource = lights;

            GridRoomsOverview.Visibility = Visibility.Visible;
            GridSingleRoom.Visibility = Visibility.Collapsed;
            GridSingleLight.Visibility = Visibility.Collapsed;
            GridLightsOverview.Visibility = Visibility.Collapsed;
            GridSettings.Visibility = Visibility.Collapsed;
        }


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
            LightHelper light = item.DataContext as LightHelper;

            var data = new TurnOnOff((bool)cb.IsChecked).ToJson();
            HueLogic.PutRequestToBridge(string.Format(HueLogic.ControlLightUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights", light.ID, "state"), data);
        }


        public T GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return (T)GetAncestorOfType<T>((FrameworkElement)parent);
            return (T)parent;
        }

        private void ImgSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridSettings.Visibility = Visibility.Visible;
            GridRoomsOverview.Visibility = Visibility.Collapsed;
            GridSingleRoom.Visibility = Visibility.Collapsed;
            GridSingleLight.Visibility = Visibility.Collapsed;
            GridLightsOverview.Visibility = Visibility.Collapsed;
        }


        private void BtnSearchBridge_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(SearchBridge);
            thread.Start();
        }

        private void SearchBridge()
        {
            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                Cursor = Cursors.Wait;
            }));

            var hueLogic = new HueLogic();
            hueLogic.FindBridgeIP();

            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                txtIp.Text = HueLogic.BridgeIP;
                TbIpCreateUser.Text = HueLogic.BridgeIP;
            }));

            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                Cursor = Cursors.Arrow;
            }));
        }

        private void BtnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            // Hier muss erst noch die aufforderung zum Button-Press kommen:

            GridCreateUser.Visibility = Visibility.Visible;
            TbIpCreateUser.Visibility = Visibility.Collapsed;

            Thread thread = new Thread(SearchBridge);
            thread.Start();

            if (!TbIpCreateUser.Text.Equals("no bridge found"))
            {
                TbIpCreateUser.Visibility = Visibility.Visible;
                lblCreateUserState.Content = "Press the pair button on your Hue Bridge";

                // Trying to create user here in loop


            }


            if (!string.IsNullOrEmpty(txtUsername.Text))
            {
                string result = HueLogic.ConnectBridge(txtUsername.Text);
            }
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


    }
}
