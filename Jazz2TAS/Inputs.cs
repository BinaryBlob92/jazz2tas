﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Jazz2TAS
{
    [XmlType]
    public class Inputs : INotifyPropertyChanged, IComparable<Inputs>, ICloneable
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
        private InputSequence _Sequence;

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
        [XmlElement]
        public InputSequence Sequence
        {
            get => _Sequence;
            set
            {
                if (value != _Sequence)
                {
                    _Sequence = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Sequence"));
                }
            }
        }

        public string SequenceDescription => Sequence == null ? "-" : string.Format(Sequence.Name ?? "{0} x {1} => {2}", Sequence.Length, Sequence.Repeats, Frame + Sequence.Length * Sequence.Repeats);

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
        
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        
        public virtual short[] GetInputs(int nextInputsFrame)
        {
            short[] output;

            if (Sequence == null)
            {
                output = new short[nextInputsFrame - _Frame];
            }
            else
            {
                output = new short[Math.Max(Sequence.Length * Sequence.Repeats, nextInputsFrame - _Frame)];
                Sequence.GetInputs().CopyTo(output, 0);
            }

            short inputs = 0;
            if (_Right) inputs |= 0x0001;
            if (_Left) inputs |= 0x000F;
            if (_Down) inputs |= 0x0010;
            if (_Up) inputs |= 0x00F0;
            if (_Jump) inputs |= 0x0800;
            if (_Shoot) inputs |= 0x0200;
            if (_Run) inputs |= 0x1000;

            for (int i = 0; i < nextInputsFrame - _Frame; i++)
                output[i] |= inputs;

            return output;
        }

        public int GetCalculatedHash()
        {
            int hash = 0;
            hash += _Frame << 16;
            hash += _Gun.HasValue ? _Gun.Value : 0xF0000;
            hash += Sequence == null ? 0xF00000 : Sequence.GetCalculatedHash();
            if (_Right) hash |= 0x0001;
            if (_Left) hash |= 0x000F;
            if (_Down) hash |= 0x0010;
            if (_Up) hash |= 0x00F0;
            if (_Jump) hash |= 0x0800;
            if (_Shoot) hash |= 0x0200;
            if (_Run) hash |= 0x1000;
            return hash;
        }

        public int CompareTo(Inputs other)
        {
            return _Frame - other._Frame;
        }

        public object Clone()
        {
            var output = new Inputs();
            output._Frame = _Frame;
            output._Right = _Right;
            output._Left = _Left;
            output._Up = _Up;
            output._Down = _Down;
            output._Jump = _Jump;
            output._Shoot = _Shoot;
            output._Run = _Run;
            output._Gun = _Gun;
            output._Sequence = (InputSequence)_Sequence?.Clone();
            return output;
        }
    }
}
