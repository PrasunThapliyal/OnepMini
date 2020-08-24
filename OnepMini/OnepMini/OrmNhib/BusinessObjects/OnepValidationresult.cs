
namespace OnepMini.OrmNhib.BusinessObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class OnepValidationresult : BusinessBase<long>
    {
        #region Declarations

        private int? _status = null;
        private OnepNetwork _onepNetwork = null;
        private IList<OnepValidochpath> _onepValidochpaths = new List<OnepValidochpath>();

        #endregion

        #region Constructors

        public OnepValidationresult() { }

        public OnepValidationresult(long defaultID) : base(defaultID) { }

        public OnepValidationresult(OnepValidationresult rhs)
        {
            this._status = rhs._status;

            CopyOnepValidationresultVolatileProperties(rhs);
        }

        partial void CopyOnepValidationresultVolatileProperties(OnepValidationresult rhs);

        #endregion

        #region Methods

        public override string BusinessSignature()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_status);

            return sb.ToString();
        }
        #endregion

        #region Properties

        #region primitives
        public virtual int? Status { get => _status; set => _status = value; }
        #endregion primitives

        public virtual OnepNetwork OnepNetwork { get => _onepNetwork; set => _onepNetwork = value; }
		public virtual IList<OnepValidochpath> OnepValidochpaths { get => _onepValidochpaths; set => _onepValidochpaths = value; }

        #endregion

    }
}
