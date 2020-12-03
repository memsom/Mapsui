using System;
using System.IO;
using System.Reflection;
using Mapsui.UI.Forms;

namespace Mapsui.Samples.Forms.Shared
{
    public class BasicMarker : Marker
    {
        public BasicMarker(): base()
        {

        }

        public BasicMarker(IMapView mapView) : base(mapView)
        {

        }

        protected override void LoadSvgData()
        {
            var assembly = typeof(BasicMarker).GetTypeInfo().Assembly;
            using var image = assembly.GetManifestResourceStream("Mapsui.Samples.Forms.Shared.Images.Marker.svg");

            if (image == null) throw new ArgumentException("EmbeddedResource not found");
            else
            {
                using var sr = new StreamReader(image);
                Svg = sr.ReadToEnd();
            }
        }
    }
}
