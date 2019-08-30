using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Interfaces;
using Microsoft.Extensions.Logging;

namespace AoTracker.Infrastructure.Logging
{
    public class AppCenterCrashDumpLoggerProvider : ILoggerProvider, ICrashDumpLogProvider
    {
        private readonly StringBuilder _logHistory = new StringBuilder();

        public ILogger CreateLogger(string categoryName)
        {
            return new CrashDumpLogger(categoryName, this);
        }

        public string GetLogs()
        {
            return _logHistory.ToString();
        }

        public void Dispose()
        {

        }

        private void WriteLog(string message)
        {
            _logHistory.AppendLine(message);
        }

        class CrashDumpLogger : ILogger
        {
            private readonly string _categoryName;
            private readonly AppCenterCrashDumpLoggerProvider _parent;
            private readonly List<ScopeLifetime> _scopes = new List<ScopeLifetime>();

            public CrashDumpLogger(string categoryName, AppCenterCrashDumpLoggerProvider parent)
            {
                _categoryName = categoryName;
                _parent = parent;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var message =$"[{DateTime.UtcNow}] [{logLevel}]";
                if (_scopes.Any())
                    message += $" [Scopes: {string.Join(", ", _scopes.Select(lifetime => lifetime.Scope))}] ";
                message += formatter(state, exception);
                _parent.WriteLog(message);
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel == LogLevel.Error 
                       || logLevel == LogLevel.Critical 
                       || logLevel == LogLevel.Warning;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                var scope = new ScopeLifetime(state.ToString(), this);
                _scopes.Add(scope);
                return scope;
            }

            class ScopeLifetime : IDisposable
            {
                private readonly CrashDumpLogger _parent;

                public string Scope { get; }

                public ScopeLifetime(string scope, CrashDumpLogger parent)
                {
                    _parent = parent;
                    Scope = scope;
                }

                public void Dispose()
                {
                    _parent._scopes.Remove(this);
                }
            }
        }
    }
}