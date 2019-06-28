using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Interfaces;

namespace AoTracker.Crawlers.Abstract
{
    public abstract class TypedSource<T> : ICrawlerSource where T : ICrawlerSourceParameters
    {
        protected abstract Task<Stream> ObtainSource(T parameters);

        public Task<Stream> ObtainSource(ICrawlerSourceParameters parameters)
        {
            return ObtainSource((T)parameters);
        }
    }
}
