
namespace OnepMini.OrmNhib.BusinessObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public enum OnepVPStatus
    {
        NilOnepVPStatus = 0,
        Passed = 1,
        Failed = 2,
        Tentative = 3,
        RequiresValidation = 4,
        Unknown = 5,
        MaxOnepVPStatus
    }

    public enum OnepTxPreCompMode
    {
        None,
        Auto,
        FixedTx
    }

    public enum OnepVPChangeInStatus
    {
        NoChange = 0,
        New = 1,
        Missing = 2,
        PreviouslyPassed = 3,
        PreviouslyFailed = 4,
        PreviouslyUnknown = 5,
        PreviouslyTentative = 6,
        //   PreviouslyNil = 7,
        Unknown = 7,
        NotValid = 8
    }

    public enum OnepVPMode
    {
        NilOnepVPMode = 0,
        Normal = 1,
        Estimation = 2,
        Normal40GOnly = 3,
        MaxOnepVPMode
    }

    public enum FiberDirection
    {
        FWD = 0,
        BWD = 1,
        BOTH = 2
    }

    public enum OnepVpType
    {
        Fix = 0,
        FtvFlex,
        SncFlex,
        FtvGridded,
        SncGridded,
    }

    public enum OnepMcMixType
    {
        NA = 0,
        Pure,
        Mix
    }

    public enum FastValidationStatus
    {
        RequiresFastValidation = 0,
        Final,
        Tentative,
        Unknown,
    }

    public enum ValidationResultStateStatus
    {
        Reset = 0,
        NonPersisted,
        Persisted,
        Pending
    }

    public partial class OnepValidochpath : BusinessBase<long>
    {
        #region Declarations

        private string _name = null;
        private double? _pmd = null;
        private OnepNetwork _onepNetwork = null;
        private OnepValidationresult _onepValidationresult = null;

        #endregion

        #region Constructors

        public OnepValidochpath() { }

        public OnepValidochpath(long defaultID) : base(defaultID) { }

        public OnepValidochpath(OnepValidochpath rhs)
        {
            this._name = rhs._name;
            this._pmd = rhs._pmd;

            CopyOnepValidochpathVolatileProperties(rhs);
        }

        partial void CopyOnepValidochpathVolatileProperties(OnepValidochpath rhs);

        #endregion

        #region Methods

        public override string BusinessSignature()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_name);
            sb.Append(_pmd);

            return sb.ToString();
        }
        #endregion

        #region Properties

        #region primitives
        public virtual string Name { get => _name; set => _name = value; }
        public virtual double? Pmd { get => _pmd; set => _pmd = value; }
        #endregion primitives

        public virtual OnepNetwork OnepNetwork { get => _onepNetwork; set => _onepNetwork = value; }
        public virtual OnepValidationresult OnepValidationresult { get => _onepValidationresult; set => _onepValidationresult = value; }

        #endregion

    }
}
