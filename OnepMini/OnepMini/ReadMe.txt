
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
