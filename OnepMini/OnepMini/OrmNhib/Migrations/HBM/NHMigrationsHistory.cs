
namespace OnepMini.OrmNhib.Migrations.HBM
{
	using System;
	using System.Collections.Generic;
	using System.Text;


	public partial class NHMigrationsHistory
    {
		#region Declarations

		private long _id = default(long);
		private string _name = null;
		private string _onepBackendVersion = null;
		private string _previousOnepBackendVersion = null;
		private string _notes = null;
		private System.DateTime? _creationTime = null;

		#endregion

		#region Constructors

		public NHMigrationsHistory() { }

		public NHMigrationsHistory(NHMigrationsHistory rhs)
		{
			this._name = rhs._name;
			this._onepBackendVersion = rhs._onepBackendVersion;
			this._previousOnepBackendVersion = rhs._previousOnepBackendVersion;
			this._notes = rhs._notes;
			this._creationTime = rhs._creationTime;
		}

		#endregion

		#region Manual Additions

		static NHMigrationsHistory() { }

		public override string ToString()
		{
			return String.Format("NHMigrationsHistory {0}", this.Name);
		}

		#endregion


		#region Properties

		public virtual long Id { get => _id; set => _id = value; }
		public virtual string Name { get => _name; set => _name = value; }
		public virtual string OnepBackendVersion { get => _onepBackendVersion; set => _onepBackendVersion = value; }
		public virtual string PreviousOnepBackendVersion { get => _previousOnepBackendVersion; set => _previousOnepBackendVersion = value; }
		public virtual string Notes { get => _notes; set => _notes = value; }
		public virtual DateTime? CreationTime { get => _creationTime; set => _creationTime = value; }

		#endregion

	}
}
