
17 May 2020
-----------

OnePlanner Mini version
-----------------------

(#) Initial Commit
		Project Created using Visual Studio 2019
			- New Project -> ASP.NET Core Web Application -> ASP.NET Core 3.1 -> API with Actions
			- No Authentication, HTTPS, No Docker Support
			- Create
		Delete default WeatherController.cs
		Add NetworksController.cs
		
(#) Add initialization code for NHibernate, few Business Object classes and their HBM mappings

(#) Added APIs in NetworksController - Create new OnepNetwork, Delete, ChangeTerminationPoint
(#) Add VP and ValidationResult
(#) Reproduce Object Deleted exception
		CreateNetworkForLEReset_01 + ResetLE_01
            // This sequence of changes results in the following exception:
            // NHibernate.ObjectDeletedException: deleted object would be re-saved by cascade (remove deleted object from associations)[OnepMini.OrmNhib.BusinessObjects.OnepValidochpath#163840]

            // Summary:
            // We already have VR01 and VR02 in the network
            // During LE reset, move VPs from under VR01 to VR02
            // And then we remove VR01 from network

			// PS: Dont change the reference of collections, for eg, dont do VR02.VPs = VR01.VPs .. that will result in another exception
				// Don't change the reference to a collection with cascade=“all-delete-orphan”

			// The other thing to note is that if we dont remove the old VR from network, everything works fine
				.. reparenting happens with no issues, except of course that the old VR is still in the network

(#) Implement ObservableList
====================================
02 Oct 2020
-----------
(#) Implement a reporting db
(#) Remove all 1P bussiness object classes, add 2 hbms to simulate fibers report in P2
(#) Note: Had to implement custom dialect - properties of type 'decimal' were getting shown in sql log as something else,
		and in the db they were actually getting created as numeric(19,5) .. strange .. nh validation was failing
	Note: Had to decorate back reference from FibersReportItem (child) to FibersReport (parent) with [JsonIgnore]
		Otherwise leading to some json exception of infinite reference looping
-----------
(#) Removed the bi-deirectional relationship (child to parent reference removed)
		This is done by commenting out the 'many-to-one' in child
		And declaring 'inverse=false' in parent's 'one-to-many' bag definition
-----------
(#) Created a root element for reporting infra that'll contain all reports
	This has 2 fold benifits - 
	(a) My mapping files for FibersReport and FibersReportItem are similar in structure now
	(b) I now have a fx where I can clearly specify which reports are included in the infra
(#) Defined uni-directional one-to-one relationship (ref: https://nhibernate.info/doc/nhibernate-reference/associations.html)
(#) Changed the unidirectional one-to-many as per recommendation here https://nhibernate.info/doc/nhibernate-reference/associations.html
		Actually, the documentation might not be updated, and on another forum I read that using a foriegn key approach is better
		(x) Unidirectonal one-to-many with foreign key
			Causes spurious update statements to update foreign key which is already put correctly by inserts
			Note: Turning it to bi-directional doesnt solve the spurious update
			In fact, the insert is done with fiberid = null, and then a later update sets it correctly
			In fact I'm lost here - now its not updating the fiberreport fk column during insert - i dont know why
			ok ok .. we need not-null=true and then the insert will have correct fiberreport column set
				<bag name="Data" lazy="true" cascade="all-delete-orphan" inverse="false" fetch="select">
				  <key column="fibersReport" not-null="true" ></key>
				  <one-to-many class="FibersReportItem"></one-to-many>
				</bag>
			Further, if we have key not-null (i.e. we ensure that the foreign key column is updated during the insert), then we can avoid extra update
			by adding this: update="false"
				<bag name="Data" lazy="true" cascade="all-delete-orphan" inverse="false" fetch="select">
				  <key column="fibersReport" not-null="true" update="false" ></key>
				  <one-to-many class="FibersReportItem"></one-to-many>
				</bag>

		(x) Unidirectonal one-to-many with join table
			Requires new table. No spurious updates, but extra inserts into the join table
			Fetch requires left outer join
			Note: I didnt implement correct mapping in my earlier commit.
			We need many-to-many with unique=true

	Anyways, I dont want an extra join table, so i'll revert to first approach
(#) DateTimeOffset
		Using DateTimeOffset in CS file isnt straight-forward with NH and postgres
		At the end it was just a matter of defining a conversion in Dialect
