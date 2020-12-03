using Mapsui.Layers;
using System.Collections.Generic;
using System.Linq;
using System;
using Mapsui.UI.Forms.Extensions;

namespace Mapsui.UI.Forms
{
    public class MapViewEx : MapView
    {
        public ISymbol SelectedMarker { get; set; }

        List<CustomMemoryLayer> customLayers { get; } = new List<CustomMemoryLayer>();

        public void AddCustomLayer<Symbol>(string name, int zindex = -1) where Symbol : ISymbol
        {
            var layer = new CustomMemoryLayer<Symbol>(this, name)
            {
                ZIndexRequest = zindex,
            };
            customLayers.Add(layer);
            layer.AttachLayer();
        }

        public bool HasLayer(string name)
        {
            return customLayers.Any(x => x.Name == name);
        }

        protected override bool IsExcludedFromHandlerLayerChanged(ILayer layer)
        {
            var result = base.IsExcludedFromHandlerLayerChanged(layer);
            if(!result)
            {
                result = customLayers.Any(x => x.Layer == layer);
            }
            return result;
        }

        protected override void RemoveLayers()
        {
            base.RemoveLayers();

            foreach(var layer in customLayers.Select(x=>x.Layer))
            {
                _mapControl.Map.Layers.Remove(layer);
            }
        }

        public CustomMemoryLayer this[string name]
        {
            get
            {
                return customLayers.FirstOrDefault(x => x.Name == name);
            }
        }

        public event EventHandler<MarkerClickedEventArgs> MarkerClicked;

        /// <summary>
        /// Occurs when selected pin changed
        /// </summary>
        public event EventHandler<SelectedMarkerChangedEventArgs> SelectedMarkerChanged;

        protected override void HandlerInfo(object sender, MapInfoEventArgs e)
        {
            base.HandlerInfo(sender, e);

            if (customLayers.FirstOrDefault(x => x.Name == e.MapInfo?.Layer?.Name) is CustomMemoryLayer layer)
            {
                ISymbol clickedMarker = null;
                var symbols = layer.RawData.ToList();

                foreach (var symbol in symbols)
                {
                    if (symbol.IsVisible && symbol.Feature.Equals(e.MapInfo.Feature))
                    {
                        clickedMarker = symbol;
                        break;
                    }
                }

                if (clickedMarker != null)
                {
                    SelectedMarker = clickedMarker;

                    SelectedMarkerChanged?.Invoke(this, new SelectedMarkerChangedEventArgs(SelectedMarker));

                    var args = new MarkerClickedEventArgs(clickedMarker, _mapControl.Viewport.ScreenToWorld(e.MapInfo.ScreenPosition).ToForms(), e.NumTaps);

                    MarkerClicked?.Invoke(this, args);

                    if (args.Handled)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
        }
    }
}
