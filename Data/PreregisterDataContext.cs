using System;
using DirectorySite.Models;

namespace DirectorySite.Data
{
    public class PreregisterDataContext
    {
        private IEnumerable<PreregisterResponse> _preregisters = [];
        private DateTime? _lastUpdate = null;
        private TimeSpan refreshInterval = TimeSpan.FromMinutes(2);

        public IEnumerable<PreregisterResponse> Preregisters
        {
            get => _preregisters;
            set {
                lock(_preregisters)
                {
                    _preregisters = value;
                }
                this._lastUpdate = DateTime.Now;
            }
        }

        public DateTime? LastUpdate { get => _lastUpdate; }

        public bool DataIsValid {
            get {
                if(_lastUpdate == null)
                {
                    return false;
                }
                return (DateTime.Now - _lastUpdate) <= refreshInterval;
            }
        }

    }
}