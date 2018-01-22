/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel;

namespace Spp.Presentation.User.Client.Models.PropertyDescriptor
{
    // Need to use this class as ExpandoObject does not raise PropertyChanged event
    public class ValueHolder : INotifyPropertyChanged
    {
        public ValueHolder(object v)
        {
            _value = v;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private object _value;

        public object Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (_value != value)
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }
    }
}
