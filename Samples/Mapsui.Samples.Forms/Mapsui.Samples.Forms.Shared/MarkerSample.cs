using System;
using System.IO;
using System.Reflection;
using Mapsui.Rendering.Skia;
using Mapsui.Samples.Common.Maps;
using Mapsui.UI;
using Mapsui.UI.Forms;
using Xamarin.Forms;

namespace Mapsui.Samples.Forms.Shared
{
    public class MarkerSample : IFormsSample
    {
        int _markerNum = 1;
        readonly Random _random = new Random();

        public string Name => "Add Marker Sample";

        public string Category => "Forms";

        public bool OnClick(object sender, EventArgs args)
        {
            if (sender is MapViewEx mapView)
            {
                if(!mapView.HasLayer("BasicMarkers"))
                {
                    mapView.AddCustomLayer<BasicMarker>("BasicMarkers");
                }

                var mapClickedArgs = (MapClickedEventArgs)args;

                var assembly = typeof(MainPageLarge).GetTypeInfo().Assembly;
                foreach (var str in assembly.GetManifestResourceNames())
                    System.Diagnostics.Debug.WriteLine(str);

                var marker = new BasicMarker(mapView)
                {
                    Label = $"PinType.Pin {_markerNum++}",
                    Position = mapClickedArgs.Point,
                    Transparency = 0.5f,
                    Scale = _random.Next(50, 130) / 100f,
                };

                mapView["BasicMarkers"].AddSymbol(marker);

                return true;
            }

            return false;
        }

        public void Setup(IMapControl mapControl)
        {
            //I like bing Hybrid
            mapControl.Map = BingSample.CreateMap(BruTile.Predefined.KnownTileSource.BingHybrid);

            ((IMapView)mapControl).UseDoubleTap = true;
        }
    }
}
