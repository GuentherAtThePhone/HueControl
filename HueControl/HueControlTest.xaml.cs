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
    /// Interaktionslogik für HueControlTest.xaml
    /// </summary>
    public partial class HueControlTest : Window
    {

        List<LightHelper> lights;
        readonly double constValue = 100 / 347.5;

        bool loop = false;

        public HueControlTest()
        {
            InitializeComponent();

            txtUsername.Text = Properties.Settings.Default.Usercode;
            txtIp.Text = Properties.Settings.Default.BridgeIP;

            HueLogic.Usercode = txtUsername.Text;
            HueLogic.BridgeIP = txtIp.Text;

            if (HueLogic.Usercode == "" | HueLogic.Usercode == null | HueLogic.BridgeIP == "" | HueLogic.BridgeIP == null)
            {
                // create / search

                if (HueLogic.BridgeIP == "" | HueLogic.BridgeIP == null)
                {
                    GridSettings.Visibility = Visibility.Visible;
                    GridRoomsOverview.Visibility = Visibility.Collapsed;
                    GridSingleRoom.Visibility = Visibility.Collapsed;
                    GridSingleLight.Visibility = Visibility.Collapsed;
                    GridLightsOverview.Visibility = Visibility.Collapsed;

                    GridCreateUser.Visibility = Visibility.Visible;
                    TbIpCreateUser.Visibility = Visibility.Collapsed;

                    lblCreateUserState.Content = "Searching for Hue Bridge";
                    TbIpCreateUser.Visibility = Visibility.Collapsed;
                    TbIpCreateUser.Text = "";
                    BtnCreateUserContinue.IsEnabled = true;
                    BtnCreateUserContinue.Visibility = Visibility.Collapsed;

                    Thread thread = new Thread(CreatingUser);
                    thread.Start();
                }
                else
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
                    }

                    if (result2 != "" && !result2.Contains("error"))
                    {
                        lblCheckConnectionResult.Content = "Connection successfull";
                    }
                    else
                    {
                        //username incorrect
                    }

                    Cursor = Cursors.Arrow;
                }
            }
            else
            {
                Thread t = new Thread(StartApplication);
                t.Start();
            }
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

        public T GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return (T)GetAncestorOfType<T>((FrameworkElement)parent);
            return (T)parent;
        }

        #region Settings
        private void ImgSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridSettings.Visibility = Visibility.Visible;
            GridMainView.Visibility = Visibility.Collapsed;
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

            GridCreateUser.Visibility = Visibility.Visible;
            TbIpCreateUser.Visibility = Visibility.Collapsed;

            lblCreateUserState.Content = "Searching for Hue Bridge";
            TbIpCreateUser.Visibility = Visibility.Collapsed;
            TbIpCreateUser.Text = "";
            BtnCreateUserContinue.IsEnabled = true;
            BtnCreateUserContinue.Visibility = Visibility.Collapsed;

            Thread thread = new Thread(CreatingUser);
            thread.Start();

        }

        private void CreatingUser()
        {
            // Search IP

            SearchBridge();

            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                if (!TbIpCreateUser.Text.Equals("no bridge found"))
                {
                    TbIpCreateUser.Visibility = Visibility.Visible;
                }
                else
                {
                    lblCreateUserState.Content = "Couldn´t find a Bridge. Please enter IP manually";
                    TbIpCreateUser.Visibility = Visibility.Visible;
                    TbIpCreateUser.Text = "";
                    BtnCreateUserContinue.IsEnabled = true;
                    BtnCreateUserContinue.Visibility = Visibility.Visible;
                    return;
                }
            }));

            // Check IP

            string result2 = "";

            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                Cursor = Cursors.Wait;
            }));
            try
            {
                Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, TbIpCreateUser.Text, "TestUserCode", "lights"));
                }));
            }
            catch (Exception)
            {
                Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    lblCreateUserState.Content = "The IP seems to  be wrong. Please check it.";
                    TbIpCreateUser.Visibility = Visibility.Visible;
                    TbIpCreateUser.Text = "";
                    BtnCreateUserContinue.IsEnabled = true;
                    BtnCreateUserContinue.Visibility = Visibility.Visible;
                    Cursor = Cursors.Arrow;
                }));
                return;
            }

            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                Cursor = Cursors.Arrow;
            }));

            // Trying to create user here in loop

            loop = true;

            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                lblCreateUserState.Content = "Please press the Button on your Bridge";
            }));
            while (loop)
            {
                try
                {
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
                        Dispatcher.BeginInvoke(new System.Action(delegate
                        {
                            Clipboard.SetText(result);
                            MessageBox.Show(result);
                            loop = false;
                        }));
                    }
                }
                catch (Exception) { }
            }

            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                txtUsername.Text = HueLogic.Usercode;
                txtIp.Text = TbIpCreateUser.Text;
                Properties.Settings.Default.BridgeIP = TbIpCreateUser.Text;
                Properties.Settings.Default.Usercode = HueLogic.Usercode;
                Properties.Settings.Default.Save();

                GridCreateUser.Visibility = Visibility.Collapsed;
                TbIpCreateUser.Visibility = Visibility.Collapsed;

                lblCreateUserState.Content = "Searching for Hue Bridge";
                TbIpCreateUser.Visibility = Visibility.Collapsed;

                BtnCreateUserContinue.IsEnabled = true;
                BtnCreateUserContinue.Visibility = Visibility.Collapsed;
            }));
        }

        private void BtnCreateUserContinue_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(CreateUserContinue);
            t.Start();
            BtnCreateUserContinue.IsEnabled = false;
        }

        private void CreateUserContinue()
        {
            // Check IP

            string result2 = "";

            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                Cursor = Cursors.Wait;
            }));

            try
            {
                Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, TbIpCreateUser.Text, "TestUserCode", "lights"));
                }));
            }
            catch (Exception)
            {
                Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    lblCreateUserState.Content = "The IP seems to  be wrong. Please check it.";
                    TbIpCreateUser.Visibility = Visibility.Visible;
                    BtnCreateUserContinue.Visibility = Visibility.Visible;
                    BtnCreateUserContinue.IsEnabled = true;
                    Cursor = Cursors.Arrow;
                }));
                return;
            }

            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                Cursor = Cursors.Arrow;
            }));

            // Trying to create user here in loop

            loop = true;
            Dispatcher.BeginInvoke(new System.Action(delegate
            {
                lblCreateUserState.Content = "Please press the Button on your Bridge";
            }));
            while (loop)
            {
                try
                {
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
                        Dispatcher.BeginInvoke(new System.Action(delegate
                        {
                            Clipboard.SetText(result);
                            MessageBox.Show(result);
                            loop = false;
                        }));
                    }
                }
                catch (Exception) { }

                Dispatcher.BeginInvoke(new System.Action(delegate
                {
                    txtUsername.Text = HueLogic.Usercode;
                    txtIp.Text = TbIpCreateUser.Text;
                    Properties.Settings.Default.BridgeIP = TbIpCreateUser.Text;
                    Properties.Settings.Default.Usercode = HueLogic.Usercode;
                    Properties.Settings.Default.Save();

                    GridCreateUser.Visibility = Visibility.Collapsed;
                    TbIpCreateUser.Visibility = Visibility.Collapsed;

                    lblCreateUserState.Content = "Searching for Hue Bridge";
                    TbIpCreateUser.Visibility = Visibility.Collapsed;
                    TbIpCreateUser.Text = "";
                    BtnCreateUserContinue.IsEnabled = true;
                    BtnCreateUserContinue.Visibility = Visibility.Collapsed;
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

        }

        private void BtnSettingsApply_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.BridgeIP = txtIp.Text;
            Properties.Settings.Default.Usercode = txtUsername.Text;
            Properties.Settings.Default.Save();
        }

        private void BtnSettingsApllyClose_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void StartApplication()
        {
            Dispatcher.BeginInvoke(new System.Action(delegate {

                // Connection Check
                string result2 = "";

                Cursor = Cursors.Wait;

                try
                {
                    result2 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights"));
                }
                catch (Exception)
                {
                    // Ip not correct
                }

                if (result2 != "" && result2 != null && !result2.Contains("error"))
                {
                    // Success
                    string result = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "groups"));
                    List<GroupHelper> groups = GroupHelper.FromJson(result);
                    LvRoomsList.ItemsSource = groups;

                    ListViewRoomsOverview.ItemsSource = groups;

                    string result3 = HueLogic.GetRequestToBridge(string.Format(HueLogic.LightsUrlTemplate, HueLogic.BridgeIP, HueLogic.Usercode, "lights"));
                    lights = LightHelper.FromJson(result3);
                    LvLightsList.ItemsSource = lights;
                    LvLightsOverviewList.ItemsSource = lights;

                    GridRoomsOverview.Visibility = Visibility.Visible;
                    GridSingleRoom.Visibility = Visibility.Collapsed;
                    GridSingleLight.Visibility = Visibility.Collapsed;
                    GridLightsOverview.Visibility = Visibility.Collapsed;
                    GridSettings.Visibility = Visibility.Collapsed;

                    Cursor = Cursors.Arrow;
                }
                else
                {
                    // Username not correct
                }

            }));
        }

    }
}