using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.LinkHandlers
{
    public class DomainLinkHandlerManager : IDomainLinkHandlerManager
    {
        private readonly IEnumerable<IDomainLinkHandler> _linkHandlers;
        private readonly ISettings _settings;
        private IDomainLinkHandler _defaultHandler;

        public DomainLinkHandlerManager(
            IEnumerable<IDomainLinkHandler> linkHandlers,
            ISettings settings)
        {
            _linkHandlers = linkHandlers;
            _settings = settings;

            _defaultHandler = _linkHandlers
                .First(handler => handler.HandlingDomain == ProxyDomain.None);
        }

        public string GenerateWebsiteLink(ICrawlerResultItem item)
        {
            if (_settings.ProxyDomain != ProxyDomain.None)
            {
                var link = _linkHandlers
                    .First(handler => handler.HandlingDomain == _settings.ProxyDomain)
                    .GenerateWebsiteLink(item);
                if (link != null)
                    return link;
            }

            return _defaultHandler.GenerateWebsiteLink(item);
        }
    }
}
