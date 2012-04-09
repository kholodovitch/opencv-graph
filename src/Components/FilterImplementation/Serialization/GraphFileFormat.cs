using System;
using System.Xml.Linq;

namespace FilterImplementation.Serialization
{
	// ReSharper disable InconsistentNaming
	internal static class GraphFileFormat
	{
		public static readonly XName NodeRoot = "OCVGraph";
		public static readonly XName NodeFormatVersion = "FormatVersion";

		public static class Ver_0_1
		{
			public static readonly Version Version = new Version(0, 1);

			public static readonly XName Node_Filters = "Filters";

			public static readonly XName Node_Filter = "Filter";
			public static readonly XName Node_Filter_Name = "Name";
			public static readonly XName Node_Filter_TypeGuid = "TypeGuid";
			public static readonly XName Node_Filter_NodeGuid = "NodeGuid";

			public static readonly XName Node_FilterProperty = "Property";
			public static readonly XName Node_FilterProperty_Name = "Name";
			public static readonly XName Node_FilterProperty_Value = "Value";

			public static readonly XName Node_Locations = "Locations";

			public static readonly XName Node_Location = "Location";
			public static readonly XName Node_Location_Node = "Node";
			public static readonly XName Node_Location_X = "X";
			public static readonly XName Node_Location_Y = "Y";
		}
	}
	// ReSharper restore InconsistentNaming
}