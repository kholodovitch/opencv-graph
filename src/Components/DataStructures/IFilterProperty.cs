namespace DataStructures
{
	public interface IFilterProperty
	{
		string Name { get; }

		FilterPropertyType Type { get; }

		object Value { get; set; }
	}
}