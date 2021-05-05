using System;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class RemoteSite
    {
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                _definitionDirty = true;
            }
        }

        private string _name;
        
        public string Reference
        {
            get
            {
                return _reference;
            }
            set
            {
                _reference = value;
                _definitionDirty = true;
            }
        }
        
        private string _reference;
        
        public string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                _definitionDirty = true;
            }
        }
        
        private string _owner;
        
        public Guid Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                _definitionDirty = true;
            }
        }
        
        private Guid _id;

        private bool _definitionDirty;
        
        
        public RemoteSite(Site site)
        {
            Name = site.Name;
            Owner = site.Owner;
            Id = site.Id;
            Reference = site.Reference;
        }
    }
}
