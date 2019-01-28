using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Jazz2TAS
{
    [XmlType]
    public class Inputs : INotifyPropertyChanged, IComparable<Inputs>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static Inputs _PreviousInputs;

        private int _Frame;
        private bool _Right;
        private bool _Left;
        private bool _Up;
        private bool _Down;
        private bool _Jump;
        private bool _Shoot;
        private bool _Run;
        private int? _Gun;

        [XmlAttribute]
        public int Frame
        {
            get => _Frame;
            set
            {
                if (value != _Frame)
                {
                    _Frame = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Frame"));
                }
            }
        }
        [XmlElement]
        public bool Right
        {
            get => _Right;
            set
            {
                if (value != _Right)
                {
                    _Right = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Right"));
                }
            }
        }
        [XmlElement]
        public bool Left
        {
            get => _Left;
            set
            {
                if (value != _Left)
                {
                    _Left = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Left"));
                }
            }
        }
        [XmlElement]
        public bool Up
        {
            get => _Up;
            set
            {
                if (value != _Up)
                {
                    _Up = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Up"));
                }
            }
        }
        [XmlElement]
        public bool Down
        {
            get => _Down;
            set
            {
                if (value != _Down)
                {
                    _Down = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Down"));
                }
            }
        }
        [XmlElement]
        public bool Jump
        {
            get => _Jump;
            set
            {
                if (value != _Jump)
                {
                    _Jump = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Jump"));
                }
            }
        }
        [XmlElement]
        public bool Shoot
        {
            get => _Shoot;
            set
            {
                if (value != _Shoot)
                {
                    _Shoot = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Shoot"));
                }
            }
        }
        [XmlElement]
        public bool Run
        {
            get => _Run;
            set
            {
                if (value != _Run)
                {
                    _Run = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Run"));
                }
            }
        }
        [XmlElement]
        public int? Gun
        {
            get => _Gun;
            set
            {
                if (value != _Gun)
                {
                    _Gun = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Gun"));
                }
            }
        }

        public Inputs()
        {
            if (_PreviousInputs != null)
            {
                _Frame = _PreviousInputs._Frame;
                _Right = _PreviousInputs._Right;
                _Left = _PreviousInputs._Left;
                _Up = _PreviousInputs._Up;
                _Down = _PreviousInputs._Down;
                _Jump = _PreviousInputs._Jump;
                _Shoot = _PreviousInputs._Shoot;
                _Run = _PreviousInputs._Run;
            }
            _PreviousInputs = this;
        }

        public Inputs(Inputs toClone)
        {
            _Frame = toClone._Frame;
            _Right = toClone._Right;
            _Left = toClone._Left;
            _Up = toClone._Up;
            _Down = toClone._Down;
            _Jump = toClone._Jump;
            _Shoot = toClone._Shoot;
            _Run = toClone._Run;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        
        public virtual short[] GetInputs()
        {
            short inputs = 0;
            if (_Right) inputs |= 0x0001;
            if (_Left) inputs |= 0x000F;
            if (_Down) inputs |= 0x0010;
            if (_Up) inputs |= 0x00F0;
            if (_Jump) inputs |= 0x0800;
            if (_Shoot) inputs |= 0x0200;
            if (_Run) inputs |= 0x1000;
            return new short[] { inputs };
        }

        public int CompareTo(Inputs other)
        {
            return _Frame - other._Frame;
        }
    }
}
