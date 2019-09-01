using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class PriceIncreasePresetsProvider : IPriceIncreasePresetsProvider
    {
        private readonly ISettings _settings;

        public PriceIncreasePresetsProvider(ISettings settings)
        {
            _settings = settings;
        }

        public (double Percentage, int Offset) GetPreset()
        {
            if (!_settings.UsePriceIncreaseProxyPresets)
                return (0, 0);

            switch (_settings.ProxyDomain)
            {
                case ProxyDomain.None:
                    return (0, 0);
                case ProxyDomain.ZenMarket:
                    return (3.5, 300);
                case ProxyDomain.FromJapan:
                    return (0, 300);
                case ProxyDomain.Neokyo:
                    return (3.6,250);
                case ProxyDomain.DeJapan:
                    return (0, 300);
                default:
                    return (0, 0);
            }
        }
    }
}
